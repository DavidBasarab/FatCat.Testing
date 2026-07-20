# Phase 05 — G5: custom-assertion extension point

- **Gap:** G5 (`gaps.md` §3). ~410 call sites in the two repos go through **project-defined** assertions.
  FatCat must offer a documented, supported way to write a custom comparer.
- **Depends on:** Phase 02 (G1), Phase 03 (G3), Phase 04 (G4) — the extension surface is documented against
  the final comparer shape.
- **Depended on by:** Phase 07 (migration guide references the extension point); future G7 (`Task<T>`
  overloads, Tier 2) builds on it.
- **Risk:** **HIGH** — defines/blesses a **public API contract** that consumers derive from; once shipped it
  is hard to change. Extra human review.

## Context (complete handoff)

Both consuming repos build custom assertions on FluentAssertions' `ReferenceTypeAssertions<TSubject,
TAssertions>` and return `AndConstraint<T>`. Crucially (`gaps.md` §G5 mitigating factor + §6.1 G17): those
custom assertions **delegate to inner `.Should()` calls** — they do NOT use `Execute.Assertion` /
`AssertionChain`. So the minimum viable extension point is: a public base to derive from + a public failure
primitive. Per ADR-006 (`00-overview.md`), **we expose the existing `ComparerBase`, we do not invent a new
base**, and we do NOT build `AndConstraint` / `AssertionChain` / `[CustomAssertion]` (those are G17, out of
scope).

Today's blockers to a consumer deriving a comparer:
- `ComparerBase<TSubject, TComparer>.Subject` is `protected` (`ComparerBase.cs:8`) — usable from a subclass,
  so probably already sufficient (see OQ-4).
- `CompareException.New(string)` is public — the failure primitive is already reachable.
- There is no documentation stating this is the supported extension mechanism, and no sample.

## Deliverable

This phase is **mostly documentation + a proving sample + minimal accessibility confirmation** — little or
no new runtime surface if OQ-4 confirms `protected` suffices.

1. **Confirm/relax accessibility (OQ-4).** Verify a comparer derived in a *separate assembly* can:
   (a) read `Subject`, (b) call `CompareException.New`, (c) return `this` for chaining. Write a proving test
   comparer in the **test project** (a different assembly) that derives from `ComparerBase` and implements a
   trivial assertion by delegating to an inner `.Should()`. If `protected Subject` is genuinely inaccessible
   for the intended pattern, add the **minimum** accessor (prefer keeping `protected`; only widen if the
   proving test forces it). Do not widen speculatively (`not-allowed.md` — no speculative surface).
2. **Document the supported pattern** in `MIGRATION.md` (and a short section in `README.md` if it fits the
   existing doc style): how to write a custom comparer — derive from `ComparerBase<TSubject, TComparer>`,
   forward the subject via the primary constructor, add assertion methods that delegate to inner `.Should()`
   calls or throw via `CompareException.New(because ?? "...")`, return `this`. Show the negated-comparer
   convention (`Not` property) as optional for consumer comparers.
3. **Provide a worked example** mirroring the consumers' real shape — a `WebResult`-style comparer
   (`ResultShouldExtensions.Should(this FakeWebResult)` → `WebResultComparer` with `BeOk()`, `BeNotFound()`)
   living in the **test project** as the proving/example code, since the library ships no web types
   (`CLAUDE.md`: no web layer). This proves the §5.4 "port the custom assertion layer" step is possible on a
   FatCat base without FluentAssertions.

## OQ-4 to resolve here

Does `protected Subject` suffice for the documented derive-a-comparer pattern, or is a `public`/accessor
needed? Assumed: `protected` suffices (consumers delegate to inner `.Should()` and rarely read `Subject`
externally). Resolve via the proving test; record the answer.

## TDD — tests first

- The proving custom comparer (in the test project) with its own `Good/Bad/BadShowsCorrectMessage/
  BadWithBecause` tests, demonstrating a consumer-style assertion built entirely on FatCat primitives.
- A test asserting the example `WebResultComparer.BeOk()` passes/fails with the expected message — this is
  the executable proof that G5 unblocks the §5.4 custom-assertion port.

## Migration obligation

Append to `MIGRATION.md` a "Writing custom assertions" section: the supported base
(`ComparerBase<TSubject, TComparer>`), the failure primitive (`CompareException.New`), the
`AndConstraint<T>` → return-`this`-for-chaining mapping, and an explicit note that
`Execute.Assertion`/`AssertionChain`/`[CustomAssertion]` have **no** FatCat equivalent (belongs in the
known-unsupported list finalised in Phase 07).

## Verification

`dotnet build`, `dotnet test`, `dotnet format style --verify-no-changes`, `dotnet csharpier .` from repo root.

## Definition of Done

- [ ] OQ-4 resolved; accessibility change (if any) is the minimum necessary and proven by a cross-assembly
      derived comparer test.
- [ ] `ComparerBase` documented as the supported extension point; worked `WebResult`-style example in the
      test project, green.
- [ ] No `AndConstraint`/`AssertionChain`/`[CustomAssertion]` machinery introduced (ADR-006).
- [ ] `MIGRATION.md` appended with the custom-assertion authoring section + unsupported-primitives note.
- [ ] All `00-overview.md` DoD gates met; one commit `[tier1_gaps 05] G5 custom-assertion extension point`.

## Rollback Procedure

`git revert <phase-05-commit>`. If an accessibility widening on `ComparerBase` shipped, the revert restores
`protected` — check no other phase's code (none should) relied on the wider access. Reverting 05 does not
require reverting later phases except 07's references to the extension-point docs.

## Hand-off (contract exposed to later phases)

- **The public extension point:** derive from `FatCat.Testing.Comparers.ComparerBase<TSubject, TComparer>`,
  read `Subject`, fail via `FatCat.Testing.Exceptions.CompareException.New`, return `this`. This is the
  documented contract the §5.4 Toolkit/Fog custom-assertion port builds on.
- The `MIGRATION.md` custom-assertion section (Phase 07 folds the unsupported-primitives note into the
  known-unsupported list).
