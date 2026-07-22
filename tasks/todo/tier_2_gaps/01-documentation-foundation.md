# Phase 01 — Documentation Foundation

- **Work item:** `tier_2_gaps`
- **Gap:** none directly — enables the documentation obligation every other phase carries
- **Risk:** **low.** No C# changes. Touches public-facing docs only; nothing compiles differently.
- **Depends on:** nothing
- **Depended on by:** 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session. Here is everything you need.

`FatCat.Testing` is a standalone assertion library replacing FluentAssertions. It has:

- `FatCat.Testing/` — the library. One folder per type family (`Booleans/`, `Characters/`, `DateTimes/`,
  `Doubles/`, `Enums/`, `Floats/`, `Guids/`, `Numbers/`, `Strings/`, `TimeSpans/`), plus `Comparers/`
  (`ComparerBase`, `NotComparerBase`) and `Exceptions/` (`CompareException`).
- `Tests.FatCat.Testing/` — xUnit tests mirroring that structure.
- `FatCat.Testing/ShouldExtensions.cs` — 27 `Should()` overloads, the public entry point.

Today `README.md` is two lines and there is **no** `MIGRATION.md`. Every later phase in this plan must
append to both documents, and fourteen isolated sessions will invent fourteen different structures unless
the structure is fixed first. That is this phase's entire job.

Source of truth for the gap analysis and migration story: `tasks/gaps.md` (read §5 in full).
Cross-cutting decisions: [00-overview.md](00-overview.md) — ADR-6 governs this phase.

---

## Scope

**In scope**

- Rewrite `README.md` with the full section skeleton and a complete, accurate catalog of the assertions
  that exist **today**.
- Create `MIGRATION.md` at the repo root with the section skeleton, the negation rule, the mapping-table
  header seeded with rows that are true today, the known-unsupported list, and the codemod regex.

**Out of scope**

- Any `.cs` file. If you change C#, you are in the wrong phase.
- `tools/Migrate-FluentAssertions.ps1` — ADR-8 keeps the codemod out of Tier 2.
- Documenting anything not yet implemented as if it were. Coverage tables mark unbuilt families
  "not yet — phase NN".

---

## Deliverable 1 — `README.md`

Replace the file wholesale. Required section order (later phases append **into** these anchors and must
never reorder them):

```markdown
# FatCat.Testing

<one-line description: a free, fluent assertion library for .NET — a replacement for FluentAssertions.>

## Why This Exists
<FluentAssertions 7.0.0 is the last Apache-2.0 release; 8.x is commercially licensed. See tasks/gaps.md.>

## Install
<dotnet add package FatCat.Testing — plus the note that it targets net10.0 and requires xUnit.>

## Quick Start
<a handful of real examples: bool, string, numeric, DateTime, and one `because` override.>

## Negation — the `.Not.` Shape
<FatCat spells negation as a property: x.Should().Not.Be(y). There are no NotXxx methods and there
never will be. Link to MIGRATION.md.>

## Failure Messages
<Messages read "<actual> should [not] <expectation>". Any assertion accepts a trailing
`because` string that replaces the generated message entirely.>

## Assertion Catalog

### Booleans
### Characters
### DateTimes
### Doubles And Floats
### Enums
### Guids
### Numbers
### Strings
### TimeSpans
### Available On Every Comparer

## Coverage Status

## Migrating From FluentAssertions
<pointer to MIGRATION.md>
```

Rules for the catalog:

- One `### <Family>` subsection per folder in `FatCat.Testing/`, in the order above. Later phases insert
  new family subsections **alphabetically** within `## Assertion Catalog`.
- Each subsection lists: the comparer types in that family, then a table of
  `| Assertion | What it asserts |`.
- The list must be **derived from the source**, not from `tasks/gaps.md` §1 — that table is a summary and
  is not authoritative. Enumerate the public methods of each comparer and its `Not` counterpart.
- Where the positive and negated forms differ in availability, say so.
- Under `### Strings`, state explicitly that `Match` is a **wildcard** match (`*` and `?`) and takes an
  `Options` argument. Under `### Numbers` and `### Doubles And Floats`, state that `Match` takes a
  **predicate**. This asymmetry is deliberate — ADR-4.
- `### Available On Every Comparer` documents `ComparerBase` / `NotComparerBase`: `BeOfType`,
  `BeAssignableTo`, `BeOneOf`, and `Satisfy` (positive form only — `NotComparerBase` has no `Satisfy`).

`## Coverage Status` is a table with a row per type family, appended to by later phases:

| Type | Status | Notes |
|---|---|---|
| `bool` / `bool?` | ✅ shipped | |
| … one row per family that exists today … | | |
| `DateTimeOffset` | ⬜ planned | tier_2_gaps phase 07 |
| `DateOnly` | ⬜ planned | tier_2_gaps phase 08 |
| `TimeOnly` | ⬜ planned | tier_2_gaps phase 09 |
| `Uri` | ⬜ planned | tier_2_gaps phase 10 |
| `Type` | ⬜ planned | tier_2_gaps phase 11 |
| `Stream` | ⬜ planned | tier_2_gaps phase 12 |
| `IDictionary<K,V>` | ⬜ blocked | needs collections (G4) |
| objects / collections / exceptions | ⬜ planned | Tier 1 — G1, G3, G4, G6 |

---

## Deliverable 2 — `MIGRATION.md`

New file at the repo root. Required section order:

```markdown
# Migrating From FluentAssertions To FatCat.Testing

## 1. What Is Different And Why
<FatCat is deliberately not a source-compatible clone. The negation shape differs by design (gaps.md
G2). Everything else is intended to map one-to-one.>

## 2. The One Rule
<`NotXxx(` becomes `Not.Xxx(`. FluentAssertions negations are always the literal `Not` followed by a
PascalCase method name, which makes the rewrite mechanical.>

## 3. Mapping Table

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|

## 4. Type Coverage

## 5. Known Unsupported

## 6. The Codemod

## 7. Per-Repo Sequence
```

### §3 Mapping Table — the append target

Four columns, and **every later phase appends rows here**:

- **FluentAssertions** — the call as written today, e.g. `.Should().NotBeNull()`
- **FatCat.Testing** — the replacement, e.g. `.Should().Not.BeNull()`
- **Status** — `✅ supported` / `⬜ pending Gx`
- **Proven by** — the test class that compiles and runs the FatCat form, e.g.
  `Tests.FatCat.Testing.Strings.StringContainTests`. gaps.md §5.5: a mapping row with no test behind it is
  a claim, not a guarantee. A row with an empty **Proven by** cell must be `⬜ pending`.

Seed it with the rows from gaps.md §5.2 **plus** rows for what already works. Mark honestly:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `.Should().NotBeNullOrEmpty()` | `.Should().Not.BeNullOrEmpty()` | ✅ supported | `Tests.FatCat.Testing.Strings.StringBeNullOrEmptyTests` |
| `.Should().NotBeNullOrWhiteSpace()` | `.Should().Not.BeNullOrWhiteSpace()` | ✅ supported | `…Strings.StringBeNullOrWhiteSpaceTests` |
| `.Should().BeGreaterOrEqualTo(x)` | `.Should().BeGreaterThanOrEqualTo(x)` | ⬜ pending G8 | — |
| `.Should().BeLessOrEqualTo(x)` | `.Should().BeLessThanOrEqualTo(x)` | ⬜ pending G8 | — |
| `.Should().MatchEquivalentOf(p)` | `.Should().MatchEquivalentOf(p)` | ⬜ pending G9 | — |
| `.Should().NotBeNull()` | `.Should().Not.BeNull()` | ⬜ pending G1 | — |
| `.Should().NotContain(x)` | `.Should().Not.Contain(x)` | ⬜ pending G4 | — |
| … the remaining gaps.md §5.2 rows … | | | |

Verify each `Proven by` class actually exists under `Tests.FatCat.Testing/` before citing it. Do not cite
a test you have not opened.

### §4 Type Coverage

Mirrors README's `## Coverage Status` but from the migration angle: which FluentAssertions assertion types
have a FatCat equivalent today, which are pending, and which will never exist.

### §5 Known Unsupported

Every FluentAssertions construct with no FatCat equivalent, and the recommended rewrite:

- `AssertionScope` (G11) — no soft assertions. Split into separate assertions, or assert once on a
  composite value.
- `ExecutionTime` (G12) — no equivalent; it also conflicts with the `Task.Delay`/`Thread.Sleep` ban.
- `.Which` (G14) — rewrite as a local variable plus a second assertion.
- `.And` (G13) — unnecessary: FatCat assertions return the comparer, so calls already chain.
- `BeEquivalentTo` option methods (`Excluding`, `Including`, `WithStrictOrdering`,
  `RespectingRuntimeType`, …) — FatCat will ship default options only (G3).
- `NotXxx(...)` method forms — always `Not.Xxx(...)`. This is the rule in §2, restated here as a
  permanent, by-design difference rather than a gap.

### §6 The Codemod

Reproduce gaps.md §5.3 verbatim in substance:

```
find:     \.Should\(\)\.Not([A-Z]\w*)\(
replace:  .Should().Not.$1(
```

…and the four cases it will not catch (chained `.And.NotContain(x)`, line-broken chains, project-defined
assertions whose names start with `Not`, and negations on subjects the library does not cover yet). State
that `tools/Migrate-FluentAssertions.ps1` is a planned deliverable and **does not exist yet** (ADR-8) —
do not imply otherwise.

### §7 Per-Repo Sequence

Summarize gaps.md §5.4: Toolkit first (it references FluentAssertions from production projects that ship
test helpers), then the custom assertion layer, then Fog. Keep it short and link to `tasks/gaps.md`.

---

## Verification

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
git status                                  # only README.md and MIGRATION.md changed
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln              # unchanged count, all green
```

Manual checks:

- Every assertion method named in the README catalog exists in the source. Spot-check at least one
  family end to end by opening the comparer and comparing method-for-method.
- Every `Proven by` test class named in `MIGRATION.md` exists under `Tests.FatCat.Testing/`.
- No row is marked `✅ supported` without a test class in the fourth column.
- Both files render as valid Markdown (tables aligned, no broken links).

---

## Definition of Done

- [ ] `README.md` rewritten with every section in the order specified, including the empty-but-present
      `## Coverage Status` table.
- [ ] The `## Assertion Catalog` lists every assertion in every existing family, derived from source.
- [ ] `Match`'s wildcard-vs-predicate split is documented under both Strings and Numbers (ADR-4).
- [ ] `MIGRATION.md` created with §1–§7 present.
- [ ] §3 mapping table seeded, four columns, every `✅ supported` row naming a real test class.
- [ ] §5 lists all six known-unsupported constructs with a recommended rewrite for each.
- [ ] §6 contains the codemod regex and its four uncatchable cases, and states the script does not exist.
- [ ] No `.cs` file changed (`git status` proves it).
- [ ] `dotnet build` and `dotnet test` still green with an unchanged test count.
- [ ] Standards review run on the uncommitted change; findings resolved.
- [ ] Exactly one commit.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-01-commit>
```

Reverting this phase **cascades to every other phase in the plan** — 02–14 all append to these two files
and their diffs will not apply cleanly without the skeleton. If phases have already landed, revert them
first, in reverse order, per [orchestrator.md](orchestrator.md).

No manual steps. No data, config, or feature-flag changes. Nothing was published.

---

## Hand-off

Later phases may rely on these anchors existing, unchanged:

**`README.md`**
- `## Assertion Catalog` — insert a new `### <Family>` subsection alphabetically.
- `### <Family>` — append rows to the existing `| Assertion | What it asserts |` table.
- `## Coverage Status` — flip a `⬜ planned` row to `✅ shipped`, or add a row.

**`MIGRATION.md`**
- `## 3. Mapping Table` — append rows: `| FluentAssertions | FatCat.Testing | Status | Proven by |`.
- `## 4. Type Coverage` — update the family's row.
- `## 5. Known Unsupported` — remove an entry only when the phase genuinely ships it.

No public C# surface changes in this phase.
