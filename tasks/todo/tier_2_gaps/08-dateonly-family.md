# Phase 08 — `DateOnly` Family

- **Work item:** `tier_2_gaps`
- **Gap:** **G10** (gaps.md §3, Tier 2 — missing type families)
- **Risk:** **low.** New folder, new overloads, no existing behaviour touched.
- **Depends on:** 01
- **Depended on by:** 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing` groups assertions one folder per type family. Value-type families ship four comparers —
`<Type>Comparer`, `Not<Type>Comparer`, `Nullable<Type>Comparer`, `NotNullable<Type>Comparer` — each taking
its subject through a primary constructor and forwarding to `ComparerBase<TSubject, TComparer>` or
`NotComparerBase<TSubject, TComparer>`. Every assertion returns the comparer so calls chain, and every
assertion takes a trailing `string because = null` that replaces the generated message.

`FatCat.Testing/DateTimes/` is the working template — open all four files first. If phase 07
(`DateTimeOffsets/`) has already landed, read it too: it is the most recent application of the same
template and its message wording is the contract this phase must match.

`DateOnly` (`System.DateOnly`, .NET 6+) has no comparer and no `Should()` overload. No consuming-repo call
site needs it — this is coverage-completeness (gaps.md A3).

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, and [00-overview.md](00-overview.md) (ADR-7, ADR-10).

---

## Scope

**In scope** — `DateOnlys/` folder, four comparers, two `ShouldExtensions` overloads, full test set, doc
updates.

**Out of scope** — `TimeOnly` (phase 09), `Should(this Task<DateOnly>)` (optional, skip for atomicity —
say so in the README), any change to `DateTimes/` or `DateTimeOffsets/`.

---

## Design

**Folder and namespace** — `FatCat.Testing/DateOnlys/`, namespace `FatCat.Testing.DateOnlys`; tests in
`Tests.FatCat.Testing/DateOnlys/`, namespace `Tests.FatCat.Testing.DateOnlys`. Plural-of-the-type-name is
the convention every existing family follows (ADR-10). `DateOnlys` is clumsy English and deliberately
chosen anyway — consistency with `DateTimes`, `TimeSpans`, `Guids` wins over prose.

**Classes** — `DateOnlyComparer`, `NotDateOnlyComparer`, `NullableDateOnlyComparer`,
`NotNullableDateOnlyComparer`. One class per file, file named after the class.

```csharp
public class DateOnlyComparer(DateOnly subject) : ComparerBase<DateOnly, DateOnlyComparer>(subject)
{
	public NotDateOnlyComparer Not { get; } = new(subject);
	…
}
```

**Assertion surface**

| Assertion | Fails when | Message |
|---|---|---|
| `Be(expected)` | `Subject != expected` | `{Subject} should be {expected}` |
| `BeAfter(expected)` | `Subject <= expected` | `{Subject} should be after {expected}` |
| `BeBefore(expected)` | `Subject >= expected` | `{Subject} should be before {expected}` |
| `BeOnOrAfter(expected)` | `Subject < expected` | `{Subject} should be on or after {expected}` |
| `BeOnOrBefore(expected)` | `Subject > expected` | `{Subject} should be on or before {expected}` |
| `BeInRange(lower, upper)` | outside the inclusive range | `{Subject} should be between {lower} and {upper}` |
| `HaveYear(expected)` | `Subject.Year != expected` | `{Subject} should have year {expected}` |
| `HaveMonth(expected)` | `Subject.Month != expected` | `{Subject} should have month {expected}` |
| `HaveDay(expected)` | `Subject.Day != expected` | `{Subject} should have day {expected}` |
| `HaveDayOfWeek(expected)` | `Subject.DayOfWeek != expected` | `{Subject} should have day of week {expected}` |

`BeInRange`'s message wording is copied from `NumericComparer.BeInRange` — verify it in the source rather
than trusting this table.

The negated comparer mirrors all of the above with `should not …`. The nullable pair adds `BeNull` and
`HaveValue`, uses a private `SubjectDisplay` rendering `null` for a missing value, and fails every other
assertion when the subject is null — copy `NullableDateTimeComparer`'s guards method for method.

**Deliberately excluded:** `BeCloseTo` (a date has no sub-day precision to be close within — `BeInRange`
covers the intent), and every time-component `Have*`. Note the exclusions in the README so they read as
decisions.

**Entry points** — in `ShouldExtensions.cs`, with the other date overloads:

```csharp
public static DateOnlyComparer Should(this DateOnly subject) { return new DateOnlyComparer(subject); }

public static NullableDateOnlyComparer Should(this DateOnly? subject) { return new NullableDateOnlyComparer(subject); }
```

`DateOnly` is a struct but not an `Enum`, so the existing enum generic cannot bind it. No ambiguity.

---

## TDD Steps

Explicit literals only — `new DateOnly(2026, 7, 21)`. **No `DateOnly.FromDateTime(DateTime.Now)`**, no
clock reads (`not-allowed.md`).

One test class per assertion method: `DateOnly<Method>Tests` and `NullableDateOnly<Method>Tests`, deriving
`BaseTest`, no fields, no constructor. Each carries the full set from `testing.md`:

1. `Good<Method>` 2. `Bad<Method>` 3. `Bad<Method>ShowsCorrectMessage` 4. `Bad<Method>WithBecause`
5. the same four through `.Not.` 6. the same again in the nullable class 7. the null-subject case

Plus boundary facts: `GoodBeOnOrAfterWhenEqual`, `GoodBeOnOrBeforeWhenEqual`, `BadBeAfterWhenEqual`,
`BadBeBeforeWhenEqual`, `GoodBeInRangeWhenOnLowerBound`, `GoodBeInRangeWhenOnUpperBound`.

Plus `NullableDateOnlyBeNullTests`.

Confirm the actual rendering of `$"{new DateOnly(2026, 7, 21)}"` from the first red run before pinning any
message — do not guess the format.

Work assertion by assertion: red, green, next.

---

## Files

**Added**

- `FatCat.Testing/DateOnlys/DateOnlyComparer.cs`
- `FatCat.Testing/DateOnlys/NotDateOnlyComparer.cs`
- `FatCat.Testing/DateOnlys/NullableDateOnlyComparer.cs`
- `FatCat.Testing/DateOnlys/NotNullableDateOnlyComparer.cs`
- `Tests.FatCat.Testing/DateOnlys/` — one class per assertion method plus `NullableDateOnlyBeNullTests.cs`

**Changed** — `FatCat.Testing/ShouldExtensions.cs`, `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog`** — new `### DateOnlys` subsection, alphabetically placed. Include
the assertion table, the deliberate exclusions (`BeCloseTo`, time components), and whether the
`Task<DateOnly>` form exists (it does not, unless you shipped it).

**`README.md` → `## Coverage Status`** — flip the `DateOnly` row to `✅ shipped`.

**`MIGRATION.md` → `## 3. Mapping Table`** — one row per assertion, positive and negated, each naming its
test class. Lead the block with a sentence stating these are coverage claims rather than fixes for live
call sites (gaps.md A3).

**`MIGRATION.md` → `## 4. Type Coverage`** — mark `DateOnly` supported.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~DateOnlys"
```

Then run the standards review on the uncommitted change and resolve every finding.

---

## Definition of Done

- [ ] Tests written before implementation, assertion by assertion; red states observed.
- [ ] Four comparers, one class per file, namespaces matching folders.
- [ ] Every assertion in the Design table implemented across all four comparers where it applies.
- [ ] Full test set per assertion, including the six boundary facts and the null-subject cases.
- [ ] `NullableDateOnlyBeNullTests` exists.
- [ ] Two `Should()` overloads added and unambiguous.
- [ ] No clock reads in tests.
- [ ] No new compiler warnings; no banned patterns; primary constructors throughout.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` and `MIGRATION.md` updated.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/08-dateonly-family.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-08-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Self-contained. Only phase 14 depends on it — revert that first if it landed. No manual steps.

---

## Hand-off

```csharp
namespace FatCat.Testing.DateOnlys;
DateOnlyComparer / NotDateOnlyComparer
NullableDateOnlyComparer / NotNullableDateOnlyComparer

namespace FatCat.Testing;
Should(this DateOnly)  -> DateOnlyComparer
Should(this DateOnly?) -> NullableDateOnlyComparer
```

Contract for phase 09: `TimeOnly` is the companion family and must use the same message wording for the
assertions they share (`Be`, `BeAfter`, `BeBefore`, `BeOnOrAfter`, `BeOnOrBefore`, `BeInRange`) and the
same null-handling shape.
