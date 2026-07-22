# Phase 07 — Equivalency engine, objects (G3)

- **Work item:** `final_gaps`
- **Gap:** **G3** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **HIGH.** This is the hardest gap in the library — a recursive member-by-member comparator with
  cycle detection, depth limiting, and a diff message that names the member that differed. 241 call sites
  depend on it. **Extra human review before commit.**
- **Depends on:** 06 (object comparer), 02 (formatter)
- **Depended on by:** 08, 09
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Today `BeEquivalentTo` exists only on strings (case-insensitive compare). Objects need recursive member-by-
member structural comparison — this is what 241 call sites use to compare DTOs without those DTOs having to
override `Equals`. The failure message must name the member that differed, or the assertion is not usable —
which is exactly why phase 02 (the formatter) came first.

This phase does **objects only**. Collection equivalency is phase 08 (it depends on both this and phase 05);
the global `Using<T>`/`WhenTypeIs<T>` configuration hook is phase 09. Splitting keeps each commit reviewable
and each independently revertible.

**Two open questions gate this phase — they must be answered before it runs** (orchestrator precondition):

- **OQ-3** — depth and breadth limits. Proposal: max depth **10**, collection cap **32** (FluentAssertions'
  number, and the same cap `ValueFormatter` already uses).
- **OQ-4** — properties only, or fields too? Proposal: **public readable instance properties only**, fields
  excluded, matching both FluentAssertions' default and `ValueFormatter`'s object-dump rule. This must be the
  **same** member-selection rule the formatter uses (phase 02 hand-off) — two different notions of "a type's
  members" in one library is a defect.

Read before starting: [00-overview.md](00-overview.md) (**ADR-6**, default options + one hook; **OQ-3**,
**OQ-4**), phase 02's hand-off (cycle detection + member selection), phase 06's hand-off,
`.claude/rules/csharp/naming-and-structure.md`, `types.md`, `errors.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope**

- `Equivalency/EquivalencyComparer.cs` — the recursive engine, returning a **result** (not throwing), naming
  the first member that differed and the path to it.
- `ObjectComparer<T>.BeEquivalentTo(T expected, string because = null)` and the negation on
  `NotObjectComparer<T>`.

**Out of scope**

- Collections — **phase 08.** The engine must be *written* to recurse into collection members (a DTO with a
  `List<string>` property), but the top-level `IEnumerable.Should().BeEquivalentTo(...)` entry point is phase
  08. Draw that line carefully: recursing into a collection-typed member is in scope; a collection *subject*
  is not.
- The `Using<T>`/`WhenTypeIs<T>` hook — **phase 09.** Write the engine so a type-keyed override *can* be
  injected later (a hook the engine consults), but ship it consulting an empty registry.
- `Excluding`, `Including`, `RespectingRuntimeType`, `WithStrictOrdering`, and the other ~45 option methods
  (ADR-6) — zero usages, never built.
- A per-call options lambda. No consumer uses one.

---

## Design

New folder `FatCat.Testing/Equivalency/`, namespace `FatCat.Testing.Equivalency`.

### The engine

Return a result, do not throw from inside the recursion — the comparer method turns a negative result into a
`CompareException`. This keeps the engine reusable by the negated form and by phase 08.

```csharp
namespace FatCat.Testing.Equivalency;

public class EquivalencyResult
{
	public bool AreEquivalent { get; }
	public string Path { get; }         // e.g. "Address.City"; empty at the root
	public string Difference { get; }   // e.g. "expected \"York\" but found \"Leeds\""
}

public static class EquivalencyComparer
{
	public static EquivalencyResult Compare(object subject, object expected) { ... }
}
```

Comparison rules, in order:

1. Both null → equivalent. One null → not, path empty, difference names which side.
2. If the runtime type **overrides `Equals`** (same test as `ValueFormatter` uses for `ToString`) → compare
   with `Equals`. This makes `string`, primitives, `Guid`, `DateTime`, enums compare by value and terminate
   the recursion. **This is the base case** — without it the engine tries to dump the members of an `int`.
3. If both are `IEnumerable` (and not string) → element comparison **deferred to phase 08's logic**; in this
   phase, recursing into a collection *member* uses order-insensitive matching (ADR-12) with the 32-cap.
   Write the helper now; phase 08 exposes it at top level. (If keeping this fully out of phase 07 is cleaner,
   compare collection-typed members by a simple ordered element walk here and let phase 08 replace it with
   the order-insensitive matcher — **state which you did in the report**.)
4. Otherwise → recurse over public readable instance properties (OQ-4). For each property, `Compare` the two
   values; the first non-equivalent property sets `Path` (prepend the property name) and `Difference`.
5. **Cycle detection** — a reference already on the current path (reference equality, per phase 02) →
   treat as equivalent to avoid infinite recursion, matching FluentAssertions' behaviour for cyclic graphs.
6. **Depth cap** at 10 (OQ-3) → beyond it, treat as equivalent and stop (do not throw); this is a safety
   valve for pathological graphs, not an expected path.

A property getter that throws → catch it and treat that property as differing with a difference of
`<threw TypeName>`, mirroring `ValueFormatter`'s handling. Comment the catch (`errors.md`).

### The comparer method

```csharp
public ObjectComparer<T> BeEquivalentTo(T expected, string because = null)
{
	var result = EquivalencyComparer.Compare(Subject, expected);

	if (!result.AreEquivalent)
	{
		CompareException.New(because ?? BuildMessage(result));
	}

	return this;
}
```

Message shape (via `ValueFormatter` for the leaf values):

- Root mismatch: `{Subject} should be equivalent to {expected}`
- Member mismatch: `{Subject} should be equivalent to {expected} but Address.City differs: expected "York" but found "Leeds"`

The path-and-difference clause is the whole point of the phase. A message that says only "not equivalent" is
a failure of this phase's Definition of Done.

`NotObjectComparer<T>.BeEquivalentTo` fails when the two **are** equivalent: `{Subject} should not be
equivalent to {expected}`. No path clause — there is nothing to diff.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Equivalency/` (engine) and
`Tests.FatCat.Testing/Objects/` (the comparer method).

1. **Red — the engine.** `EquivalencyComparerTests.cs`, asserting on `EquivalencyResult` directly:
   - `GoodCompareEqualScalars`, `GoodCompareEqualStrings`
   - `GoodCompareStructurallyEqualDtos` — two DTOs, same property values, **no `Equals` override** → equivalent
   - `BadCompareReportsDifferingProperty` — differ on `Name` → `Path == "Name"`, difference names both values
   - `BadCompareReportsNestedPath` — DTO with an `Address` property differing on `City` → `Path == "Address.City"`
   - `GoodCompareCyclicGraph` — mutually referencing objects → returns, equivalent
   - `GoodCompareRespectsDepthCap`
   - `GoodCompareUsesEqualsForTypesThatOverrideIt` — a type overriding `Equals` compares by value, not by member
   - `BadCompareThrowingPropertyIsReported`
   - `GoodCompareIgnoresFields` (OQ-4) — a public field differing does **not** make them inequivalent;
     document loudly, this is the surprising one

2. **Red — the comparer.** `ObjectBeEquivalentToTests.cs`:
   - `GoodBeEquivalentTo` — structurally equal DTOs
   - `BadBeEquivalentTo`
   - `BadBeEquivalentToShowsCorrectMessage` — the message **names the member**, e.g.
     `... but Name differs: expected "Bob" but found "Alice"`
   - `BadBeEquivalentToNestedShowsPath` → `Address.City`
   - `BadBeEquivalentToWithBecause`
   - `GoodNotBeEquivalentTo` / `BadNotBeEquivalentTo` / `BadNotBeEquivalentToShowsCorrectMessage` /
     `BadNotBeEquivalentToWithBecause`

3. **Green.** Implement `EquivalencyComparer`, `EquivalencyResult`, and the two comparer methods.

4. Run the whole suite — the string `BeEquivalentTo` (case-insensitive compare, a different method on a
   different comparer) must be untouched and still green.

Fixtures: `Person { Name, Age, Address }`, `Address { City, Postcode }`, a `Money` type overriding `Equals`,
a type with a throwing property, a cyclic pair. One class per file.

---

## Files

**Added**

- `FatCat.Testing/Equivalency/EquivalencyComparer.cs`
- `FatCat.Testing/Equivalency/EquivalencyResult.cs`
- `Tests.FatCat.Testing/Equivalency/EquivalencyComparerTests.cs`
- `Tests.FatCat.Testing/Objects/ObjectBeEquivalentToTests.cs`
- Fixture types under both test folders — one class per file

**Changed**

- `FatCat.Testing/Objects/ObjectComparer.cs`, `NotObjectComparer.cs` — one method each
- `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `### Objects`** — append `BeEquivalentTo`. Explain: recursive, compares public readable
instance properties (not fields — OQ-4), uses `Equals` for types that override it, is cycle-safe, and names
the differing member in the failure. Contrast with `Be` (which uses the type's own `Equals`).

**`MIGRATION.md` → `## 3. Mapping Table`** — flip:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().BeEquivalentTo(obj)` (object) | `.Should().BeEquivalentTo(obj)` | `Tests.FatCat.Testing.Objects.ObjectBeEquivalentToTests` |
| `.Should().NotBeEquivalentTo(obj)` | `.Should().Not.BeEquivalentTo(obj)` | `Tests.FatCat.Testing.Objects.ObjectBeEquivalentToTests` |

**`MIGRATION.md` → `## 5. Behavioural Differences`** — add: object `BeEquivalentTo` compares public
properties only (fields excluded); FluentAssertions can be configured to include fields, but that option is
not shipped (ADR-6). And: collection `BeEquivalentTo` is order-insensitive (points to phase 08).

**`MIGRATION.md` → `## 6. Known Unsupported`** — list the equivalency option methods not shipped
(`Excluding`, `Including`, `WithStrictOrdering`, `RespectingRuntimeType`, `ComparingByMembers`, …) with the
recommended rewrite (assert the specific members, or use `Satisfy`).

---

## Verification

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
dotnet format style Fatcat.Testing.sln
dotnet format analyzers Fatcat.Testing.sln
dotnet csharpier .
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Equivalency"
```

Standards review, then **stop for human review before committing** (high-risk gate).

---

## Definition of Done

- [ ] OQ-3 and OQ-4 answered (recorded in the report with the chosen values).
- [ ] Tests written before implementation; red observed and recorded.
- [ ] `EquivalencyComparer.Compare` returns a result naming the path and difference; never throws from the
      recursion.
- [ ] Base case: types overriding `Equals` compare by value and terminate recursion — pinned by a test.
- [ ] Member selection is public readable instance properties, **identical** to `ValueFormatter`'s rule;
      fields excluded, pinned by a test.
- [ ] Cycle-safe and depth-capped; both pinned by tests.
- [ ] Throwing property getter caught and reported, catch commented.
- [ ] `BeEquivalentTo` failure message **names the differing member and path** — pinned at root and nested
      depth.
- [ ] `Not.BeEquivalentTo` implemented with full set.
- [ ] String `BeEquivalentTo` untouched and still green.
- [ ] No new warnings; no `#nullable enable` in new files; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + two mapping rows + behavioural-difference + known-unsupported entries written.
- [ ] Standards review clean; high-risk human review done.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/07-equivalency-engine-objects.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-07-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

**Phases 08 and 09 build directly on the engine** — revert them first. Removing `BeEquivalentTo` from the
object comparers and the engine is otherwise self-contained.

---

## Hand-off

Public surface:

```csharp
namespace FatCat.Testing.Equivalency;
EquivalencyComparer.Compare(object subject, object expected) : EquivalencyResult
EquivalencyResult { AreEquivalent, Path, Difference }

namespace FatCat.Testing.Objects;
ObjectComparer<T>.BeEquivalentTo(T expected, string because = null)
NotObjectComparer<T>.BeEquivalentTo(T expected, string because = null)
```

For **phase 08**: the collection-member matching helper is here — expose it at top level for
`IEnumerable.Should().BeEquivalentTo(...)` and for `ContainEquivalentOf`. Reuse `EquivalencyComparer.Compare`
per element pair; do not write a second comparator.

For **phase 09**: the engine consults a type-keyed override registry that is currently empty. Phase 09 fills
it (the `Using<DateTime>().WhenTypeIs<DateTime>()` equivalent). The injection point — where `Compare` checks
"is there a custom rule for this runtime type before falling to `Equals`/member walk" — is the seam; name it
clearly so phase 09 plugs in without restructuring.

State in the report which choice you made for collection-typed *members* (order-insensitive matcher now, or
simple walk now + phase 08 replaces) — phase 08 needs to know.
