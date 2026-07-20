# Migrating from FluentAssertions to FatCat.Testing

## Why migrate

FluentAssertions **7.0.0 is the last release published under the Apache-2.0 licence** — every version from
8.0.0 onward ships under a commercial licence. Projects that need to stay on a permissive, free-to-use
assertion library can no longer take FluentAssertions updates. FatCat.Testing is an Apache-licensed,
drop-in-shaped replacement so those projects can move off FluentAssertions entirely.

FatCat.Testing is deliberately **not** a source-compatible clone of FluentAssertions. The one shape that
differs by design is negation: FatCat expresses negation through the `Not` property
(`x.Should().Not.Be(y)`) rather than through `NotXxx` methods. Positive assertions map one-to-one. This
guide is the migration path — anybody coming from FluentAssertions can follow it, not just the FatCat repos.

The `NotXxx( → Not.Xxx(` rewrite is mechanical and is automated by the codemod described below; the residue
it cannot safely rewrite is small and enumerated for you.

## Running the codemod

`tools/Migrate-FluentAssertions.ps1` applies the single migration transform across a source tree:

```
.Should().NotXxx(   ->   .Should().Not.Xxx(
```

```powershell
# Preview every change and the manual-review list without touching a file
pwsh -Command '. $PROFILE; ./tools/Migrate-FluentAssertions.ps1 -Path C:\Code\FatCat.Toolkit -WhatIf'

# Apply the rewrite
pwsh -Command '. $PROFILE; ./tools/Migrate-FluentAssertions.ps1 -Path C:\Code\FatCat.Toolkit'
```

- **Idempotent** — running it twice is a no-op. An already-converted `.Should().Not.Xxx(` is never rewritten
  again, because the transform only matches an uppercase letter immediately after `Not`.
- **`-WhatIf`-able** — with `-WhatIf` it reports the intended edits and the review list and changes nothing.
- **Reports, never silently skips** the cases the regex cannot safely rewrite — chained negations after
  `.And.` (deferred, gap G13), line-broken `.Should()` / `.NotXxx(` chains, and project-defined names that
  begin with `Not` — to a manual-review list, with file, line, and reason.

Run it with `-WhatIf` first, apply, review the manual list, then build and fix the residue by hand.

This document grows one section per gap as gaps close. Each gap appends its rows to the mapping table below.

## Mapping Table

The rule is uniform: **`NotXxx(` → `Not.Xxx(`**. FluentAssertions negations are always the literal `Not`
followed by a PascalCase method name, which makes the rewrite mechanical. Positive assertions that FatCat now
supports for a new subject type map one-to-one.

Every row below traces to a proving test in `Tests.FatCat.Testing` (the FluentAssertions-shaped call,
rewritten per this table, compiled and executed), so the mapping is verified rather than asserted in prose.

**Objects and reference types (G1)**

| FluentAssertions | FatCat.Testing | Sites | Proving test |
|---|---|---|---|
| `.Should().Be(x)` (object) | `.Should().Be(x)` | 712 | `Objects/ObjectMigrationTests.BeRewrite` |
| `.Should().NotBe(x)` | `.Should().Not.Be(x)` | 1 | `Objects/ObjectBeTests.GoodNotBe` |
| `.Should().BeNull()` (object) | `.Should().BeNull()` | 111 | `Objects/ObjectBeNullTests` |
| `.Should().NotBeNull()` | `.Should().Not.BeNull()` | 216 | `Objects/ObjectMigrationTests.NotBeNullRewrite` |
| `.Should().NotBeSameAs(x)` | `.Should().Not.BeSameAs(x)` | 6 | `Objects/ObjectMigrationTests.NotBeSameAsRewrite` |

**Structural equivalence (G3)**

| FluentAssertions | FatCat.Testing | Sites | Proving test |
|---|---|---|---|
| `.Should().BeEquivalentTo(x)` (object graphs) | `.Should().BeEquivalentTo(x)` | 233 | `Objects/ObjectMigrationTests.BeEquivalentToRewrite` |
| `.Should().NotBeEquivalentTo(x)` | `.Should().Not.BeEquivalentTo(x)` | 1 | `Objects/ObjectMigrationTests.NotBeEquivalentToRewrite` |
| `options.Using<T>((a, b) => ...).WhenTypeIs<T>()` | `EquivalencyOptions.Using<T>((subject, expected) => ...)` | 2 | `Equivalency/EquivalencyOptionsTests` |

**Collections (G4)**

| FluentAssertions | FatCat.Testing | Sites | Proving test |
|---|---|---|---|
| `.Should().Contain(x)` (collection) | `.Should().Contain(x)` | — | `Collections/CollectionMigrationTests.ContainRewrite` |
| `.Should().NotContain(x)` | `.Should().Not.Contain(x)` | 12 | `Collections/CollectionMigrationTests.NotContainRewrite` |
| `.Should().BeEmpty()` (collection) | `.Should().BeEmpty()` | — | `Collections/CollectionMigrationTests.BeEmptyRewrite` |
| `.Should().NotBeEmpty()` | `.Should().Not.BeEmpty()` | 9 | `Collections/CollectionMigrationTests.NotBeEmptyRewrite` |
| `.Should().HaveCount(n)` | `.Should().HaveCount(n)` | 16 | `Collections/CollectionMigrationTests.HaveCountRewrite` |
| `.Should().ContainSingle()` | `.Should().ContainSingle()` | 11 | `Collections/CollectionMigrationTests.ContainSingleRewrite` |
| `.Should().ContainEquivalentOf(x)` | `.Should().ContainEquivalentOf(x)` | 12 | `Collections/CollectionMigrationTests.ContainEquivalentOfRewrite` |
| `.Should().NotContainEquivalentOf(x)` | `.Should().Not.ContainEquivalentOf(x)` | 1 | `Collections/CollectionMigrationTests.NotContainEquivalentOfRewrite` |
| `.Should().OnlyContain(predicate)` | `.Should().OnlyContain(predicate)` | 5 | `Collections/CollectionMigrationTests.OnlyContainRewrite` |
| `.Should().Equal(x)` (order-sensitive) | `.Should().Equal(x)` | 4 | `Collections/CollectionMigrationTests.EqualRewrite` |
| `.Should().BeEquivalentTo(x)` (collection, order-insensitive) | `.Should().BeEquivalentTo(x)` | — | `Collections/CollectionMigrationTests.BeEquivalentToRewrite` |
| `.Should().OnlyHaveUniqueItems()` | `.Should().OnlyHaveUniqueItems()` | 2 | `Collections/CollectionMigrationTests.OnlyHaveUniqueItemsRewrite` |
| `.Should().BeInDescendingOrder()` | `.Should().BeInDescendingOrder()` | 1 | `Collections/CollectionMigrationTests.BeInDescendingOrderRewrite` |

**Exceptions (G6)**

| FluentAssertions | FatCat.Testing | Sites | Proving test |
|---|---|---|---|
| `action.Should().Throw<T>()` | `action.Should().Throw<T>()` | 8 | `Delegates/ActionThrowTests` |
| `func.Should().ThrowAsync<T>()` | `func.Should().Throw<T>()` (on a `Func<Task>`) | — | `Delegates/AsyncActionThrowTests` |
| `action.Should().NotThrow()` | `action.Should().Not.Throw()` | — | `Delegates/ActionNotThrowTests` |
| `action.Should().Throw<T>().WithMessage(m)` | `action.Should().Throw<T>().WithMessage(m)` | — | `Delegates/ActionThrowWithMessageTests` |

**String negations that already shipped before Tier 1**

These were not part of the Tier-1 gap work, but they follow the same rule and the codemod rewrites them, so
they are listed for completeness. Both are backed by existing tests.

| FluentAssertions | FatCat.Testing | Sites | Proving test |
|---|---|---|---|
| `.Should().NotBeNullOrEmpty()` | `.Should().Not.BeNullOrEmpty()` | 4 | `Strings/StringBeNullOrEmptyTests` |
| `.Should().NotBeNullOrWhiteSpace()` | `.Should().Not.BeNullOrWhiteSpace()` | 3 | `Strings/StringBeNullOrWhiteSpaceTests` |

**Each gap task appends its rows here as it lands.** The table above covers only the **proven-in-use Tier-1
calls** from the consumer call-site inventory — it is **not** the complete FluentAssertions negation surface.
A negation on any subject FatCat already supports rewrites the same way (`NotXxx( → Not.Xxx(`) whether or not
it appears here; a negation on a subject whose gap has not landed is on the known-unsupported list below.

> **Deprecated FluentAssertions aliases (not rewritten).** FluentAssertions' own deprecated numeric aliases
> `BeGreaterOrEqualTo` / `BeLessOrEqualTo` normalise to the modern `BeGreaterThanOrEqualTo` /
> `BeLessThanOrEqualTo`. FatCat does **not** yet ship `...OrEqualTo` on the numeric comparers (only
> `BeGreaterThan` / `BeLessThan`), so these two calls (1 site each) are **deferred to gap G8, Tier 2** and are
> intentionally left out of the codemod and the mapping above until that assertion lands. Rewrite them by hand
> after G8, or restructure the assertion.

## G3 — `BeEquivalentTo` structural equivalence and the config hook

`BeEquivalentTo` performs recursive, member-by-member structural equality over public readable instance
properties, recursing into nested objects, with reference-identity cycle detection. Failures report the
member path and both values, e.g. `Expected Address.City to be "Boston" but found "Austin"`.

### The equivalency configuration hook

FluentAssertions lets a suite register a per-type comparison rule through the options builder, typically once
in shared test infrastructure:

```csharp
options.Using<DateTime>((a, b) => a.Should().BeCloseTo(b, 10.Seconds())).WhenTypeIs<DateTime>()
```

FatCat.Testing replaces the per-call options builder with a global, static registration consulted by the
equivalency engine. Register the rule once; the engine uses it instead of `Equals` whenever it compares two
values of that runtime type (including nested members):

```csharp
EquivalencyOptions.Using<DateTime>((subject, expected) => Math.Abs((subject - expected).TotalSeconds) <= 10);
```

| FluentAssertions | FatCat.Testing |
|---|---|
| `options.Using<T>((a, b) => ...).WhenTypeIs<T>()` | `EquivalencyOptions.Using<T>((subject, expected) => ...)` |

Because the registry is global static, a suite that registers a rule must clear it so it does not bleed into
other tests: call `EquivalencyOptions.Reset()` (register → assert → reset, e.g. in a `finally`).

The two Fog registration sites map directly:

- `Fog` `EndToEndTest.cs` — `DateTime` closeness at 10 seconds → `EquivalencyOptions.Using<DateTime>((subject, expected) => Math.Abs((subject - expected).TotalSeconds) <= 10);`
- `Fog` `BrumeTests.cs` — `DateTime` closeness at 1 second → `EquivalencyOptions.Using<DateTime>((subject, expected) => Math.Abs((subject - expected).TotalSeconds) <= 1);`

## G4 — collection assertions

Any `IEnumerable<T>` gains a `.Should()` returning a `CollectionComparer<T>`. The entry point is
`ShouldExtensions.Should<T>(this IEnumerable<T>)` — generic over `IEnumerable<T>`, so it does not collide
with the enum generic or the object overload. `string` is `IEnumerable<char>` but keeps binding to the more
specific `Should(this string)`, so strings are **not** treated as collections. A DTO that itself implements
`IEnumerable<T>` binds to the collection comparer by design.

The positive assertions map one-to-one; the negations follow the uniform `NotXxx( → Not.Xxx(` rule:

| Assertion | Passes when |
|---|---|
| `Contain(item)` | the sequence contains `item` |
| `BeEmpty()` | the sequence has no elements |
| `HaveCount(n)` | the element count equals `n` |
| `ContainSingle()` | the sequence has exactly one element |
| `ContainEquivalentOf(item)` | some element is **structurally equivalent** to `item` (reuses the G3 engine) |
| `OnlyContain(predicate)` | every element satisfies the predicate |
| `Equal(expected)` | element-wise equal in the **same order** |
| `BeEquivalentTo(expected)` | the same elements in **any order** — multiset equivalence (reuses the G3 engine) |
| `OnlyHaveUniqueItems()` | no duplicate elements |
| `BeInDescendingOrder()` | the sequence is sorted descending |

`Equal` is order-sensitive; collection `BeEquivalentTo` is order-insensitive (multiset). `ContainEquivalentOf`
and collection `BeEquivalentTo` compare elements through the same structural-equivalency engine used by object
`BeEquivalentTo`, so structurally-equal-but-not-reference-equal elements match.

## G5 — Writing custom assertions

Both source repos define their own assertions on top of FluentAssertions — roughly 410 call sites go through
project-defined assertions such as `WebResultAssertions`. Those custom assertions derive from
FluentAssertions' `ReferenceTypeAssertions<TSubject, TAssertions>`, return `AndConstraint<T>`, and
**delegate to inner `.Should()` calls** — they do not use `Execute.Assertion` / `AssertionChain`. That last
point is what makes the port possible: the minimum viable extension point is a base to derive from plus a
failure primitive, both of which FatCat already ships.

### The supported extension point

Derive from `FatCat.Testing.Comparers.ComparerBase<TSubject, TComparer>` — the same base every built-in
comparer uses. It is the documented, supported way to author a custom assertion. There is no separate
"custom assertion" base to learn.

- **Base:** `ComparerBase<TSubject, TComparer>`, generic over the subject type and the concrete comparer, so
  chained assertion methods return your derived type rather than the base.
- **Subject:** forward it through the primary constructor to the base. `Subject` is `protected` and is
  readable from a derived comparer in a **consumer assembly** — no accessor widening is required.
- **Failure primitive:** `FatCat.Testing.Exceptions.CompareException.New(message)`. This is the one exception
  the library throws for a failed assertion; call it directly, or delegate to an inner `.Should()` call and
  let the built-in comparer throw it for you.
- **Chaining:** each assertion method returns `this` (typed as your comparer), so calls chain — this is the
  FatCat replacement for returning `AndConstraint<T>`.
- **The `because` override:** every assertion method takes a trailing `string because = null` and uses
  `because ?? "<generated message>"`, so a caller-supplied reason always wins.

An entry point is an extension method returning your comparer, mirroring the built-in `Should()` overloads:

```csharp
public static class ResultShouldExtensions
{
	public static WebResultComparer Should(this WebResult subject)
	{
		return new WebResultComparer(subject);
	}
}

public class WebResultComparer(WebResult subject) : ComparerBase<WebResult, WebResultComparer>(subject)
{
	public WebResultComparer BeOk(string because = null)
	{
		Subject.StatusCode.Should().Be(200, because ?? $"Expected OK but web result was {Subject.StatusCode}");

		return this;
	}

	public WebResultComparer BeNotFound(string because = null)
	{
		Subject.StatusCode.Should().Be(404, because ?? $"Expected Not Found but web result was {Subject.StatusCode}");

		return this;
	}
}
```

`webResult.Should().BeOk()` now reads and behaves exactly like the FluentAssertions original. A worked,
tested version of this example (plus a minimal proving comparer) lives in the test project under
`Tests.FatCat.Testing/CustomAssertions/`, proving a comparer derived in a **separate assembly** can read
`Subject`, fail through `CompareException.New`, and chain.

### The `AndConstraint<T>` → return-`this` mapping

| FluentAssertions | FatCat.Testing |
|---|---|
| `ReferenceTypeAssertions<TSubject, TAssertions>` | `ComparerBase<TSubject, TComparer>` |
| `new AndConstraint<TAssertions>(this)` returned from each method | `return this;` (typed as your comparer) |
| `Execute.Assertion.ForCondition(...).FailWith(...)` | delegate to an inner `.Should()`, or `CompareException.New(because ?? "...")` |
| `subject.Should()` entry point | an extension method returning your comparer |

The negated-comparer convention the built-ins follow (a `Not` property returning a matching
`Not<Type>Comparer`) is **optional** for consumer comparers — add one only if your assertion genuinely needs
a negated form.

### No FatCat equivalent — known unsupported

`Execute.Assertion`, `AssertionChain`, and the `[CustomAssertion]` stack-trace attribute have **no** FatCat
equivalent and are not part of this extension point. Because the consumers' custom assertions delegate to
inner `.Should()` calls rather than driving the assertion pipeline directly, none of them are needed for the
port. If a future assertion truly required that machinery it would be new, out-of-scope work. These
primitives are on the consolidated **[Known Unsupported Constructs](#known-unsupported-constructs)** list.

## G6 — exception assertions

Delegates gain a `.Should()` that runs the delegate and asserts on what it threw. The fluent surface stays
**synchronous** — the comparer runs the delegate and observes the outcome; `.Should()` never becomes async
(`async.md`). Two entry points cover the sync and async delegate shapes:

- `ShouldExtensions.Should(this Action)` → `ActionComparer` — for synchronous throwing code.
- `ShouldExtensions.Should(this Func<Task>)` → `AsyncActionComparer` — for asynchronous throwing code. The
  comparer observes the returned `Task` with a single, isolated `Subject().GetAwaiter().GetResult()` (the one
  permitted blocking call under `async.md`), so the assertion API itself is not async.

`Action` and `Func<Task>` are more specific than `object`, so a delegate value binds to these overloads and
never falls through to `Should(this object)`; neither is an `IEnumerable<T>` nor an enum, so there is no
collision with the collection or enum overloads (pinned by `DelegateShouldOverloadTests`).

| Assertion | Passes when | Notes |
|---|---|---|
| `Throw<TException>(because)` | running the delegate throws a `TException` (or derived) | retains the caught exception so `WithMessage` can chain; returns the comparer |
| `WithMessage(expected, because)` | the retained exception's `Message` **equals** `expected` | chains after `Throw<T>()`; exact match, not wildcard |
| `Not.Throw(because)` | running the delegate throws nothing | the FatCat form of FluentAssertions `NotThrow()` |
| `Not.Throw<TException>(because)` | running the delegate does not throw a `TException` | throwing nothing, or throwing an unrelated type, both pass |

### Negation — `Not.Throw()`, not a `NotThrow` method (ADR-003)

FluentAssertions' `NotThrow()` maps to `.Should().Not.Throw()`. Per ADR-003 (`.Not.` is the only negation
shape — do not add a negated *method*), the library exposes negation through the `Not` property rather than a
`NotThrow` method on the positive comparer. The phase-spec deliverable table listed a positive-comparer
`NotThrow`; it is realised here as `Not.Throw()` to honour ADR-003, which is binding on every phase.

| FluentAssertions | FatCat.Testing |
|---|---|
| `action.Should().Throw<T>()` | `action.Should().Throw<T>()` |
| `func.Should().ThrowAsync<T>()` | `func.Should().Throw<T>()` (`func` is a `Func<Task>`) |
| `action.Should().NotThrow()` | `action.Should().Not.Throw()` |
| `action.Should().Throw<T>().WithMessage(m)` | `action.Should().Throw<T>().WithMessage(m)` |

### Failure messages

Exception types render by their bare CLR `Name` and are pinned by tests:

- `Throw<T>` — `Expected InvalidOperationException but no exception was thrown` / `Expected InvalidOperationException but ArgumentException was thrown`.
- `Not.Throw()` — `Expected no exception but InvalidOperationException was thrown`.
- `Not.Throw<T>()` — `Expected no InvalidOperationException but InvalidOperationException was thrown`.
- `WithMessage` — `Expected exception message "boom" but found "bang"` (the message strings are rendered through `ValueFormatter.Format`, so they are quoted).

### No FatCat equivalent — known unsupported (deferred)

The minimal surface intentionally omits the richer FluentAssertions exception assertions
(`ThrowExactly<T>()`, `WithInnerException<T>()`, `Where(predicate)`, `WithParameterName(name)`, and the
async-completion / timing helpers). These have **no** FatCat equivalent yet; they and their recommended
rewrites are on the consolidated **[Known Unsupported Constructs](#known-unsupported-constructs)** list.

## Known Unsupported Constructs

These FluentAssertions constructs have **no** FatCat.Testing equivalent. The codemod does **not** touch them —
each needs a manual rewrite. They fall outside the Tier-1 migration scope on purpose; the recommended
workaround is listed for each.

### Cross-cutting features

| FluentAssertions | Status | Recommended rewrite / workaround |
|---|---|---|
| `AssertionScope` — batch multiple failures (gap G11) | No equivalent | Drop the `using (new AssertionScope())` wrapper and let each assertion throw on the first failure. Where the batched report genuinely mattered, split into independent `[Fact]`s so each failure is reported on its own. |
| `ExecutionTime` / `.Should().ExecuteWithin(...)` (gap G12) | No equivalent | Measure with `System.Diagnostics.Stopwatch` and assert on the elapsed `TimeSpan` with the existing `TimeSpan` comparers. |
| `.Which` — drill into the asserted value (gap G14) | No equivalent | Capture the value in a local before asserting, then assert on the local: `var caught = ...; caught.Message.Should()...`. |
| `.And` — chain a second assertion on the same subject (gap G13) | No equivalent | Write the second assertion as its own statement on the same subject. The codemod flags `.And.NotXxx(` chains for exactly this reason. |

### `BeEquivalentTo` option methods

FatCat's `BeEquivalentTo` compares public readable instance properties recursively (order-insensitive for
collections) and takes no options builder. The FluentAssertions option lambda methods are not supported:

| FluentAssertions | Status | Recommended rewrite / workaround |
|---|---|---|
| `.BeEquivalentTo(x, options => options.Excluding(...))` | No equivalent | Compare a projection that omits the member, or assert the members you care about individually. |
| `... .Including(...)` | No equivalent | Build an object holding only the included members and compare that. |
| `... .WithStrictOrdering()` | No equivalent | Use collection `Equal(...)` — it is order-sensitive — instead of `BeEquivalentTo`. |
| `... .RespectingRuntimeType()` | No equivalent | Not needed: FatCat's engine already walks the runtime type's public readable properties. |
| The per-call `options.Using<T>(...).WhenTypeIs<T>()` rule | Different shape | Register once, globally, with `EquivalencyOptions.Using<T>((subject, expected) => ...)` and clear it with `EquivalencyOptions.Reset()` (see the G3 section). |

### Custom-assertion primitives

| FluentAssertions | Status | Recommended rewrite / workaround |
|---|---|---|
| `Execute.Assertion.ForCondition(...).FailWith(...)` | No equivalent | Delegate to an inner `.Should()` call, or throw with `CompareException.New(because ?? "...")`. |
| `AssertionChain` | No equivalent | Return `this` from each assertion method to chain (see the custom-assertion section above). |
| `[CustomAssertion]` attribute | No equivalent | Not needed — FatCat does not rewrite stack traces to hide the assertion frame. |

### Richer exception assertions (deferred)

The minimal exception surface (G6) covers `Throw<T>()`, `Not.Throw()`, `Not.Throw<T>()`, and `WithMessage`.
The rest are deferred:

| FluentAssertions | Status | Recommended rewrite / workaround |
|---|---|---|
| `ThrowExactly<T>()` | No equivalent | Use `Throw<T>()` (accepts derived types); if the exact type genuinely matters, catch it and assert `caught.GetType().Should().Be(typeof(T))`. |
| `WithInnerException<T>()` / `WithInnerExceptionExactly<T>()` | No equivalent | Catch the exception, then assert on `caught.InnerException` with the object comparers. |
| `Where(predicate)` | No equivalent | Catch the exception and assert on its members directly. |
| `WithParameterName(name)` | No equivalent | Catch the `ArgumentException` and assert `((ArgumentException)caught).ParamName.Should().Be(name)`. |
| `NotThrowAfter(...)` and async-completion / timing helpers | No equivalent | Measure with `Stopwatch` and assert on the elapsed time. |

## Per-Repo Migration Sequence

FluentAssertions is a **transitive** dependency in `Fog` — it arrives through the `FatCat.*` packages — so the
order is forced. Migrate **`FatCat.Toolkit` first, then `Fog`**.

1. **`FatCat.Toolkit` first.** It references FluentAssertions from **production** projects (`ToolKit`,
   `Toolkit.WebServer`), not just tests, because it ships test helpers under `ToolKit\Testing\`. Replacing
   FluentAssertions there changes the public contract of the Toolkit NuGet package — treat that as its own
   release step, not a side effect of a test cleanup.
2. **Port the custom-assertion layer** (G5) — `WebResultAssertions`, `WebResultClosedOverAssertions`,
   `FatResultAssertions`, `EndpointTestExtensions` — using the [custom-assertion pattern](#g5--writing-custom-assertions)
   above. The Toolkit and Toolkit.WebServer copies are near-duplicates (`FatWebResponse` vs `WebResult`);
   consolidate them during the port rather than porting the same code twice.
3. **`Fog` last**, once it can pick up a FluentAssertions-free Toolkit.

The mechanical steps per repo are small:

1. Swap `global using FluentAssertions;` → `global using FatCat.Testing;` in the `GlobalUsings.cs` files
   (6 across the repos), and drop the ~27 per-file `using FluentAssertions;` in Fog.
2. Run the codemod with `-WhatIf`, review the intended edits and the manual-review list, then run it for real.
3. Hand-fix the manual-review residue (chained `.And.` negations, line-broken chains, project-defined `Not*`
   names).
4. Build, and fix whatever the compiler flags — a negation on a subject whose FatCat gap has not landed will
   only surface here.
