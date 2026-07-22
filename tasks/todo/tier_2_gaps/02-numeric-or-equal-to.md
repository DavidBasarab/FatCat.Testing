# Phase 02 — Numeric `BeGreaterThanOrEqualTo` / `BeLessThanOrEqualTo` (`Numbers/`)

- **Work item:** `tier_2_gaps`
- **Gap:** **G8** (gaps.md §3, Tier 2)
- **Risk:** **low.** Purely additive: four new method pairs on existing comparers. No existing signature,
  message, or behaviour changes. No overload-resolution risk — no method of these names exists today.
- **Depends on:** 01
- **Depended on by:** 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing` is a fluent assertion library. `FatCat.Testing/Numbers/` holds four comparers:

| File | Type | Base |
|---|---|---|
| `NumericComparer.cs` | `NumericComparer<T> where T : INumber<T>` | `ComparerBase<T, NumericComparer<T>>` |
| `NotNumericComparer.cs` | `NotNumericComparer<T>` | `NotComparerBase<T, NotNumericComparer<T>>` |
| `NullableIntComparer.cs` | `NullableIntComparer(int? subject)` | `ComparerBase<int?, NullableIntComparer>` |
| `NotNullableIntComparer.cs` | `NotNullableIntComparer(int? subject)` | `NotComparerBase<int?, NotNullableIntComparer>` |

All four already have `BeGreaterThan` and `BeLessThan`. **None** has the `…OrEqualTo` forms — but
`TimeSpanComparer` does, and it is the pattern to copy. From `FatCat.Testing/TimeSpans/TimeSpanComparer.cs`:

```csharp
public TimeSpanComparer BeGreaterThanOrEqualTo(TimeSpan expected, string because = null)
{
	if (Subject < expected) { CompareException.New(because ?? $"{Subject} should be greater than or equal to {expected}"); }

	return this;
}
```

…and its negated twin in `NotTimeSpanComparer.cs`:

```csharp
public NotTimeSpanComparer BeGreaterThanOrEqualTo(TimeSpan expected, string because = null)
{
	if (Subject >= expected) { CompareException.New(because ?? $"{Subject} should not be greater than or equal to {expected}"); }

	return this;
}
```

Twelve call sites across the consuming repos need this, plus two using FluentAssertions' deprecated
`BeGreaterOrEqualTo` / `BeLessOrEqualTo` aliases, which are normalized to the modern names during
migration rather than reproduced (gaps.md §5.2).

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, and [00-overview.md](00-overview.md).

---

## Scope

**In scope** — `BeGreaterThanOrEqualTo` and `BeLessThanOrEqualTo` on all four `Numbers/` comparers, with
the full required test set and the README/MIGRATION updates.

**Out of scope**

- `Doubles/` and `Floats/` — phase 03. Do not touch those files.
- `NotBeInRange`, `NotBeApproximately`, or anything else from gaps.md §6.3. Not this phase.
- The deprecated alias names `BeGreaterOrEqualTo` / `BeLessOrEqualTo`. **Do not add them** — they are
  FluentAssertions' own deprecated spellings and the migration rewrites them (gaps.md §5.2).

---

## Design

Signatures — four files, two methods each:

```csharp
// NumericComparer<T>
public NumericComparer<T> BeGreaterThanOrEqualTo(T expected, string because = null)
public NumericComparer<T> BeLessThanOrEqualTo(T expected, string because = null)

// NotNumericComparer<T>
public NotNumericComparer<T> BeGreaterThanOrEqualTo(T expected, string because = null)
public NotNumericComparer<T> BeLessThanOrEqualTo(T expected, string because = null)

// NullableIntComparer
public NullableIntComparer BeGreaterThanOrEqualTo(int expected, string because = null)
public NullableIntComparer BeLessThanOrEqualTo(int expected, string because = null)

// NotNullableIntComparer
public NotNullableIntComparer BeGreaterThanOrEqualTo(int expected, string because = null)
public NotNullableIntComparer BeLessThanOrEqualTo(int expected, string because = null)
```

Semantics and messages:

| Comparer | Fails when | Message |
|---|---|---|
| `NumericComparer<T>.BeGreaterThanOrEqualTo` | `Subject < expected` | `{Subject} should be greater than or equal to {expected}` |
| `NumericComparer<T>.BeLessThanOrEqualTo` | `Subject > expected` | `{Subject} should be less than or equal to {expected}` |
| `NotNumericComparer<T>.BeGreaterThanOrEqualTo` | `Subject >= expected` | `{Subject} should not be greater than or equal to {expected}` |
| `NotNumericComparer<T>.BeLessThanOrEqualTo` | `Subject <= expected` | `{Subject} should not be less than or equal to {expected}` |
| `NullableIntComparer.BeGreaterThanOrEqualTo` | `!Subject.HasValue \|\| Subject.Value < expected` | `{SubjectDisplay} should be greater than or equal to {expected}` |
| `NullableIntComparer.BeLessThanOrEqualTo` | `!Subject.HasValue \|\| Subject.Value > expected` | `{SubjectDisplay} should be less than or equal to {expected}` |
| `NotNullableIntComparer.*` | mirror the null handling of `BeGreaterThan`/`BeLessThan` **in that same file** | `{…} should not be …` |

Rules that bite here:

- Methods are placed **alphabetically** among their neighbours, matching the existing ordering in each
  file (`BeGreaterThan`, `BeGreaterThanOrEqualTo`, `BeInRange`, `BeLessThan`, `BeLessThanOrEqualTo`, …).
- `SubjectDisplay` already exists as a private property on `NullableIntComparer`. Use it; do not add a
  second one.
- Braces on every `if`, even single-statement. Blank line before `return this;`. No expression bodies.
- `string because = null` — never `string?`.
- Do **not** open `NotNullableIntComparer.cs` and guess at null handling; read its existing
  `BeGreaterThan` and mirror it exactly, so a null subject behaves consistently across the file.

---

## TDD Steps

Tests first. Red before green — run the suite and see the new tests fail to compile/pass before writing
any comparer code.

1. **Red.** Add `Tests.FatCat.Testing/Numbers/IntBeGreaterThanOrEqualToTests.cs`, deriving `BaseTest`,
   namespace `Tests.FatCat.Testing.Numbers`. Model it on
   `Tests.FatCat.Testing/TimeSpans/TimeSpanBeGreaterThanOrEqualToTests.cs`, which is the exact shape
   required. Facts:

   - `GoodBeGreaterThanOrEqualToWhenGreater`
   - `GoodBeGreaterThanOrEqualToWhenEqual`
   - `BadBeGreaterThanOrEqualTo`
   - `BadBeGreaterThanOrEqualToShowsCorrectMessage` → `"3 should be greater than or equal to 7"`
   - `BadBeGreaterThanOrEqualToWithBecause` → `"custom because"`
   - `GoodNotBeGreaterThanOrEqualTo`
   - `BadNotBeGreaterThanOrEqualTo`
   - `BadNotBeGreaterThanOrEqualToShowsCorrectMessage` → `"7 should not be greater than or equal to 3"`
   - `BadNotBeGreaterThanOrEqualToWithBecause`
   - `BadNotBeGreaterThanOrEqualToWhenEqual` — the equal case must fail through `Not`

2. **Red.** `IntBeLessThanOrEqualToTests.cs` — the same ten, mirrored
   (`"7 should be less than or equal to 3"`, `"3 should not be less than or equal to 7"`).

3. **Red.** `NullableIntBeGreaterThanOrEqualToTests.cs` and `NullableIntBeLessThanOrEqualToTests.cs` —
   the same set against `int?`, **plus**:

   - `BadBeGreaterThanOrEqualToWhenNull` → message `"null should be greater than or equal to 3"`
   - the matching `Not` case, whichever way the existing file's null handling resolves it

   Declare the subject as `int? value = 7;` so the `int?` overload is selected, not the `int` one.

4. **Green.** Implement the eight methods across the four comparer files.

5. **Refactor.** Nothing to extract — resist the urge to introduce a shared comparison helper. The library
   deliberately repeats these four-line methods (ADR-7 symmetry).

Use explicit literals (`3`, `7`) — the failure message is the thing under test, so a random value cannot be
matched against an expected string (`testing.md`, "Test Data").

---

## Files

**Changed**

- `FatCat.Testing/Numbers/NumericComparer.cs`
- `FatCat.Testing/Numbers/NotNumericComparer.cs`
- `FatCat.Testing/Numbers/NullableIntComparer.cs`
- `FatCat.Testing/Numbers/NotNullableIntComparer.cs`
- `README.md`
- `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Numbers/IntBeGreaterThanOrEqualToTests.cs`
- `Tests.FatCat.Testing/Numbers/IntBeLessThanOrEqualToTests.cs`
- `Tests.FatCat.Testing/Numbers/NullableIntBeGreaterThanOrEqualToTests.cs`
- `Tests.FatCat.Testing/Numbers/NullableIntBeLessThanOrEqualToTests.cs`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog` → `### Numbers`** — add two rows:

| Assertion | What it asserts |
|---|---|
| `BeGreaterThanOrEqualTo(expected)` | the value is greater than or equal to `expected` |
| `BeLessThanOrEqualTo(expected)` | the value is less than or equal to `expected` |

Note in that subsection that both are available on the nullable `int?` comparer and through `.Not.`.

**`MIGRATION.md` → `## 3. Mapping Table`** — append (flip the two G8 `⬜ pending` rows seeded in phase 01
to supported rather than duplicating them):

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `.Should().BeGreaterThanOrEqualTo(x)` | `.Should().BeGreaterThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Numbers.IntBeGreaterThanOrEqualToTests` |
| `.Should().BeLessThanOrEqualTo(x)` | `.Should().BeLessThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Numbers.IntBeLessThanOrEqualToTests` |
| `.Should().BeGreaterOrEqualTo(x)` *(deprecated alias)* | `.Should().BeGreaterThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Numbers.IntBeGreaterThanOrEqualToTests` |
| `.Should().BeLessOrEqualTo(x)` *(deprecated alias)* | `.Should().BeLessThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Numbers.IntBeLessThanOrEqualToTests` |
| `.Should().NotBeGreaterThanOrEqualTo(x)` | `.Should().Not.BeGreaterThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Numbers.IntBeGreaterThanOrEqualToTests` |
| `.Should().NotBeLessThanOrEqualTo(x)` | `.Should().Not.BeLessThanOrEqualTo(x)` | ✅ supported | `Tests.FatCat.Testing.Numbers.IntBeLessThanOrEqualToTests` |

`MIGRATION.md` → `## 4. Type Coverage`: note that `double` and `float` still lack the `…OrEqualTo` forms
until phase 03 lands. Do not claim numeric coverage is complete in this commit.

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

Then run the standards review on the uncommitted change and resolve every finding before committing.

Targeted run:

```pwsh
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~OrEqualTo"
```

---

## Definition of Done

- [ ] Tests written before implementation; the red state was observed and is recorded in the phase report.
- [ ] All eight methods implemented across the four `Numbers/` comparers.
- [ ] Four new test classes, each carrying the full `Good` / `Bad` / `BadShowsCorrectMessage` /
      `BadWithBecause` set and its `Not` equivalents.
- [ ] Boundary case (subject equal to expected) tested on both the positive and negated forms.
- [ ] Null-subject case tested on both nullable comparers.
- [ ] No new compiler warnings (the five pre-existing IDE1006 `TestGuid` warnings are baseline).
- [ ] Namespaces match folder paths.
- [ ] No banned patterns: no expression bodies, no braceless `if`, no `string?`, no underscores in test
      names, no `+` concatenation.
- [ ] `dotnet test` green; total count is baseline + the new facts.
- [ ] `dotnet format style` and `dotnet format analyzers` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` and `MIGRATION.md` updated as specified above.
- [ ] Standards review clean.
- [ ] Exactly one commit, message referencing `tasks/todo/tier_2_gaps/02-numeric-or-equal-to.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-02-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Nothing depends on this phase except 14 (close-out audit). If 14 has landed, revert it first. No manual
steps — no data, config, or feature-flag changes, and nothing was published.

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing.Numbers;

NumericComparer<T>.BeGreaterThanOrEqualTo(T expected, string because = null)
NumericComparer<T>.BeLessThanOrEqualTo(T expected, string because = null)
NotNumericComparer<T>.BeGreaterThanOrEqualTo(T expected, string because = null)
NotNumericComparer<T>.BeLessThanOrEqualTo(T expected, string because = null)
NullableIntComparer.BeGreaterThanOrEqualTo(int expected, string because = null)
NullableIntComparer.BeLessThanOrEqualTo(int expected, string because = null)
NotNullableIntComparer.BeGreaterThanOrEqualTo(int expected, string because = null)
NotNullableIntComparer.BeLessThanOrEqualTo(int expected, string because = null)
```

Reachable from every `Should()` overload returning `NumericComparer<T>` — `byte`, `decimal`, `int`, `long`,
`nint`, `nuint`, `sbyte`, `short`, `uint`, `ulong`, `ushort` — plus `int?`.

For phase 03: the message wording established here (`should be greater than or equal to`) is the contract.
`DoubleComparer` and `FloatComparer` must produce identical wording.

For phase 06: `Task<int>` and `Task<int?>` overloads inherit these methods for free — no extra work there.
