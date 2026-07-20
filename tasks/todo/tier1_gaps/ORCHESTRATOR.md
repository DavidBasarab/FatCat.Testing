# tier1_gaps — Phase Orchestrator (Runbook)

This runbook executes the Tier-1 phases in dependency order. It is a **runbook, not an automated script**,
because the hard requirement below cannot be satisfied any other way.

## The non-negotiable rule: one fresh session per phase

**Context isolation is not optional.** Each phase runs in its **own fresh Claude Code session/context
window**. Never run two phases in one session. The phase file (`NN-*.md`) plus `00-overview.md` plus the
`.claude/rules/csharp` rules are the complete handoff — an executing session needs nothing carried over from
a prior phase's context.

There is therefore no single script that "runs all phases." The orchestrator is: for each phase, in order,
open a new session, hand it the phase file, let it complete through commit, verify, then move on.

## Execution order (respect the dependency graph in `00-overview.md`)

```
01  (formatting)            ─ root, no deps
02  (G1 object Should)      ─ needs 01
03  (G3 BeEquivalentTo)     ─ needs 01, 02
04  (G4 collections)        ─ needs 01, 02, 03
05  (G5 extension point)    ─ needs 02, 03, 04
06  (G6 exceptions)         ─ independent — run any time (good parallel/warm-up track)
07  (G2 migration tooling)  ─ needs 02, 03, 04, 06 (05 contributes)
```

Recommended linear order: **01 → 02 → 03 → 04 → 05 → 06 → 07**. 06 may be pulled earlier or run on a parallel
worktree since it has no code dependency; if parallelised, it must still land before 07.

## Per-phase procedure

For each phase `NN`:

1. **Fresh session.** Start a new session. Provide only: `tasks/todo/tier1_gaps/NN-*.md`,
   `tasks/todo/tier1_gaps/00-overview.md`, and the repo.
2. **Branch.** Ensure work is on `task/tier1_gaps` (create off `main` if not present; if already on a
   non-`main` branch use it — `task.md` policy). Never work on `main`.
3. **Confirm prerequisites landed.** The phase's `Depends on` commits are present on the branch.
4. **Execute the phase** TDD-first, honouring every DoD gate in `00-overview.md` and the phase file. Resolve
   the phase's open question(s) before finalising; **stop and ask the human** if an OQ answer would
   materially change the design (esp. OQ-1 object equality, OQ-2 config-hook shape).
5. **Quality gates (run from the repo root, where the sln lives):**
   ```
   dotnet build   Fatcat.Testing.sln
   dotnet test    Fatcat.Testing.sln
   dotnet format style     Fatcat.Testing.sln
   dotnet format analyzers Fatcat.Testing.sln
   dotnet format style     Fatcat.Testing.sln --verify-no-changes
   dotnet csharpier .
   ```
   Then run `code-review` (standards-review skill) on the uncommitted diff and resolve findings.
6. **Commit — exactly one, atomic.** Message references the phase file, e.g.
   `[tier1_gaps 03] G3 BeEquivalentTo + config hook (tasks/todo/tier1_gaps/03-g3-be-equivalent-to.md)`.
   **Never** squash, amend across phases, rebase published history, force-push, or push to remote — human
   review gates the push.
7. **Phase report** (from `task.md`): files added/changed/deleted, test counts (new/total/passing), the
   **deviation log** (every departure from the phase file + why — an empty log is a claim, not a default),
   and open questions/discovered risks for the reviewer.
8. **Advance** to the next phase in a new session.

## Halt-on-failure policy

If a phase's **Definition of Done cannot be met after 2 self-correction attempts**:

1. **STOP the pipeline.** Do not start any dependent phase.
2. **Leave the working tree clean** — `git stash` or discard the incomplete work so the branch stays at the
   last good commit.
3. **Write a failure report** (what failed, the two attempts, the blocking cause, suggested next step) under
   `tasks/todo/tier1_gaps/` (e.g. `NN-FAILURE.md`).
4. Wait for human intervention. Because reverts cascade downward (see `00-overview.md`), a halt at phase N
   leaves N+1… unstarted by design.

## Guardrails (apply to every phase)

- Never force-push, never rebase published history, never push to remote.
- One commit per phase; commit boundary between every phase; no squashing/amending across phases.
- `main` is never a working branch.
- Each phase's `## Rollback Procedure` is the down-migration; a revert of phase N cascades to its
  `Depended on by` set.

## Post-pipeline

After 07 lands, all Tier-1 gaps are closed and `MIGRATION.md` + the codemod exist. The actual per-repo
migration (`gaps.md` §5.4: Toolkit first, then Fog) is **downstream work outside this work item** and is not
part of this pipeline.
