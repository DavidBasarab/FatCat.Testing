# Phase 10 — Custom-assertion extension point (G5)

- **Work item:** `final_gaps`
- **Gap:** **G5** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **medium.** The code change is small (an accessibility decision on `ComparerBase.Subject`); the
  risk is choosing the supported extension contract wrongly and having consumers build on something that
  later changes.
- **Depends on:** 06 (object comparer — the pattern a custom comparer follows)
- **Depended on by:** — (G17, if it ever happens, follows from this phase's report)
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

367 call sites in the consuming repos go through **project-defined** assertions — `BeOk`, `BeNotFound`,
`BeGet`, `BeSuccessful`, `BePost`, `BeBadRequest`, `BeUnauthorized`, `BeDelete`, `BeUnsuccessful`. Both repos
build them on FluentAssertions' `ReferenceTypeAssertions<TSubject, TAssertions>` returning `AndConstraint<T>`.

**This is smaller than the number suggests** (`remaining_gaps.md` §4 G5). Those assertions delegate to inner
`.Should()` calls — there is **zero** dependency on FluentAssertions' `Execute.Assertion` / `AssertionChain`
engine. They need only two things:

1. a public base class with an **accessible `Subject`**, and
2. a chainable return.

`ComparerBase<TSubject, TComparer>` already provides both — assertion methods return `(TComparer)this` and it
holds `Subject`. So G5 is mostly *deciding and documenting* that `ComparerBase` is the supported extension
point, and resolving the one accessibility mismatch.

**Repo boundary (ADR-1, ADR-9).** `remaining_gaps.md` says "port one of the Toolkit assertion classes as
proof". This plan **cannot touch Toolkit**. Instead, the proof is a custom comparer built **in this repo's
test project**, modelled on the `WebResultAssertions` shape — a subject with a status-code-ish property and
`BeOk`-style methods — proving it chains and throws `CompareException`.

**OQ-1 gates this phase** (orchestrator precondition): should `ComparerBase.Subject` become `public`, or stay
`protected`? Today it is `protected`, which is enough for a **derived** comparer. Toolkit's helpers read
`Subject` from **outside** the type. FluentAssertions' `ReferenceTypeAssertions.Subject` is public. Proposal:
make it `public` — but build the proof comparer **both ways** and let the result decide before committing.

Read before starting: [00-overview.md](00-overview.md) (**ADR-1**, **ADR-9**, **ADR-11**, **OQ-1**),
`FatCat.Testing/Comparers/ComparerBase.cs`, phase 06's object comparer as the pattern,
`.claude/rules/csharp/naming-and-structure.md`, `types.md`, `errors.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope**

- The accessibility decision on `ComparerBase.Subject` (OQ-1) and, if `public`, the one-line change.
- A worked custom-comparer example in `README.md` → `## Custom Comparers`.
- A proof custom comparer in `Tests.FatCat.Testing/Extensibility/`, with tests proving it chains, negates,
  and throws `CompareException` with the expected message.
- A statement in the phase report of whether a failing custom assertion points at the **test** line or the
  **library** line (this is what decides whether G17 is ever needed — ADR-11).

**Out of scope**

- **G17 authoring primitives** (ADR-11): no `AssertionChain`, `ForCondition`, `FailWith`, `BecauseOf`,
  `Given`, message templating. G5 alone unblocks both repos; G17 is judged *after* this, from this phase's
  stack-trace observation.
- Any edit to Toolkit or Fog (ADR-1).
- `becauseArgs` (ADR-2) — the extension point uses `string because = null` like everything else. A custom
  comparer that wants interpolation writes `$"..."`.
- The `Task<T>` overloads (a `tier_2_gaps` concern, its ADR-2).

---

## Design

### The decision

Making `ComparerBase<TSubject, TComparer>.Subject` **public** is the recommendation:

- FluentAssertions exposes `Subject` publicly; consumers migrating expect to read it.
- Toolkit's assertions read `Subject` from outside the comparer type (they compose, delegating to
  `Subject.StatusCode.Should()...`), which `protected` does not allow.
- It changes nothing for existing internal callers.

Build the proof comparer first with `Subject` `protected` and observe exactly where it fails to compile
(reading `Subject` from a composing helper), then change to `public`. That failure is the evidence for the
decision; record it. **Get OQ-1 confirmed before committing** — this widens the public contract of the base
class, which is a real API commitment.

Apply the same change to `NotComparerBase` if the symmetry warrants it (it does — a custom negated comparer
would hit the same wall). Note `remaining_gaps.md` §7's observation that `NotComparerBase` lacks `Satisfy`;
adding it is **out of scope** here but flag it in the report.

### The proof comparer (test project)

`Tests.FatCat.Testing/Extensibility/` — model the `WebResultAssertions` shape without any Toolkit
dependency:

```csharp
// A stand-in for the kind of subject consumers assert on.
public class FakeWebResponse
{
	public int StatusCode { get; set; }
}

// The custom comparer a consumer would write, built on the library's public base.
public class FakeWebResponseComparer(FakeWebResponse subject)
	: ComparerBase<FakeWebResponse, FakeWebResponseComparer>(subject)
{
	public FakeWebResponseComparer BeOk(string because = null)
	{
		if (Subject.StatusCode != 200) { CompareException.New(because ?? $"status code {Subject.StatusCode} should be 200 (OK)"); }

		return this;
	}

	public FakeWebResponseComparer BeNotFound(string because = null)
	{
		if (Subject.StatusCode != 404) { CompareException.New(because ?? $"status code {Subject.StatusCode} should be 404 (Not Found)"); }

		return this;
	}
}

public static class FakeWebResponseShouldExtensions
{
	public static FakeWebResponseComparer Should(this FakeWebResponse subject) { return new FakeWebResponseComparer(subject); }
}
```

The point this proves: a consumer builds a custom comparer with **only** `ComparerBase`, `Subject`,
`CompareException.New`, and a `Should` extension — no library internals, no FluentAssertions engine. If any of
that requires something the library does not publicly offer, that is the G5 gap, and this phase closes it.

This lives in the **test project**, so it also demonstrates the composing pattern (`BeOk` delegating to
`Subject.StatusCode.Should().Be(200)` is an alternative body — show both the direct form above and, in the
README, the delegating form, since that is what the real repos do).

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Extensibility/`, namespace
`Tests.FatCat.Testing.Extensibility`.

1. **Red.** `CustomComparerTests.cs`:
   - `GoodBeOk` — `new FakeWebResponse { StatusCode = 200 }.Should().BeOk();`
   - `BadBeOk` / `BadBeOkShowsCorrectMessage` → `"status code 500 should be 200 (OK)"` / `BadBeOkWithBecause`
   - `GoodBeOkChains` — `.Should().BeOk().BeOk()` returns the comparer and chains (proves the `this` return)
   - `GoodBeNotFound` / `BadBeNotFound...`
   - `GoodCustomComparerUsesInheritedBeOfType` — the custom comparer inherits `BeOfType` etc. from the base

2. **The accessibility experiment.** First write a variant of the comparer that reads `Subject` from a
   **separate composing helper class** (mimicking how Toolkit's helpers read it from outside). Observe the
   compile error with `Subject` `protected`. Record it. Then apply the `public` change and confirm it
   compiles. Keep the composing variant as a test so the capability stays proven.

3. **Green.** Make `ComparerBase.Subject` (and `NotComparerBase.Subject`) `public` if OQ-1 confirms.

4. Run the whole suite — the accessibility widening cannot break anything (widening never does), which the
   green suite confirms.

5. **Stack-trace observation.** Deliberately fail a custom assertion and note in the report whether the xUnit
   failure points at the **test** line or at `CompareException.New` inside the comparer. This is the ADR-11
   input; it is an observation, not a fix.

---

## Files

**Changed**

- `FatCat.Testing/Comparers/ComparerBase.cs` — `Subject` accessibility (if OQ-1 = public)
- `FatCat.Testing/Comparers/NotComparerBase.cs` — same
- `README.md`, `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Extensibility/FakeWebResponse.cs`
- `Tests.FatCat.Testing/Extensibility/FakeWebResponseComparer.cs`
- `Tests.FatCat.Testing/Extensibility/FakeWebResponseShouldExtensions.cs`
- `Tests.FatCat.Testing/Extensibility/CustomComparerTests.cs`

---

## Documentation Updates

**`README.md` → `## Custom Comparers`** — replace the phase-10 placeholder with a worked example:

1. Derive from `ComparerBase<TSubject, TComparer>`, forwarding the subject via the primary constructor.
2. Expose a `Not` property returning a `NotComparerBase`-derived twin (optional but recommended).
3. Return `this` so calls chain.
4. Throw failures with `CompareException.New(because ?? $"...")` — `because` replaces, never `+`-concatenate.
5. Add a `Should(this TSubject)` extension in your own static class.
6. Read `Subject` (now public) and format values with `ValueFormatter.Format` for object/collection subjects.

Show both the direct form and the delegating form (`BeOk` calling `Subject.StatusCode.Should().Be(200)`),
since the real consumer assertions delegate.

**`MIGRATION.md` → `## 3. Mapping Table`** — one row per replaced base concept:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `ReferenceTypeAssertions<T, TAssertions>` | `ComparerBase<TSubject, TComparer>` | `Tests.FatCat.Testing.Extensibility.CustomComparerTests` |
| `AndConstraint<T>` return | return `this` (the comparer) | `Tests.FatCat.Testing.Extensibility.CustomComparerTests` |

**`MIGRATION.md` → `## 5. Behavioural Differences`** — a ported custom assertion drops `becauseArgs`; rewrite
`("...{0}", x)` as `($"...{x}")`. And `because` replaces the message rather than appending.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Extensibility"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] OQ-1 confirmed; the accessibility decision and the compile-error evidence recorded in the report.
- [ ] Tests written before implementation; red observed and recorded.
- [ ] A custom comparer built in the test project on `ComparerBase` alone, proven to pass, fail with the
      expected message, chain, and use `because`.
- [ ] The composing-helper variant (reading `Subject` from outside) compiles — proving the accessibility
      decision.
- [ ] `README.md` → `## Custom Comparers` documents the full recipe with both direct and delegating forms.
- [ ] The phase report states whether a failing custom assertion points at the test line or the library line
      (the ADR-11 / G17 input).
- [ ] `NotComparerBase.Satisfy` gap noted in the report (not fixed here).
- [ ] No new warnings; no banned patterns; the `becauseArgs` form is **not** added.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/10-extension-point.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-10-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Reverting narrows `Subject` back to `protected` and removes the proof comparer. Nothing in the library
depends on the widened accessibility, so the suite stays green. If a later custom comparer (none in this
plan) came to rely on public `Subject`, it would need `protected`-access instead.

---

## Hand-off

The supported extension contract, now documented and proven:

- Derive from `ComparerBase<TSubject, TComparer>` (public, `Subject` public after this phase).
- Return `this`; throw via `CompareException.New`; format via `ValueFormatter.Format`.
- Add a `Should` extension in a consumer-owned static class.

G5 is closed for both repos' needs — they can port `WebResultAssertions`, `FatResultAssertions`,
`EndpointTestExtensions`, and the Fog assertions onto this base **in their own repos**, consolidating the
Toolkit/Toolkit.WebServer near-duplicates during that port (`remaining_gaps.md` §4, "Also").

**G17 decision input:** the phase report's stack-trace observation. If failing custom assertions already
point at the test line, G17 is unnecessary. If they point at the library line, a `[CustomAssertion]`-
equivalent becomes a candidate follow-up plan — but not part of this one (ADR-11).
