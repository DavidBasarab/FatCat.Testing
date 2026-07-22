# Phase 13 — G26 completeness: Collections

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **low.** Additive methods on the collection comparers from phases 04/05/08. No new entry point,
  no overload resolution.
- **Depends on:** 05 (collection comparers), and for the two equivalency-based methods, 08
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

G26 collection completeness — the FluentAssertions collection methods no consumer call site uses but a
third-party replacement should have. The collection comparers already exist:
`FatCat.Testing/Collections/CollectionComparer.cs` and `NotCollectionComparer.cs`, with an `items` snapshot
field, `ValueFormatter` messages (phase 02), and — after phase 08 — access to `EquivalencyComparer`.

**Read those two comparer files and phases 05 and 08's hand-offs before writing.** Reuse `items` and the
formatter; mirror the null handling.

Read before starting: [00-overview.md](00-overview.md) (**ADR-12**, **ADR-13**), `gaps.md` §6.3 (Collections
row), `.claude/rules/csharp/naming-and-structure.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** — on the collection comparers (`gaps.md` §6.3 Collections, minus what phases 04/05/08 shipped):

- `BeSubsetOf(IEnumerable<T> superset)`
- `IntersectWith(IEnumerable<T> other)` — the two share at least one element
- `AllSatisfy(Action<T> inspector)` — every element passes the inspector without throwing
- `AllBeOfType<TExpected>()` / `AllBeOfType(Type)`
- `SatisfyRespectively(params Action<T>[] inspectors)` — element _i_ passes inspector _i_; counts must match
- `HaveCountGreaterThan(int)` / `HaveCountLessThan(int)`
- `HaveSameCount(IEnumerable<T> other)`
- `ContainNulls()` / (via `Not`) not contain nulls
- `ContainMatch(string wildcard)` — for `IEnumerable<string>`; at least one element matches the wildcard
- `HaveElementAt(int index, T expected)`
- `BeInAscendingOrder()` — the ascending counterpart to phase 05's `BeInDescendingOrder`
- `AllBeEquivalentTo(T expected)` — every element equivalent to `expected` (reuses phase 08's engine)

**Out of scope**

- Anything shipped in 04/05/08: `Contain`, `BeEmpty`, `HaveCount`, `ContainSingle`, `Equal`, `OnlyContain`,
  `OnlyHaveUniqueItems`, `ContainInOrder`, `BeInDescendingOrder`, `BeEquivalentTo`, `ContainEquivalentOf`.
- Dictionaries (`tier_2_gaps/13`, a different plan).
- `ContainInConsecutiveOrder`, `HaveElementPreceding`/`Succeeding`, `ContainItemsAssignableTo` — deep-cut
  methods with no consumer and marginal value; note them as a possible future top-up, do not build. Record
  the deliberate omission in the report so the close-out audit (phase 20) does not read the family as
  complete when it is not.

`ContainMatch` reuses the string wildcard helper (`FatCat.Testing/Strings/StringEqualityHelper` —
`MatchesWildcard`). Do not reimplement wildcard matching.

---

## Design

Standard additive methods returning `this`, `string because = null`, subjects/expected via `ValueFormatter`.
Mirror null handling to the existing methods. Equality uses `EqualityComparer<T>.Default` except
`AllBeEquivalentTo` (structural, via `EquivalencyComparer`).

| Method | Fails when | Message |
|---|---|---|
| `BeSubsetOf(super)` | null subject, or an element is not in `super` | `{items} should be a subset of {super}` |
| `IntersectWith(other)` | no shared element | `{items} should intersect with {other}` |
| `AllSatisfy(inspector)` | any element throws in the inspector | `{items} should have every element satisfy the inspector but element at index {i} did not` |
| `AllBeOfType<T>()` | any element is not exactly `T` | `{items} should have all elements of type {T}` |
| `SatisfyRespectively(inspectors)` | count mismatch, or element _i_ throws | `{items} should satisfy {n} inspectors respectively but ...` |
| `HaveCountGreaterThan(n)` | `Count <= n` | `{items} should have count greater than {n} but has {count}` |
| `HaveCountLessThan(n)` | `Count >= n` | `{items} should have count less than {n} but has {count}` |
| `HaveSameCount(other)` | counts differ | `{items} should have the same count as {other} ({otherCount}) but has {count}` |
| `ContainNulls()` | no null element | `{items} should contain nulls` |
| `ContainMatch(wildcard)` | no element matches | `{items} should contain a match for {wildcard}` |
| `HaveElementAt(i, expected)` | out of range, or element ≠ expected | `{items} should have {expected} at index {i}` |
| `BeInAscendingOrder()` | an element is greater than a later one | `{items} should be in ascending order` |
| `AllBeEquivalentTo(expected)` | any element not equivalent | `{items} should have all elements equivalent to {expected}` |

Negations on `NotCollectionComparer<T>` only where they read naturally (`Not.ContainNulls`,
`Not.IntersectWith`). Where a negation is meaningless, skip it and note the skip.

`AllBeOfType(Type)` — a `T[]` of `object` may hold mixed runtime types; that is the interesting case to test.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Collections/`, one class per method, full
`Good`/`Bad`/`BadShowsCorrectMessage`/`BadWithBecause` (+ `Not` where it exists), plus the null-subject case.

Write one test class per method (`CollectionBeSubsetOfTests`, `CollectionAllSatisfyTests`, …). For
`SatisfyRespectively`, include a count-mismatch case and an inspector-fails case. For `AllBeEquivalentTo`,
include a structural (non-`Equals`) element. For `BeInAscendingOrder`, mirror phase 05's descending tests.

Green: implement across the two comparer files. Run the whole suite.

---

## Files

**Changed**

- `FatCat.Testing/Collections/CollectionComparer.cs`, `NotCollectionComparer.cs`
- `README.md`, `MIGRATION.md`

**Added**

- One test class per method under `Tests.FatCat.Testing/Collections/`

---

## Documentation Updates

**`README.md` → `### Collections`** — append every shipped method. Note the deliberate omissions (the
deep-cut methods) so the catalog does not over-claim.

**`MIGRATION.md` → `## 3. Mapping Table`** — one `✅ supported` coverage row per method, each naming its test
class. Mark as coverage-only (no consumer call site).

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

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] Every in-scope method shipped with the full test set, `Not` where natural, and null-subject cases.
- [ ] `AllBeEquivalentTo` reuses `EquivalencyComparer`; `ContainMatch` reuses `StringEqualityHelper`.
- [ ] Deliberate omissions listed in the phase report.
- [ ] No new entry point, no `#nullable enable`, no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION coverage rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/13-g26-collections.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-13-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (revert phase 20 first if landed).

---

## Hand-off

Public surface added on the collection comparers per the table above. G26 collections closed except the
listed deep-cut omissions. Phase 20 counts it toward the replacement-claim audit and should surface the
omissions.
