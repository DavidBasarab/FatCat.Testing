# Phase 12 — G26 completeness: DateTimes

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **medium.** Zero call sites in either repo (this is replacement-claim work, not migration), but
  the **fluent difference chains** introduce a new chainable shape and, per OQ-6, a new numeric-extension
  surface (`2.Hours()`). That is the risk, not the individual assertions.
- **Depends on:** 01
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

G26 is method-level completeness in families FatCat **already covers**. It is not migration-blocking — zero
call sites — but it is what makes the "replacement for FluentAssertions" claim honest for a third party. This
phase is the DateTimes family, the largest single G26 gap.

`FatCat.Testing/DateTimes/` has four comparers. `DateTimeComparer` already has `Be`, `BeAfter`, `BeBefore`,
`BeOnOrAfter`, `BeOnOrBefore`, `BeCloseTo`, `BeUtc`, `BeLocal`, the `Have*` component set, `HaveKind`,
`HaveOffset`. It uses a private `SubjectFormatted` (`"yyyy-MM-dd HH:mm:ss"`) for messages — **match that
format** in every new message so the family is consistent.

**Read `DateTimeComparer.cs` and `NotDateTimeComparer.cs` in full before writing.** Mirror their message
format, their `Not` symmetry, and their component style.

**OQ-6 gates this phase** (orchestrator precondition): the fluent difference chains
(`BeLessThan(2.Hours()).Before(x)`) need a `2.Hours()` surface that does not exist. Proposal: ship
`Hours()`/`Minutes()`/`Seconds()`/`Days()`/`Milliseconds()` as `int` extensions returning `TimeSpan`, as part
of this phase. That is new public API beyond assertions. **Alternative if OQ-6 says no:** the chains take a
`TimeSpan` argument directly (`BeLessThan(TimeSpan.FromHours(2)).Before(x)`). Confirm which before starting;
it changes the surface and the tests.

Read before starting: [00-overview.md](00-overview.md) (**OQ-6**), `gaps.md` §6.3 (the DateTimes row),
`.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** — on the DateTime comparers:

- **The fluent difference chains** — the headline. `BeLessThan(span).Before(x)`, `BeMoreThan(span).After(x)`,
  `BeWithin(span).Before(x)` / `.After(x)`, `BeAtLeast(span).Before/After(x)`, `BeExactly(span).Before/After(x)`.
- `BeSameDateAs(DateTime)` — same calendar date, ignoring time.
- `BeIn(DateTimeKind)` — `Subject.Kind == kind`.
- `BeOneOf(params DateTime[])` — already on the base, but confirm it is reachable and add a DateTime-flavoured
  test; if the base `BeOneOf` message reads poorly for dates, note it (do not fix the base here).
- If OQ-6 = yes: the `int` → `TimeSpan` extensions (`Hours`, `Minutes`, `Seconds`, `Days`, `Milliseconds`).

**Out of scope**

- `DateTimeOffset`, `DateOnly`, `TimeOnly` — those are `tier_2_gaps` phases 07–09, a **different plan**. Do
  not add them here.
- Nullable-DateTime versions of the chains, **unless** the base pattern makes them trivial — the chains are
  complex; ship them on `DateTimeComparer` first. Note the nullable gap in the report for a possible
  follow-up; do not half-build it.
- Any other G26 family (each is its own phase, 13–19).

---

## Design

### The difference chains — a two-step builder

`BeLessThan(2.Hours())` returns an intermediate that carries the subject and the tolerance and exposes
`.Before(DateTime)` and `.After(DateTime)`:

```csharp
public DateTimeDifferenceChain BeLessThan(TimeSpan tolerance) { return new DateTimeDifferenceChain(Subject, tolerance, DifferenceKind.LessThan); }
```

`DateTimeDifferenceChain` (own file, `FatCat.Testing/DateTimes/`) holds `subject`, `tolerance`, and which
comparison, and:

```csharp
public DateTimeComparer Before(DateTime other, string because = null) { ... }   // asserts (other - subject) satisfies the kind vs tolerance, and subject < other
public DateTimeComparer After(DateTime other, string because = null)  { ... }
```

The difference kinds:

| Builder | Passes when the gap is |
|---|---|
| `BeLessThan(t)` | `< t` |
| `BeMoreThan(t)` | `> t` |
| `BeAtLeast(t)` | `>= t` |
| `BeWithin(t)` | `<= t` |
| `BeExactly(t)` | `== t` |

`Before(other)` means subject is earlier than other by a gap in that relationship; `After(other)` means
later. Define "gap" precisely: `Before` uses `other - subject` (must be positive — subject genuinely before
other), `After` uses `subject - other`. If the direction is wrong (subject is after other but `.Before` was
called), that is a failure with a message saying so. Pin this in tests — the direction is the subtle part.

Return `DateTimeComparer` from `.Before`/`.After` so the chain rejoins the normal fluent surface.

Message shape (use `SubjectFormatted`'s format):
`{subject} should be less than 02:00:00 before {other}` / `... but the difference is 03:00:00`.

### `BeSameDateAs`, `BeIn`

Standard single-step assertions on `DateTimeComparer` and its `Not` twin:

| Method | Fails when | Message |
|---|---|---|
| `BeSameDateAs(expected)` | `Subject.Date != expected.Date` | `{subject} should be on the same date as {expected:yyyy-MM-dd}` |
| `BeIn(kind)` | `Subject.Kind != kind` | `{subject} should be in {kind}` |

`BeIn` overlaps `HaveKind`; ship it as the FluentAssertions-named alias and note the overlap in `README.md`.

### The `int` → `TimeSpan` extensions (if OQ-6 = yes)

New file `FatCat.Testing/DateTimes/TimeSpanExtensions.cs` (or `NumericTimeExtensions.cs` — name it for what
it does), a static class with `Hours`/`Minutes`/`Seconds`/`Days`/`Milliseconds` on `int` returning
`TimeSpan.FromX`. Use an **extension block** (`extension(int value) { ... }`, accepted per `types.md` C# 14)
to group them. These are general-purpose; document that they exist primarily for the difference chains but
are usable anywhere.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/DateTimes/`, one class per assertion method.
**Construct explicit `DateTime` values — never `DateTime.Now`** (`not-allowed.md`, `testing.md`).

1. `DateTimeBeLessThanBeforeTests.cs` — `GoodBeLessThanBefore`, `BadBeLessThanBefore` (gap too large),
   `BadBeLessThanBeforeWrongDirection` (subject after other), `...ShowsCorrectMessage`, `...WithBecause`,
   and the `.After` counterpart. Use fixed timestamps, e.g. `new DateTime(2026, 1, 1, 10, 0, 0)` and
   `new DateTime(2026, 1, 1, 11, 30, 0)`.
2. One test class per builder × direction that reads distinctly — at minimum `BeLessThan`, `BeMoreThan`,
   `BeWithin`, `BeAtLeast`, `BeExactly`, each with `.Before` and `.After`. Group sensibly but keep one
   assertion method per class per `testing.md`.
3. `DateTimeBeSameDateAsTests.cs`, `DateTimeBeInTests.cs` — full sets with `Not`.
4. If OQ-6 = yes: `NumericTimeExtensionsTests.cs` — `2.Hours()` equals `TimeSpan.FromHours(2)`, etc.
5. **Green.** Implement `DateTimeDifferenceChain`, the builder methods, `BeSameDateAs`, `BeIn`, and (if
   confirmed) the extensions.
6. Run the whole suite — existing DateTime tests untouched.

---

## Files

**Added**

- `FatCat.Testing/DateTimes/DateTimeDifferenceChain.cs`
- `FatCat.Testing/DateTimes/DifferenceKind.cs` (enum) — if the design uses one
- `FatCat.Testing/DateTimes/NumericTimeExtensions.cs` — if OQ-6 = yes
- Test classes under `Tests.FatCat.Testing/DateTimes/` — one per assertion method

**Changed**

- `FatCat.Testing/DateTimes/DateTimeComparer.cs`, `NotDateTimeComparer.cs`
- `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `### DateTimes`** — append the difference chains (with a worked example showing the two-step
shape and the `2.Hours()` surface), `BeSameDateAs`, `BeIn`. Note the `BeIn`/`HaveKind` overlap. If the
`int` → `TimeSpan` extensions ship, document them in their own short subsection.

**`MIGRATION.md` → `## 3. Mapping Table`** — flip the DateTime G26 rows to `✅ supported`, each naming its
test class. These are coverage rows (no consumer call site — mark them so, matching `tier_2_gaps` ADR-3's
convention for coverage-only rows).

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~DateTime"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] OQ-6 confirmed; the `2.Hours()` decision recorded in the report.
- [ ] Tests written before implementation; red observed and recorded.
- [ ] The five difference builders each ship with `.Before` and `.After`, returning `DateTimeComparer`.
- [ ] The direction check (subject before vs after other) is correct and pinned by a wrong-direction test.
- [ ] `BeSameDateAs`, `BeIn` shipped with full sets and `Not` equivalents.
- [ ] All timestamps in tests are explicit literals; no `DateTime.Now`.
- [ ] If shipped, the `int` → `TimeSpan` extensions are tested and documented.
- [ ] Message format matches `SubjectFormatted` (`yyyy-MM-dd HH:mm:ss`).
- [ ] No new warnings; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION coverage rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/12-g26-datetimes.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-12-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (phase 20 audits it — revert 20 first if landed). If the `int` → `TimeSpan`
extensions were used by any later leaf, they revert with this commit; no later leaf in this plan depends on
them.

---

## Hand-off

Public surface added on `DateTimeComparer`: the five difference builders returning `DateTimeDifferenceChain`,
`BeSameDateAs`, `BeIn`; and optionally the `int` → `TimeSpan` extensions in `FatCat.Testing.DateTimes`.

This is a G26 leaf — nothing else depends on it. Phase 20 counts it toward the replacement-claim audit.
The nullable-DateTime chains and the `BeMoreThan`/`BeExactly` nullable variants, if wanted, are noted here as
a possible follow-up plan — deliberately not built (see Scope).
