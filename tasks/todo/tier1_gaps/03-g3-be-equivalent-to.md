# Phase 03 — G3: structural (deep) equality — `BeEquivalentTo`

- **Gap:** G3 (`gaps.md` §3). 233 call sites. Plus the **global configuration hook** Fog needs for its
  `DateTime` closeness rule (two files: `EndToEndTest.cs` @ 10s, `BrumeTests.cs` @ 1s).
- **Depends on:** Phase 01 (formatting — the diff message is unusable without it), Phase 02 (extends
  `ObjectComparer`/`NotObjectComparer`).
- **Depended on by:** Phase 04 (collection `ContainEquivalentOf` / `BeEquivalentTo` reuse this engine),
  Phase 05, Phase 07.
- **Risk:** **HIGH** — public API contract (`BeEquivalentTo` semantics + the config hook surface), the most
  algorithmically involved phase, and OQ-2 (config-hook shape) is the plan's highest-uncertainty decision.
  Flagged for extra human review.

## Context (complete handoff)

Today `BeEquivalentTo` exists only on strings (case-insensitive compare, in the string comparers). G3 is
recursive member-by-member structural equality for objects (and, in Phase 04, collections) with cycle
detection and a readable diff. Per `gaps.md`, ship the **default-options path only** — do **not** build
`Excluding`, `Including`, `RespectingRuntimeType`, `WithStrictOrdering` (zero usages). The one option that
*is* used is `Using<DateTime>(...).WhenTypeIs<DateTime>()`, which G3 must satisfy via a config hook.

## Deliverable

### 1. The equivalency engine — `FatCat.Testing/Equivalency/`
An internal `StructuralEquivalency` service:

- Compares two objects member-by-member over **public readable instance properties** (and public fields if a
  consumer DTO uses them — verify against Fog/Toolkit DTOs; assume properties, add fields only if needed).
- Recurses into nested objects and (in Phase 04) enumerables.
- **Cycle detection:** a visited-pair set keyed by reference identity; equal references short-circuit.
- Primitives/`string`/`Guid`/`DateTime`/enum compare by `Equals` (subject to the config hook below).
- Produces a **structured diff**: the member path (`Address.City`) and the two `ValueFormatter.Format`-ed
  values, so the failure message reads e.g.
  `Expected Address.City to be "Boston" but found "Austin"` (exact wording proposed here, pinned by test).

### 2. `BeEquivalentTo` on the object comparers (extends Phase 02)
Add to `ObjectComparer` and `NotObjectComparer`:

```csharp
public ObjectComparer BeEquivalentTo(object expected, string because = null) { ... }
```

Passes when the engine reports structural equality; on failure `CompareException.New(because ?? diff)`.
`Not.BeEquivalentTo` passes when they are **not** structurally equal.

### 3. The global configuration hook (OQ-2 — resolve FIRST)
Fog registers, once in test infrastructure:
`options.Using<DateTime>((a, b) => a.Should().BeCloseTo(b, 10.Seconds())).WhenTypeIs<DateTime>()`.

**Assumed FatCat shape (confirm before building — see OQ-2 in `00-overview.md`):** a global static
registration consulted by the engine:

```csharp
EquivalencyOptions.Using<DateTime>((subject, expected) => /* true if close enough */);
```

- Global-static because the consumers register it **once** in shared test setup, not per call. A per-call
  `BeEquivalentTo(expected, options => ...)` lambda is the FluentAssertions shape but is **not** how the
  consumers use it and adds an option-builder surface `gaps.md` says to avoid.
- Registration is keyed by type; the engine, when comparing two values whose runtime type has a registered
  rule, uses the rule instead of `Equals`.
- **If a global static is judged unacceptable (thread-safety across parallel test runs, test bleed), STOP
  and ASK.** This is the single decision most likely to need human input. If confirmed global, use a
  `ConcurrentDictionary<Type, Func<object,object,bool>>` (`types.md` thread-safe collections rule).

## TDD — tests first

`Tests.FatCat.Testing/Objects/ObjectBeEquivalentToTests` + `Tests.FatCat.Testing/Equivalency/` for the engine
and the config hook. Cover:

- Equal graphs pass; single differing member fails with the exact path+values message.
- Nested object difference reports the full member path.
- Cycles do not stack-overflow.
- `Not.BeEquivalentTo` both directions.
- Config hook: register a `DateTime` closeness rule, two `DateTime`s 3s apart are equivalent at 10s
  tolerance and not equivalent at 1s. Use **explicit constructed `DateTime` values**, never `DateTime.Now`
  (`not-allowed.md`).
- Migration-proof: `subject.Should().BeEquivalentTo(expected)` and `subject.Should().Not.BeEquivalentTo(x)`.

## Migration obligation

Append to `MIGRATION.md`: `BeEquivalentTo` (object graphs, 233 sites, G3), `NotBeEquivalentTo` →
`Not.BeEquivalentTo(x)` (1). Document the config-hook migration: FluentAssertions
`options.Using<T>().WhenTypeIs<T>()` → `EquivalencyOptions.Using<T>(...)` (final name per OQ-2), with the
two Fog registration sites called out.

## Verification

`dotnet build`, `dotnet test`, `dotnet format style --verify-no-changes`, `dotnet csharpier .` from repo root.

## Definition of Done

- [ ] OQ-2 resolved (config-hook shape decided/confirmed) and recorded in the phase report.
- [ ] Structural engine with cycle detection + structured diff, all tests green.
- [ ] `BeEquivalentTo` / `Not.BeEquivalentTo` on the object comparers, full test set.
- [ ] Config hook implemented and proven by the `DateTime`-closeness test.
- [ ] Default-options-only — no `Excluding`/`Including`/`WithStrictOrdering` present.
- [ ] `MIGRATION.md` appended with G3 rows incl. the config-hook mapping.
- [ ] All `00-overview.md` DoD gates met; one commit `[tier1_gaps 03] G3 BeEquivalentTo + config hook`.

## Rollback Procedure

`git revert <phase-03-commit>`. Manual: remove the G3 section from `MIGRATION.md` if orphaned. Reverting 03
**requires reverting 04, 05 first** (04's `ContainEquivalentOf` and 05's docs reference the engine).

## Hand-off (contract exposed to later phases)

- `FatCat.Testing.Equivalency.StructuralEquivalency` (internal) — the reusable deep-equality engine. Phase 04
  calls it for `ContainEquivalentOf` / collection `BeEquivalentTo`; do not reimplement structural comparison.
- `FatCat.Testing.Equivalency.EquivalencyOptions.Using<T>(...)` (final name per OQ-2) — the global config
  hook. Phase 04's collection equivalence honours the same registered rules.
- `ObjectComparer.BeEquivalentTo` / `NotObjectComparer.BeEquivalentTo` — object-graph equivalence entry.
