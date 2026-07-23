# Phase 05 — Collection assertions, remainder (G4)

- **Work item:** `final_gaps`
- **Gap:** **G4** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **low.** Purely additive methods on the two comparer files phase 04 created. No new entry point,
  no overload resolution, no new type.
- **Depends on:** 04
- **Depended on by:** 08, 13
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Phase 04 created `FatCat.Testing/Collections/CollectionComparer.cs` and `NotCollectionComparer.cs`, the three
`Should<T>` entry points, and the core assertions (`Contain`, `BeEmpty`, `NotBeEmpty`, `HaveCount`,
`ContainSingle`). It also established two contracts you must reuse, not re-invent:

- a private snapshot field `items` (the subject `.ToList()`'d once, or `null` if the subject was null), read
  by every assertion;
- failure messages built through `ValueFormatter.Format` (phase 02).

**Read `CollectionComparer.cs` and `NotCollectionComparer.cs` before writing anything.** Mirror their null
handling and message style exactly — a null subject must behave consistently across every method in the file.

This phase adds the remaining G4 assertions the consumer inventory names. It does **not** add
`ContainEquivalentOf` (phase 08 — needs the equivalency engine) or the G26 completeness extras (phase 13).

Read before starting: [00-overview.md](00-overview.md) (**ADR-12**, order-insensitivity;
**ADR-13**, formatting), phase 04's hand-off, `naming-and-structure.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** — on the existing collection comparers:

- `Equal(IEnumerable<T> expected)` — order-**sensitive**, element-by-element equality (ADR-12: this is the
  ordered assertion; `BeEquivalentTo` in phase 08 is the unordered one)
- `OnlyContain(Func<T, bool> predicate)` — every element satisfies the predicate
- `OnlyHaveUniqueItems()` — no duplicates
- `ContainInOrder(params T[] expected)` — the expected elements appear in this relative order (not
  necessarily contiguous)
- `BeInDescendingOrder()` — using `Comparer<T>.Default`
- `ContainSingle(Func<T, bool> predicate)` — exactly one element matches

**Out of scope**

- `ContainEquivalentOf`, `NotContainEquivalentOf` — **phase 08.**
- Everything under G26 collections (phase 13): `BeSubsetOf`, `IntersectWith`, `AllSatisfy`, `AllBeOfType`,
  `SatisfyRespectively`, `HaveCountGreaterThan`/`LessThan`, `HaveSameCount`, `ContainNulls`, `ContainMatch`,
  `HaveElementAt`, `BeInAscendingOrder`. Do not pull them forward.
- Any dictionary work (`tier_2_gaps/13`).

---

## Design

All methods return the comparer (`this`) so calls chain, and take a trailing `string because = null`. Format
subjects and expected values with `ValueFormatter.Format`.

| Method | Fails when | Message |
|---|---|---|
| `Equal(expected)` | null subject, or lengths differ, or any position differs by `Equals` | `{items} should equal {expected}` |
| `OnlyContain(predicate)` | null subject, or any element fails the predicate | `{items} should only contain elements matching the predicate` |
| `OnlyHaveUniqueItems()` | null subject, or a duplicate exists | `{items} should only have unique items` |
| `ContainInOrder(expected)` | null subject, or the subsequence is not present in order | `{items} should contain {expected} in order` |
| `BeInDescendingOrder()` | null subject, or an element is less than a later one | `{items} should be in descending order` |
| `ContainSingle(predicate)` | null subject, or the match count `!= 1` | `{items} should contain a single element matching the predicate but {count} matched` |

Negated forms on `NotCollectionComparer<T>` — provide the ones a consumer would write through `.Not.`:
`NotContainInOrder` is not a thing; the only negations in the inventory are `NotContainEquivalentOf` (phase
08) and generic `NotContain`/`NotBeEmpty`/`NotHaveCount` (phase 04). So this phase adds **positive-only**
methods, plus `Not.OnlyHaveUniqueItems` and `Not.OnlyContain` only if the mirror reads naturally — if it
does not, skip it and note the omission in the report rather than inventing a message no one will read.

`OnlyContain`/`ContainSingle` take a `Func<T, bool>`. The generated message cannot describe the predicate,
so it says "matching the predicate". `because` is how a caller makes it specific — document that in
`README.md`.

`Equal` accepts `IEnumerable<T>`; snapshot the expected argument with `.ToList()` before comparing, same as
the subject, so a lazy expected argument is safe too.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Collections/`, one class per method, full
`Good`/`Bad`/`BadShowsCorrectMessage`/`BadWithBecause` set (plus `Not` where the method has a negated form).

1. `CollectionEqualTests.cs` — include `BadEqualWhenReorderedShowsItIsOrderSensitive`: `[1,2,3].Equal([3,2,1])`
   **fails**, proving `Equal` is ordered (the counterpart to phase 08's unordered `BeEquivalentTo`).
2. `CollectionOnlyContainTests.cs`
3. `CollectionOnlyHaveUniqueItemsTests.cs`
4. `CollectionContainInOrderTests.cs` — include a case where the elements are present but out of order
   (fails) and one where they are present in order but non-contiguous (passes).
5. `CollectionBeInDescendingOrderTests.cs`
6. `CollectionContainSinglePredicateTests.cs` — distinct from phase 04's parameterless `ContainSingle`.
7. **Green.** Implement across the two comparer files.
8. Run the whole suite — phase 04's tests and everything else stay green.

Explicit literals throughout; the contents appear in the messages.

---

## Files

**Changed**

- `FatCat.Testing/Collections/CollectionComparer.cs`
- `FatCat.Testing/Collections/NotCollectionComparer.cs` (only if a natural negation is added)
- `README.md`, `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Collections/CollectionEqualTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionOnlyContainTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionOnlyHaveUniqueItemsTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionContainInOrderTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionBeInDescendingOrderTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionContainSinglePredicateTests.cs`

---

## Documentation Updates

**`README.md` → `### Collections`** — append the six methods to the existing table. Note that `Equal` is
order-sensitive and `BeEquivalentTo` (phase 08) is not, and that predicate-based methods describe the
predicate only through `because`.

**`MIGRATION.md` → `## 3. Mapping Table`** — flip to `✅ supported`:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().Equal(x)` | `.Should().Equal(x)` | `Tests.FatCat.Testing.Collections.CollectionEqualTests` |
| `.Should().OnlyContain(p)` | `.Should().OnlyContain(p)` | `Tests.FatCat.Testing.Collections.CollectionOnlyContainTests` |
| `.Should().OnlyHaveUniqueItems()` | same | `Tests.FatCat.Testing.Collections.CollectionOnlyHaveUniqueItemsTests` |
| `.Should().ContainInOrder(x)` | same | `Tests.FatCat.Testing.Collections.CollectionContainInOrderTests` |
| `.Should().BeInDescendingOrder()` | same | `Tests.FatCat.Testing.Collections.CollectionBeInDescendingOrderTests` |
| `.Should().ContainSingle(p)` | same | `Tests.FatCat.Testing.Collections.CollectionContainSinglePredicateTests` |

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Collections"
```

Then run the standards review and resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] Six methods implemented on the phase-04 comparers, reusing the `items` snapshot and `ValueFormatter`.
- [ ] Each has the full `Good`/`Bad`/`BadShowsCorrectMessage`/`BadWithBecause` set plus null-subject case.
- [ ] `Equal` proven order-sensitive by a reordered-input test.
- [ ] `ContainInOrder` proven to accept non-contiguous-in-order and reject out-of-order.
- [ ] No new entry point, no new comparer type, no `#nullable enable` added.
- [ ] No banned patterns.
- [ ] `dotnet test` green; total is baseline + new facts.
- [ ] `dotnet format` / `csharpier` run in order, csharpier last.
- [ ] README and the six MIGRATION rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/05-collection-assertions-remainder.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-05-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Additive only. **Phase 08 adds `ContainEquivalentOf` to the same files and phase 13 adds the G26 extras** —
revert those first if they landed. Otherwise this reverts alone.

---

## Hand-off

Public surface added on `CollectionComparer<T>`: `Equal`, `OnlyContain`, `OnlyHaveUniqueItems`,
`ContainInOrder`, `BeInDescendingOrder`, `ContainSingle(predicate)`.

For **phase 08**: `Equal` is the order-sensitive baseline; `BeEquivalentTo` must be visibly different
(order-insensitive, structural per element). For **phase 13**: the G26 collection extras extend these same
two files; the `items` snapshot and null convention remain the contract.
