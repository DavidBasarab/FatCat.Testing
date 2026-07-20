# Phase 07 — G2: migration tooling (`MIGRATION.md` finalisation + codemod)

- **Gap:** G2 (`gaps.md` §3, decided) realised as the migration workstream (`gaps.md` §5). The `.Not.`
  decision is a **codemod**, not a library feature (ADR-003).
- **Depends on:** Phase 02 (G1), Phase 03 (G3), Phase 04 (G4), Phase 06 (G6) — every mapping row must point
  at a shipped, tested assertion (`gaps.md` §5.5: a mapping row with no test behind it is a claim). Phase 05
  contributes the custom-assertion authoring section.
- **Depended on by:** nothing in this plan (it is the terminal Tier-1 phase). The actual per-repo migration
  (§5.4, run inside Toolkit/Fog) is downstream work, out of this work item's scope.
- **Risk:** **low–medium** — ships a **public product deliverable** (`MIGRATION.md`, codemod) but writes no
  library runtime code. Codemod is idempotent and `-WhatIf`-able, reducing blast radius.

## Context (complete handoff)

`MIGRATION.md` has been grown row-by-row by Phases 02–06 (each appended its own §5.2 mappings — the "living
document" model of `gaps.md` §4). This phase **finalises** it and delivers the automation. Per ADR-003 the
rewrite is mechanical: FluentAssertions negations are always literal `Not` + a PascalCase method, so
`NotXxx(` → `Not.Xxx(` is regex-clean.

## Deliverable

1. **Finalise `MIGRATION.md`** (repo root, the shipped guide — `gaps.md` §5.1):
   - Intro/motivation (FA 7.0.0 last Apache release; `.Not.` shape difference).
   - The complete §5.2 mapping table, consolidated from Phases 02–06. Verify every row has a proving test in
     the library test project (§5.5); list any row lacking one as a defect to fix, not to ship.
   - The **known-unsupported list** (§5.1.3): `AssertionScope` (G11), `ExecutionTime` (G12), `.Which` (G14),
     the `BeEquivalentTo` option methods (`Excluding`/`Including`/`WithStrictOrdering`/…), and the custom-
     assertion primitives (`Execute.Assertion`/`AssertionChain`/`[CustomAssertion]`, from Phase 05) — each
     with its recommended rewrite.
   - The custom-assertion authoring section from Phase 05 (link or fold in).
   - The §5.4 per-repo sequence (Toolkit first — it references FA from **production** projects; then Fog).

2. **The codemod — `tools/Migrate-FluentAssertions.ps1`** (`gaps.md` §5.3, follows
   `.claude/rules/powershell/powershell.md`):
   - One function per file, `Verb-Noun`, approved verb, PascalCase params, `[switch]` flags, typed params,
     no aliases, full cmdlet names. No Pester tests (PowerShell needs no tests per the rules).
   - Core transform: `find \.Should\(\)\.Not([A-Z]\w*)\(` → `replace .Should().Not.$1(`.
   - **Idempotent** (running twice is a no-op) and **`-WhatIf`-able**; operates on a directory tree.
   - **Reports, does not silently skip**, the cases the regex cannot catch (§5.3): chained negations
     (`.And.NotContain` — `.And` is deferred G13), line-broken `.Should()` / `.NotXxx(` chains, project-
     defined names starting with `Not`, and negations on subjects whose gap has not landed. Emit these to a
     review list.

3. **Verify the mapping table is complete** against the Tier-1 call-site inventory (`gaps.md` §2/§5.2) — the
   table above covers only proven-in-use calls; note that explicitly (it is not the complete negation
   surface).

## TDD note

This phase writes no C#, so the C# TDD rule does not apply, and per the PowerShell rules the codemod gets no
Pester tests. Instead, **prove the codemod on a throwaway fixture directory** (create sample files under the
scratchpad, run `-WhatIf`, then for real, assert the transform + the review-list output by eye) and record
that in the phase report. The mapping table's correctness is proven by the library tests written in
Phases 02–06 (§5.5), not here.

## Verification

- `MIGRATION.md` renders correctly; every mapping row traces to a phase and a test.
- `tools/Migrate-FluentAssertions.ps1` run with `-WhatIf` on a fixture reports intended edits and the review
  list; run for real is idempotent on a second pass.
- Invoke via `pwsh` with the user profile per global rules: `pwsh -Command '. $PROFILE; ...'`.
- `dotnet build` / `dotnet test` still green (no C# changed, but confirm the tree is clean).

## Definition of Done

- [ ] `MIGRATION.md` finalised: intro + complete §5.2 table (every row test-backed) + known-unsupported list
      + custom-assertion authoring + §5.4 per-repo sequence.
- [ ] `tools/Migrate-FluentAssertions.ps1` — idempotent, `-WhatIf`-able, tree-wide, reports uncatchable
      cases, follows the PowerShell rules.
- [ ] Codemod proven on a fixture (recorded in the phase report).
- [ ] All applicable `00-overview.md` DoD gates met; one commit `[tier1_gaps 07] G2 migration tooling`.

## Rollback Procedure

`git revert <phase-07-commit>` removes `MIGRATION.md` finalisation and the codemod script. Because the
codemod only ships here (it edits *consumer* repos, not this one), reverting it has no effect on
FatCat.Testing's own code. No dependents.

## Hand-off (contract exposed to later phases / downstream)

- `MIGRATION.md` — the shipped, public migration guide (product deliverable).
- `tools/Migrate-FluentAssertions.ps1` — the codemod the §5.4 per-repo migration (Toolkit, then Fog) runs.
  That per-repo migration is downstream of this work item.
