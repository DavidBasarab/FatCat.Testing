# Phase 14 — Tier 2 Close-Out And E2E Walk

- **Work item:** `tier_2_gaps`
- **Gap:** none new — this phase **proves** G7, G8, G9, and G10 are actually closed
- **Risk:** **low.** Verification and documentation. The only code it may add is one test class (OQ-6).
- **Depends on:** 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12 (13 is blocked and **not** required)
- **Depended on by:** nothing
- **Precondition:** **OQ-6 answered** — see "Path A / Path B" below.
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Eleven phases have landed on this branch, each adding assertions and each claiming to have updated
`README.md` and `MIGRATION.md`. The work item's acceptance criterion is:

> - [ ] Items in tier 2 completed with an update to the read me with each phase.

That is a claim about eleven separate commits made in eleven isolated sessions. **This phase verifies it
rather than assuming it.** An audit that finds nothing is only credible if it actually looked.

What landed:

| Phase | Shipped |
|---|---|
| 02 | `BeGreaterThanOrEqualTo` / `BeLessThanOrEqualTo` on `Numbers/` |
| 03 | the same on `Doubles/` and `Floats/` |
| 04 | `MatchEquivalentOf` on strings; `Match` semantics documented |
| 05 | `TaskResultReader` + `Task<T>` overloads for bool/string/numerics |
| 06 | `Task<T>` overloads for char/DateTime/TimeSpan/Guid/enums |
| 07–12 | `DateTimeOffset`, `DateOnly`, `TimeOnly`, `Uri`, `Type`, `Stream` families |
| 13 | **blocked** — `IDictionary<K,V>`, gated on G4 |

Read before starting: [00-overview.md](00-overview.md), every phase file's `## Hand-off` section, and
`tasks/gaps.md` §3 (Tier 2) and §5.

---

## Scope

**In scope** — the audit, the E2E walk, the discrepancy fixes it turns up, and the final close-out summary
in `MIGRATION.md`.

**Out of scope**

- New assertions. If the audit finds a genuinely missing assertion, **do not implement it here** — record
  it as a finding and let it be its own phase. This phase fixes documentation and tests, not surface.
- Anything Tier 1 or Tier 3.
- Unblocking phase 13.

---

## The E2E Walk

Work through this in order. Each step produces evidence for the report — a command's output, a file
reference, or an explicit "verified by reading X".

### Step 1 — Every Tier 2 gap has shipping code

For each of G7, G8, G9, G10, name the source files that close it and the tests that prove it:

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
Get-ChildItem FatCat.Testing -Recurse -Filter *.cs | Select-Object -ExpandProperty FullName
Select-String -Path FatCat.Testing\**\*.cs -Pattern 'BeGreaterThanOrEqualTo|BeLessThanOrEqualTo|MatchEquivalentOf'
Select-String -Path FatCat.Testing\ShouldExtensions.cs -Pattern 'Task<'
```

G10 is complete **except** `IDictionary<K,V>` (phase 13, blocked). Say that plainly — a "G10 closed" claim
without that caveat is false.

### Step 2 — The suite is green and grew

```pwsh
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Compare the total against the baseline the orchestrator recorded before phase 01. Report both numbers. Zero
skipped tests; zero new warnings beyond the five baseline IDE1006 `TestGuid` ones.

### Step 3 — Every new assertion is reachable from `Should()`

For each family added in 07–12, write one throwaway assertion in a scratch test and confirm it compiles and
passes. If they already exist from the phase's own test set, cite those instead of duplicating them — the
point is to prove the entry point resolves from a consumer's position, not to pad the suite.

### Step 4 — README catalog matches the code

**Path A (preferred, if OQ-6 was answered yes):** add one reflection-based test,
`Tests.FatCat.Testing/Documentation/AssertionCatalogTests.cs`:

- locate `README.md` by walking up from `AppContext.BaseDirectory` until the file is found;
- reflect over every public type in the `FatCat.Testing` assembly whose name ends in `Comparer`;
- for each public assertion method (excluding `Not`, property getters, and members inherited from
  `object`), assert its name appears somewhere in the README;
- one `[Fact]`, one assertion, failure message naming every missing method.

This is the only reflection-and-file-IO test in the project. It is justified because it converts the work
item's acceptance criterion from a promise into a gate. Keep it in its own folder and its own file, derive
from `BaseTest`, block bodies only, no underscores in the name.

**Path B (fallback, if OQ-6 was answered no):** do it by hand. Enumerate the public assertion methods per
family, diff them against the README catalog by eye, and record the comparison **in the phase report** as a
table so the audit is reviewable. Do not claim it was checked without showing the table.

Either way: fix any drift found — the README is what changes, not the code.

### Step 5 — Every `MIGRATION.md` mapping row is honest

For every row in `## 3. Mapping Table`:

- `✅ supported` requires a non-empty **Proven by** cell;
- the named test class must exist — verify, do not trust;
- that class must actually exercise the FatCat form in the second column (gaps.md §5.5: a mapping row with
  no test behind it is a claim, not a guarantee);
- `⬜ pending` rows name a real gap ID that is genuinely still open.

```pwsh
Get-ChildItem Tests.FatCat.Testing -Recurse -Filter *Tests.cs | Select-Object -ExpandProperty Name
```

Downgrade any row that fails the check and record it as a finding.

### Step 6 — The doc-update obligation held for every phase

```pwsh
git log --oneline --stat main..HEAD
```

Every phase commit from 02 onward must touch `README.md` **and** `MIGRATION.md`. Any that did not is a
finding: fix the documentation here, and name the phase in the report.

### Step 7 — Coverage tables agree with reality

`README.md` `## Coverage Status` and `MIGRATION.md` `## 4. Type Coverage` must tell the same story, and it
must match the code: `DateTimeOffset`, `DateOnly`, `TimeOnly`, `Uri`, `Type`, `Stream`, `Task<T>` shipped;
`IDictionary<K,V>` blocked on G4; objects, collections, exceptions, and structural equality pending Tier 1.

### Step 8 — The library's stated invariants still hold

```pwsh
Select-String -Path FatCat.Testing\**\*.cs -Pattern '\.Result\b|\.Wait\(\)|GetAwaiter\(\)'
Select-String -Path FatCat.Testing\**\*.cs -Pattern '=>\s*[^;]+;' 
Select-String -Path FatCat.Testing\**\*.cs -Pattern 'string\?'
Select-String -Path Tests.FatCat.Testing\**\*.cs -Pattern 'DateTime\.(Now|UtcNow)'
```

Expected: exactly one blocking hit (`Tasks/TaskResultReader.cs`); no expression-bodied members outside
lambdas — inspect each hit, the pattern is deliberately loose; `string?` only in the pre-existing
`Strings/` region and `ShouldExtensions.cs` plus phase 04's two new methods (OQ-2, a recorded deviation);
no clock reads in tests. Anything else is a finding.

---

## Documentation Updates

**`MIGRATION.md` → new `## 8. Tier 2 Status`** — the one section this plan adds after phase 01 fixed the
skeleton (ADR-6 permits it because it is a close-out, and it is stated here rather than invented in the
moment). Contents:

- G7, G8, G9 closed; G10 closed except `IDictionary<K,V>`;
- what a migrating repo can now do that it could not before Tier 2;
- what still blocks a real migration: G1, G3, G4, G5, G6 (Tier 1) — a repo cannot migrate on Tier 2 alone,
  and the guide must not imply otherwise;
- a pointer to `tasks/gaps.md` for the full picture.

**`README.md` → `## Coverage Status`** — final pass so the table is accurate as of this commit.

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

Then run the standards review across the whole branch diff (`main..HEAD`), not just this commit — this is
the last gate before human review.

---

## Definition of Done

- [ ] All eight E2E steps executed, each with evidence in the report.
- [ ] Suite green; baseline and final test counts both reported.
- [ ] Every Tier 2 gap mapped to the files that close it, with the `IDictionary` caveat stated.
- [ ] README catalog verified against the code — Path A test added and passing, or Path B table in the
      report.
- [ ] Every `✅ supported` mapping row has a verified `Proven by` test class; unverifiable rows downgraded.
- [ ] Every phase commit from 02 onward confirmed to touch both docs; exceptions named.
- [ ] Coverage tables in both documents agree with each other and with the code.
- [ ] Invariant greps run; every hit explained.
- [ ] `MIGRATION.md` `## 8. Tier 2 Status` written, including what still blocks migration.
- [ ] No new assertions added.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/14-tier-2-closeout.md`.

---

## Final Report (in addition to the standard phase report)

This is the report a human reads before deciding to push the branch:

```markdown
## tier_2_gaps — Tier 2 Close-Out

### Acceptance Criteria
- [ ] Items in tier 2 completed with an update to the readme with each phase
      <verdict, with the per-phase evidence from Step 6>

### Gap Status
| Gap | Status | Closed by | Proven by |
|---|---|---|---|
| G7 | ... | ... | ... |
| G8 | ... | ... | ... |
| G9 | ... | ... | ... |
| G10 | partial — IDictionary blocked on G4 | ... | ... |

### Test Counts
baseline N -> final N (+N)

### Findings And Fixes
<every discrepancy the audit found, and whether this phase fixed it or deferred it>

### Deviation Log (whole plan)
<consolidated from all eleven phase reports — including phase 04's `string?` deviation
 and phase 05's async.md blocking exception>

### Open Questions And Risks For The Human Reviewer
- OQ-1 (StartWith/EndWithEquivalentOf casing) — still open?
- OQ-2 (#nullable enable cleanup in Strings/) — recommend a follow-up phase
- phase 03's NaN comparison behaviour
- phase 05's sync-over-async deadlock hazard
- phase 13 blocked on G4
- G1 collision inventory: Should(this Uri), Should(this Type), Should(this Stream),
  Should<T>(this Task<T>) where T : struct, Enum

### Branch
<branch name>, N commits, NOT pushed — human review gates the push.
```

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-14-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Nothing depends on this phase. Reverting it removes the audit, the close-out section, and (on Path A) the
catalog test — the shipped assertions are untouched. This phase must be reverted **before** any phase it
audits.

---

## Hand-off

No public C# surface added on Path B. On Path A, one internal-facing test class:
`Tests.FatCat.Testing.Documentation.AssertionCatalogTests`, which every future phase that adds an assertion
must keep green — meaning **the README stops being optional** and becomes a build gate. Say so explicitly
in the report; it is the most consequential thing this phase can leave behind.

The Tier 2 workstream ends here. The next work is Tier 1 (G1 → G3/G4/G5 → G6), and the G1 plan inherits the
collision inventory listed in the final report.
