# Phase 11 — Migration codemod + `MIGRATION.md` completion (G2)

- **Work item:** `final_gaps`
- **Gap:** **G2** (`remaining_gaps.md` §4 · `gaps.md` §5)
- **Risk:** **low.** A PowerShell script operating on fixture files in this repo, plus documentation. No
  library C# changes. **Runs late** despite its shallow dependency (only 01) because its mapping table must
  describe a finished library — run it after 04–10 land.
- **Depends on:** 01 (documents), and in content: 04–10 (it describes what they shipped)
- **Depended on by:** 20 (close-out audits the codemod)
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

FatCat.Testing is **not** a source-compatible clone of FluentAssertions. The negation shape differs by design
(ADR-A: `.Not.Be(x)`, never `NotBe(x)`). So the library ships a migration path, and that path is a product
deliverable — anyone coming from FluentAssertions must be able to follow it.

This phase builds the codemod script and completes `MIGRATION.md`. **`tier_2_gaps` ADR-8 deferred the
codemod** on the grounds that its output does not compile until G1/G3/G4 close. This plan closes them
(phases 04–08), so the objection has expired (ADR-10).

**Repo boundary (ADR-1).** The script is **tested against fixture files inside this repo** — never run
against Toolkit or Fog. Those repos run it themselves, later, in their own repos.

**OQ-7 gates this phase** (orchestrator precondition): does the script ship inside the NuGet package or only
in the GitHub repo? Proposal: **repo-only** under `tools/` — packaging a PowerShell script into a library
`.nupkg` changes the package layout for no consumer benefit. Confirm before committing.

Read before starting: [00-overview.md](00-overview.md) (**ADR-10**, **OQ-7**), `tasks/gaps.md` §5 (the whole
migration plan — §5.1 deliverables, §5.2 mapping table, §5.3 the codemod and its uncatchable cases, §5.4
per-repo sequence, §5.5 testability), `.claude/rules/powershell/powershell.md`, and the current
`MIGRATION.md`.

---

## Scope

**In scope**

- `tools/Migrate-FluentAssertions.ps1` — the codemod, following the PowerShell rules (one function per file,
  `Verb-Noun`, typed params, `[switch]` flags, no aliases, no `#Requires`).
- Fixture `.txt`/`.cs.fixture` files in this repo the script transforms, and a verification that the
  transform is correct and idempotent.
- `MIGRATION.md` → `## 7. The Codemod` — the regex, how to run it, and the four cases it cannot catch.
- A final pass over `MIGRATION.md` §3 to confirm **every** row that a phase marked `✅ supported` names a
  real test class, and every still-`⬜ pending` row points at a real phase (there should be none pending for
  in-scope gaps by now).

**Out of scope**

- Running the script against Toolkit or Fog (ADR-1). Fixtures only.
- Building the FluentAssertions→FatCat **Roslyn analyzer** (G25) — that is a different, larger idea; the
  regex codemod is what §5.3 specifies.
- Rewriting any library code. This phase adds a tool and finishes a document.
- Handling `.And` chaining rewrites automatically — `.And` is deferred (G13); the script **reports** those
  for manual rewrite (§5.3).

---

## Design

### The core transform (`gaps.md` §5.3)

```
find:     \.Should\(\)\.Not([A-Z]\w*)\(
replace:  .Should().Not.$1(
```

`.Should().NotBeNull()` → `.Should().Not.BeNull()`, uniform because FluentAssertions negations are always the
literal `Not` + a PascalCase method name.

### Script contract (`.claude/rules/powershell/`)

`tools/Migrate-FluentAssertions.ps1` contains exactly one function, `Invoke-FluentAssertionsMigration` (an
approved verb — `Invoke`), file named after the function per the one-function-per-file rule. Wait: the file
must be named after the function. So either name the file `Invoke-FluentAssertionsMigration.ps1`, or name the
function `Migrate-FluentAssertions` — but `Migrate` is **not** an approved verb (`Get-Verb`). **Use
`Convert-FluentAssertions`** (`Convert` is approved) so the file `tools/Convert-FluentAssertions.ps1` and the
function agree. Record this naming resolution in the report; `remaining_gaps.md` §5.1 says
`Migrate-FluentAssertions.ps1`, and this is a deliberate, rules-driven deviation.

```powershell
function Convert-FluentAssertions
{
	param(
		[Parameter(Mandatory = $true)]
		[string]$Path,

		[switch]$WhatIf
	)

	# recurse *.cs under $Path, apply the regex, honour -WhatIf, report uncatchable cases
}
```

Requirements:

- **Idempotent** — running twice changes nothing the second time (already-rewritten `.Not.Xxx(` does not
  match the find pattern).
- **`-WhatIf`** — lists intended changes without writing.
- **Reports, never silently skips**, the four uncatchable cases (`gaps.md` §5.3):
  1. Chained negations `.And.NotContain(x)` — `.And` deferred (G13); flag for manual split.
  2. Line-broken chains where `.Should()` and `.NotXxx(` are on different lines.
  3. Custom assertions whose names begin with `Not` but are project-defined, not FluentAssertions.
  4. Negations on a subject whose gap has not landed — not applicable inside this repo, but the report
     wording must still describe it for consumers.
- Excludes `bin/` and `obj/`.
- No aliases; full cmdlet names (`Get-ChildItem`, `Where-Object`).

### Fixtures

`tools/fixtures/` (or `Tests.../MigrationFixtures/` — pick one and state it) with small text files exercising:
a plain `.Should().NotBeNull()`; an already-migrated `.Should().Not.BeNull()` (proves idempotency); a
line-broken chain (must be reported, not mangled); a `.And.NotContain(x)` (reported); a project-defined
`NotSomething(` that is *not* a FluentAssertions negation (reported). The verification asserts the script's
output against expected post-transform fixtures and asserts the report lists exactly the cases it should.

Because PowerShell needs no tests (`.claude/rules/powershell/`), the "verification" is a scripted run in the
phase's Verification block comparing output to expected fixtures — not an xUnit test. Do not add a Pester
test.

---

## Steps

No TDD (no C#). Work in this order:

1. Write the fixtures and their expected post-transform counterparts.
2. Write `tools/Convert-FluentAssertions.ps1`.
3. Run it with `-WhatIf` over the fixtures; confirm the intended changes match expectations.
4. Run it for real over a **copy** of the fixtures; diff against expected; run again to prove idempotency.
5. Confirm the uncatchable-case report lists exactly the four fixtures designed to trigger it.
6. Write `MIGRATION.md` §7 (the regex, the run command with `-WhatIf`, the four uncatchable cases, the §5.4
   per-repo sequence note).
7. Audit `MIGRATION.md` §3: every `✅` names a real test class; resolve any `⬜ pending` for an in-scope gap
   (flip it or explain why it is still pending). List any genuinely-still-pending rows in the report.

---

## Files

**Added**

- `tools/Convert-FluentAssertions.ps1`
- `tools/fixtures/` — input and expected-output fixture files

**Changed**

- `MIGRATION.md`

---

## Documentation Updates

**`MIGRATION.md` → `## 7. The Codemod`** — replace the phase-01 reservation. Content: the regex; the run
command (`. $PROFILE; ./tools/Convert-FluentAssertions.ps1 -Path <dir> -WhatIf` then without `-WhatIf`);
idempotency note; the four uncatchable cases with the recommended manual fix for each; and the §5.4 per-repo
order (Toolkit first — production projects, NuGet contract change — then the G5 custom-assertion port, then
Fog).

**`MIGRATION.md` → `## 3` audit** — as step 7.

**`README.md`** — one line under `## Coming From FluentAssertions` pointing at `MIGRATION.md` and the codemod.

---

## Verification

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing

# Library must still be green — this phase touched no C#, confirm nothing regressed.
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln

# Codemod dry run, then real run over a scratch copy of the fixtures, then idempotency.
Copy-Item tools/fixtures tools/fixtures-run -Recurse
./tools/Convert-FluentAssertions.ps1 -Path tools/fixtures-run -WhatIf
./tools/Convert-FluentAssertions.ps1 -Path tools/fixtures-run
./tools/Convert-FluentAssertions.ps1 -Path tools/fixtures-run   # second run: no changes
Remove-Item tools/fixtures-run -Recurse -Force
```

Compare the transformed scratch files to the expected fixtures (diff). Confirm the uncatchable-case report.
Then run the standards review — the C# rules do not apply to PowerShell or markdown, but the PowerShell rules
do; resolve any finding.

---

## Definition of Done

- [ ] OQ-7 confirmed (repo-only vs packaged); recorded in the report.
- [ ] `tools/Convert-FluentAssertions.ps1` exists, one function, approved verb, file named after the
      function, typed params, `[switch] $WhatIf`, no aliases, no `#Requires`.
- [ ] The regex transform is correct on the fixtures, idempotent on a second run.
- [ ] `-WhatIf` lists changes without writing.
- [ ] All four uncatchable cases are **reported**, not silently skipped — proven against trigger fixtures.
- [ ] `bin/`/`obj/` excluded.
- [ ] `MIGRATION.md` §7 written; §3 audited so every `✅` names a real test class and no in-scope gap is left
      `⬜ pending`.
- [ ] The naming deviation (`Convert-` not `Migrate-`) recorded with its rules justification.
- [ ] `dotnet build`/`dotnet test` still green (no C# changed).
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/11-migration-codemod.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched — including: the script was run only against fixtures
      in this repo, never against Toolkit or Fog.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-11-commit>
```

Removes the script, fixtures, and `MIGRATION.md` §7. No code impact; build and tests unaffected. Phase 20
audits the codemod — revert it first if it landed.

---

## Hand-off

Deliverables:

- `tools/Convert-FluentAssertions.ps1` — the shipped codemod, tested against in-repo fixtures.
- `MIGRATION.md` complete: the one rule (§2), the full mapping table with every row proven (§3), type
  coverage (§4), behavioural differences (§5), known-unsupported (§6), the codemod (§7).

For the consuming repos (their own future work, not this plan): run the script per §5.4 — Toolkit first, then
port the custom assertions onto the G5 base (phase 10), then Fog. The script reports what it cannot rewrite;
those are hand-edited there.

For **phase 20**: the codemod and `MIGRATION.md` are inputs to the replacement-claim audit — every mapping
row must have a test, and the codemod must run clean over the fixtures.
