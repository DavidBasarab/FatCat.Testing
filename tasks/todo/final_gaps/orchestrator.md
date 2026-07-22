# final_gaps — Phase Orchestrator (Runbook)

Executes the phases in [00-overview.md](00-overview.md) in dependency order, **one fresh session per
phase**. This runbook is the operator's script — follow it literally.

---

## Hard Rules

1. **One phase per session.** Never run two phases in the same session or context window. The phase file is
   the complete handoff; a session that has already executed a phase is contaminated with assumptions the
   next phase must not inherit.
2. **One commit per phase.** No squashing, no amending, no fixup commits rolled into the next phase.
3. **Never push.** Human review gates the push. No `git push`, no force-push, no rebase of published history.
4. **Never touch `C:\Code\FatCat.Toolkit` or `C:\Code\Fog`.** ADR-1. A session that believes it needs to
   edit either repo has misread its phase file — stop and report instead.
5. **Halt on failure.** See the policy below. A failed phase stops the pipeline — dependent phases do not
   start.
6. **Clean tree between phases.** `git status` must be clean before a session starts and after it commits.

---

## Setup (once, before phase 01)

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
git status                                  # must be clean; commit or stash unrelated work first
git branch --show-current
```

- If the current branch is `main` → `git checkout -b task/final_gaps`.
- If the current branch is anything else → stay on it; that branch is the task branch for every phase.
  (At the time this plan was written the current branch was `FillingInMoreGaps`.)

Record the branch name here before starting: `__________________________`

Baseline the suite so later phases can compare counts:

```pwsh
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Note the passing test count: `__________`. Five pre-existing IDE1006 `TestGuid` warnings are baseline
noise, not a phase failure.

Confirm the documentation state so phase 01 knows which half of ADR-8 it is executing:

```pwsh
Get-Content README.md | Measure-Object -Line
Test-Path MIGRATION.md
```

---

## Execution Order

Run strictly top to bottom. Phases on the same line have no dependency on each other and may be run in any
order, but still **one session each**.

| Step | Phase file | Precondition |
|---|---|---|
| 1 | `01-documentation-foundation.md` | branch set, suite green |
| 2 | `02-value-formatting-engine.md` | 01 committed |
| 3 | `03-exception-assertions.md` | 01 committed |
| 4 | `04-collection-entry-points-and-core.md` | 02 committed. **High risk — extra human review before the commit.** |
| 5 | `05-collection-assertions-remainder.md` | 04 committed |
| 6 | `06-object-comparer.md` | 04 committed. **High risk — extra human review before the commit.** |
| 7 | `07-equivalency-engine-objects.md` | 06 committed, **OQ-3 and OQ-4 answered**. **High risk.** |
| 8 | `08-equivalency-collections.md` | 05 and 07 committed |
| 9 | `09-equivalency-configuration.md` | 07 committed, **OQ-5 answered** |
| 10 | `10-extension-point.md` | 06 committed, **OQ-1 answered** |
| 11 | `12-g26-datetimes.md` | 01 committed, **OQ-6 answered** |
| 12 | `13-g26-collections.md` | 05 committed |
| 13 | `14-g26-exceptions.md` | 03 committed |
| 14 | `15-g26-strings.md` | 01 committed |
| 15 | `16-g26-numerics.md` | 01 committed |
| 16 | `17-g26-enums.md` | 01 committed |
| 17 | `18-g26-booleans.md` | 01 committed |
| 18 | `19-g26-guids.md` | 01 committed |
| 19 | `11-migration-codemod.md` | 04–10 committed (it documents what they shipped), **OQ-7 answered** |
| 20 | `20-final-closeout.md` | every phase above committed, **OQ-8 answered** |

Phase 11 is numbered for its dependency position (it only *needs* 01) but is **run late** — its mapping
table has to describe a finished library. Run it earlier only if the pipeline is being paused.

---

## Per-Phase Session Procedure

For each step above:

1. **Start a fresh session** in `C:\Code\FatCat.Testing`.
2. Give it exactly this prompt, substituting the phase file:

   ```
   Execute tasks/todo/final_gaps/<NN-phase-file>.md. That file is your complete
   specification — read it and the rules in .claude/rules/csharp/ before writing
   any code. Follow TDD: tests first, red before green. Do not modify anything
   outside c:\Code\FatCat.Testing. Do not start any other phase. Stop and report
   if the Definition of Done cannot be met.
   ```

3. The session works through the phase file's TDD steps and quality gates.
4. Before committing, the session runs the phase's **Verification** block and ticks the
   **Definition of Done**.
5. Commit message format — one commit, no more:

   ```
   final_gaps phase <NN>: <phase title>

   Implements <gap id> per tasks/todo/final_gaps/<NN-phase-file>.md
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

## Extra Gate For High-Risk Phases (04, 06, 07)

These three change public overload resolution or add a recursive engine. Before the commit, additionally:

1. Run the full suite twice — once normally, once with `--filter "FullyQualifiedName~OverloadResolution"`.
2. Paste the overload-resolution test results into the phase report.
3. **Stop and get a human to read the report before committing.** These are the three phases where a green
   build can still be wrong.

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
   git stash push -u -m "final_gaps phase <NN> failed attempt"   # or: git checkout . ; git clean -fd
   git status                                                    # must be clean
   ```

4. Write a failure report to `tasks/todo/final_gaps/FAILURE-<NN>.md` containing: what was attempted, the
   exact failing output, which Definition-of-Done item could not be met, and the smallest change to the
   phase file that would make it achievable.
5. **Do not start any dependent phase.** Consult the dependency graph in `00-overview.md`. A failure in 01
   halts everything. A failure in 02 halts 04–10 and 13. A failure in 04 halts 05–10 and 13. A failure in 06
   halts 07–10. A failure in 07 halts 08 and 09. A failure in 03 halts 14. A failure in a leaf (12, 15–19)
   halts only 20.
6. Independent leaf phases may continue **only** after a human reads the failure report and says so.

---

## Phase Report (produced by every session before it finishes)

```markdown
## final_gaps phase <NN> — <title>

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

### Repo boundary
- Confirm: no file outside c:\Code\FatCat.Testing was created, modified, or deleted.
```

---

## Rollback

Each phase file carries its own `## Rollback Procedure`. To unwind the pipeline as a whole, revert in
**reverse** dependency order — 20 first, then the leaves, then the chain, then 01 last:

```pwsh
git revert --no-edit <commit-20>
git revert --no-edit <commit-11>
git revert --no-edit <commit-19> <commit-18> <commit-17> <commit-16> <commit-15> <commit-14> <commit-13> <commit-12>
git revert --no-edit <commit-10> <commit-09> <commit-08> <commit-07> <commit-06>
git revert --no-edit <commit-05> <commit-04> <commit-03> <commit-02> <commit-01>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

**The one trap:** reverting 04 without reverting 06 leaves `Should<T>(this T) where T : class` in place with
no concrete-shape collection overloads to beat it — every collection call site then binds to the object
comparer and compiles green while asserting the wrong thing (ADR-4). If 04 is reverted, 06 must be reverted
first.

Never `git reset --hard` a commit that has been reviewed, and never force-push.
