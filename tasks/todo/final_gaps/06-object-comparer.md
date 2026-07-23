# Phase 06 — Object comparer + object entry point (G1)

- **Work item:** `final_gaps`
- **Gap:** **G1** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **HIGH.** Adds `Should<T>(this T) where T : class` — the overload that, per ADR-C, silently
  swallows collections unless phase 04's concrete-shape overloads beat it. This is the single largest gap
  (~1,100 call sites) and the one most able to compile green while asserting the wrong thing. **Extra human
  review before commit** (orchestrator high-risk gate).
- **Depends on:** 04 (its concrete-shape overloads must already exist), 02 (formatter)
- **Depended on by:** 07, 08, 09, 10
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

There is no `Should()` for reference types. `result.Should()` on a class does not compile. The only generic
entry point is `Should<T>(this T) where T : struct, Enum`. This phase adds the object entry point and
comparer, unblocking object `Be`, `NotBeNull` (261 sites), `BeNull` (131), `BeSameAs`/`NotBeSameAs` (10), and
object `BeOfType`.

**Why this phase runs after phase 04, not with it (ADR-4).** A generic `Should<T>(this T) where T : class`
binds a `List<string>` by *identity* conversion, which beats the *reference* conversion to
`Should<T>(this IEnumerable<T>)` — so without phase 04's concrete `List<T>` / `T[]` / `IEnumerable<T>`
overloads already in place, every collection call site would bind here and assert the wrong thing, silently.
Phase 04 laid those overloads and the regression suite. **This phase's central obligation is to prove they
still win now that the object overload exists.**

**Two constraints must coexist in `ShouldExtensions`.** `remaining_gaps.md` §3 ADR-C point 1: two
`Should<T>(this T)` overloads differing only by constraint **cannot** live in the same static class — that is
`CS0111`, because constraints are not part of a signature. The existing enum overload is
`Should<T>(this T) where T : struct, Enum` in `ShouldExtensions`. Therefore the object overload **must go in
a separate static class**, e.g. `ObjectShouldExtensions`. `where T : struct, Enum` and `where T : class` are
disjoint, so across two static classes each binds correctly.

Read before starting: [00-overview.md](00-overview.md) (**ADR-4**, **ADR-5**, **ADR-13**),
`remaining_gaps.md` §3 (all of ADR-C), phase 04's hand-off, `.claude/rules/csharp/naming-and-structure.md`,
`types.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope**

- `Objects/ObjectComparer.cs` + `Objects/NotObjectComparer.cs`, generic on `T` constrained `where T : class`.
- A **separate** static class `ObjectShouldExtensions` with `Should<T>(this T) where T : class`.
- Assertions: `Be` (via `Equals`), `BeNull`, `BeSameAs`, `Satisfy` — plus `BeOfType` / `BeAssignableTo` /
  `BeOneOf` **inherited from `ComparerBase`** (do not re-implement).
- Extending phase 04's `CollectionOverloadResolutionTests` to prove nothing mis-binds.

**Out of scope**

- `BeEquivalentTo` — **phase 07.** It arrives with the equivalency engine. Do not stub it.
- `Match(predicate)`, `As<T>()` (G26 objects, `gaps.md` §6.3) — not in this plan's phase list; note them as
  future work, do not add them.
- Any change to the enum overload or any existing concrete overload.

---

## Design

New folder `FatCat.Testing/Objects/`, namespace `FatCat.Testing.Objects`.

```csharp
public class ObjectComparer<T>(T subject) : ComparerBase<T, ObjectComparer<T>>(subject)
	where T : class
{
	public NotObjectComparer<T> Not { get; } = new(subject);
	// ...
}
```

`ComparerBase<TSubject, TComparer>` already provides `BeOfType`, `BeAssignableTo`, `BeOneOf`, `Satisfy`, and
a private `FormatSubject` that (after phase 02) delegates to `ValueFormatter`. The object comparer inherits
all of it. Only the object-specific methods are new.

### Entry point — new file `ObjectShouldExtensions.cs`

```csharp
namespace FatCat.Testing;

public static class ObjectShouldExtensions
{
	public static ObjectComparer<T> Should<T>(this T subject)
		where T : class
	{
		return new ObjectComparer<T>(subject);
	}
}
```

Do **not** put this in `ShouldExtensions.cs` — `CS0111` against the enum overload. A separate file for a
separate static class also satisfies the one-class-per-file rule.

`OQ-2`: this new file must **not** carry `#nullable enable`, so `because` is `string because = null`.

### Assertions

| Method | Comparer | Fails when | Message (via `ValueFormatter`) |
|---|---|---|---|
| `Be(T expected)` | `ObjectComparer` | `!Equals(Subject, expected)` | `{Subject} should be {expected}` |
| `BeNull()` | `ObjectComparer` | `Subject is not null` | `{Subject} should be null` |
| `BeSameAs(T expected)` | `ObjectComparer` | `!ReferenceEquals(Subject, expected)` | `{Subject} should be the same instance as {expected}` |
| `Be(T expected)` | `NotObjectComparer` | `Equals(Subject, expected)` | `{Subject} should not be {expected}` |
| `BeNull()` | `NotObjectComparer` | `Subject is null` | `{Subject} should not be null` |
| `BeSameAs(T expected)` | `NotObjectComparer` | `ReferenceEquals(Subject, expected)` | `{Subject} should not be the same instance as {expected}` |

**ADR-5 — `Be` uses `Equals`, `BeSameAs` uses `ReferenceEquals`.** A DTO overriding `Equals` compares by
value through `Be`. Reference identity is a distinct concept with its own method. Use the static
`object.Equals(a, b)` so a null subject is handled without a NullReferenceException.

`Not.BeNull()` — the 261-site workhorse — must produce a clean message for a null subject: `null should not
be null`.

The `Be` message on two objects that differ but format identically (same `ToString`, no member dump because
they override `ToString`) is a known readability limit; `BeEquivalentTo` (phase 07) is the assertion that
explains *why* two objects differ. Note that in `README.md` so `Be` is not misread as structural.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Objects/`, namespace
`Tests.FatCat.Testing.Objects`.

1. **Red — extend phase 04's overload suite, do not create a new one.** Open
   `Tests.FatCat.Testing/Collections/CollectionOverloadResolutionTests.cs` and add:
   - `DtoBindsToObjectComparer` — a plain reference type → `ObjectComparer<Dto>`
   - `InterfaceTypedReferenceBindsToObjectComparer` — a variable typed as an interface → object comparer
   - Re-assert every existing case in that file still holds: `List<T>`, `T[]`, `IEnumerable<T>` →
     `CollectionComparer<T>`; `string` → string comparer; `int` → numeric; enum → enum comparer.

   This file is now the executable statement of ADR-C across both phases. It living in the `Collections`
   folder is deliberate — the trap is a collection-vs-object question. Note in the phase report that the
   object cases were appended there.

2. **Red.** `ObjectBeTests.cs` — a fixture `Dto` overriding `Equals`/`GetHashCode` on a `Name` property:
   - `GoodBe` — two equal-by-value DTOs
   - `GoodBeUsesEqualsNotReference` — two **distinct instances** that are `Equals` → passes (proves ADR-5)
   - `BadBe` / `BadBeShowsCorrectMessage` / `BadBeWithBecause`
   - `GoodNotBe` / `BadNotBe` / `BadNotBeShowsCorrectMessage` / `BadNotBeWithBecause`

3. **Red.** `ObjectBeNullTests.cs`:
   - `GoodBeNull` — `Dto d = null; d.Should().BeNull();`
   - `GoodNotBeNull` — `new Dto().Should().Not.BeNull();`
   - `BadBeNull` / `BadBeNullShowsCorrectMessage` / `BadBeNullWithBecause`
   - `BadNotBeNull` → `"null should not be null"` / `BadNotBeNullWithBecause`

4. **Red.** `ObjectBeSameAsTests.cs`:
   - `GoodBeSameAs` — same instance
   - `BadBeSameAsWhenEqualButNotSame` — two `Equals`-equal but distinct instances **fail** `BeSameAs`
     (proves `BeSameAs` ≠ `Be`)
   - full `Bad*`/`Not` set, `GoodNotBeSameAs` on two distinct instances

5. **Red.** `ObjectSatisfyTests.cs` and one test each confirming inherited `BeOfType` / `BeAssignableTo` /
   `BeOneOf` work through the object comparer (a single `GoodBeOfType` etc. — the base is already tested, this
   just confirms reachability).

6. **Green.** Implement the two comparer files and `ObjectShouldExtensions.cs`.

7. **The green-but-wrong check.** Run the whole suite. Then specifically confirm: a `List<int>` variable
   still binds to `CollectionComparer<int>` and a `string` still binds to the string comparer, **with the
   object overload now present**. If either moved, the concrete-shape overloads are not winning and the phase
   has failed its central obligation — stop, do not commit.

Explicit fixtures; one class per file for every fixture type.

---

## Files

**Added**

- `FatCat.Testing/Objects/ObjectComparer.cs`
- `FatCat.Testing/Objects/NotObjectComparer.cs`
- `FatCat.Testing/ObjectShouldExtensions.cs`
- `Tests.FatCat.Testing/Objects/ObjectBeTests.cs`
- `Tests.FatCat.Testing/Objects/ObjectBeNullTests.cs`
- `Tests.FatCat.Testing/Objects/ObjectBeSameAsTests.cs`
- `Tests.FatCat.Testing/Objects/ObjectSatisfyTests.cs`
- `Tests.FatCat.Testing/Objects/ObjectInheritedAssertionsTests.cs`
- Fixture types under `Tests.FatCat.Testing/Objects/` — one class per file

**Changed**

- `Tests.FatCat.Testing/Collections/CollectionOverloadResolutionTests.cs` — appended, not replaced
- `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `### Objects`** — replace the phase-06 placeholder. Table of `Be`, `BeNull`, `BeSameAs`,
`Satisfy`, and the inherited `BeOfType`/`BeAssignableTo`/`BeOneOf`. State plainly: `Be` uses `Equals`
(value), `BeSameAs` uses reference identity, and structural comparison is `BeEquivalentTo` (phase 07). Note
that a bare `null` literal cannot receive `Should()` — the variable must be typed (`Dto d = null;`).

**`MIGRATION.md` → `## 3. Mapping Table`** — flip to `✅ supported`:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().Be(obj)` (reference type) | `.Should().Be(obj)` | `Tests.FatCat.Testing.Objects.ObjectBeTests` |
| `.Should().NotBe(obj)` | `.Should().Not.Be(obj)` | `Tests.FatCat.Testing.Objects.ObjectBeTests` |
| `.Should().BeNull()` (reference type) | `.Should().BeNull()` | `Tests.FatCat.Testing.Objects.ObjectBeNullTests` |
| `.Should().NotBeNull()` | `.Should().Not.BeNull()` | `Tests.FatCat.Testing.Objects.ObjectBeNullTests` |
| `.Should().BeSameAs(obj)` | `.Should().BeSameAs(obj)` | `Tests.FatCat.Testing.Objects.ObjectBeSameAsTests` |
| `.Should().NotBeSameAs(obj)` | `.Should().Not.BeSameAs(obj)` | `Tests.FatCat.Testing.Objects.ObjectBeSameAsTests` |

**`MIGRATION.md` → `## 4. Type Coverage`** — `object` / reference types move to supported for
`Be`/`BeNull`/`BeSameAs`; note `BeEquivalentTo` on objects still pending phase 07.

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

**High-risk gate** — run and paste both into the report:

```pwsh
dotnet test Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~OverloadResolution"
```

Standards review, then **stop for human review before committing**.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `ObjectComparer<T>` / `NotObjectComparer<T>` constrained `where T : class`; `ObjectComparer` exposes
      `Not`.
- [ ] Object `Should<T>` lives in a **separate** static class (`ObjectShouldExtensions`), not
      `ShouldExtensions`, to avoid `CS0111`.
- [ ] `Be` (Equals), `BeNull`, `BeSameAs` (ReferenceEquals) implemented with full test sets and `Not`
      equivalents; `Satisfy` and inherited `BeOfType`/`BeAssignableTo`/`BeOneOf` confirmed reachable.
- [ ] A distinct-but-`Equals`-equal pair passes `Be` and fails `BeSameAs` — both pinned (proves ADR-5).
- [ ] `CollectionOverloadResolutionTests` **extended** (not duplicated) with `Dto` → object and
      interface-typed → object, and every prior case re-asserted.
- [ ] With the object overload present, `List<T>`/`T[]`/`IEnumerable<T>` still bind to `CollectionComparer`
      and `string`/`int`/enum are unchanged. Explicitly re-checked; result in the report.
- [ ] Subjects rendered via `ValueFormatter`.
- [ ] No new compiler warnings. New files carry no `#nullable enable`.
- [ ] No banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README `### Objects` and the six MIGRATION rows written.
- [ ] Standards review clean; high-risk human review done.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/06-object-comparer.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-06-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Removes the object overload and comparer; the appended overload-resolution cases revert with it. **Phases
07–10 build on the object comparer** — revert them first. **Reverting this phase is also the required first
step before reverting phase 04** (ADR-4): with the object overload gone, phase 04's collection overloads are
safe to remove; with it present and phase 04 gone, collections mis-bind.

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing;
ObjectComparer<T> Should<T>(this T subject) where T : class   // in ObjectShouldExtensions

namespace FatCat.Testing.Objects;
ObjectComparer<T>.Be(T) / BeNull() / BeSameAs(T) / Satisfy(Action<T>) / Not   // + inherited base assertions
NotObjectComparer<T>.Be(T) / BeNull() / BeSameAs(T)
```

For **phase 07 (BeEquivalentTo)**: `ObjectComparer<T>` is where `BeEquivalentTo(T expected, ...)` lands, and
`NotObjectComparer<T>` gets the negation. The engine it calls is new; the comparer method is a thin entry
point. Phase 07 must reconcile its member-selection rule with `ValueFormatter`'s object-dump rule (phase 02
hand-off) — they should agree on "a type's members".

For **phases 08/09/10**: the object entry point and the `where T : class` constraint are the foundation.
Anything reference-typed now has a `Should()`.

**The overload-resolution contract is now fixed across phases 04 and 06.** Any future phase adding a
`Should` overload extends `CollectionOverloadResolutionTests` and proves nothing moved.
