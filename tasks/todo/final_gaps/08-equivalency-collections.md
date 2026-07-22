# Phase 08 — Equivalency for collections (G3)

- **Work item:** `final_gaps`
- **Gap:** **G3** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **medium.** Reuses phase 07's engine; the new work is order-insensitive matching and the two
  collection entry points. The surprising default (order-insensitivity) is the thing to get right and
  document.
- **Depends on:** 05 (collection comparers), 07 (equivalency engine)
- **Depended on by:** —
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Phase 07 built `EquivalencyComparer.Compare(object, object)` (in `FatCat.Testing/Equivalency/`), which does
recursive structural comparison of objects and reports the differing member. Phase 05 built the collection
comparers with an `items` snapshot and `ValueFormatter` messages. This phase joins them: `BeEquivalentTo` and
`ContainEquivalentOf` on collections.

**Read phase 07's hand-off first.** It states whether phase 07 already wrote the order-insensitive matching
helper or left a simple ordered walk for this phase to replace. Follow whichever it says — do not assume.

**ADR-12 — collection equivalency is order-insensitive by default.** `[1,2,3]` is equivalent to `[3,2,1]`.
This matches FluentAssertions and it is the single most surprising default in the library. There is no
`WithStrictOrdering` opt-out (zero usages, ADR-6). The order-**sensitive** assertion is `Equal` (phase 05).

Read before starting: [00-overview.md](00-overview.md) (**ADR-12**, **ADR-13**), phase 05 and phase 07
hand-offs, `.claude/rules/csharp/naming-and-structure.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope**

- `CollectionComparer<T>.BeEquivalentTo(IEnumerable<T> expected, string because = null)` — order-insensitive
  structural equivalency of two collections.
- `NotCollectionComparer<T>.BeEquivalentTo(...)`.
- `CollectionComparer<T>.ContainEquivalentOf(T expected, string because = null)` — the collection contains an
  element structurally equivalent to `expected`.
- `NotCollectionComparer<T>.ContainEquivalentOf(...)` — the 1 `NotContainEquivalentOf` site.

**Out of scope**

- Object `BeEquivalentTo` — shipped in phase 07.
- The configuration hook — **phase 09.** This phase's matching calls `EquivalencyComparer.Compare` as it
  stands; when phase 09 fills the registry, this code picks it up for free (the registry lives inside the
  engine).
- `AllBeEquivalentTo`, `SatisfyRespectively` — G26 collections, phase 13.

---

## Design

### Order-insensitive matching (`BeEquivalentTo`)

Two collections are equivalent when there is a one-to-one pairing such that each pair is equivalent by
`EquivalencyComparer.Compare`. Greedy matching is sufficient and is FluentAssertions' approach:

1. Lengths differ → not equivalent; message states the counts.
2. Copy expected into a mutable list. For each actual element, find and **remove** the first expected element
   it is `Compare`-equivalent to. No match → not equivalent; the message names the actual element that had
   no partner.
3. All matched and expected list emptied → equivalent.

Greedy (rather than full bipartite matching) can theoretically mis-pair when elements are ambiguously
equivalent; FluentAssertions accepts that, no consumer hits it, and the alternative is a Hopcroft–Karp
matcher that is over-engineering here (`not-allowed.md`). **Document the greedy limitation** in `README.md`.

Cap the message at the 32-element formatter limit (ADR-13) — do not dump a 10,000-element mismatch.

### `ContainEquivalentOf`

The collection contains at least one element for which `Compare(element, expected).AreEquivalent`. Message:
`{items} should contain an element equivalent to {expected}`. Negated: fails when one *is* found.

### Messages (via `ValueFormatter`)

| Method | Fails when | Message |
|---|---|---|
| `BeEquivalentTo(expected)` | null subject, count mismatch, or no full pairing | `{items} should be equivalent to {expected}` (+ the unmatched element when known) |
| `ContainEquivalentOf(expected)` | null subject, or no equivalent element | `{items} should contain an element equivalent to {expected}` |
| `Not.BeEquivalentTo(expected)` | they are equivalent | `{items} should not be equivalent to {expected}` |
| `Not.ContainEquivalentOf(expected)` | an equivalent element exists | `{items} should not contain an element equivalent to {expected}` |

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Collections/`.

1. **Red.** `CollectionBeEquivalentToTests.cs`:
   - `GoodBeEquivalentToSameOrder`
   - `GoodBeEquivalentToDifferentOrder` — `[1,2,3].BeEquivalentTo([3,2,1])` **passes** (ADR-12, the headline)
   - `GoodBeEquivalentToStructuralElements` — `List<Dto>` compared structurally without `Equals` overrides
   - `BadBeEquivalentToCountMismatch`
   - `BadBeEquivalentToElementMissing` / `...ShowsCorrectMessage` — names the unmatched element
   - `BadBeEquivalentToWithBecause`
   - `GoodNotBeEquivalentTo` / `BadNotBeEquivalentTo` / `...ShowsCorrectMessage` / `...WithBecause`
   - `BadBeEquivalentToOnNull`

2. **Red.** `CollectionContainEquivalentOfTests.cs` — full set, including a structural (non-`Equals`) element
   match, plus the `Not` form and null-subject case.

3. **Green.** Add the four methods across the two collection comparer files, calling into
   `EquivalencyComparer`.

4. Run the whole suite. Phase 05's `Equal` tests (order-sensitive) must still pass — the contrast between
   `Equal` (ordered) and `BeEquivalentTo` (unordered) is now both live and tested.

---

## Files

**Changed**

- `FatCat.Testing/Collections/CollectionComparer.cs`
- `FatCat.Testing/Collections/NotCollectionComparer.cs`
- `README.md`, `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Collections/CollectionBeEquivalentToTests.cs`
- `Tests.FatCat.Testing/Collections/CollectionContainEquivalentOfTests.cs`
- Any new fixture types — one class per file

---

## Documentation Updates

**`README.md` → `### Collections`** — append `BeEquivalentTo` and `ContainEquivalentOf`. State prominently:
collection `BeEquivalentTo` **ignores order** (contrast `Equal`), compares elements structurally (reusing the
object engine), matching is greedy, and output is capped at 32 elements.

**`MIGRATION.md` → `## 3. Mapping Table`** — flip:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().BeEquivalentTo(coll)` (collection) | `.Should().BeEquivalentTo(coll)` | `Tests.FatCat.Testing.Collections.CollectionBeEquivalentToTests` |
| `.Should().ContainEquivalentOf(x)` | `.Should().ContainEquivalentOf(x)` | `Tests.FatCat.Testing.Collections.CollectionContainEquivalentOfTests` |
| `.Should().NotContainEquivalentOf(x)` | `.Should().Not.ContainEquivalentOf(x)` | `Tests.FatCat.Testing.Collections.CollectionContainEquivalentOfTests` |

**`MIGRATION.md` → `## 5. Behavioural Differences`** — confirm the order-insensitive row seeded in phase 01
is now backed by a shipped assertion; add that matching is greedy.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Equivalent"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `BeEquivalentTo` on collections is order-insensitive — `[1,2,3]` ≡ `[3,2,1]` pinned by a test.
- [ ] Elements compared structurally via `EquivalencyComparer.Compare` (no `Equals` override required),
      pinned by a `List<Dto>` test.
- [ ] `ContainEquivalentOf` and both `Not` forms implemented, full sets, null-subject cases.
- [ ] Phase 05's order-sensitive `Equal` still passes — the ordered/unordered contrast is live.
- [ ] Messages via `ValueFormatter`, capped at 32 elements.
- [ ] No new warnings; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + three mapping rows + behavioural-difference note written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/08-equivalency-collections.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-08-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Nothing depends on this phase. Reverts alone. (It uses phase 07's engine but does not modify it.)

---

## Hand-off

Public surface added on the collection comparers: `BeEquivalentTo(IEnumerable<T>)`, `ContainEquivalentOf(T)`,
and their `Not` forms. G3 is now complete for both objects (phase 07) and collections (this phase); phase 09
adds the one configuration hook.

For **phase 13** (G26 collections): `AllBeEquivalentTo` and `SatisfyRespectively` extend these files and reuse
`EquivalencyComparer`. The greedy-matching and 32-cap conventions established here are the contract.
