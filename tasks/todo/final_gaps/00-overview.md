# final_gaps — Overview

Phased implementation plan for every gap in [../../remaining_gaps.md](../remaining_gaps.md) — the subset of
`tasks/gaps.md` that had no plan: **G15** (value formatting), **G1** (objects), **G4** (collections),
**G3** (`BeEquivalentTo`), **G5** (extension point), **G6** (exceptions), **G2** (migration codemod),
**G26** (method-level completeness), plus the **G16** and **G17** decisions.

Goal: FatCat.Testing becomes a complete free replacement for FluentAssertions 7.0.0, shipped as a NuGet
package that `C:\Code\FatCat.Toolkit` and `C:\Code\Fog` can adopt.

> **Repo boundary — read this before anything else.** No phase in this plan modifies `C:\Code\FatCat.Toolkit`
> or `C:\Code\Fog`. Not one file, not one call site, for any reason. Those repos migrate on their own
> schedule, in their own repos, against the published package. See **ADR-1**.

`tasks/todo/tier_2_gaps/` is a **separate, independent plan** covering G7–G10. It has not been executed
(there is no `MIGRATION.md` and `README.md` is still a two-line stub). This plan does not depend on it and
does not duplicate it. See **ADR-8** for how the two share documentation, and **ADR-14** for the one place
they interlock.

Every phase carries the two standing documentation obligations from `gaps.md` §5.5, as part of its
Definition of Done:

1. **`README.md`** — the assertion catalog gains the methods the phase shipped.
2. **`MIGRATION.md`** — the mapping table gains a row for every FluentAssertions call the phase now
   replaces, each row naming the test class that proves it.

---

## Phase Index

| # | Phase | Gap | Depends on | Risk |
|---|---|---|---|---|
| 01 | [Documentation foundation](01-documentation-foundation.md) | — | — | low |
| 02 | [Value formatting engine](02-value-formatting-engine.md) | G15 | 01 | medium |
| 03 | [Exception assertions](03-exception-assertions.md) | G6 | 01 | medium |
| 04 | [Collection entry points + core assertions](04-collection-entry-points-and-core.md) | G4 | 01, 02 | **high** |
| 05 | [Collection assertions — remainder](05-collection-assertions-remainder.md) | G4 | 04 | low |
| 06 | [Object comparer + object entry point](06-object-comparer.md) | G1 | 02, 04 | **high** |
| 07 | [Equivalency engine — objects](07-equivalency-engine-objects.md) | G3 | 02, 06 | **high** |
| 08 | [Equivalency — collections](08-equivalency-collections.md) | G3 | 05, 07 | medium |
| 09 | [Equivalency — configuration hook](09-equivalency-configuration.md) | G3 | 07 | medium |
| 10 | [Custom-assertion extension point](10-extension-point.md) | G5 | 06 | medium |
| 11 | [Migration codemod + MIGRATION.md completion](11-migration-codemod.md) | G2 | 01 *(content: 04–10)* | low |
| 12 | [G26 — DateTimes](12-g26-datetimes.md) | G26 | 01 | medium |
| 13 | [G26 — Collections](13-g26-collections.md) | G26 | 05 | low |
| 14 | [G26 — Exceptions](14-g26-exceptions.md) | G26 | 03 | medium |
| 15 | [G26 — Strings](15-g26-strings.md) | G26 | 01 | low |
| 16 | [G26 — Numerics](16-g26-numerics.md) | G26 | 01 | low |
| 17 | [G26 — Enums](17-g26-enums.md) | G26 | 01 | low |
| 18 | [G26 — Booleans](18-g26-booleans.md) | G26 | 01 | low |
| 19 | [G26 — Guids](19-g26-guids.md) | G26 | 01 | low |
| 20 | [Close-out and replacement-claim audit](20-final-closeout.md) | — | 02–19 | low |

Execution order and the fresh-session rule live in [orchestrator.md](orchestrator.md).

**Dependency graph**

```
01 ─┬─ 02 ─┬─ 04 ─┬─ 05 ─┬──────────── 13 ──┐
    │      │      │      └─ 08 ─┐           │
    │      │      └─ 06 ─┬─ 07 ─┴─ 09 ──┐   │
    │      │             └─ 10 ─────────┤   │
    │      ├─ 12 ────────────────────────┤   │
    │      └─ 15, 16, 17, 18, 19 ────────┼───┤
    ├─ 03 ─── 14 ────────────────────────┤   │
    └─ 11 ────────────────────────────────┴───┴─ 20
```

Reverting 01 cascades to everything. Reverting 02 cascades to 04, 05, 06, 07, 08, 09, 10, 13.
Reverting 04 cascades to 05, 06, 07, 08, 09, 10, 13. Reverting 06 cascades to 07, 08, 09, 10.
Reverting 07 cascades to 08, 09. Reverting 03 cascades to 14. 12, 15, 16, 17, 18, 19 are leaves — each
reverts alone. 20 depends on all and must be reverted first.

---

## Architecture Decision Records

### ADR-1 — This plan changes only the `FatCat.Testing` repo

**Decision.** Every phase commits inside `c:\Code\FatCat.Testing`. No phase reads-to-edit, edits, builds, or
commits anything in `C:\Code\FatCat.Toolkit` or `C:\Code\Fog`.

**Context.** `remaining_gaps.md` §6 states the exit criteria as "Toolkit builds with no FluentAssertions
reference … Fog builds against that Toolkit". Those repos consume FatCat.Testing as a **NuGet package**.
The removal work happens in those repos, after this package ships.

**Consequence.** The §6 exit criteria are **restated for this repo** (see *Exit criteria, restated* below):
this plan is done when the library can support every construct those repos use, proven by tests here, and
the migration tooling and guide ship with the package. It is not done by watching another repo turn green.
Phase 20 audits the library-side claim only.

**Consequence.** G5's original acceptance ("one of the Toolkit assertion classes is ported to it as proof")
cannot be met as written. Phase 10 substitutes a custom comparer written **in this repo's test project**,
modelled on the shape those repos use. See ADR-9.

**Rejected.** Cross-repo phases — would put commits in two more repos under an orchestrator that cannot
verify them, and would edit a NuGet package's public contract from a plan that does not own it.

### ADR-2 — No `becauseArgs`. `because` stays a single replacement string

**Decision.** Every assertion in the library keeps exactly `string because = null`, and `because` **replaces**
the generated message. No `params object[] becauseArgs` overload is added anywhere.

**Context.** Open question OQ-1 in `remaining_gaps.md` §7. FluentAssertions takes
`string because, params object[] becauseArgs` and *appends* a formatted reason. Toolkit's helpers use the
args form in a handful of places.

**Consequence.** Adding the args form would double the parameter surface of every assertion in the library
and introduce a second message-formatting path. Instead, `MIGRATION.md` documents the rewrite — args form
becomes string interpolation, which `naming-and-structure.md` requires anyway — and documents the
replace-vs-append behavioural difference as a known, deliberate divergence. Consumers do that rewrite in
their own repos (ADR-1).

**Rejected.** `params object[] becauseArgs` on every assertion. Source compatibility is not a goal — the
library already diverges on negation (ADR-A / `.Not.`).

### ADR-3 — FatCat.Testing stays xUnit-coupled (G16 deferred, not answered)

**Decision.** `CompareException` keeps deriving from `XunitException`; `xunit.assert` stays the library's
one package reference. No phase in this plan touches framework detection.

**Context.** Open question OQ-2. Both consuming repos are xUnit, so this blocks nothing. A detection shim
would touch every failure path in the library and re-pin every message test.

**Consequence.** `README.md` states the xUnit requirement plainly as a known limitation, so the
"replacement for FluentAssertions" claim is scoped honestly rather than overstated. The cost of deciding
later is acknowledged and accepted: this plan roughly doubles the assertion surface, so a future shim is
more expensive than it is today. **Flagged for the human reviewer** — this is the one decision in the plan
that gets harder, not easier, by waiting.

**Rejected.** Building the shim now — real work, zero migration value, and it re-pins every message test in
the suite on the same commit as the riskiest new surface.

### ADR-4 — Collections (G4) ship **before** objects (G1), not alongside

**Decision.** Phase 04 lands the collection entry points and comparer. Phase 06 lands the object entry point.
In that order, in separate commits.

**Context.** ADR-C in `remaining_gaps.md` §3 established that a generic `Should<T>(this T) where T : class`
silently swallows every collection — for a `List<string>`, binding `T = List<string>` is an *identity*
conversion while `IEnumerable<T>` needs a reference conversion, and identity wins. ADR-C's remedy was
"land them together, or gate G1 on G4".

**Decision detail.** "Together" is rejected because it makes one commit out of the two highest-risk pieces
of work in the plan. Ordering G4 first is strictly safer: at the moment phase 04 lands, no object entry
point exists, so no call site can silently bind to the wrong comparer. By the time phase 06 adds
`Should<T>(this T) where T : class`, the concrete-shape overloads that beat it already exist and are
covered by a regression test class.

**Consequence.** Phase 04 owns `Tests.FatCat.Testing/Collections/CollectionOverloadResolutionTests.cs`, and
phase 06 **extends** that same class rather than creating a second one. That file is the executable form of
ADR-C. Reverting 04 without reverting 06 leaves the object comparer swallowing collections — the graph
above says so, and phase 06's rollback repeats it.

### ADR-5 — Object `Be` uses `Equals`; `BeSameAs` is reference identity

**Decision.** `ObjectComparer<T>.Be(T expected)` compares with `Equals`. Reference identity is
`BeSameAs`/`Not.BeSameAs`, using `ReferenceEquals`.

**Context.** Open question OQ-3. This is what FluentAssertions does, and the 10 `BeSameAs`/`NotBeSameAs`
call sites in the consuming repos mean reference identity must exist as a distinct concept.

**Consequence.** A DTO that overrides `Equals` compares by value through `Be` — matching consumer
expectations at ~1,100 object call sites. `Be` is *not* `BeEquivalentTo`: structural comparison of types
that do not override `Equals` is phase 07's job and is a separate method.

### ADR-6 — `BeEquivalentTo` ships default options plus exactly one hook

**Decision.** Inherits ADR-B from `remaining_gaps.md` §3. No `Excluding`, `Including`,
`RespectingRuntimeType`, `WithStrictOrdering`, or the other ~45 option methods — all have zero usages in
either repo. The one option that is built is the `Using<T>()` / `WhenTypeIs<T>()` equivalent, because Fog
registers a `DateTime` closeness rule in two places and those suites break without it.

**Consequence.** Phase 09 builds a **global default registration** only. A per-call options lambda is
explicitly deferred — no consumer call site uses one. `MIGRATION.md` lists every unsupported option method
under "known unsupported" with the recommended rewrite.

### ADR-7 — The value formatter is public API

**Decision.** `FatCat.Testing.Formatting.ValueFormatter` is `public static`, with a public
`Format(object value)` entry point.

**Context.** G15 asks whether formatting is public API. It becomes public the moment a consumer writes a
custom comparer (G5) and needs its failure messages to render a subject the same way the library does.

**Consequence.** `Format` is a supported contract from phase 02 onward — its output shape is pinned by
tests and documented in `README.md` under the custom-comparer section (phase 10). Phases may add cases;
they may not silently change the rendering of a shape that already has a pinned test.

### ADR-8 — Documentation scaffolding is created once, by whichever plan runs first

**Decision.** Phase 01 creates `README.md`'s full section skeleton and `MIGRATION.md` **only if they are not
already present in the required shape**. `tier_2_gaps/01-documentation-foundation.md` creates the same
skeleton. Whichever runs first creates it; the second verifies and appends.

**Context.** Two independent plans both open with a documentation-foundation phase, and neither can assume
the other has run. As of writing, neither has: `README.md` is two lines and there is no `MIGRATION.md`.

**Consequence.** Phase 01 is written to be idempotent and states the exact section anchors. Phases 02–19
**append** to named sections and never restructure them — sixteen fresh isolated sessions will each rewrite
a document's structure if allowed to.

### ADR-9 — G5 is a documentation-and-accessibility phase, proven in this repo

**Decision.** G5 ships: (a) `ComparerBase.Subject` made accessible to derived types outside the assembly,
(b) a worked custom-comparer example in `README.md`, (c) a custom comparer built in this repo's test
project that mirrors the shape Toolkit and Fog use (`WebResultAssertions`-style — a status-code-ish subject
with `BeOk`-style methods), with tests proving it chains and throws `CompareException`.

**Context.** `remaining_gaps.md` §4 G5: both repos' custom assertions delegate to inner `.Should()` calls
and have **zero** dependency on FluentAssertions' `Execute.Assertion` / `AssertionChain` engine. They need
only a public base with an accessible `Subject` and a chainable return — `ComparerBase<TSubject, TComparer>`
already provides both. ADR-1 forbids porting a Toolkit class.

**Consequence.** G5 is mostly *deciding and documenting*. The proof lives in
`Tests.FatCat.Testing/Extensibility/`. Whether `Subject` becomes `public` or stays `protected` is settled in
phase 10, not here — `protected` is already sufficient for a derived comparer, and the mismatch is that
Toolkit's assertions read `Subject` from *outside* the type. Phase 10 resolves it against real usage.

### ADR-10 — The codemod ships from this repo

**Decision.** `tools/Migrate-FluentAssertions.ps1` is built in phase 11, in this repo, following
`.claude/rules/powershell/powershell.md`. It ships with the package as a migration aid.

**Context.** `tier_2_gaps` ADR-8 deferred the codemod on the grounds that its output does not compile until
G1/G3/G4 close. This plan closes them, so the objection expires.

**Consequence.** The script operates on a directory tree given as a parameter, is idempotent, supports
`-WhatIf`, and **reports** the four cases the regex cannot catch (`gaps.md` §5.3) instead of silently
skipping them. Phase 11 tests it against fixture files **inside this repo** — never against Toolkit or Fog
(ADR-1).

### ADR-11 — G17 (authoring primitives) stays out

**Decision.** No `AssertionChain`, no `ForCondition`/`FailWith`/`BecauseOf`/`Given`, no message templating.
The `[CustomAssertion]`-equivalent stack-trace question is recorded as an open question, not a phase.

**Context.** `remaining_gaps.md` §4 G17: G5 alone unblocks both repos' custom assertions. The one genuine
loss is stack traces pointing at the library line instead of the test line — judged on developer experience
*after* G5 ships, not before.

**Consequence.** Phase 10's report is required to state whether a failing custom assertion points at the
test or at the library. That observation, not speculation, decides whether G17 becomes a follow-up plan.

### ADR-12 — Collection equivalency is order-insensitive by default

**Decision.** `BeEquivalentTo` on collections ignores order; `Equal` (phase 05) is the order-sensitive
assertion.

**Context.** FluentAssertions' default. `WithStrictOrdering` — its opt-in for the other behaviour — has zero
usages in either repo (ADR-6), so the default is the whole surface.

**Consequence.** Phase 08 implements greedy matching: each expected element must find one unmatched actual
element it is equivalent to. The failure message names the unmatched elements. Documented in `README.md` and
`MIGRATION.md` as intentional, because it is the single most surprising default in the library.

### ADR-13 — Top-level strings render bare; nested strings render quoted

**Decision.** `ValueFormatter.Format("hello")` returns `hello`. A string *inside* a collection or an object
dump renders as `"hello"`.

**Context.** G15 proposes quoting strings. Every existing string failure message interpolates the subject
raw and is pinned by a test — `"hello world should contain xyz"`. Quoting at the top level rewrites dozens
of pinned messages for no gain, while *not* quoting inside a collection makes `[a, b]` ambiguous against
`[a, b]` of some other type.

**Consequence.** Position determines quoting. This is the single most likely thing to be got wrong by a
later phase, so phase 02 pins both cases with tests and this ADR is quoted in every phase that formats a
value.

### ADR-14 — The one interlock with `tier_2_gaps`

**Decision.** `tier_2_gaps/13-dictionary-family-blocked.md` is gated on G4. Phase 04 of this plan closes
that gate. Neither plan is otherwise sequenced against the other.

**Consequence.** Phase 04's hand-off states explicitly that phase 13 of the other plan is now runnable, and
what it must re-probe: whether a dedicated `Should<K, V>(this IDictionary<K, V>)` beats
`Should<T>(this IEnumerable<T>)` binding to `IEnumerable<KeyValuePair<K, V>>`. That probe is **unverified**
in `remaining_gaps.md` §3 and stays that plan's job.

### ADR-15 — Branch and commit boundaries

**Decision.** All phases commit to the branch that is current when the pipeline starts. Per `task.md`, a
task branch `task/final_gaps` is created **only if** the current branch is `main`; on any other branch, that
branch is the task branch. At the time of writing the current branch is `FillingInMoreGaps`.

Exactly one commit per phase, message referencing the phase file. **Nothing is pushed** — human review
gates the push.

---

## Exit criteria, restated for this repo

`remaining_gaps.md` §6 states the exit criteria in terms of Toolkit and Fog building green. ADR-1 puts both
out of reach. The library-side equivalent, which phase 20 audits:

1. Every FluentAssertions construct that the 2,580 audited call sites use has a FatCat equivalent, and
   `MIGRATION.md` §5.2 has a row for it.
2. **Every row in that table names a test class that compiles the rewritten form.** A row with no test
   behind it is a claim, not a guarantee (`gaps.md` §5.5).
3. `tools/Migrate-FluentAssertions.ps1` runs clean over a fixture tree in this repo and reports every case
   it cannot rewrite.
4. `README.md` documents the full assertion catalog, the custom-comparer extension point, the xUnit
   requirement (ADR-3), and the known behavioural divergences (`.Not.` shape, `because` replaces rather than
   appends, order-insensitive collection equivalency).
5. `dotnet build` and `dotnet test` green with no new warnings.

---

## Assumptions

- **A1** — `tier_2_gaps` has **not** run. Phase 01 verifies rather than assumes (ADR-8). If it has run, phase
  01 becomes a no-op plus an append, which is exactly its design.
- **A2** — The consuming repos will adopt the published NuGet package, not a project reference. No phase
  needs either repo present on disk. The call-site counts in `gaps.md` are taken as given — re-auditing them
  would require reading those repos, which ADR-1 permits (read-only) but no phase needs.
- **A3** — `.editorconfig`, `.csharpierrc`, and `.claude/rules/csharp/` are unchanged for the duration.
  Every phase runs the same quality gates.
- **A4** — Adding a `Should` overload for a type that has none today cannot break existing consumer code,
  because no call currently resolves to it. **This assumption fails for phases 04 and 06** — both add
  overloads that compete with existing ones. Both are rated high risk for precisely this reason and both
  carry an overload-resolution regression suite.
- **A5** — The five pre-existing IDE1006 `TestGuid` warnings are baseline noise, not a phase failure.
- **A6** — `ImplicitUsings` is on and `Nullable` is disabled in both projects. New files use
  `string because = null` and never `string?` — see OQ-2 for the pre-existing violation this plan must not
  spread.

## Open Questions

Answer before the phase that names it runs. None blocks phase 01.

- **OQ-1** *(phase 10)* — Should `ComparerBase.Subject` become `public`, or stay `protected`? `protected` is
  enough for a derived comparer; Toolkit's helpers read `Subject` from outside the type. Phase 10 proposes
  `public` (FluentAssertions' `ReferenceTypeAssertions.Subject` is public) and builds the proof comparer
  both ways before choosing. **Confirm before phase 10 commits.**
- **OQ-2** *(phases 04, 06, 07)* — `ShouldExtensions.cs`, `NullableStringComparer.cs`, and
  `NotNullableStringComparer.cs` carry `#nullable enable` and `string?` parameters, which `not-allowed.md`
  bans outright (`gaps.md` §7). Phases 04 and 06 add overloads **inside** `ShouldExtensions.cs`'s nullable
  region and are therefore forced into `string?` for any `because` they declare there — new comparers must
  not extend the region. Recommend a dedicated cleanup phase; it is not in this plan. Also unfixed and
  noted: `NullableStringComparer.SubjectDisplay` uses an expression-bodied getter, which the rules ban.
- **OQ-3** *(phase 07)* — Depth and breadth limits for the equivalency engine and the object member dump.
  Phase 07 proposes max depth 10 and a 32-item collection cap (FluentAssertions' number). **Confirm.**
- **OQ-4** *(phase 07)* — Does `BeEquivalentTo` compare **fields** as well as properties? FluentAssertions
  compares public properties by default. Phase 07 proposes public readable instance properties only, with
  fields excluded. **Confirm** — it changes what a DTO with public fields asserts.
- **OQ-5** *(phase 09)* — The global equivalency registration is process-wide mutable state, which makes
  test-run order matter. Phase 09 proposes a `ConcurrentDictionary` keyed by type with last-registration-wins
  and documents that registration belongs in a fixture, not a test. **Confirm the shape.**
- **OQ-6** *(phase 12)* — The DateTime fluent difference chains (`BeLessThan(2.Hours()).Before(x)`) need a
  `2.Hours()` numeric-extension surface that does not exist in this library. Phase 12 proposes shipping
  `Hours()`/`Minutes()`/`Seconds()`/`Days()`/`Milliseconds()` on `int` as part of the phase. That is new
  public API beyond assertions. **Confirm, or the chains take a `TimeSpan` argument instead.**
- **OQ-7** *(phase 11)* — Does the codemod ship inside the NuGet package (as a content file) or only in the
  GitHub repo? Phase 11 proposes repo-only — packaging a PowerShell script into a library `.nupkg` changes
  the package layout for no consumer benefit. **Confirm.**
- **OQ-8** *(phase 20)* — G16 (ADR-3) is deferred, not answered. Phase 20 must re-surface it with the full
  surface in view so the decision is made deliberately rather than by omission.

## Known Deviations From `remaining_gaps.md`

- §3 ADR-C says G1 and G4 "land together, or gate G1 on G4". **ADR-4** takes the second option explicitly and
  fixes the order — G4 first — rather than leaving it open.
- §4 G5 says "one of the Toolkit assertion classes is ported to it as proof". **ADR-1 and ADR-9** replace
  that with an equivalent comparer built in this repo's test project.
- §6 exit criteria are stated in terms of Toolkit and Fog building green. **ADR-1** restates them for this
  repo; see *Exit criteria, restated*.
- §4 G15 says `string` → quoted. **ADR-13** qualifies it: bare at top level, quoted when nested.
- §7 OQ-4 ("does FatCat ship the `Task<T>` overloads?") is already answered by `tier_2_gaps` ADR-2 — FatCat
  ships them. This plan does not revisit it, and phases 04/06 note the overload-resolution interaction.

## Out Of Scope

Tier 3 in full — G11 `AssertionScope`, G12 `ExecutionTime`, G13 `.And` chaining, G14 `.Which`/`.Subject`.
G16 framework detection (ADR-3, deferred). G17 authoring primitives (ADR-11). G20–G25 — reflection and
architecture tests, event monitoring, XML, JSON, serializability, the analyzers package. The `BeEquivalentTo`
option methods other than `Using`/`WhenTypeIs` (ADR-6). G7–G10, which belong to `tier_2_gaps`. Any edit to
`C:\Code\FatCat.Toolkit` or `C:\Code\Fog` (ADR-1). The `string?` / expression-body cleanup in the existing
`Strings/` files (OQ-2). Publishing or pushing anything.
