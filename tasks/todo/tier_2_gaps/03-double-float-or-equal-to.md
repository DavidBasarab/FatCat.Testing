# Phase 03 — Numeric `BeGreaterThanOrEqualTo` / `BeLessThanOrEqualTo` (`Doubles/` + `Floats/`)

- **Work item:** `tier_2_gaps`
- **Gap:** **G8** (gaps.md §3, Tier 2)
- **Risk:** **low.** Purely additive; no existing signature, message, or behaviour changes. The one thing
  to get right is `NaN`, which fails every comparison in both directions.
- **Depends on:** 01
- **Depended on by:** 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/Doubles/` and `FatCat.Testing/Floats/` each hold two comparers:

| File | Type | Base |
|---|---|---|
| `Doubles/DoubleComparer.cs` | `DoubleComparer(double subject)` | `ComparerBase<double, DoubleComparer>` |
| `Doubles/NotDoubleComparer.cs` | `NotDoubleComparer(double subject)` | `NotComparerBase<double, NotDoubleComparer>` |
| `Floats/FloatComparer.cs` | `FloatComparer(float subject)` | `ComparerBase<float, FloatComparer>` |
| `Floats/NotFloatComparer.cs` | `NotFloatComparer(float subject)` | `NotComparerBase<float, NotFloatComparer>` |

Each has `BeApproximately`, `BeGreaterThan`, `BeInRange`, `BeLessThan`, `BeNaN`, `BeNegative`,
`BePositive`, `BeZero`, `Match`. None has the `…OrEqualTo` forms. There is **no** nullable comparer for
`double` or `float` — do not create one.

Phase 02 added the same two methods to `Numbers/`. **This phase must produce identical message wording.**
The established contract from `TimeSpanComparer` and phase 02:

- positive → `{Subject} should be greater than or equal to {expected}` / `… less than or equal to …`
- negated → `{Subject} should not be greater than or equal to {expected}` / `… less than or equal to …`

If phase 02 has not landed yet, that is fine — this phase does not depend on it. Copy the wording from
`FatCat.Testing/TimeSpans/TimeSpanComparer.cs`, which has both methods today.

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, and [00-overview.md](00-overview.md).

---

## Scope

**In scope** — `BeGreaterThanOrEqualTo` / `BeLessThanOrEqualTo` on `DoubleComparer`, `NotDoubleComparer`,
`FloatComparer`, `NotFloatComparer`, with tests and doc updates.

**Out of scope**

- `Numbers/` — phase 02.
- `NotBeApproximately`, `NotBeInRange` (gaps.md §6.3). Not this phase.
- Any change to `BeNaN`, `BeApproximately`, or the existing comparison methods.
- A nullable `double?` / `float?` comparer. `ShouldExtensions` has no such overload and adding one is a new
  family, not a G8 method backfill.

---

## Design

```csharp
// DoubleComparer
public DoubleComparer BeGreaterThanOrEqualTo(double expected, string because = null)
public DoubleComparer BeLessThanOrEqualTo(double expected, string because = null)

// NotDoubleComparer
public NotDoubleComparer BeGreaterThanOrEqualTo(double expected, string because = null)
public NotDoubleComparer BeLessThanOrEqualTo(double expected, string because = null)

// FloatComparer / NotFloatComparer — the same, with float
```

| Comparer | Fails when | Message |
|---|---|---|
| `DoubleComparer.BeGreaterThanOrEqualTo` | `Subject < expected` | `{Subject} should be greater than or equal to {expected}` |
| `DoubleComparer.BeLessThanOrEqualTo` | `Subject > expected` | `{Subject} should be less than or equal to {expected}` |
| `NotDoubleComparer.BeGreaterThanOrEqualTo` | `Subject >= expected` | `{Subject} should not be greater than or equal to {expected}` |
| `NotDoubleComparer.BeLessThanOrEqualTo` | `Subject <= expected` | `{Subject} should not be less than or equal to {expected}` |

Float rows are identical with `float`.

### `NaN` — the one real design point

IEEE semantics: every comparison involving `NaN` is false, so `NaN < expected` is false and
`NaN >= expected` is also false. Consequently, with the conditions written exactly as above:

- `double.NaN.Should().BeGreaterThanOrEqualTo(1)` **passes** — because `NaN < 1` is false.
- `double.NaN.Should().Not.BeGreaterThanOrEqualTo(1)` **also passes** — because `NaN >= 1` is false.

Both forms passing for the same subject is surprising, but it matches what `BeGreaterThan` /
`BeLessThan` already do in these files, and consistency inside the family beats cleverness. **Do not add a
`NaN` guard.** Instead:

- Write explicit tests pinning this behaviour so it is deliberate and cannot regress silently.
- Document it in the README under `### Doubles And Floats`: comparisons against `NaN` are always
  unsatisfied in both directions; use `BeNaN()` to assert on `NaN`.
- Record it in the phase report as a discovered risk for the human reviewer.

Mirror the existing file layout: methods alphabetical among their neighbours, braces on every `if`, blank
line before `return this;`, block bodies only, `string because = null`.

---

## TDD Steps

1. **Red.** `Tests.FatCat.Testing/Doubles/DoubleBeGreaterThanOrEqualToTests.cs`, deriving `BaseTest`,
   namespace `Tests.FatCat.Testing.Doubles`. Facts:

   - `GoodBeGreaterThanOrEqualToWhenGreater` — `3.5.Should().BeGreaterThanOrEqualTo(1.5)`
   - `GoodBeGreaterThanOrEqualToWhenEqual` — `1.5.Should().BeGreaterThanOrEqualTo(1.5)`
   - `BadBeGreaterThanOrEqualTo`
   - `BadBeGreaterThanOrEqualToShowsCorrectMessage` → `"1.5 should be greater than or equal to 3.5"`
   - `BadBeGreaterThanOrEqualToWithBecause` → `"custom because"`
   - `GoodNotBeGreaterThanOrEqualTo`
   - `BadNotBeGreaterThanOrEqualTo`
   - `BadNotBeGreaterThanOrEqualToShowsCorrectMessage` → `"3.5 should not be greater than or equal to 1.5"`
   - `BadNotBeGreaterThanOrEqualToWithBecause`
   - `BadNotBeGreaterThanOrEqualToWhenEqual`
   - `GoodBeGreaterThanOrEqualToWhenNaN` — pins the IEEE behaviour described above
   - `GoodNotBeGreaterThanOrEqualToWhenNaN` — pins the other half

   Confirm the exact interpolated form of the literals before pinning a message: `$"{1.5}"` renders as
   `1.5`, but pick values whose `ToString()` is unambiguous and check the first red run's actual message
   rather than assuming.

2. **Red.** `DoubleBeLessThanOrEqualToTests.cs` — the mirrored set.

3. **Red.** `Tests.FatCat.Testing/Floats/FloatBeGreaterThanOrEqualToTests.cs` and
   `FloatBeLessThanOrEqualToTests.cs` — the same sets with `float` literals (`1.5f`, `3.5f`). Declare the
   subject as `float value = 1.5f;` so the `float` overload is selected and not `double`.

4. **Green.** Implement the eight methods.

5. **Refactor.** None. Do not extract a shared comparison helper across `Doubles/` and `Floats/` — the two
   folders deliberately duplicate.

---

## Files

**Changed**

- `FatCat.Testing/Doubles/DoubleComparer.cs`
- `FatCat.Testing/Doubles/NotDoubleComparer.cs`
- `FatCat.Testing/Floats/FloatComparer.cs`
- `FatCat.Testing/Floats/NotFloatComparer.cs`
- `README.md`
- `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Doubles/DoubleBeGreaterThanOrEqualToTests.cs`
- `Tests.FatCat.Testing/Doubles/DoubleBeLessThanOrEqualToTests.cs`
- `Tests.FatCat.Testing/Floats/FloatBeGreaterThanOrEqualToTests.cs`
- `Tests.FatCat.Testing/Floats/FloatBeLessThanOrEqualToTests.cs`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog` → `### Doubles And Floats`** — add:

| Assertion | What it asserts |
|---|---|
| `BeGreaterThanOrEqualTo(expected)` | the value is greater than or equal to `expected` |
| `BeLessThanOrEqualTo(expected)` | the value is less than or equal to `expected` |

Plus a short note under that subsection: comparisons involving `NaN` are unsatisfied in both the positive
and negated forms — assert on `NaN` with `BeNaN()`.

**`MIGRATION.md` → `## 3. Mapping Table`** — append:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `double.Should().BeGreaterThanOrEqualTo(x)` | same | ✅ supported | `Tests.FatCat.Testing.Doubles.DoubleBeGreaterThanOrEqualToTests` |
| `double.Should().BeLessThanOrEqualTo(x)` | same | ✅ supported | `Tests.FatCat.Testing.Doubles.DoubleBeLessThanOrEqualToTests` |
| `float.Should().BeGreaterThanOrEqualTo(x)` | same | ✅ supported | `Tests.FatCat.Testing.Floats.FloatBeGreaterThanOrEqualToTests` |
| `float.Should().BeLessThanOrEqualTo(x)` | same | ✅ supported | `Tests.FatCat.Testing.Floats.FloatBeLessThanOrEqualToTests` |
| `double.Should().NotBeGreaterThanOrEqualTo(x)` | `.Should().Not.BeGreaterThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Doubles.DoubleBeGreaterThanOrEqualToTests` |
| `double.Should().NotBeLessThanOrEqualTo(x)` | `.Should().Not.BeLessThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Doubles.DoubleBeLessThanOrEqualToTests` |

**`MIGRATION.md` → `## 4. Type Coverage`** — mark the numeric `…OrEqualTo` line complete **only if phase 02
has already landed**. If it has not, say `double`/`float` are covered and `int` and friends are pending
phase 02. Do not claim more than the tree proves.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~OrEqualTo"
```

Then run the standards review on the uncommitted change and resolve every finding.

---

## Definition of Done

- [ ] Tests written before implementation; red state observed and recorded.
- [ ] Eight methods implemented across the four files.
- [ ] Four new test classes with the full required set plus the two `NaN` pins per direction.
- [ ] Message wording is byte-identical to `TimeSpanComparer`'s (and phase 02's, if it landed).
- [ ] No new compiler warnings; namespaces match folders.
- [ ] No banned patterns (expression bodies, braceless `if`, `string?`, underscores in test names).
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` and `MIGRATION.md` updated, including the `NaN` note.
- [ ] `NaN` behaviour recorded in the phase report as a discovered risk.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/03-double-float-or-equal-to.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-03-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Independent of phase 02 in both directions — reverting one does not require reverting the other, though
`MIGRATION.md` §4 wording may need a follow-up correction if only one of the two remains. Only phase 14
depends on this; revert it first if it has landed. No manual steps.

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing.Doubles;
DoubleComparer.BeGreaterThanOrEqualTo(double expected, string because = null)
DoubleComparer.BeLessThanOrEqualTo(double expected, string because = null)
NotDoubleComparer.BeGreaterThanOrEqualTo(double expected, string because = null)
NotDoubleComparer.BeLessThanOrEqualTo(double expected, string because = null)

namespace FatCat.Testing.Floats;
FloatComparer.BeGreaterThanOrEqualTo(float expected, string because = null)
FloatComparer.BeLessThanOrEqualTo(float expected, string because = null)
NotFloatComparer.BeGreaterThanOrEqualTo(float expected, string because = null)
NotFloatComparer.BeLessThanOrEqualTo(float expected, string because = null)
```

Documented behaviour later phases must not contradict: comparisons against `NaN` never fail in either
direction; `BeNaN()` is the way to assert on `NaN`.
