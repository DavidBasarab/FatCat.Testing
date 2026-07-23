# final_gaps — Close-out and Replacement-Claim Summary

This is the phase 20 audit of the `final_gaps` plan (phases 01–19). It records the library-side replacement
claim exactly as the code supports it — no more, no less. See [00-overview.md](00-overview.md) *Exit criteria,
restated* for what "done" means for this repo (ADR-1 puts Toolkit and Fog out of reach; this repo can only
prove that the library **can** support what they use).

## Gaps closed

| Gap | Description | Delivered by |
|---|---|---|
| G15 | Value formatting engine (`ValueFormatter`, public) | phase 02 |
| G6 | Exception assertions (`Throw`/`ThrowExactly`/`NotThrow`, async forms) | phases 03, 14 |
| G4 | Collections entry points + assertions | phases 04, 05 |
| G1 | Object comparer + `Should<T>(this T) where T : class` | phase 06 |
| G3 | `BeEquivalentTo` equivalency engine (objects, collections, `Equivalency.Using<T>`) | phases 07, 08, 09 |
| G5 | Custom-comparer extension point (public `Subject`, worked example) | phase 10 |
| G2 | Migration codemod (`tools/Convert-FluentAssertions.ps1`) + `MIGRATION.md` | phase 11 |
| G26 | Method-level completeness across the type families | phases 12–19 |

## Families that are complete for the audited surface

Booleans, Characters, DateTimes, Doubles/Floats, Enums, Guids, Numbers, Strings, TimeSpans, Collections,
Objects, and Exceptions all have `.Should()` entry points and cover every FluentAssertions construct the
audited call-site inventory shows in use. Every `✅ supported` row in `MIGRATION.md` §3 names a test class that
exists, compiles, and passes as part of the green suite (verified programmatically — see Audit results below).

## Deliberate omissions (the library does NOT claim these)

These are chosen cuts, not oversights. They are documented so the "replacement for FluentAssertions" claim is
scoped honestly.

- **Collections (G26 deep cuts):** `ContainInConsecutiveOrder`, `HaveElementPreceding` /
  `HaveElementSucceeding`, and `ContainItemsAssignableTo` are not shipped — no consumer, marginal value.
- **Exceptions (G26 / wrappers):** FluentAssertions' `Awaiting` / `Invoking` / `Enumerating` wrappers,
  `WithResult`, and `CompleteWithinAsync(TimeSpan)` are not shipped (`CompleteWithinAsync` would require a
  banned timing primitive).
- **`WithMessage` is exact-match only** — no `*` wildcard support (rewrite as `Where(e => e.Message.Contains(...))`).
- **`because` replaces rather than appends; no `becauseArgs`** (ADR-2).
- **xUnit-only coupling** (ADR-3, see the G16 memo below).
- **`BeEquivalentTo` ships default options plus one hook** (`Equivalency.Using<T>`); the other ~45 option
  methods are unsupported (ADR-6).
- **Collection `BeEquivalentTo` is order-insensitive with no `WithStrictOrdering` opt-out** (ADR-12).

## Known coverage gaps — shipped code that is not fully test-complete

These methods exist and work, but their test coverage is not the full `Good`/`Bad`/`Not`/nullable set the
standards call for. They are recorded here so the catalog is not read as claiming total coverage. Closing them
is a follow-up top-up, not part of this plan.

- **Nullable DateTime difference chains.** The `BeLessThan(t).Before(x)` / `.After(x)` family (and the four
  other builders) ship on `DateTimeComparer` but **not** on `NullableDateTimeComparer`.
- **Positive `BeInRange` for `double` / `float`.** `DoubleComparer.BeInRange` and `FloatComparer.BeInRange`
  ship and are exercised through the negated `DoubleNotBeInRangeTests` / `FloatNotBeInRangeTests`, but there is
  no dedicated positive `DoubleBeInRangeTests` / `FloatBeInRangeTests` class. (`Numbers` has `IntBeInRangeTests`.)
- **No `Nullable*ContainEquivalentOfTests`** for collections — the nullable-element equivalency path has no
  dedicated test class.

## Out of scope — owned by the `tier_2_gaps` plan (not this plan)

`MIGRATION.md` §3 carries three rows relabeled from a misleading "pending (phase 15/16)" to "out of scope —
`tier_2_gaps`". These are G7–G10 work owned by the independent `tier_2_gaps` plan and were **deliberately not**
shipped by `final_gaps` phases 15/16. Verified genuinely absent from the library by grep at close-out:

- `BeGreaterOrEqualTo` / `BeLessOrEqualTo` on **numerics** (FA numeric spelling) — `tier_2_gaps` phases 02/03
  (G8). *(Note: `TimeSpan` already ships `BeGreaterThanOrEqualTo` / `BeLessThanOrEqualTo` with tests — that is
  a different type family and unaffected.)*
- `MatchEquivalentOf` on **string** — `tier_2_gaps` phase 04 (G9).

## Audit results (exit criteria 1–5)

1. **Every audited FluentAssertions construct has a FatCat equivalent and a `MIGRATION.md` row.** Confirmed for
   the audited surface. The only rows without a FatCat equivalent are the `tier_2_gaps`-owned rows above and
   the §6 Known-Unsupported list, each with a recommended rewrite.
2. **Every `✅ supported` row names a real, compiling, passing test class.** Verified programmatically: all
   "Proven by" tokens in §3 resolve to test classes that exist under `Tests.FatCat.Testing/`. Misses: **0**.
3. **Codemod clean and idempotent.** `tools/Convert-FluentAssertions.ps1` re-run over the in-repo fixture tree
   rewrote 6 negations on the first pass, 0 on the second (idempotent), and reported all four uncatchable
   cases. Output matched every `tools/fixtures/expected/*.expected` file.
4. **README documents** the full assertion catalog, the custom-comparer extension point, the xUnit requirement
   (ADR-3), and the behavioural divergences (`.Not.` shape, `because` replaces, order-insensitive collection
   equivalency). Confirmed.
5. **`dotnet build` and `dotnet test` green, no new warnings.** 1869 passing / 1869 total (baseline), build
   exit 0. (Built and tested with `-p:CSharpier_Bypass=true` per the verified toolchain protocol.)

Documentation-catalog check: performed as a **manual audit** (the phase's stated default) rather than a
reflection test — every public assertion method name on the library comparers was checked to appear in the
README Assertion Catalog; all present. The reflection-test option (`DocumentationCatalogTests`) remains
available if a standing guard is later wanted.

## G16 / OQ-8 — framework coupling: a decision for the human reviewer

**This is a decision to be made by a human, not by this phase.** The plan deliberately deferred it (ADR-3) so
it could be re-surfaced now, with the full — now roughly doubled — assertion surface in view.

**Current state (the coupling as it stands today):**

- `CompareException` derives from xUnit's `XunitException`.
- `xunit.assert` is the library's single package reference.
- A failure raised by FatCat.Testing is only recognised as a test failure by an **xUnit** runner. NUnit,
  MSTest, TUnit, and MSpec are unsupported today.

**Why deciding later is more expensive than deciding earlier:**

- When ADR-3 deferred this, the assertion surface was roughly half its current size. This plan added
  collections, objects, the equivalency engine, exceptions, and the G26 completeness pass — the surface
  roughly **doubled**.
- A framework-detection shim touches **every failure path** in the library (every site that raises a
  `CompareException`) and would **re-pin every message-asserting test** in the suite (the message text is part
  of the public contract and is asserted directly). Both of those grew with the surface.
- ADR-3's warning stands and is now realised: this is the one decision in the plan that gets **harder, not
  easier, by waiting**.

**The concrete cost of deciding each way, now:**

- **Keep the xUnit coupling (do nothing).** Zero engineering cost. The library stays honest about being
  xUnit-only (already stated in README `## Requirements` and `## Known Limitations`). Both known consumers
  (Toolkit, Fog) are xUnit, so nothing is blocked today. The cost is entirely external: any future non-xUnit
  consumer cannot use the library.
- **Build the detection shim now.** Real work with zero migration value for the current consumers: introduce a
  framework-neutral failure-raising seam, route every `CompareException.New` through it, and re-verify /
  re-pin every message test in the suite. Riskiest to land on top of the freshly-doubled surface; cheaper now
  than after any further growth, but still substantial.
- **Defer again.** Legitimate, but the price keeps rising with every future assertion added — the surface only
  grows.

**Recommendation to the reviewer:** none is made here by design. The trade-off is a product decision (is a
non-xUnit consumer a real target?), not a code decision. This memo exists so that decision is made
deliberately rather than by omission.

## Hand-off (not this plan's work)

1. The consuming repos migrate themselves (ADR-1), in dependency order: Toolkit, then the custom-assertion
   port, then Fog — against the published package, using `MIGRATION.md` and the codemod.
2. The G16 decision above is open and flagged.
3. `tier_2_gaps` (G7–G10) is an independent plan; the three out-of-scope rows above are its work.
4. Deferred/optional items across the phases: G17 authoring primitives, the OQ-2 `string?` /
   expression-body cleanup in `Strings/`, the coverage gaps listed above, and the G26 deep-cut
   collection/exception omissions.

Nothing is pushed. Human review gates the push.
