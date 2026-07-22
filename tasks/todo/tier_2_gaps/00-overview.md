# tier_2_gaps — Overview

Phased implementation plan for the **Tier 2** gaps in `tasks/gaps.md` (§3): **G7** (`Task<T>` overloads),
**G8** (numeric `…OrEqualTo`), **G9** (string `MatchEquivalentOf`), **G10** (missing type families).

Tier 3 (G11–G14) is explicitly out of scope. Tier 1 (G1, G3–G6) is **not** planned here.

Every phase carries the same two documentation obligations, which are part of its Definition of Done:

1. **README.md** — the assertion catalog gains the methods the phase shipped.
2. **MIGRATION.md** — the mapping table gains a row for every FluentAssertions call the phase now replaces,
   each row naming the test class that proves it (gaps.md §5.5).

---

## Phase Index

| # | Phase | Gap | Depends on | Risk |
|---|---|---|---|---|
| 01 | [Documentation foundation](01-documentation-foundation.md) | — | — | low |
| 02 | [Numeric `…OrEqualTo` — `Numbers/`](02-numeric-or-equal-to.md) | G8 | 01 | low |
| 03 | [Numeric `…OrEqualTo` — `Doubles/` + `Floats/`](03-double-float-or-equal-to.md) | G8 | 01 | low |
| 04 | [String `MatchEquivalentOf`](04-string-match-equivalent-of.md) | G9 | 01 | low |
| 05 | [`Task<T>` unwrap helper + bool/string/numeric overloads](05-task-overloads-core.md) | G7 | 01 | **high** |
| 06 | [`Task<T>` overloads — remaining families](06-task-overloads-remaining.md) | G7 | 05 | medium |
| 07 | [`DateTimeOffset` family](07-datetimeoffset-family.md) | G10 | 01 | low |
| 08 | [`DateOnly` family](08-dateonly-family.md) | G10 | 01 | low |
| 09 | [`TimeOnly` family](09-timeonly-family.md) | G10 | 01 | low |
| 10 | [`Uri` family](10-uri-family.md) | G10 | 01 | medium |
| 11 | [`Type` family](11-type-family.md) | G10 | 01 | medium |
| 12 | [`Stream` family](12-stream-family.md) | G10 / G19 | 01 | medium |
| 13 | [`IDictionary<K,V>` family — **BLOCKED**](13-dictionary-family-blocked.md) | G10 / G18 | 01 **+ G4 (Tier 1)** | medium |
| 14 | [Tier 2 close-out and E2E walk](14-tier-2-closeout.md) | — | 02–12 | low |

Execution order and the fresh-session rule live in [orchestrator.md](orchestrator.md).

**Dependency graph**

```
01 ──┬── 02 ──┐
     ├── 03 ──┤
     ├── 04 ──┤
     ├── 05 ── 06 ──┤
     ├── 07 ──┤
     ├── 08 ──┤
     ├── 09 ──┼── 14
     ├── 10 ──┤
     ├── 11 ──┤
     ├── 12 ──┘
     └── 13   (also gated on G4 — not runnable in this plan)
```

Reverting 01 cascades to every other phase. Reverting 05 cascades to 06. 02, 03, 04, 07–12 are leaves —
each reverts alone. 14 depends on all of them and must be reverted before any of them.

---

## Architecture Decision Records

### ADR-1 — Tier 2 stays runnable without Tier 1

**Decision.** Every phase except 13 executes against the library exactly as it stands today. Nothing in
02–12 references `ObjectComparer`, `CollectionComparer`, or the G5 extension point.

**Context.** gaps.md sequences G7 after G5 and G10's dictionary work after G4, but no Tier 1 plan exists
yet. Waiting would leave four self-contained gaps unstarted for no benefit.

**Consequence.** `IDictionary<K,V>` is carved out into phase 13, which declares G4 as an external
precondition and does not run in this pipeline. `Task<T>` overloads (05/06) cover only the types that
already have comparers; the generic reference-type `Should<T>(this Task<T>)` is deferred to a follow-on
phase in the G1 plan.

**Rejected.** Assuming Tier 1 lands first — blocks all of Tier 2 on unplanned work. Splitting the plan
into a "now" and "gated" half at the file level — phase 13's `Depends on:` already expresses the gate.

### ADR-2 — FatCat ships the `Task<T>` overloads (G7)

**Decision.** `Should(this Task<T>)` overloads live in `ShouldExtensions.cs`. Toolkit's
`TaskTestExtensions.cs` and Fog's `Should(Task<T>)` shims are deleted at migration time, not ported.

**Context.** Both consuming repos hand-rolled near-identical sync-over-async shims. FluentAssertions never
shipped these, so the duplication is FatCat's to absorb.

**Consequence.** The library must block on a task to keep the fluent surface synchronous, which `async.md`
bans except as a documented top-level exception. The blocking call is isolated in exactly one internal
method (`TaskResultReader.Read`) with a comment stating why, and no other file in the library may block.
This is a deliberate, reviewed deviation — phase 05 is rated high risk because of it.

**Rejected.** Leaving the shims consumer-side (keeps duplication, and the shims would still need a FatCat
base to build on). Rewriting every call site to `(await svc.Get()).Should()` — large migration edit surface
across two repos for no library benefit.

### ADR-3 — All seven G10 type families are in scope

**Decision.** `DateTimeOffset`, `DateOnly`, `TimeOnly`, `Uri`, `Type`, `Stream`, and `IDictionary<K,V>` all
get comparers, one family per phase.

**Context.** None of these appear in the consumer call-site inventory. They exist to make the
"replacement for FluentAssertions" claim honest, not to unblock migration.

**Consequence.** Six phases (07–12) plus the blocked phase 13. Each is independently revertible, so a
family can be dropped later without touching the others.

### ADR-4 — `Match` is not unified across families

**Decision.** `Match` stays a **wildcard pattern** on strings (`Match("he*o")`) and a **predicate** on
numerics, doubles, and floats (`Match(x => x > 3)`). Neither changes.

**Context.** The string form is FluentAssertions-compatible and has call sites depending on it. The numeric
predicate form is a FatCat extra with no FluentAssertions counterpart. Unifying either direction breaks
existing tests and the message contract.

**Consequence.** README documents the split explicitly under each family so the asymmetry is intentional
and visible rather than a surprise. `MatchEquivalentOf` (phase 04) is the case-insensitive wildcard and
exists only on strings.

### ADR-5 — `MatchEquivalentOf` takes no `Options` parameter

**Decision.** The signature is `MatchEquivalentOf(string pattern, string because = null)`. It is always
case-insensitive.

**Context.** Existing string methods take `Options options = Options.CaseSensitive`. But "EquivalentOf" in
FluentAssertions *means* ignore case — an `Options.CaseSensitive` argument to `MatchEquivalentOf` would be
a contradiction in terms.

**Consequence.** Internally it calls `StringEqualityHelper.MatchesWildcard(Subject, pattern,
Options.IgnoreCase)`. See OQ-1 — the existing `StartWithEquivalentOf` / `EndWithEquivalentOf` do take
`Options` and default to case-sensitive, which is the inconsistency this ADR refuses to spread.

### ADR-6 — Documentation shape is fixed in phase 01 and only appended to afterwards

**Decision.** Phase 01 creates the full section skeleton of `README.md` and `MIGRATION.md`. Phases 02–13
**append** to named sections and never restructure them.

**Context.** Fourteen phases running in fresh isolated sessions will each rewrite a document's structure if
allowed to. Fixed anchors keep the diffs small and reviewable and keep phases independently revertible.

**Consequence.** Each phase file names the exact section it appends to. A phase that needs a new section
(none currently do) must say so explicitly in its file.

### ADR-7 — New families follow the existing comparer symmetry

**Decision.** A **value-type** family ships four comparers — `<Type>Comparer`, `Not<Type>Comparer`,
`Nullable<Type>Comparer`, `NotNullable<Type>Comparer` — matching `DateTimes/` and `TimeSpans/`. A
**reference-type** family (`Uri`, `Type`, `Stream`) ships two — `<Type>Comparer`, `Not<Type>Comparer` —
that tolerate a null subject, matching the `Strings/` precedent where `NullableStringComparer` is the only
string comparer.

**Context.** `naming-and-structure.md` fixes the naming scheme. Reference types have no separate nullable
form to model — the subject is already nullable.

**Consequence.** Reference-type comparers are named `UriComparer` / `NotUriComparer` (not
`NullableUriComparer`), and each carries a private `SubjectDisplay` returning `"null"` for a null subject,
exactly as `NullableStringComparer` does.

### ADR-10 — New folders are named for the plural of the type

**Decision.** `DateTimeOffsets/`, `DateOnlys/`, `TimeOnlys/`, `Uris/`, `Types/`, `Streams/`,
`Dictionaries/`. Namespace matches the folder exactly, and the test project mirrors it.

**Context.** Every existing family folder is the plural of the type it asserts on — `Booleans`,
`DateTimes`, `Doubles`, `Guids`, `Strings`, `TimeSpans` (`Characters` and `Numbers` are the two loose
ones). `naming-and-structure.md` requires the namespace to match the folder path with no exceptions.

**Consequence.** `DateOnlys` and `TimeOnlys` are clumsy English, and `FatCat.Testing.Types` sits close to
`System.Type`. Both are accepted: consistency with the existing eleven folders beats prose, and inventing
`Dates/`, `Times/`, or `Reflection/` would leave the convention ambiguous for whoever adds the next family.

**Rejected.** `Dates/` + `Times/` for `DateOnly`/`TimeOnly` — reads better, but breaks the rule that tells
you where to look for a type. `Reflection/` for `Type` — same problem.

### ADR-8 — The codemod script is not Tier 2 work

**Decision.** `tools/Migrate-FluentAssertions.ps1` (gaps.md §5.1 deliverable 2) is **not** built by this
plan. `MIGRATION.md` documents the regex and the cases it cannot catch; the script itself belongs to the
migration workstream that runs per consuming repo after Tier 1 lands.

**Context.** The codemod is only runnable once G1/G3/G4 have closed — its output does not compile before
then. Building it now means shipping an untestable script.

### ADR-9 — Branch and commit boundaries

**Decision.** All phases commit to `task/tier_2_gaps`, created from `main` by the orchestrator before phase
01. Exactly one commit per phase, message referencing the phase file. Nothing is pushed.

---

## Assumptions

- **A1** — xUnit-only coupling (G16) is accepted and unchanged. `CompareException` keeps deriving from
  `XunitException`; no phase touches the framework-detection question.
- **A2** — The value formatting engine (G15) is **not** a Tier 2 prerequisite. Every subject introduced by
  Tier 2 has a usable `ToString()`, except `Type` and `Stream`, whose phases format the subject explicitly
  (`type.FullName`, `stream.GetType().Name`). No shared formatter is introduced — that stays a G15/Tier 1
  decision.
- **A3** — No consumer call sites exist for any G10 family. Their MIGRATION rows are coverage claims, not
  migration fixes, and are marked as such.
- **A4** — `.editorconfig`, `.csharpierrc`, and the rules in `.claude/rules/csharp/` are unchanged for the
  duration. Every phase runs the same quality gates.
- **A5** — Adding a public `Should` overload for a type that has none today cannot break existing consumer
  code, because no call currently resolves to it.

## Open Questions

Answer before the phase that names it runs. None blocks phase 01.

- **OQ-1** *(phase 04)* — `StartWithEquivalentOf` and `EndWithEquivalentOf` currently take
  `Options options = Options.CaseSensitive`, so by default they behave identically to `StartWith` /
  `EndWith`. That contradicts the name. Phase 04 **does not** change them (it would break existing tests
  and the message contract). Recommend a separate follow-up phase to make them case-insensitive by default.
  **Needs a human decision; not Tier 2.**
- **OQ-2** *(phases 04, 05, 06, 10–12)* — `ShouldExtensions.cs`, `NullableStringComparer.cs`, and
  `NotNullableStringComparer.cs` carry `#nullable enable` and `string?` parameters, which
  `not-allowed.md` bans outright (gaps.md §7). Two consequences, both recorded rather than silently
  absorbed: **phase 04** adds two methods *inside* that region and is forced to write
  `string? because = null` — writing `string because = null` there raises CS8625, a new compiler warning
  the Definition of Done forbids. It is a deviation, logged as one. **Every other phase** must not add new
  `string?` parameters and must not extend the nullable region to a new file. Cleaning up the existing
  violation is out of Tier 2 scope. **Recommend a dedicated cleanup phase.**
- **OQ-3** *(phase 05)* — A non-generic `Should(this Task)` overload is only meaningful for exception
  assertions, which are G6 (Tier 1). Deferred; phase 05 ships `Task<T>` only.
- **OQ-4** *(phase 11)* — How much of the G20 reflection surface belongs in `TypeComparer`? The phase
  proposes a minimal set (`Be`, `BeDerivedFrom`, `Implement`, `BeAbstract`, `BeSealed`, `BeStatic`,
  `BeInNamespace`, `BeDecoratedWith<T>`) and deliberately excludes the `Types.InAssembly(...)` selection
  DSL, method/property/assembly assertions. **Confirm the surface before the phase runs.**
- **OQ-5** *(phase 12)* — `HaveBufferSize` only applies to `BufferedStream`. The phase excludes it.
  Confirm.
- **OQ-6** *(phase 14)* — The E2E walk proposes one reflection-based test that reads `README.md` and
  asserts every public assertion method appears in the catalog. It is the only test of its kind in the
  project. If that is unwanted, phase 14 falls back to a documented manual audit — both paths are written
  into the phase file.

## Known Deviations From gaps.md

- gaps.md sequences G7 after G5. ADR-1/ADR-2 override that: G7 ships without the extension point, covering
  concrete comparer types only.
- gaps.md lists `Stream` under both G10 and G19, and `IDictionary` under both G10 and G18. This plan treats
  phase 12 as satisfying G19 and phase 13 as satisfying G18 — one implementation, two gap IDs closed.
- gaps.md §5.1 lists the codemod as a deliverable of the migration workstream. ADR-8 keeps it out of Tier 2.

## Out Of Scope

Tier 3 in full (G11 `AssertionScope`, G12 `ExecutionTime`, G13 `.And`, G14 `.Which`), all Tier 1 gaps
(G1, G3, G4, G5, G6), G15 formatting engine, G16 framework detection, G17 authoring primitives, G20–G25,
the §6.3 method-level backfill beyond G8/G9, and the §7 cleanup items.
