# Phase 04 — G4: collection assertions

- **Gap:** G4 (`gaps.md` §3). ~270 call sites across `Contain`, `BeEmpty`, `HaveCount`, `NotContain`,
  `ContainSingle`, `ContainEquivalentOf`, `NotBeEmpty`, `OnlyContain`, `Equal`, `OnlyHaveUniqueItems`,
  `NotContainEquivalentOf`, `BeInDescendingOrder`.
- **Depends on:** Phase 01 (formatting), Phase 02 (G1 — overload interplay with `Should(this object)`),
  Phase 03 (G3 — `ContainEquivalentOf` / collection `BeEquivalentTo` reuse the equivalency engine).
- **Depended on by:** Phase 05, Phase 07.
- **Risk:** **HIGH** — public API contract, and the overload collision between `IEnumerable<T>`, `string`
  (which is `IEnumerable<char>`), and G1's `object` overload is exactly the trap `gaps.md` flags. Extra
  human review.

## Context (complete handoff)

There is no `Collections/` folder. Per ADR-002 (`00-overview.md`), collections enter via
`Should<T>(this IEnumerable<T> subject)` returning `CollectionComparer<T>`. This generic is over
`IEnumerable<T>`, not `T`, so it does not collide with the enum generic or the object overload — **but the
binding of `string`, plain objects, and actual collections must be pinned by tests, not assumed.**

## Deliverable

New folder `FatCat.Testing/Collections/`:

- `CollectionComparer<T>` : `ComparerBase<IEnumerable<T>, CollectionComparer<T>>`, `Not` →
  `NotCollectionComparer<T>`.
- `NotCollectionComparer<T>` : `NotComparerBase<...>`.

Assertions (return `this`, trailing `string because = null`, format via `ValueFormatter.Format`). Map each to
the call-site inventory; the negated forms are reached through `.Not.` (ADR-003):

| Method | Passes when | Notes |
|---|---|---|
| `Contain(T item)` | sequence contains `item` (`Equals`) | `NotContain` = `Not.Contain` (12 sites) |
| `BeEmpty()` | no elements | `NotBeEmpty` = `Not.BeEmpty` (9) |
| `HaveCount(int n)` | element count == n | 16 sites |
| `ContainSingle()` | exactly one element | 11 sites — returns `this` (no `.Which` drill-down; G14 deferred) |
| `ContainEquivalentOf(T item)` | some element is **structurally equivalent** (G3 engine) | 12 sites; `Not.` form 1 |
| `OnlyContain(Func<T,bool>)` | every element matches predicate | 5 sites |
| `Equal(IEnumerable<T>)` | **order-sensitive** element-wise equality | 4 sites — distinct from `BeEquivalentTo` |
| `BeEquivalentTo(IEnumerable<T>)` | **order-insensitive** multiset equivalence (G3 engine) | see OQ-3 |
| `OnlyHaveUniqueItems()` | no duplicates | 2 sites |
| `BeInDescendingOrder()` | sorted descending | 1 site |

`NotCollectionComparer<T>` implements the negated cases the inventory needs (`Contain`, `BeEmpty`,
`ContainEquivalentOf`, `BeEquivalentTo`).

### Overload interplay — pin with tests (this is where G4 breaks)
- `"text".Should()` → `NullableStringComparer` (string overload strictly more specific than
  `IEnumerable<char>`). **Regression test required.**
- `new int[]{1,2,3}.Should()` / `new List<string>().Should()` → `CollectionComparer<T>`.
- `new SomeDto().Should()` (not enumerable) → `ObjectComparer` (Phase 02), not collection.
- A DTO that implements `IEnumerable<T>` binds to `CollectionComparer<T>` — intended; note it.

### OQ-3 to resolve here
`BeEquivalentTo` (collections) is order-insensitive (multiset), `Equal` is order-sensitive. Confirm against
the 4 `Equal` sites before finalising. Assumed as stated.

## TDD — tests first

`Tests.FatCat.Testing/Collections/` mirrors source. One class per assertion method, full
`Good/Bad/BadShowsCorrectMessage/BadWithBecause` + `Not` set, no underscores. Use **explicit literal
collections** so the value lines up with the pinned message (`testing.md` Test Data). Plus:

- Overload-resolution guard tests for the four binding cases above (string must NOT become a collection).
- `ContainEquivalentOf` reuses the G3 engine — a test with structurally-equal-but-not-reference-equal
  elements passes.
- Migration-proof rewrites: `Not.Contain(x)`, `Not.BeEmpty()`, `Not.ContainEquivalentOf(x)`.

## Migration obligation

Append to `MIGRATION.md`: `NotContain` → `Not.Contain(x)` (12), `NotBeEmpty` → `Not.BeEmpty()` (9),
`NotContainEquivalentOf` → `Not.ContainEquivalentOf(x)` (1), plus the positive collection methods
(`Contain`, `BeEmpty`, `HaveCount`, `ContainSingle`, `ContainEquivalentOf`, `OnlyContain`, `Equal`,
`OnlyHaveUniqueItems`, `BeInDescendingOrder`).

## Verification

`dotnet build`, `dotnet test`, `dotnet format style --verify-no-changes`, `dotnet csharpier .` from repo root.

## Definition of Done

- [ ] ADR-002 honoured: `Should<T>(this IEnumerable<T>)`; four overload guard tests green (string stays a
      string).
- [ ] All ten assertion methods + needed `Not` forms, full test set each, green.
- [ ] `ContainEquivalentOf` / collection `BeEquivalentTo` reuse the Phase 03 engine (no reimplementation).
- [ ] OQ-3 resolved and recorded.
- [ ] `MIGRATION.md` appended with G4 rows.
- [ ] All `00-overview.md` DoD gates met; one commit `[tier1_gaps 04] G4 collection assertions`.

## Rollback Procedure

`git revert <phase-04-commit>`. Manual: remove the G4 section from `MIGRATION.md` if orphaned. Reverting 04
**requires reverting 05 first** (05 documents the collection comparer as part of the extension-point surface).

## Hand-off (contract exposed to later phases)

- `FatCat.Testing.Collections.CollectionComparer<T>` / `NotCollectionComparer<T>` and
  `ShouldExtensions.Should<T>(this IEnumerable<T>) : CollectionComparer<T>` — the collection entry point and
  comparers.
- Confirms the final overload set on `ShouldExtensions` (object + collection + existing 27). Phase 05's
  extension-point docs describe this complete surface.
