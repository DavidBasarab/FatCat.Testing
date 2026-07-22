# Phase 20 — Close-out and replacement-claim audit

- **Work item:** `final_gaps`
- **Gap:** — (verifies the whole plan)
- **Risk:** **low.** Auditing and documentation. May add one reflection-based test. No new assertions.
- **Depends on:** every phase 02–19
- **Depended on by:** —
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Every gap in `remaining_gaps.md` should now be closed: G15 (02), G6 (03), G4 (04/05/08), G1 (06), G3
(07/08/09), G5 (10), G2 (11), G26 (12–19). This phase proves the **library-side replacement claim** (00-overview
*Exit criteria, restated*) and re-surfaces the one deferred decision.

**Repo boundary (ADR-1).** The `remaining_gaps.md` §6 exit criteria are stated in terms of Toolkit and Fog
building green. Those repos are **out of reach** — they migrate later, in their own repos, against the
published package. This phase audits only what this repo can prove: that the library **can** support
everything they use, that every mapping row has a test, and that the migration tooling and docs ship.

**OQ-8 gates this phase** (orchestrator precondition): G16 (framework coupling, ADR-3) was deferred, not
answered. This phase must re-surface it with the full, now-doubled surface in view, so the decision is made
deliberately rather than by omission.

Read before starting: [00-overview.md](00-overview.md) (*Exit criteria, restated*; **ADR-3**; **OQ-8**), all
of `MIGRATION.md`, all of `README.md`, `tasks/gaps.md` §6 (the full-surface comparison),
`tasks/remaining_gaps.md` §6, and `.claude/rules/csharp/testing.md`.

---

## Scope

**In scope**

- An end-to-end audit that every `MIGRATION.md` §3 mapping row names a real, compiling test class.
- Confirming `dotnet build` + `dotnet test` are green with no new warnings across the whole suite.
- Running the codemod (`tools/Convert-FluentAssertions.ps1`) over its fixtures once more and confirming it is
  clean and reports its uncatchable cases.
- A **replacement-claim summary** in `README.md` (or a `tasks/todo/final_gaps/CLOSEOUT.md`): which gaps
  closed, which families are complete, which methods were deliberately omitted (the G26 deep cuts, the
  exception wrappers), and the standing limitations (xUnit-only, `.Not.` shape, `because` replaces,
  order-insensitive equivalency).
- Re-surfacing G16/OQ-8 for a human decision.
- **OQ-6 (phase 12) `2.Hours()` extensions** — confirm they did not collide with anything and are documented.

**Out of scope**

- Any new assertion or comparer. If the audit finds a missing method, that is a **new leaf phase**, not work
  done here — record it and stop.
- Editing Toolkit or Fog (ADR-1).
- Making the G16 decision — this phase **presents** it; the human decides.
- Publishing or pushing.

---

## The audit — how to do it

### 1. Mapping-table integrity

For every row in `MIGRATION.md` §3 marked `✅ supported`:

- The named test class exists under `Tests.FatCat.Testing/`.
- It compiles and passes (it is part of the green suite).
- Its assertion actually exercises the FatCat form in the `FatCat.Testing` column.

Any `⬜ pending` row for an **in-scope** gap (G1–G6, G15, G26, G2) is a **failure** of this audit — every
in-scope gap has a phase that should have flipped it. Rows pending for `tier_2_gaps` gaps (G7–G10) are fine —
that is a different plan. List anything still pending in the report with which plan owns it.

### 2. The optional reflection test (OQ-6 from `tier_2_gaps`, restated here)

Consider one reflection-based test that reads `README.md`'s Assertion Catalog and asserts every public
assertion method on every comparer appears in the catalog — the "the docs cannot silently fall behind the
code" guard. It is the only test of its kind in the project.

- If wanted: `Tests.FatCat.Testing/DocumentationCatalogTests.cs`, reflecting over public methods on the
  comparer types and checking each name appears in `README.md`. Exclude inherited `object` methods and the
  base-class shared assertions (or assert those once). Keep it forgiving of formatting.
- If unwanted: fall back to a **manual audit** — walk each comparer, tick each public method against the
  catalog, record the result in the report. Both paths satisfy this phase; state which was taken.

Get a human steer if unsure; default to the **manual audit** (simpler, no brittle reflection test to
maintain), and note the reflection option as available.

### 3. Codemod re-run

```pwsh
Copy-Item tools/fixtures tools/fixtures-run -Recurse
./tools/Convert-FluentAssertions.ps1 -Path tools/fixtures-run
./tools/Convert-FluentAssertions.ps1 -Path tools/fixtures-run   # idempotent
Remove-Item tools/fixtures-run -Recurse -Force
```

Confirm the transform and the uncatchable-case report are still correct.

### 4. Standing-limitations statement

Confirm `README.md` `## Known Limitations` and `MIGRATION.md` §5/§6 cover: xUnit-only (ADR-3); `.Not.` shape;
`because` replaces rather than appends; no `becauseArgs`; order-insensitive collection equivalency; default
equivalency options only; the deliberate G26 omissions.

### 5. G16 / OQ-8 re-surfacing

Write a short section in the close-out summary stating: the surface roughly doubled across this plan, so a
future framework-detection shim is now more expensive than when ADR-3 deferred it; the decision is still open;
here is the cost either way. **Flag it for the human reviewer** — this is the one decision the plan
deliberately left for after the surface grew, and now is the moment to make it.

---

## Steps

1. Run the full quality-gate suite (below); confirm green, no new warnings.
2. Do the mapping-table integrity audit (§1). Record every discrepancy.
3. Decide the reflection-test question (§2) with a human or take the manual-audit default; execute it.
4. Re-run the codemod (§3).
5. Write the replacement-claim summary (§4, §5) into `README.md`'s closing sections and/or `CLOSEOUT.md`.
6. If the audit found a genuinely missing in-scope method, **stop** and record it as a needed new phase — do
   not implement it here.

---

## Files

**Changed**

- `README.md` (closing sections), `MIGRATION.md` (final consistency pass)

**Added (one of)**

- `Tests.FatCat.Testing/DocumentationCatalogTests.cs` — if the reflection test is chosen
- `tasks/todo/final_gaps/CLOSEOUT.md` — the replacement-claim summary, if kept separate from README

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

Full green, no new warnings. Then the codemod re-run (§3). Then standards review; resolve every finding.

---

## Definition of Done

- [ ] Every `✅ supported` row in `MIGRATION.md` §3 names a real, compiling, passing test class — verified,
      not assumed.
- [ ] No in-scope gap (G1–G6, G15, G26, G2) left `⬜ pending`; any remaining pending row names the plan that
      owns it (only `tier_2_gaps` gaps may remain).
- [ ] The documentation-catalog check done (reflection test **or** manual audit); which was chosen recorded.
- [ ] Codemod re-run clean and idempotent over its fixtures.
- [ ] `README.md` `## Known Limitations` and `MIGRATION.md` §5/§6 cover every standing divergence and the
      deliberate G26 omissions.
- [ ] The replacement-claim summary written (README closing sections or `CLOSEOUT.md`).
- [ ] G16 / OQ-8 re-surfaced with the cost stated and **flagged for the human reviewer**.
- [ ] Any missing in-scope method discovered by the audit recorded as a needed new phase, **not** implemented
      here.
- [ ] `dotnet build`/`dotnet test` green, no new warnings.
- [ ] `dotnet format` / `csharpier` in order, csharpier last (if any C# was added).
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/20-final-closeout.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-20-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Removes the audit artefacts and any documentation-catalog test. No library assertion is affected. This is the
**first** commit to revert when unwinding the whole pipeline (orchestrator rollback), because every other
phase is a dependency of it.

---

## Hand-off

The library-side replacement claim is audited and documented. What remains is **not this plan's work**:

1. **The consuming repos migrate themselves** (ADR-1) — Toolkit first (production projects, NuGet contract
   change), then the G5 custom-assertion port, then Fog — using `MIGRATION.md` and
   `tools/Convert-FluentAssertions.ps1`, in their own repos, against the published package.
2. **The G16 framework-coupling decision** (OQ-8) is open and now flagged with its cost.
3. **`tier_2_gaps`** (G7–G10) is an independent plan; its dictionary phase (13) is unblocked by this plan's
   phase 04 but still owned there.
4. Deferred/optional items recorded across the phases: G17 authoring primitives (decided from phase 10's
   stack-trace observation), the OQ-2 `string?`/expression-body cleanup, nullable-DateTime difference chains
   (phase 12), and the G26 deep-cut collection/exception omissions (phases 13/14).

Nothing is pushed. Human review gates the push.
