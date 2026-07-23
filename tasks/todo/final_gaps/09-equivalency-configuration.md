# Phase 09 — Equivalency configuration hook (G3)

- **Work item:** `final_gaps`
- **Gap:** **G3** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **medium.** Small surface, but it introduces process-wide mutable state, which makes test-run
  order matter if used carelessly. The design must make the safe usage obvious.
- **Depends on:** 07 (equivalency engine)
- **Depended on by:** —
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Phase 07 built `EquivalencyComparer.Compare`, with a type-keyed override registry that it consults before
falling back to `Equals`/member-walk. That registry is currently **empty**. This phase fills the mechanism:
the `Using<T>()` / `WhenTypeIs<T>()` equivalent.

**Why it must exist.** Fog registers a `DateTime` closeness rule in two places, and those suites break
without an equivalent (`remaining_gaps.md` §4 G3):

```
Brume\Tests.Brume\BrumeTests.cs:13              options.Using<DateTime>(...).WhenTypeIs<DateTime>()   // 1s tolerance
EndToEndTests\Infrastructure\EndToEndTest.cs:28 options.Using<DateTime>(...).WhenTypeIs<DateTime>()   // 10s tolerance
```

Per **ADR-6**, this is the **only** equivalency option that ships — `Excluding`, `Including`,
`WithStrictOrdering`, and the other ~45 have zero usages and are not built.

**OQ-5 gates this phase** (orchestrator precondition): the registration is process-wide mutable state.
Proposal: a `ConcurrentDictionary` keyed by `Type`, last-registration-wins, and documentation that
registration belongs in a **fixture** (a collection fixture or a static constructor), never inside a `[Fact]`
— because a rule registered by one test then affects every later test in the run.

Read before starting: [00-overview.md](00-overview.md) (**ADR-6**, **OQ-5**), phase 07's hand-off (the
injection seam), `.claude/rules/csharp/types.md` (ConcurrentDictionary for shared mutable state),
`naming-and-structure.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope**

- A global registry of per-type equivalency rules and a fluent registration surface reading naturally as
  `Equivalency.Using<DateTime>((subject, expected) => ...)` (see the API note below).
- The engine (phase 07) consulting that registry per node before its default comparison.
- A `Reset()` / clear entry point so a fixture can tear down after itself.

**Out of scope**

- Any other equivalency option (ADR-6).
- A **per-call** options lambda (`BeEquivalentTo(x, opts => opts.Using...)`). No consumer uses one, and it
  would balloon every `BeEquivalentTo` signature. Global registration only.
- Rewriting the two Fog call sites — that is Fog's own migration, in Fog's repo (ADR-1). This phase ships the
  mechanism and proves it here.

---

## Design

New file `FatCat.Testing/Equivalency/EquivalencyOptions.cs` (or `EquivalencyConfiguration.cs` — pick the name
that reads best and state it in the report), namespace `FatCat.Testing.Equivalency`.

### API shape

FluentAssertions writes `options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation,
1.Seconds())).WhenTypeIs<DateTime>()`. FatCat does not have that `ctx` object and should not grow one for two
call sites. The FatCat equivalent is a **global** registration taking a predicate:

```csharp
namespace FatCat.Testing.Equivalency;

public static class Equivalency
{
	public static void Using<T>(Func<T, T, bool> areEquivalent) { ... }   // register a rule for type T

	public static void Reset() { ... }                                     // clear all registered rules
}
```

Usage in a consumer fixture:

```csharp
Equivalency.Using<DateTime>((subject, expected) => (subject - expected).Duration() <= TimeSpan.FromSeconds(1));
```

This is a **deliberate divergence** from FluentAssertions' `Using().WhenTypeIs()` chain — simpler, global,
and matched to actual usage. `MIGRATION.md` documents the rewrite from the FluentAssertions form. Do **not**
reproduce the `WhenTypeIs<T>()` chain; the `<T>` on `Using` already carries the type.

### Storage

```csharp
private static readonly ConcurrentDictionary<Type, Func<object, object, bool>> rules = new();
```

`Using<T>` wraps the typed predicate into the `object`-keyed form. `Reset()` calls `rules.Clear()`.
Last-registration-wins (`rules[typeof(T)] = wrapped;`), matching FluentAssertions where a later `Using`
overrides an earlier one for the same type.

### Engine integration

`EquivalencyComparer.Compare`, at each node, **before** the `Equals`/member-walk decision (phase 07's seam):

```
if a rule is registered for the node's runtime type -> use it; its result decides equivalency for this node.
```

The rule short-circuits the recursion for that type — a `DateTime` node with a closeness rule never recurses
into `DateTime`'s properties. This is exactly the Fog use case.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Equivalency/`.

Because these tests mutate global state, **each test registers and then resets** in a `try/finally` (or the
test class implements `IDisposable` and calls `Equivalency.Reset()` in `Dispose`). Model the safe pattern
here — it is the pattern consumers must copy, so it has to be exemplary. Note: `BaseTest` has no teardown
hook, so the `IDisposable` goes on the test class itself; this is allowed and does not modify `BaseTest`.

1. **Red.** `EquivalencyUsingTests.cs`:
   - `GoodUsingDateTimeCloseness` — register a 1-second `DateTime` rule; two DTOs whose `Timestamp` differs
     by 500ms are equivalent
   - `BadUsingDateTimeCloseness` — the same DTOs 2 seconds apart are **not** equivalent
   - `GoodUsingShortCircuitsRecursion` — the rule fires and `DateTime` sub-properties are never compared
   - `GoodResetClearsRules` — after `Reset()`, the closeness rule no longer applies and exact comparison
     resumes
   - `GoodLastRegistrationWins` — registering twice for `DateTime` uses the second rule
   - Each wrapped so global state is clean afterwards.

2. **Green.** Implement `Equivalency` and wire the registry check into `EquivalencyComparer.Compare`.

3. Run the whole suite twice, in both orders if the runner allows, to prove no leakage: phase 07's and phase
   08's equivalency tests must pass regardless of whether an `EquivalencyUsingTests` ran first. If a leak
   appears, the `Reset()` discipline in the tests is wrong — fix the test hygiene, that is the lesson for
   consumers.

---

## Files

**Added**

- `FatCat.Testing/Equivalency/Equivalency.cs` (registration surface)
- `Tests.FatCat.Testing/Equivalency/EquivalencyUsingTests.cs`
- Fixture types — one class per file

**Changed**

- `FatCat.Testing/Equivalency/EquivalencyComparer.cs` — consult the registry at each node
- `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `### Objects`** (equivalency subsection) — document `Equivalency.Using<T>(...)` and
`Equivalency.Reset()`. **Prominently**: it is global, so register it in a fixture and reset after; never
inside a test. Show the `DateTime` closeness example.

**`MIGRATION.md` → `## 3. Mapping Table`**:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `options.Using<T>(...).WhenTypeIs<T>()` | `Equivalency.Using<T>((s, e) => ...)` | `Tests.FatCat.Testing.Equivalency.EquivalencyUsingTests` |

**`MIGRATION.md` → `## 5. Behavioural Differences`** — the registration is **global**, not per-`BeEquivalentTo`
-call as in FluentAssertions. Rewrite `Using().WhenTypeIs()` chains to a single `Equivalency.Using<T>` in a
fixture, and add `Equivalency.Reset()` to the fixture teardown.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~EquivalencyUsing"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] OQ-5 confirmed; chosen registry shape recorded in the report.
- [ ] Tests written before implementation; red observed and recorded.
- [ ] `Equivalency.Using<T>` registers a per-type rule; `Reset()` clears all.
- [ ] Storage is `ConcurrentDictionary<Type, ...>`, last-registration-wins (`types.md`).
- [ ] The engine consults the registry per node before its default comparison; a rule short-circuits
      recursion for that type — pinned by a test.
- [ ] `DateTime` closeness example works, both pass and fail cases pinned.
- [ ] Tests register-and-reset so global state never leaks; the suite passes regardless of test order.
- [ ] No new warnings; no banned patterns; `BaseTest` unchanged.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + mapping row + behavioural-difference note written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/09-equivalency-configuration.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-09-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Reverting removes the registry check from the engine and the registration surface. Nothing depends on it.
Reverts alone.

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing.Equivalency;
Equivalency.Using<T>(Func<T, T, bool> areEquivalent)
Equivalency.Reset()
```

G3 is complete: object equivalency (07), collection equivalency (08), and the one configuration hook (this
phase). The registry is process-global and must be reset by fixtures — that discipline is documented and
must be repeated wherever a consumer registers a rule.
