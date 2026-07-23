# Phase 16 — G26 completeness: Numerics

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **low.** Additive method pairs on the existing numeric/double/float comparers.
- **Depends on:** 01
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/Numbers/` has `NumericComparer<T>` (`where T : INumber<T>`), `NotNumericComparer<T>`,
`NullableIntComparer`, `NotNullableIntComparer`. `Doubles/` and `Floats/` have their own comparer pairs.
Existing numeric assertions include `Be`, `BeGreaterThan`, `BeLessThan`, `BeInRange`, `BeAround`,
`BeNegative`, `BePositive`, `BeZero`, `Match` (predicate), and — on doubles/floats — `BeApproximately`.

**`BeGreaterThanOrEqualTo` / `BeLessThanOrEqualTo` (G8) belong to `tier_2_gaps` phases 02/03**, a different
plan. Do **not** ship them here. This phase ships only the G26 numeric methods `gaps.md` §6.3 names:
`NotBeInRange`, `NotBeApproximately`.

Read before starting: [00-overview.md](00-overview.md), `gaps.md` §6.3 (Numerics row),
`FatCat.Testing/Numbers/NumericComparer.cs` and `NotNumericComparer.cs`, `Doubles/DoubleComparer.cs`,
`Floats/FloatComparer.cs`, `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`.

---

## Scope

**In scope**

- `NotBeInRange(T low, T high)` — on `NumericComparer<T>` (via `Not`), `DoubleComparer`, `FloatComparer`, and
  the nullable int comparer. Fails when the subject **is** within `[low, high]`.
- `NotBeApproximately(value, tolerance)` — on `DoubleComparer` and `FloatComparer` (via `Not`). Fails when
  the subject **is** within `tolerance` of `value`. (Numerics have no `BeApproximately`, so no
  `NotBeApproximately` there.)

**Out of scope**

- `BeGreaterThanOrEqualTo` / `BeLessThanOrEqualTo` (G8, `tier_2_gaps/02` and `/03`).
- Any non-numeric family.

`NotBeInRange` — since `BeInRange` is on the **positive** comparer, its negation lives on the **negated**
comparer as `BeInRange`... but the FluentAssertions name is `NotBeInRange`, and under ADR-A the consumer
writes `.Not.BeInRange(low, high)`. So the method to add is `BeInRange` on `NotNumericComparer<T>` /
`NotDoubleComparer` / `NotFloatComparer` (fails when in range). **Check whether `NotNumericComparer` already
has `BeInRange`** — if it does, this half is done and the phase is smaller; record that.

Likewise `NotBeApproximately` → `BeApproximately` on the negated double/float comparers.

---

## Design

| Consumer writes | Method added | Comparer | Fails when | Message |
|---|---|---|---|---|
| `.Not.BeInRange(lo, hi)` | `BeInRange(lo, hi)` | `NotNumericComparer<T>` etc. | `lo <= Subject <= hi` | `{Subject} should not be in range {lo} to {hi}` |
| `.Not.BeApproximately(v, t)` | `BeApproximately(v, t)` | `NotDoubleComparer`, `NotFloatComparer` | `abs(Subject - v) <= t` | `{Subject} should not be approximately {v} (± {t})` |

Match the message wording of the existing positive `BeInRange` / `BeApproximately`, negated. Mirror null
handling on the nullable int comparer from its existing `BeInRange`.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Numbers/`, `/Doubles/`, `/Floats/`, one class per
(method, comparer-family).

1. `IntNotBeInRangeTests.cs` — via `.Not.BeInRange`: passes when out of range, fails when in range (including
   the boundary values `lo` and `hi`, which are **in** range and therefore fail); full set.
2. `NullableIntNotBeInRangeTests.cs` — same, plus null-subject case.
3. `DoubleNotBeInRangeTests.cs`, `FloatNotBeInRangeTests.cs`.
4. `DoubleNotBeApproximatelyTests.cs`, `FloatNotBeApproximatelyTests.cs` — passes when outside tolerance,
   fails when inside; full set.
5. **Green.** Implement on the negated comparers (and nullable int).
6. Run the whole suite.

Explicit literals — the values appear in the messages.

---

## Files

**Changed**

- `FatCat.Testing/Numbers/NotNumericComparer.cs`, `NotNullableIntComparer.cs`
- `FatCat.Testing/Doubles/NotDoubleComparer.cs`, `FatCat.Testing/Floats/NotFloatComparer.cs`
- `README.md`, `MIGRATION.md`

**Added**

- One test class per (method, family) under the matching test folders

---

## Documentation Updates

**`README.md` → `### Numbers`** and `### Doubles And Floats` — note `BeInRange` and `BeApproximately` are
negatable via `.Not.`.

**`MIGRATION.md` → `## 3. Mapping Table`** — coverage rows:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().NotBeInRange(lo, hi)` | `.Should().Not.BeInRange(lo, hi)` | `Tests.FatCat.Testing.Numbers.IntNotBeInRangeTests` |
| `.Should().NotBeApproximately(v, t)` | `.Should().Not.BeApproximately(v, t)` | `Tests.FatCat.Testing.Doubles.DoubleNotBeApproximatelyTests` |

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~NotBeInRange|FullyQualifiedName~NotBeApproximately"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `.Not.BeInRange` on numeric, nullable-int, double, float; `.Not.BeApproximately` on double, float —
      full sets, boundary cases, null-subject where applicable.
- [ ] `BeGreaterThanOrEqualTo`/`BeLessThanOrEqualTo` were **not** added (G8, `tier_2_gaps`).
- [ ] Pre-existing negated methods checked; any already-present ones recorded rather than duplicated.
- [ ] No new warnings; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/16-g26-numerics.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-16-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (revert phase 20 first if landed).

---

## Hand-off

Numeric G26 methods shipped. G8 remains `tier_2_gaps`'s to deliver. Phase 20 counts this toward the audit.
