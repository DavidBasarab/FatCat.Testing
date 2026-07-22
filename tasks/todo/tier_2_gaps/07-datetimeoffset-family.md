# Phase 07 — `DateTimeOffset` Family

- **Work item:** `tier_2_gaps`
- **Gap:** **G10** (gaps.md §3, Tier 2 — missing type families)
- **Risk:** **low.** A brand-new folder and two brand-new `Should()` overloads for a type that has none
  today, so nothing existing can change behaviour. It is the **largest** phase in the plan by file count —
  budget accordingly, but it is still exactly one commit.
- **Depends on:** 01
- **Depended on by:** 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing` groups assertions into one folder per type family, each following a strict four-comparer
symmetry for value types (`naming-and-structure.md`, ADR-7 in [00-overview.md](00-overview.md)):

| Pattern | Role |
|---|---|
| `<Type>Comparer` | assertions on the non-nullable value |
| `Not<Type>Comparer` | the negated form, reached via the `Not` property |
| `Nullable<Type>Comparer` | assertions on `<Type>?` |
| `NotNullable<Type>Comparer` | negated nullable form |

`FatCat.Testing/DateTimes/` is the exact template for this phase — four comparers, ~18 assertions each,
mirrored by 35 test files in `Tests.FatCat.Testing/DateTimes/`. **Open all four files before writing
anything.** Copy their structure, their message wording, and their null handling.

`DateTimeOffset` has no comparer and no `Should()` overload today. No consuming-repo call site needs it
(gaps.md A3) — this is coverage-completeness for the "replacement for FluentAssertions" claim.

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, and [00-overview.md](00-overview.md) (ADR-7, ADR-10).

---

## Scope

**In scope** — a `DateTimeOffsets/` folder with four comparers, two `ShouldExtensions` overloads, the full
test set, and doc updates.

**Out of scope**

- `Should(this Task<DateTimeOffset>)`. Optional per phase 06's Hand-off; if you skip it — and you should,
  to keep this commit atomic — the README row must say the `Task` form is not available for this type.
- The DateTime fluent difference chains (`BeLessThan(2.Hours()).Before(x)`) from gaps.md §6.3. Not G10.
- `BeSameDateAs`, `BeIn(DateTimeKind)`, `BeOneOf` on DateTime (gaps.md §6.3). Not this phase.
- Touching `DateTimes/` in any way.

---

## Design

**Folder and namespace** — `FatCat.Testing/DateTimeOffsets/`, namespace `FatCat.Testing.DateTimeOffsets`,
tests in `Tests.FatCat.Testing/DateTimeOffsets/`, namespace `Tests.FatCat.Testing.DateTimeOffsets`. The
plural-of-the-type-name folder convention is what every existing family uses (ADR-10).

**Files** — one class per file, named after the class:

```csharp
public class DateTimeOffsetComparer(DateTimeOffset subject)
	: ComparerBase<DateTimeOffset, DateTimeOffsetComparer>(subject)
{
	public NotDateTimeOffsetComparer Not { get; } = new(subject);
	…
}
```

…plus `NotDateTimeOffsetComparer`, `NullableDateTimeOffsetComparer`, `NotNullableDateTimeOffsetComparer`,
each mirroring its `DateTimes/` counterpart's shape exactly, including the private `SubjectDisplay`
property on the nullable pair.

**Assertion surface** — mirror `DateTimeComparer`, minus what `DateTimeOffset` does not have:

| Assertion | Fails when | Message |
|---|---|---|
| `Be(expected)` | `Subject != expected` | `{Subject} should be {expected}` |
| `BeAfter(expected)` | `Subject <= expected` | `{Subject} should be after {expected}` |
| `BeBefore(expected)` | `Subject >= expected` | `{Subject} should be before {expected}` |
| `BeOnOrAfter(expected)` | `Subject < expected` | `{Subject} should be on or after {expected}` |
| `BeOnOrBefore(expected)` | `Subject > expected` | `{Subject} should be on or before {expected}` |
| `BeCloseTo(expected, precision)` | difference exceeds `precision` | copy `DateTimeComparer.BeCloseTo`'s wording verbatim |
| `HaveYear(expected)` … `HaveMillisecond(expected)` | the component differs | `{Subject} should have year {expected}`, etc. |
| `HaveOffset(expected)` | `Subject.Offset != expected` | `{Subject} should have offset {expected}` |

**Excluded deliberately:** `BeUtc`, `BeLocal`, `HaveKind` — `DateTimeOffset` has no `DateTimeKind`. Its
UTC-ness is expressed by `Offset`, which `HaveOffset` already covers. Say so in the README so the omission
reads as a decision rather than an oversight.

`HaveOffset` on `DateTimeOffset` is genuinely `Subject.Offset`. Note that `DateTimeComparer` also has a
`HaveOffset` — read what it actually compares before copying its message, and do not assume the two are
implemented the same way.

The nullable pair adds `BeNull` and `HaveValue`, exactly as `NullableDateTimeComparer` does, and every
other assertion fails on a null subject in the positive comparer. Mirror `NullableDateTimeComparer`'s null
guards method for method.

**Entry points** — add to `ShouldExtensions.cs`, placed with the other `DateTime` overloads:

```csharp
public static DateTimeOffsetComparer Should(this DateTimeOffset subject) { return new DateTimeOffsetComparer(subject); }

public static NullableDateTimeOffsetComparer Should(this DateTimeOffset? subject) { return new NullableDateTimeOffsetComparer(subject); }
```

Plus `using FatCat.Testing.DateTimeOffsets;`. No overload-resolution risk: `DateTimeOffset` is a struct but
not an `Enum`, so the existing enum generic cannot bind it, and no other overload accepts it.

---

## TDD Steps

**Never call `DateTimeOffset.Now` or `UtcNow` in a test** (`not-allowed.md`). Construct explicit values:

```csharp
var subject = new DateTimeOffset(2026, 7, 21, 10, 30, 0, TimeSpan.FromHours(-5));
```

Before pinning any message, run the test red once and copy the **actual** rendering of
`$"{subject}"` — `DateTimeOffset.ToString()` is culture-sensitive and its default format is not obvious.
If the rendering proves fragile across machines, note it in the phase report as a discovered risk (it is
the same exposure `DateTimes/` already carries).

One test class per assertion method, named `DateTimeOffset<Method>Tests` and
`NullableDateTimeOffset<Method>Tests`, each deriving `BaseTest` with no fields and no constructor. Per
`testing.md`, each class carries:

1. `Good<Method>` — passing case
2. `Bad<Method>` — failing case
3. `Bad<Method>ShowsCorrectMessage` — the exact message
4. `Bad<Method>WithBecause` — `because` replaces the message
5. `GoodNot<Method>` / `BadNot<Method>` / `BadNot<Method>ShowsCorrectMessage` /
   `BadNot<Method>WithBecause`
6. the same set again in the `Nullable…` class, plus the null-subject case
7. a `NullableDateTimeOffsetBeNullTests` class covering the null path itself

Boundary cases matter here: `BeOnOrAfter` and `BeOnOrBefore` each need a `Good…WhenEqual` fact, and
`BeAfter` / `BeBefore` each need a `Bad…WhenEqual` fact.

Work one assertion at a time — red, green, next. Do not write all 30 test classes before the first
implementation.

---

## Files

**Added**

- `FatCat.Testing/DateTimeOffsets/DateTimeOffsetComparer.cs`
- `FatCat.Testing/DateTimeOffsets/NotDateTimeOffsetComparer.cs`
- `FatCat.Testing/DateTimeOffsets/NullableDateTimeOffsetComparer.cs`
- `FatCat.Testing/DateTimeOffsets/NotNullableDateTimeOffsetComparer.cs`
- `Tests.FatCat.Testing/DateTimeOffsets/` — one test class per assertion method, plus
  `NullableDateTimeOffsetBeNullTests.cs`

**Changed**

- `FatCat.Testing/ShouldExtensions.cs`
- `README.md`
- `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog`** — new `### DateTimeOffsets` subsection, alphabetically between
`### DateTimes` and `### Doubles And Floats`. Table of assertions per the Design section, plus:

- a note that `BeUtc` / `BeLocal` / `HaveKind` do not exist on this family, and why (`HaveOffset` instead);
- whether the `Task<DateTimeOffset>` form is available (it is not, unless you shipped it).

**`README.md` → `## Coverage Status`** — flip the `DateTimeOffset` row from `⬜ planned` to `✅ shipped`.

**`MIGRATION.md` → `## 3. Mapping Table`** — append a row per assertion, following the established
pattern; these are coverage claims, not fixes for live call sites (gaps.md A3), so say so in a leading
sentence for the block:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `dto.Should().Be(x)` | `dto.Should().Be(x)` | ✅ supported | `Tests.FatCat.Testing.DateTimeOffsets.DateTimeOffsetBeTests` |
| `dto.Should().BeAfter(x)` | same | ✅ supported | `…DateTimeOffsetBeAfterTests` |
| `dto.Should().NotBeAfter(x)` | `.Should().Not.BeAfter(x)` | ✅ supported | `…DateTimeOffsetBeAfterTests` |
| … one row per assertion, positive and negated … | | | |

**`MIGRATION.md` → `## 4. Type Coverage`** — mark `DateTimeOffset` supported.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~DateTimeOffsets"
```

Then run the standards review on the uncommitted change — it specifically catches the one-class-per-file
rule, which a phase adding this many files is most likely to trip. Resolve every finding.

---

## Definition of Done

- [ ] Tests written before implementation, assertion by assertion; red states observed.
- [ ] Four comparers, one class per file, file named after the class, namespace matching the folder.
- [ ] `DateTimeOffsetComparer` exposes `Not`; the nullable comparer exposes `Not` and `BeNull` /
      `HaveValue`.
- [ ] Every assertion in the Design table implemented on all four comparers where it applies.
- [ ] Every assertion has its full six-part test set, plus the boundary facts for `BeOnOrAfter` /
      `BeOnOrBefore` / `BeAfter` / `BeBefore`.
- [ ] `NullableDateTimeOffsetBeNullTests` exists.
- [ ] Two `Should()` overloads added; both compile and resolve unambiguously.
- [ ] No `DateTimeOffset.Now` / `UtcNow` anywhere in the tests.
- [ ] No new compiler warnings; no banned patterns; primary constructors used throughout.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` (catalog + coverage) and `MIGRATION.md` (mapping + type coverage) updated.
- [ ] Any message-format fragility recorded in the phase report.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/07-datetimeoffset-family.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-07-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Self-contained: a new folder plus two additive overloads. Nothing but phase 14 depends on it — revert 14
first if it landed. No manual steps.

---

## Hand-off

**Public surface added**

```csharp
namespace FatCat.Testing.DateTimeOffsets;
DateTimeOffsetComparer / NotDateTimeOffsetComparer
NullableDateTimeOffsetComparer / NotNullableDateTimeOffsetComparer

namespace FatCat.Testing;
Should(this DateTimeOffset)  -> DateTimeOffsetComparer
Should(this DateTimeOffset?) -> NullableDateTimeOffsetComparer
```

**Contracts for later phases**

- ADR-10's folder convention (plural of the type name) is now proven by a second family; phases 08–12
  follow it.
- The four-comparer symmetry is the template for every remaining **value-type** family (08, 09).
  Reference-type families (10, 11, 12) ship two comparers instead — see ADR-7.
- The message wording established here (`should be after`, `should be on or after`, `should have offset`)
  is the contract for any future date/time family. `DateOnly` (08) and `TimeOnly` (09) must match it.
