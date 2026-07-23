# Phase 01 — Documentation foundation (`README.md` + `MIGRATION.md`)

- **Work item:** `final_gaps`
- **Gap:** — (enabler for every phase)
- **Risk:** **low.** No C# changes. Two markdown files.
- **Depends on:** nothing
- **Depended on by:** every other phase
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing` is a fluent assertion library replacing FluentAssertions 7.0.0. Today `README.md` is two
lines:

```markdown
# FatCat.Testing
Doing my own Fluent Assertions
```

There is no `MIGRATION.md`. Every later phase in this plan has a Definition-of-Done item requiring it to
append a catalog entry to `README.md` and a mapping row to `MIGRATION.md`. Those documents must exist, with
stable section anchors, before any of that can happen.

**A second plan does the same thing.** `tasks/todo/tier_2_gaps/01-documentation-foundation.md` creates the
same skeleton for a different set of gaps. Per **ADR-8** in [00-overview.md](00-overview.md), whichever runs
first creates the files; the second verifies and appends. **Check first:**

```pwsh
Test-Path MIGRATION.md
Get-Content README.md
```

- If `MIGRATION.md` does not exist and `README.md` is the two-line stub → you are creating both. This is the
  expected case.
- If they already exist → **do not restructure them.** Verify every anchor listed below is present, add only
  the ones that are missing, and record what you found in the phase report.

Read before starting: [00-overview.md](00-overview.md), `tasks/gaps.md` §5 (the migration plan),
`tasks/remaining_gaps.md`, and `.claude/rules/csharp/naming-and-structure.md`.

---

## Scope

**In scope** — `README.md` and `MIGRATION.md` only.

**Out of scope**

- Any `.cs` file. This phase writes no C#.
- `tools/Migrate-FluentAssertions.ps1` — phase 11 builds it. This phase only reserves the section that
  documents it.
- Documenting assertions that do not exist yet. The catalog covers **what ships today**; later phases append
  their own rows. A catalog entry for an unimplemented method is a lie the whole plan then has to maintain.

---

## Design

### `README.md` — required section anchors

Later phases append to these by exact name. Do not rename or reorder them.

```markdown
# FatCat.Testing

## What It Is
## Installing
## Quick Start
## Requirements            <- xUnit-only statement lives here (ADR-3)
## Assertion Catalog
### Booleans
### Characters
### Collections           <- created empty with "(phase 04)" placeholder
### DateTimes
### Doubles And Floats
### Enums
### Exceptions            <- created empty with "(phase 03)" placeholder
### Guids
### Numbers
### Objects               <- created empty with "(phase 06)" placeholder
### Strings
### TimeSpans
### Shared (all comparers)
## Negation
## Custom Comparers        <- created empty with "(phase 10)" placeholder
## Value Formatting        <- created empty with "(phase 02)" placeholder
## Coming From FluentAssertions
## Known Limitations
```

Content requirements:

- **What It Is** — a free, Apache-2.0 assertion library; a replacement for FluentAssertions, whose 8.x
  releases are commercially licensed.
- **Requirements** — state plainly that **FatCat.Testing requires xUnit**. `CompareException` derives from
  `XunitException` and `xunit.assert` is the library's only package reference. NUnit, MSTest, TUnit, and
  MSpec are not supported. This is ADR-3 and it must not be soft-pedalled.
- **Assertion Catalog** — one table per family, columns `Assertion | What it asserts`. Populate the families
  that exist today, from `tasks/gaps.md` §1's inventory table, verified against the actual source in
  `FatCat.Testing/`. **Verify, do not copy** — the gaps table is an audit, not a spec.
- **Negation** — `x.Should().Not.Be(y)`. State that there are no `NotXxx` methods and there never will be
  (ADR-A), and link to `MIGRATION.md`.
- **Known Limitations** — xUnit-only; `because` replaces the generated message rather than appending to it;
  no `AssertionScope`; no `.And` / `.Which`; `BeEquivalentTo` ships default options only.

### `MIGRATION.md` — required section anchors

```markdown
# Migrating From FluentAssertions

## 1. Why
## 2. The One Rule            <- NotXxx( -> Not.Xxx(
## 3. Mapping Table
## 4. Type Coverage
## 5. Behavioural Differences
## 6. Known Unsupported
## 7. The Codemod             <- reserved; phase 11 fills it
```

Content requirements:

- **§2 The One Rule** — `.Should().NotXxx(` → `.Should().Not.Xxx(`, uniform because FluentAssertions
  negations are always the literal `Not` plus a PascalCase method name.
- **§3 Mapping Table** — columns `FluentAssertions | FatCat.Testing | Status | Proven by`. Seed it from
  `gaps.md` §5.2 **plus** every method in §2's ranked usage table. Status is `✅ supported` **only** where a
  test class exists today and is named in *Proven by*; everything else is `⬜ pending` with the phase number
  that will deliver it. An unproven `✅` is exactly the failure mode `gaps.md` §5.5 warns about.
- **§4 Type Coverage** — which subject types have a `Should()` overload today. Mark `object`, collections,
  `Action`, and `Func<Task>` as pending with their phase numbers.
- **§5 Behavioural Differences** — the divergences that compile fine and behave differently, which is what
  makes them dangerous:
  1. `because` **replaces** the generated message; FluentAssertions **appends** a reason to it (ADR-2).
  2. No `params object[] becauseArgs` — rewrite `("...{0}", x)` as `($"...{x}")` (ADR-2).
  3. Negation is `.Not.` (ADR-A).
  4. Collection `BeEquivalentTo` is order-insensitive by default, matching FluentAssertions, and there is no
     `WithStrictOrdering` opt-out (ADR-6, ADR-12). Add this row now, marked *(from phase 08)*.
- **§6 Known Unsupported** — `AssertionScope`, `ExecutionTime`, `.And`, `.Which`/`.Subject`, XML, JSON,
  serializability, event monitoring, the reflection/architecture DSL, and the ~45 `BeEquivalentTo` option
  methods other than `Using`/`WhenTypeIs`. Each with the recommended rewrite.

---

## Steps

No TDD — there is no code. Work in this order:

1. Read `FatCat.Testing/` family by family and write the catalog from the **source**, not from `gaps.md`.
   Cross-check against `gaps.md` §1 afterwards and note any disagreement in the phase report — a
   disagreement means the audit has drifted from the code and the next reader needs to know.
2. Write `README.md` with every anchor above. Placeholder sections read exactly:
   `_Ships in phase NN — see `tasks/todo/final_gaps/NN-...md`._`
3. Write `MIGRATION.md` with every anchor above and the seeded mapping table.
4. Re-read both against the anchor lists. A missing anchor breaks a later phase in a fresh session that
   cannot ask you about it.

---

## Files

**Changed**

- `README.md`

**Added**

- `MIGRATION.md`

---

## Verification

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Both must be green — this phase cannot have broken them, and confirming that establishes the baseline count
every later phase compares against. Record the count in the phase report.

Then run the standards review on the uncommitted change. `.claude/rules/powershell/` and the C# rules do not
apply to markdown, so findings should be none; resolve any that appear.

---

## Definition of Done

- [ ] `README.md` contains every anchor in the list above, in that order.
- [ ] Every family that exists in `FatCat.Testing/` today has a catalog table, written from source.
- [ ] No catalog entry describes a method that does not exist.
- [ ] The xUnit-only requirement is stated in `## Requirements` (ADR-3).
- [ ] `MIGRATION.md` exists with every anchor in the list above.
- [ ] The mapping table is seeded from `gaps.md` §5.2 and §2, with `✅` used only where a *Proven by* test
      class is named.
- [ ] §5 lists all four behavioural differences.
- [ ] `dotnet build` and `dotnet test` green; baseline test count recorded in the phase report.
- [ ] Phase report states whether the files already existed (the ADR-8 branch taken) and any `gaps.md`
      disagreement found in step 1.
- [ ] Exactly one commit, message referencing `tasks/todo/final_gaps/01-documentation-foundation.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-01-commit>
```

No build or test impact — this phase touches no code. **Every other phase depends on this one**: reverting
it strands their documentation edits, so revert them first. No manual steps; nothing was published.

---

## Hand-off

Two documents with stable anchors. Every later phase appends to:

- `README.md` → `## Assertion Catalog` → `### <Family>` — one row per assertion shipped.
- `MIGRATION.md` → `## 3. Mapping Table` — one row per FluentAssertions call replaced, flipping the seeded
  `⬜ pending` row to `✅ supported` and naming the test class in *Proven by*.

Phases that own a placeholder section, which they replace rather than append to:

| Section | Owner |
|---|---|
| `README.md` → `## Value Formatting` | phase 02 |
| `README.md` → `### Exceptions` | phase 03 |
| `README.md` → `### Collections` | phases 04, 05, 13 |
| `README.md` → `### Objects` | phases 06, 07 |
| `README.md` → `## Custom Comparers` | phase 10 |
| `MIGRATION.md` → `## 7. The Codemod` | phase 11 |

The baseline test count recorded here is the number every later phase's report compares against.
