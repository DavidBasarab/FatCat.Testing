# Phase 04 — Collection entry points + core assertions (G4)

- **Work item:** `final_gaps`
- **Gap:** **G4** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **HIGH.** This adds `Should<T>` overloads on `IEnumerable<T>`, `List<T>`, and `T[]` — the first
  overloads that compete with the existing concrete overloads (`string` is `IEnumerable<char>`) and that
  phase 06's object overload will compete with. A wrong overload resolution compiles green and asserts the
  wrong thing. **Extra human review before commit** (orchestrator, high-risk gate).
- **Depends on:** 02
- **Depended on by:** 05, 06, 07, 08, 09, 10, 13, and `tier_2_gaps/13` (external)
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

There is no `Collections/` folder and no `Should()` overload for any collection type. This phase creates the
comparer and the entry points and ships the highest-volume assertions; phase 05 ships the remainder.

**The overload-resolution trap, from `remaining_gaps.md` §3 ADR-C — read it before writing any code.** A
generic `Should<T>(this T) where T : class` (which phase 06 adds) silently swallows every collection: for a
`List<string>`, binding `T = List<string>` is an *identity* conversion, while binding to `IEnumerable<T>`
needs a reference conversion, and identity wins. The fix is **concrete-shape overloads** —
`Should<T>(this IEnumerable<T>)`, `Should<T>(this List<T>)`, `Should<T>(this T[])` — because a constructed
type beats a bare type parameter in overload resolution.

**ADR-4 in [00-overview.md](00-overview.md) makes the ordering explicit: G4 ships before G1.** At the moment
this phase commits, no object overload exists yet, so nothing can mis-bind. This phase's job is to lay down
the concrete-shape overloads **and the regression test class** that phase 06 will extend, so that when the
object overload arrives it provably loses to these.

`string` is `IEnumerable<char>`. The existing `Should(this string)` overload must keep winning for strings —
verify it, do not assume it.

Read before starting: [00-overview.md](00-overview.md) (**ADR-4**, **ADR-12**, **ADR-13**),
`remaining_gaps.md` §3, `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, and phase 02's hand-off (use `ValueFormatter`).

---

## Scope

**In scope**

- `Collections/CollectionComparer.cs` + `NotCollectionComparer.cs`, generic on `T`.
- The three concrete-shape `Should<T>` entry points.
- The overload-resolution regression suite (extended by phase 06).
- Core assertions by call volume: `Contain(T)`, `NotContain` (via `Not`), `BeEmpty`, `NotBeEmpty`,
  `HaveCount`, `ContainSingle`.

**Out of scope — phase 05:** `OnlyContain`, `Equal`, `OnlyHaveUniqueItems`, `ContainInOrder`,
`BeInDescendingOrder`, `HaveCountGreaterThan`/`LessThan`, and the rest of G26's collection list (phase 13).

**Out of scope — phase 08:** `ContainEquivalentOf`, `NotContainEquivalentOf`. They need the equivalency
engine (G3), which does not exist yet. Do not stub them.

**Out of scope — always:** any edit to `Dictionaries/` or `IDictionary` — that is `tier_2_gaps/13`, a
different plan (ADR-14). Do not add a dictionary overload here.

---

## Design

New folder `FatCat.Testing/Collections/`, namespace `FatCat.Testing.Collections`.

```csharp
public class CollectionComparer<T>(IEnumerable<T> subject)
	: ComparerBase<IEnumerable<T>, CollectionComparer<T>>(subject)
{
	public NotCollectionComparer<T> Not { get; } = new(subject);
	// ...
}
```

### Entry points — `ShouldExtensions.cs`

```csharp
public static CollectionComparer<T> Should<T>(this IEnumerable<T> subject) { return new CollectionComparer<T>(subject); }

public static CollectionComparer<T> Should<T>(this List<T> subject) { return new CollectionComparer<T>(subject); }

public static CollectionComparer<T> Should<T>(this T[] subject) { return new CollectionComparer<T>(subject); }
```

Why all three, when `IEnumerable<T>` seemingly covers the others: a variable **typed** as `List<string>` or
`string[]` binds by identity to phase 06's future `Should<T>(this T)` unless a more-specific constructed
overload exists. `List<T>` and `T[]` are those overloads. This is the entire point of the phase — do not
drop them as redundant. `IEnumerable<T>` catches interface-typed and lazy-query subjects.

`OQ-2`: these go inside `ShouldExtensions.cs`'s `#nullable enable` region, so any `because` declared **in
this file** is `string?`. The comparer files themselves do **not** carry `#nullable enable` and use
`string because = null`.

### Multiple enumeration

An `IEnumerable<T>` may be a lazy query that enumerates differently or has side effects each time. **Snapshot
once** in the constructor:

```csharp
private readonly List<T> items = subject is null ? null : subject.ToList();
```

Every assertion reads `items`, never re-enumerates `subject`. A null subject is preserved as null (not an
empty list) so `Not.BeEmpty` on a null subject can produce a sensible message rather than a false pass.

### Assertions in this phase

| Method | Fails when | Message (via `ValueFormatter`) |
|---|---|---|
| `Contain(T expected)` | no element equals `expected` | `{items} should contain {expected}` |
| `BeEmpty()` | `items` is null or non-empty | `{items} should be empty` |
| `NotBeEmpty()` | `items` is null or empty | `{items} should not be empty` |
| `HaveCount(int expected)` | `items` is null or `Count != expected` | `{items} should have count {expected} but has {actual}` |
| `ContainSingle()` | `items` is null or `Count != 1` | `{items} should contain a single element but has {actual}` |

`NotCollectionComparer<T>` provides the negation: `Contain` (fails when it *does* contain — this is the 18
`NotContain` sites), `BeEmpty`, `HaveCount`. Mirror the null handling exactly between the two files.

Element equality uses `EqualityComparer<T>.Default` — i.e. `Equals`. Structural (`ContainEquivalentOf`) is
phase 08.

Format subjects with `ValueFormatter.Format` (phase 02 hand-off) — a raw `$"{items}"` renders
``System.Collections.Generic.List`1...`` which is exactly what phase 02 exists to prevent. The expected
value is formatted too, so `Contain("a")` on a `List<string>` reads `should contain "a"` (nested string,
quoted — ADR-13).

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Collections/`, namespace
`Tests.FatCat.Testing.Collections`.

1. **Red — the overload suite first.** `CollectionOverloadResolutionTests.cs`. This is the most important
   file in the phase. **Phase 06 extends this exact class**, so structure it to be appended to.
   - `ListBindsToCollectionComparer` — `List<string>` → `CollectionComparer<string>`
   - `ArrayBindsToCollectionComparer` — `string[]` → `CollectionComparer<string>`
   - `EnumerableBindsToCollectionComparer` — `IEnumerable<string>` (a `Where` query) → `CollectionComparer<string>`
   - `StringStillBindsToStringComparer` — `"hello".Should()` is still the string comparer, **not**
     `CollectionComparer<char>`. Assert on a string-only method (e.g. `Contain` with the string message) or
     on the returned type via a helper. This is the `IEnumerable<char>` trap; it must be pinned.
   - `IntStillBindsToNumericComparer` — `42.Should()` unchanged.
   Assert the bound type by calling a method unique to the expected comparer and checking the **message
   shape**, since `.GetType()` on the comparer is the cleanest proof — use whichever the rules allow; a
   helper returning the comparer and asserting `comparer.Should().BeOfType<CollectionComparer<string>>()`
   works and reads well.

2. **Red.** `CollectionContainTests.cs` — full set:
   - `GoodContain` — `new List<int> { 1, 2, 3 }.Should().Contain(2);`
   - `BadContain`
   - `BadContainShowsCorrectMessage` → `"[1, 2, 3] should contain 5"`
   - `BadContainWithBecause`
   - `GoodNotContain` / `BadNotContain` / `BadNotContainShowsCorrectMessage` → `"[1, 2, 3] should not contain 2"` / `BadNotContainWithBecause`
   - `BadContainOnNull` — a null subject fails `Contain` with a `null should contain 5` message

3. **Red.** `CollectionBeEmptyTests.cs`, `CollectionNotBeEmptyTests.cs`, `CollectionHaveCountTests.cs`,
   `CollectionContainSingleTests.cs` — each the full set, including the null-subject case.

4. **Red.** `CollectionMultipleEnumerationTests.cs` — a query with a side-effect counter; assert the
   assertion enumerates the source **once**. This proves the snapshot.

5. **Green.** Implement the two comparer files and the three entry points.

6. **Verify the string trap in place.** Run the whole suite. Every existing string test must still pass —
   if `"hello".Should()` started binding to `CollectionComparer<char>`, string tests go red. That red is the
   signal, not a test to edit.

Use explicit literals — the collection contents appear verbatim in the failure message.

---

## Files

**Added**

- `FatCat.Testing/Collections/CollectionComparer.cs`
- `FatCat.Testing/Collections/NotCollectionComparer.cs`
- `Tests.FatCat.Testing/Collections/CollectionOverloadResolutionTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionContainTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionBeEmptyTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionNotBeEmptyTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionHaveCountTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionContainSingleTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionMultipleEnumerationTests.cs`

**Changed**

- `FatCat.Testing/ShouldExtensions.cs` — three overloads
- `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `### Collections`** — replace the phase-04 placeholder. Table of the shipped methods. Note:
collections are matched by element equality (`Equals`); `ContainEquivalentOf` (structural) arrives in phase
08; a lazy `IEnumerable` is snapshotted once so side-effecting queries are safe.

**`MIGRATION.md` → `## 3. Mapping Table`** — flip to `✅ supported`:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().Contain(x)` (collection) | `.Should().Contain(x)` | `Tests.FatCat.Testing.Collections.CollectionContainTests` |
| `.Should().NotContain(x)` | `.Should().Not.Contain(x)` | `Tests.FatCat.Testing.Collections.CollectionContainTests` |
| `.Should().BeEmpty()` (collection) | `.Should().BeEmpty()` | `Tests.FatCat.Testing.Collections.CollectionBeEmptyTests` |
| `.Should().NotBeEmpty()` | `.Should().Not.BeEmpty()` | `Tests.FatCat.Testing.Collections.CollectionNotBeEmptyTests` |
| `.Should().HaveCount(n)` | `.Should().HaveCount(n)` | `Tests.FatCat.Testing.Collections.CollectionHaveCountTests` |
| `.Should().ContainSingle()` | `.Should().ContainSingle()` | `Tests.FatCat.Testing.Collections.CollectionContainSingleTests` |

**`MIGRATION.md` → `## 4. Type Coverage`** — collections move to supported (core methods); note
`ContainEquivalentOf` still pending phase 08.

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
```

**High-risk gate** — run twice and paste both into the phase report:

```pwsh
dotnet test Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~OverloadResolution"
```

Then run the standards review and **stop for human review before committing** (orchestrator high-risk gate).

---

## Definition of Done

- [ ] Tests written before implementation; red state observed and recorded.
- [ ] Three concrete-shape entry points: `IEnumerable<T>`, `List<T>`, `T[]`. All three present — none
      dropped as "redundant".
- [ ] `CollectionOverloadResolutionTests` proves `List<T>`, `T[]`, and `IEnumerable<T>` bind to
      `CollectionComparer<T>`, and that `string` and `int` are unchanged.
- [ ] Lazy `IEnumerable` snapshotted once; proven by the multiple-enumeration test.
- [ ] `Contain`, `BeEmpty`, `NotBeEmpty`, `HaveCount`, `ContainSingle` implemented with the full test set and
      `Not` equivalents, including the null-subject case on each.
- [ ] Subjects and expected values rendered via `ValueFormatter` — no bare `$"{items}"`.
- [ ] The full existing suite (strings especially) stays green — the `IEnumerable<char>` trap did not fire.
- [ ] No new compiler warnings. New comparer files carry no `#nullable enable`; `ShouldExtensions.cs`
      additions respect its region.
- [ ] No banned patterns.
- [ ] `dotnet format style` / `analyzers` run; `dotnet csharpier .` **last**.
- [ ] `README.md` and the six `MIGRATION.md` rows written.
- [ ] Standards review clean; high-risk human review done.
- [ ] Exactly one commit, message referencing `tasks/todo/final_gaps/04-collection-entry-points-and-core.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-04-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

**Cascade:** everything from 05 onward except 03/12/14–19 builds on this. **The trap:** if phase 06 has
landed, reverting 04 alone leaves `Should<T>(this T) where T : class` with no concrete-shape overloads to
beat it, so every collection call site silently binds to the object comparer. **Revert 06 first** (ADR-4,
orchestrator rollback note).

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing;
CollectionComparer<T> Should<T>(this IEnumerable<T> subject)
CollectionComparer<T> Should<T>(this List<T> subject)
CollectionComparer<T> Should<T>(this T[] subject)

namespace FatCat.Testing.Collections;
CollectionComparer<T>.Contain(T) / BeEmpty() / NotBeEmpty() / HaveCount(int) / ContainSingle() / Not
NotCollectionComparer<T>.Contain(T) / BeEmpty() / HaveCount(int)
```

**For phase 06 (objects) — the critical contract.** When you add `Should<T>(this T) where T : class`, these
three concrete-shape overloads must keep winning for collections. **Extend
`CollectionOverloadResolutionTests`** with: `Dto → ObjectComparer`, and re-run every case in this file to
prove none moved. Do not create a second overload-resolution class.

**For phase 05:** add remaining collection assertions to these same two comparer files. The snapshot field
`items` and its null convention are the contract — reuse them.

**For phase 08:** `ContainEquivalentOf` / `NotContainEquivalentOf` are added here once G3 exists; they call
the equivalency engine per element.

**For `tier_2_gaps/13` (external plan, now unblocked):** G4 exists, so the dictionary phase can run. It must
**re-probe** whether `Should<K, V>(this IDictionary<K, V>)` beats these binding to
`IEnumerable<KeyValuePair<K, V>>` — that is unverified (`remaining_gaps.md` §3) and stays that plan's job.
Do not add the dictionary overload from this phase.
