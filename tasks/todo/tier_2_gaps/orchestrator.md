# tier_2_gaps — Phase Orchestrator (Runbook)

Executes the phases in [00-overview.md](00-overview.md) in dependency order, **one fresh session per
phase**. This runbook is the operator's script — follow it literally.

---

## Hard Rules

1. **One phase per session.** Never run two phases in the same session or context window. The phase file
   is the complete handoff; a session that has already executed a phase is contaminated with assumptions
   the next phase must not inherit.
2. **One commit per phase.** No squashing, no amending, no fixup commits rolled into the next phase.
3. **Never push.** Human review gates the push. No `git push`, no force-push, no rebase of published
   history.
4. **Halt on failure.** See the policy below. A failed phase stops the pipeline — dependent phases do not
   start.
5. **Clean tree between phases.** `git status` must be clean before a session starts and after it commits.

---

## Setup (once, before phase 01)

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
git status                                  # must be clean; stash or commit unrelated work first
git branch --show-current
```

- If the current branch is `main` → `git checkout -b task/tier_2_gaps`.
- If the current branch is anything else → stay on it; that branch is the task branch for every phase.

Record the branch name here before starting: `__________________________`

Baseline the suite so later phases can compare counts:

```pwsh
dotnet test Fatcat.Testing.sln
```

Note the passing test count: `__________`. Per `project_toolchain_gotchas`, five pre-existing IDE1006
`TestGuid` warnings are baseline noise — they are not a phase failure.

---

## Execution Order

Run strictly top to bottom. Phases on the same line have no dependency on each other and may be run in any
order, but still **one session each**.

| Step | Phase file | Precondition |
|---|---|---|
| 1 | `01-documentation-foundation.md` | branch created, suite green |
| 2 | `02-numeric-or-equal-to.md` | 01 committed |
| 3 | `03-double-float-or-equal-to.md` | 01 committed |
| 4 | `04-string-match-equivalent-of.md` | 01 committed, **OQ-1 answered** |
| 5 | `05-task-overloads-core.md` | 01 committed, **ADR-2 confirmed by a human reviewer** |
| 6 | `06-task-overloads-remaining.md` | 05 committed |
| 7 | `07-datetimeoffset-family.md` | 01 committed |
| 8 | `08-dateonly-family.md` | 01 committed |
| 9 | `09-timeonly-family.md` | 01 committed |
| 10 | `10-uri-family.md` | 01 committed |
| 11 | `11-type-family.md` | 01 committed, **OQ-4 answered** |
| 12 | `12-stream-family.md` | 01 committed, **OQ-5 answered** |
| — | `13-dictionary-family-blocked.md` | **DO NOT RUN.** Gated on G4 (Tier 1). |
| 13 | `14-tier-2-closeout.md` | 02–12 all committed, **OQ-6 answered** |

---

## Per-Phase Session Procedure

For each step above:

1. **Start a fresh session** in `C:\Code\FatCat.Testing`.
2. Give it exactly this prompt, substituting the phase file:

   ```
   Execute tasks/todo/tier_2_gaps/<NN-phase-file>.md. That file is your complete
   specification — read it and the rules in .claude/rules/csharp/ before writing
   any code. Follow TDD: tests first, red before green. Do not start any other
   phase. Stop and report if the Definition of Done cannot be met.
   ```

3. The session works through the phase file's TDD steps and quality gates.
4. Before committing, the session runs the phase's **Verification** block and ticks the
   **Definition of Done**.
5. Commit message format — one commit, no more:

   ```
   tier_2_gaps phase <NN>: <phase title>

   Implements <gap id> per tasks/todo/tier_2_gaps/<NN-phase-file>.md
   README.md and MIGRATION.md updated.
   ```

6. The session ends with the **Phase Report** (below). Read it before starting the next phase.
7. Verify from the operator side:

   ```pwsh
   git log --oneline -1
   git status            # must be clean
   ```

---

## Quality Gates (every phase, in this order)

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
dotnet build Fatcat.Testing.sln                  # zero new warnings
dotnet test Fatcat.Testing.sln                   # all green
dotnet format style Fatcat.Testing.sln
dotnet format analyzers Fatcat.Testing.sln
dotnet csharpier .                               # ALWAYS the final step after any C# change
dotnet build Fatcat.Testing.sln                  # confirm formatting did not break the build
```

Then, before the commit, run a standards review on the uncommitted change and resolve every finding:

```
/standards-review   (review all uncommitted changes)
```

`dotnet csharpier .` is the last tool to touch C# in every phase. Do not hand-format afterwards.

---

## Halt-On-Failure Policy

A phase **fails** when its Definition of Done cannot be met — a red test, a new compiler warning, a
standards-review finding that cannot be resolved within the phase's scope, or a design assumption in the
phase file that turns out to be wrong.

On failure:

1. The session may make **two** self-correction attempts. Not three.
2. If still failing, **stop**. Do not commit partial work.
3. Leave the tree clean:

   ```pwsh
   git stash push -u -m "tier_2_gaps phase <NN> failed attempt"   # or: git checkout . ; git clean -fd
   git status                                                     # must be clean
   ```

4. Write a failure report to `tasks/todo/tier_2_gaps/FAILURE-<NN>.md` containing: what was attempted, the
   exact failing output, which Definition-of-Done item could not be met, and the smallest change to the
   phase file that would make it achievable.
5. **Do not start any dependent phase.** Consult the dependency graph in `00-overview.md` — a failure in 01
   halts everything; a failure in 05 halts 06 and 14; a failure in a leaf phase (02, 03, 04, 07–12) halts
   only 14.
6. Independent leaf phases may continue **only** after a human reads the failure report and says so.

---

## Phase Report (produced by every session before it finishes)

```markdown
## tier_2_gaps phase <NN> — <title>

### Files
- Added: ...
- Changed: ...
- Deleted: ...

### Tests
- New test classes: N
- New [Fact] methods: N
- Suite: N passing / N total (baseline was N)

### Deviation Log
- <every departure from the phase file, and why. "None" is a claim that must be
  true, not a default — if the phase ran exactly as written, say so explicitly.>

### Open Questions / Discovered Risks
- <anything the next phase or the human reviewer needs to know>

### Documentation
- README.md sections touched: ...
- MIGRATION.md rows added: N
```

---

## Rollback

Each phase file carries its own `## Rollback Procedure`. To unwind the pipeline as a whole, revert in
**reverse** dependency order — 14 first, then leaves, then 06, then 05, then 01 last:

```pwsh
git revert --no-edit <commit-14>
git revert --no-edit <commit-12> <commit-11> <commit-10> <commit-09> <commit-08> <commit-07>
git revert --no-edit <commit-06> <commit-05> <commit-04> <commit-03> <commit-02>
git revert --no-edit <commit-01>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Never `git reset --hard` a commit that has been reviewed, and never force-push.
