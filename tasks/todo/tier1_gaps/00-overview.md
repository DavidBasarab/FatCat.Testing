# tier1_gaps — Phased Plan Overview

Work item: **tier1_gaps**
Source spec: `tasks/task.md`
Feature context: `tasks/gaps.md` (§3 Tier 1, §4 Sequence, §5 Migration)

This is the handoff document for the Tier-1 gap closure of FatCat.Testing — making the library a
drop-in, Apache-licensed replacement for FluentAssertions 7.0.0 in `FatCat.Toolkit` and `Fog`. Tier 1 is
the set of gaps that **block migration from even starting**.

Everything in this folder is a **plan**. No production code was written in the planning session. Each phase
file is a complete, context-isolated handoff: an executing session should be able to open exactly one phase
file (plus this overview and the referenced rules) and finish that phase without any other prior context.

---

## Scope

**In scope (Tier 1, from `gaps.md` §3):**

| Gap | Title | Phase |
|---|---|---|
| G1 | `Should()` for reference types / objects | Phase 02 |
| G3 | Structural (deep) equality — `BeEquivalentTo` for object graphs + config hook | Phase 03 |
| G4 | Collection assertions | Phase 04 |
| G5 | Custom-assertion extension point | Phase 05 |
| G6 | Exception assertions | Phase 06 |
| G2 | `.Not.` shape — **decided** — realised as the migration workstream (`MIGRATION.md` + codemod) | Phase 07 |

**Enabling work pulled in because Tier 1 cannot ship without it:**

| Phase | Why it exists |
|---|---|
| Phase 01 — minimal value formatting | `gaps.md` names the value-formatting engine (G15) as *"a prerequisite for G3 and G4, not a follow-up"*. A `BeEquivalentTo` or collection failure whose message renders a `List<T>` as `System.Collections.Generic.List\`1[...]` is not a usable assertion. Phase 01 delivers **only the minimum-viable slice** G3/G4 need — not the full G15 engine. See ADR-004. |

**Out of scope (everything not in the Tier-1 block — `task.md` non-goals):**
Tier 2 (G7 `Task<T>` overloads, G8 numeric `…OrEqualTo`, G9 string `MatchEquivalentOf`, G10 remaining type
families), Tier 3 (G11 `AssertionScope`, G12 `ExecutionTime`, G13 `.And`, G14 `.Which`), and the whole §6
complete-surface set (G15 full engine, G16 framework decoupling, G17 authoring primitives, G18–G25). The
`§7` cleanups (`string?` in the nullable string comparers, `NotComparerBase` missing `Satisfy`,
`BeOneOf(params)` dropping `because`) are **not** Tier-1 and are not addressed here except where a phase
touches the same file for its own reason (noted in that phase).

---

## Dependency Graph

```
        01 (formatting, internal)
         │  depended on by 02, 03, 04
         ▼
        02 — G1 object Should()
         │  depended on by 03, 04, 05
   ┌─────┴─────┐
   ▼           ▼
  03 — G3     (03 also feeds 04: ContainEquivalentOf reuses deep equality)
   │           │
   └────►  04 — G4 collections
                 │  depended on by 05
                 ▼
                05 — G5 extension point
                 │
                 ▼
                07 — G2 migration tooling  ◄──── 06 (G6) also feeds 07

        06 — G6 exceptions   [independent — may run any time after 01 is not even required]
```

`Depends on` / `Depended on by` is restated at the top of every phase file. A revert cascades **downward**:
reverting 02 requires reverting 03, 04, 05 (and re-checking 07); reverting 03 requires reverting 04, 05;
06 can be reverted alone. 01 is the root — reverting it cascades to 02, 03, 04.

**Parallelism:** 06 (G6) is fully independent and is the recommended warm-up / parallel track. 01 is also
independent and should land first because 02/03/04 all consume it. Everything else is a chain.

---

## Architecture Decision Records

These are lightweight ADRs — decision, context, alternatives rejected. They are cross-cutting; a phase that
contradicts one of these must update the ADR here rather than silently diverging.

### ADR-001 — The object `Should()` entry point is **non-generic** `Should(this object)`
**Decision.** Add `public static ObjectComparer Should(this object subject)` returning a **non-generic**
`ObjectComparer` whose `Subject` is typed `object`. Do **not** add a generic `Should<T>(this T subject)`.

**Context.** `ShouldExtensions` already declares `Should<T>(this T subject) where T : struct, Enum`
(`ShouldExtensions.cs:68`). C# does not treat generic constraints as part of a method signature, so a second
`Should<T>(this T subject)` — with *any* constraint, including `where T : class` — is a duplicate-member
compile error (CS0111), not an overload. Therefore the object entry point **cannot** be a `Should<T>`.
A non-generic `Should(this object)` sidesteps the collision entirely: value types keep binding to their
concrete overloads, enums to the enum generic, `string` to the string overload (all more specific than
`object`), and only reference types with no more-specific overload fall through to `object`.

**Alternatives rejected.**
- `Should<T>(this T) where T : class` → `ObjectComparer<T>` (what `gaps.md` §G1 tentatively sketched):
  does not compile — CS0111 against the enum generic. This is the single most important finding of the G1
  prototype and the reason the doc said "prototype overload resolution first."
- A differently-named entry point (e.g. `ShouldObject()`): breaks the uniform `.Should()` surface and the
  migration codemod, which assumes `.Should()`.

**Consequence.** `ObjectComparer` loses the compile-time subject type. `Be`/`BeSameaAs`/`BeEquivalentTo`
take `object`. This is acceptable — every consumer call site (`NotBeNull`, `BeNull`, `Be`, `BeSameAs`) works
on `object`. If a typed drill-down (`.Which`, `As<T>()`) is ever wanted it is Tier 3 (G14) / §6.3, not here.

### ADR-002 — Collections enter via `Should<T>(this IEnumerable<T>)`
**Decision.** Add `public static CollectionComparer<T> Should<T>(this IEnumerable<T> subject)`. This generic
is over `IEnumerable<T>`, not `T`, so it does **not** collide with the enum generic or with ADR-001's
`object` overload.

**Context / overload interplay to verify in the phase (not assume):** `string` is `IEnumerable<char>` and
must keep binding to `Should(this string)` (more specific wins) — a regression test pins this. A
`List<int>` / `int[]` binds to the collection overload (more specific than `object`). A reference type that
*also* implements `IEnumerable<T>` will bind to the collection overload — that is the intended behaviour for
DTO collections; a type where it is wrong is out of the Tier-1 call-site inventory.

### ADR-003 — `.Not.` is the only negation shape (G2, already decided upstream)
**Decision.** Negation stays the `Not` property (`x.Should().Not.Be(y)`). No `NotXxx` alias methods, not even
`[Obsolete]` shims. Every new comparer in Phases 02–06 ships a matching `Not<X>Comparer` following the
existing symmetry. The FluentAssertions `NotXxx(` → `Not.Xxx(` rewrite is a **codemod** (Phase 07), not a
library feature. This ADR simply records that the upstream decision in `gaps.md` §G2 is binding on every
phase: do not add a negated *method*.

### ADR-004 — Ship a **minimal** value formatter now, not the full G15 engine
**Decision.** Phase 01 introduces one internal helper — `ValueFormatter.Format(object)` — handling null,
`string` (quoted), `char` (quoted), `IEnumerable` (bounded, element-wise), and arbitrary objects
(type + public member dump, bounded). Existing message construction (`ComparerBase.FormatSubject`) is routed
through it. Every new comparer in 02–04 formats subjects/expecteds through it.

**Context.** `gaps.md` G15 is a §6 item (not Tier 1) but is explicitly called a prerequisite for G3/G4.
Building the full FluentAssertions formatter subsystem (`IValueFormatter`, `FormattedObjectGraph`,
`MaxLinesExceededException`, scoped registration) is a large, separate effort and is **out of scope**.

**Alternatives rejected.**
- Do nothing and let G3/G4 interpolate raw (`$"{subject}"`): produces unreadable messages for collections
  and DTOs — fails the "usable assertion" bar the phases are tested against.
- Build full G15 first: scope creep beyond Tier 1; not needed to unblock migration.

**Consequence.** The minimal formatter is deliberately not pluggable and has no public formatter-registration
API. When full G15 is scheduled it replaces `ValueFormatter` internals; the `Format(object)` call sites stay.

### ADR-005 — Build on the existing `CompareException` / `xunit.assert` coupling
**Decision.** Every new failure path throws via `CompareException.New(...)`, keeping the single-exception
contract. The library stays xUnit-coupled (G16 is out of scope). No new package references are added to
`FatCat.Testing` — G1/G3/G4/G6 are all implementable with BCL + reflection only.

### ADR-006 — The extension point (G5) exposes `ComparerBase`, it does not invent a new base
**Decision.** G5 makes the *existing* `ComparerBase<TSubject, TComparer>` the documented, supported
extension point: relax `Subject` from `protected` to `protected`-readable in a way a derived comparer in a
consumer assembly can use, expose `CompareException.New` as the documented failure primitive, and ship a
`MIGRATION.md` / doc sample showing a consumer comparer (mirroring Toolkit's `WebResultAssertions` shape:
derive, delegate to inner `.Should()` calls, return `this`). No `AndConstraint`, no `AssertionChain`, no
`[CustomAssertion]` stack-trace machinery (those are G17, out of scope). Consumers' custom assertions
delegate to inner `.Should()` calls, so the base class + `CompareException.New` is provably enough
(`gaps.md` §G5 mitigating factor).

---

## Assumptions

1. **The sln is at the repo root**, `Fatcat.Testing.sln` — there is no `src/` folder. All toolchain commands
   (`dotnet build/test/format`, `dotnet csharpier .`) run from the repo root, as `toolchain.md` and the
   `standards-review` skill now state (both were corrected when the source tree was moved to the root).
2. `net10.0`, `ImplicitUsings` on, `Nullable` disabled at the project level. `ShouldExtensions.cs` carries a
   file-local `#nullable enable` and `string?` params today; new files follow the project default (no
   nullable ref annotations) per `types.md` unless they live inside `ShouldExtensions.cs` and must match its
   existing local style.
3. Every phase runs on branch `task/tier1_gaps` (created off `main`; if the executing session is already on
   a non-`main` branch, it uses that branch — per `task.md` commit policy). No pushes to remote — human
   review gates the push.
4. TDD is mandatory and enforced per phase: tests are written and committed red-first where the orchestrator
   captures that, and the full `Good/Bad/BadShowsCorrectMessage/BadWithBecause` + `Not` + nullable/relevant
   variants exist before a phase is "done" (`testing.md` "Required Coverage").
5. The exact failure-message wording is part of the public contract and is pinned by tests. Each phase file
   proposes message strings; if an executing session changes one it updates the phase's test list and logs a
   deviation.

## Open Questions (resolve before or during the noted phase; stop and ask if the answer changes the design)

- **OQ-1 (Phase 02, G1) — object equality semantics.** `ObjectComparer.Be` compares via `object.Equals`.
  Confirm that is the intended semantic for the ~712 `Be` sites (value equality, not reference equality).
  `BeSameAs` covers reference identity separately. *Assumed: `Equals`.* Materiality: medium — changes what
  `Be` means. **Prototype in Phase 02 against a sample of real Fog/Toolkit call sites before finalising.**
- **OQ-2 (Phase 03, G3) — the config hook surface.** Fog registers a `DateTime` closeness rule via
  `options.Using<DateTime>(...).WhenTypeIs<DateTime>()` in two files. FatCat must offer *some* equivalent or
  those suites break. **Assumed shape:** a global, static registration (e.g.
  `EquivalencyOptions.Using<DateTime>((a, b) => ...)`) applied by `BeEquivalentTo`, because the consumers
  register it once in test infrastructure, not per call. Confirm global-static vs per-call `options =>`
  lambda before building — it changes the public API surface of G3. This is the highest-uncertainty
  decision in the plan; Phase 03 must pin it first and **stop and ask if a global static is unacceptable**.
- **OQ-3 (Phase 04, G4) — `BeEquivalentTo` on collections ordering.** `gaps.md` says do not build
  `WithStrictOrdering` (zero usages). Assumed: collection `BeEquivalentTo` is order-insensitive (multiset
  equivalence) matching FluentAssertions' default. Confirm against the 4 `Equal` (order-sensitive) sites —
  `Equal` stays order-sensitive, `BeEquivalentTo` does not.
- **OQ-4 (Phase 05, G5) — how much to relax `Subject`.** Making `Subject` usable from a consumer-assembly
  subclass without making it fully `public`. Options: keep `protected` (works for subclasses, which is the
  documented pattern) vs add a `public` accessor. *Assumed: `protected` is sufficient* because the
  documented pattern is "derive a comparer," and consumers' existing assertions delegate to inner `.Should()`
  rather than reading `Subject`. Confirm no consumer needs external `Subject` read.

---

## Migration obligation (applies to every phase)

Per `gaps.md` §4/§5.5, a gap is **not done** until its FluentAssertions→FatCat mappings are appended to
`MIGRATION.md` (§5.2 table) and proven by a compiling test (the FluentAssertions-shaped call, rewritten per
§5.2, in that phase's test set). `MIGRATION.md` is created by the first phase that needs it (Phase 02) and
grown by each subsequent phase. Phase 07 delivers the codemod and the known-unsupported list and verifies
the table is complete against the Tier-1 call-site inventory.

## Definition of Done (every phase — from `task.md`)

- Tests written before implementation (TDD, red→green).
- No compiler warnings introduced; namespaces match folder paths exactly.
- Follows all rules in `.claude/rules/csharp` — no exceptions; no banned patterns (`not-allowed.md`).
- All tests pass (`dotnet test Fatcat.Testing.sln`).
- `dotnet format style` + `dotnet format analyzers` run on modified files; `dotnet build` (runs CSharpier);
  `dotnet csharpier .` as the final step.
- `code-review` (standards-review skill) run on uncommitted code; findings resolved before commit.
- One atomic commit on `task/tier1_gaps`, message referencing the phase file. No push.
- Phase report produced (files touched, test counts, deviation log, discovered risks).

See `ORCHESTRATOR.md` for how phases are sequenced, the fresh-session rule, and the halt-on-failure policy.
