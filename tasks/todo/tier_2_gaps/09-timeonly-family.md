# Phase 09 — `TimeOnly` Family

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
`<Type>Comparer`, `Not<Type>Comparer`, `Nullable<Type>Comparer`, `NotNullable<Type>Comparer` — built on
`ComparerBase<TSubject, TComparer>` / `NotComparerBase<TSubject, TComparer>`, each taking its subject
through a primary constructor, each assertion returning the comparer and accepting a trailing
`string because = null`.

Templates to open before writing anything: `FatCat.Testing/TimeSpans/` (closest in subject matter — it has
the `Have*` component assertions and `BeCloseTo`) and `FatCat.Testing/DateTimes/`. If phase 08
(`DateOnlys/`) has landed, its wording for `Be`, `BeAfter`, `BeBefore`, `BeOnOrAfter`, `BeOnOrBefore`, and
`BeInRange` is the contract this phase must match.

`TimeOnly` (`System.TimeOnly`, .NET 6+) has no comparer and no `Should()` overload. No consuming-repo call
site needs it — coverage-completeness (gaps.md A3).

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, and [00-overview.md](00-overview.md) (ADR-7, ADR-10).

---

## Scope

**In scope** — `TimeOnlys/` folder, four comparers, two `ShouldExtensions` overloads, full test set, doc
updates.

**Out of scope** — `DateOnly` (phase 08), `Should(this Task<TimeOnly>)` (optional; skip and say so in the
README), any change to `TimeSpans/` or `DateTimes/`.

---

## Design

**Folder and namespace** — `FatCat.Testing/TimeOnlys/`, namespace `FatCat.Testing.TimeOnlys`; tests in
`Tests.FatCat.Testing/TimeOnlys/`, namespace `Tests.FatCat.Testing.TimeOnlys` (ADR-10).

**Classes** — `TimeOnlyComparer`, `NotTimeOnlyComparer`, `NullableTimeOnlyComparer`,
`NotNullableTimeOnlyComparer`. One class per file, named after the class.

**Assertion surface**

| Assertion | Fails when | Message |
|---|---|---|
| `Be(expected)` | `Subject != expected` | `{Subject} should be {expected}` |
| `BeAfter(expected)` | `Subject <= expected` | `{Subject} should be after {expected}` |
| `BeBefore(expected)` | `Subject >= expected` | `{Subject} should be before {expected}` |
| `BeOnOrAfter(expected)` | `Subject < expected` | `{Subject} should be on or after {expected}` |
| `BeOnOrBefore(expected)` | `Subject > expected` | `{Subject} should be on or before {expected}` |
| `BeCloseTo(expected, precision)` | the gap exceeds `precision` | copy `TimeSpanComparer.BeCloseTo`'s wording |
| `BeInRange(lower, upper)` | outside the inclusive range | `{Subject} should be between {lower} and {upper}` |
| `HaveHour(expected)` | `Subject.Hour != expected` | `{Subject} should have hour {expected}` |
| `HaveMinute(expected)` | `Subject.Minute != expected` | `{Subject} should have minute {expected}` |
| `HaveSecond(expected)` | `Subject.Second != expected` | `{Subject} should have second {expected}` |
| `HaveMillisecond(expected)` | `Subject.Millisecond != expected` | `{Subject} should have millisecond {expected}` |

Copy the `Have*` message wording from `DateTimeComparer` (singular components: "should have hour"), **not**
from `TimeSpanComparer` (plural: "should have hours"). `TimeOnly` exposes a clock reading, so the singular
form is correct. Verify both in source and state the choice in the phase report.

`BeCloseTo(TimeOnly expected, TimeSpan precision)` — compute the absolute gap as
`(Subject - expected).Duration()`. **Do not** try to be clever about midnight wrap-around: 23:59 and 00:01
are 23h58m apart, not 2 minutes. Write a test pinning that, and document it in the README.

The negated comparer mirrors all of the above with `should not …`. The nullable pair adds `BeNull` /
`HaveValue`, uses a private `SubjectDisplay`, and fails every other assertion on a null subject — copy
`NullableTimeSpanComparer`'s guards method for method.

**Entry points** — in `ShouldExtensions.cs`:

```csharp
public static TimeOnlyComparer Should(this TimeOnly subject) { return new TimeOnlyComparer(subject); }

public static NullableTimeOnlyComparer Should(this TimeOnly? subject) { return new NullableTimeOnlyComparer(subject); }
```

`TimeOnly` is a struct but not an `Enum` — no ambiguity with the enum generic.

---

## TDD Steps

Explicit literals only — `new TimeOnly(10, 30, 0)`. **No `TimeOnly.FromDateTime(DateTime.Now)`** and no
other clock read (`not-allowed.md`).

One test class per assertion method: `TimeOnly<Method>Tests` and `NullableTimeOnly<Method>Tests`, deriving
`BaseTest`, no fields, no constructor, full `Good` / `Bad` / `BadShowsCorrectMessage` / `BadWithBecause`
set plus the `Not` equivalents plus the nullable repeat plus the null-subject case.

Boundary and behaviour pins:

- `GoodBeOnOrAfterWhenEqual`, `GoodBeOnOrBeforeWhenEqual`, `BadBeAfterWhenEqual`, `BadBeBeforeWhenEqual`
- `GoodBeInRangeWhenOnLowerBound`, `GoodBeInRangeWhenOnUpperBound`
- `BadBeCloseToAcrossMidnight` — `new TimeOnly(23, 59)` is **not** close to `new TimeOnly(0, 1)` within
  two minutes. This pins the deliberate no-wrap-around decision.

Confirm the rendering of `$"{new TimeOnly(10, 30, 0)}"` from the first red run before pinning messages.

Work assertion by assertion: red, green, next.

---

## Files

**Added**

- `FatCat.Testing/TimeOnlys/TimeOnlyComparer.cs`
- `FatCat.Testing/TimeOnlys/NotTimeOnlyComparer.cs`
- `FatCat.Testing/TimeOnlys/NullableTimeOnlyComparer.cs`
- `FatCat.Testing/TimeOnlys/NotNullableTimeOnlyComparer.cs`
- `Tests.FatCat.Testing/TimeOnlys/` — one class per assertion method plus `NullableTimeOnlyBeNullTests.cs`

**Changed** — `FatCat.Testing/ShouldExtensions.cs`, `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog`** — new `### TimeOnlys` subsection, alphabetically placed. Include
the assertion table, the midnight non-wrap behaviour of `BeCloseTo`, and whether the `Task<TimeOnly>` form
exists.

**`README.md` → `## Coverage Status`** — flip the `TimeOnly` row to `✅ shipped`.

**`MIGRATION.md` → `## 3. Mapping Table`** — one row per assertion, positive and negated, each naming its
test class; lead the block with the coverage-claim sentence (gaps.md A3).

**`MIGRATION.md` → `## 4. Type Coverage`** — mark `TimeOnly` supported.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~TimeOnlys"
```

Then run the standards review on the uncommitted change and resolve every finding.

---

## Definition of Done

- [ ] Tests written before implementation, assertion by assertion; red states observed.
- [ ] Four comparers, one class per file, namespaces matching folders.
- [ ] Every assertion in the Design table implemented across all four comparers where it applies.
- [ ] `Have*` messages use the singular form, matching `DateTimeComparer`; the choice is recorded in the
      phase report.
- [ ] `BeCloseTo` midnight non-wrap pinned by a test and documented in the README.
- [ ] Full test set per assertion, including boundaries and null-subject cases.
- [ ] `NullableTimeOnlyBeNullTests` exists.
- [ ] Two `Should()` overloads added and unambiguous.
- [ ] No clock reads in tests.
- [ ] No new compiler warnings; no banned patterns; primary constructors throughout.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` and `MIGRATION.md` updated.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/09-timeonly-family.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-09-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Self-contained. Only phase 14 depends on it — revert that first if it landed. No manual steps.

---

## Hand-off

```csharp
namespace FatCat.Testing.TimeOnlys;
TimeOnlyComparer / NotTimeOnlyComparer
NullableTimeOnlyComparer / NotNullableTimeOnlyComparer

namespace FatCat.Testing;
Should(this TimeOnly)  -> TimeOnlyComparer
Should(this TimeOnly?) -> NullableTimeOnlyComparer
```

Documented behaviour later phases must not contradict: `BeCloseTo` on `TimeOnly` measures the linear gap
within the day and does not wrap around midnight.

Phases 10–12 switch to the **reference-type** shape — two comparers, not four (ADR-7).
