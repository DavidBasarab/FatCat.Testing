# Migrating from FluentAssertions to FatCat.Testing

FatCat.Testing is deliberately **not** a source-compatible clone of FluentAssertions. The negation shape
differs by design: FatCat expresses negation through the `Not` property (`x.Should().Not.Be(y)`) rather than
`NotXxx` methods. This guide is the migration path — anybody coming from FluentAssertions can follow it, not
just the FatCat repos.

This document grows one section per gap as gaps close. Each gap appends its rows to the mapping table below.

## Mapping Table

The rule is uniform: **`NotXxx(` → `Not.Xxx(`**. FluentAssertions negations are always the literal `Not`
followed by a PascalCase method name, which makes the rewrite mechanical. Positive assertions that FatCat now
supports for a new subject type map one-to-one.

| FluentAssertions | FatCat.Testing | Sites | Gap |
|---|---|---|---|
| `.Should().NotBeNull()` | `.Should().Not.BeNull()` | 216 | G1 |
| `.Should().NotBeSameAs(x)` | `.Should().Not.BeSameAs(x)` | 6 | G1 |
| `.Should().NotBe(x)` | `.Should().Not.Be(x)` | 1 | G1 |
| `.Should().Be(x)` (object) | `.Should().Be(x)` | 712 | G1 |
| `.Should().BeNull()` (object) | `.Should().BeNull()` | 111 | G1 |
| `.Should().BeEquivalentTo(x)` (object graphs) | `.Should().BeEquivalentTo(x)` | 233 | G3 |
| `.Should().NotBeEquivalentTo(x)` | `.Should().Not.BeEquivalentTo(x)` | 1 | G3 |
| `.Should().Contain(x)` (collection) | `.Should().Contain(x)` | — | G4 |
| `.Should().NotContain(x)` | `.Should().Not.Contain(x)` | 12 | G4 |
| `.Should().BeEmpty()` (collection) | `.Should().BeEmpty()` | — | G4 |
| `.Should().NotBeEmpty()` | `.Should().Not.BeEmpty()` | 9 | G4 |
| `.Should().HaveCount(n)` | `.Should().HaveCount(n)` | 16 | G4 |
| `.Should().ContainSingle()` | `.Should().ContainSingle()` | 11 | G4 |
| `.Should().ContainEquivalentOf(x)` | `.Should().ContainEquivalentOf(x)` | 12 | G4 |
| `.Should().NotContainEquivalentOf(x)` | `.Should().Not.ContainEquivalentOf(x)` | 1 | G4 |
| `.Should().OnlyContain(predicate)` | `.Should().OnlyContain(predicate)` | 5 | G4 |
| `.Should().Equal(x)` (order-sensitive) | `.Should().Equal(x)` | 4 | G4 |
| `.Should().BeEquivalentTo(x)` (collection, order-insensitive) | `.Should().BeEquivalentTo(x)` | — | G4 |
| `.Should().OnlyHaveUniqueItems()` | `.Should().OnlyHaveUniqueItems()` | 2 | G4 |
| `.Should().BeInDescendingOrder()` | `.Should().BeInDescendingOrder()` | 1 | G4 |
| `action.Should().Throw<T>()` | `action.Should().Throw<T>()` | 8 | G6 |
| `func.Should().ThrowAsync<T>()` | `func.Should().Throw<T>()` (on a `Func<Task>`) | — | G6 |
| `action.Should().NotThrow()` | `action.Should().Not.Throw()` | — | G6 |
| `action.Should().Throw<T>().WithMessage(m)` | `action.Should().Throw<T>().WithMessage(m)` | — | G6 |

**Each gap task appends its rows here as it lands.** The table above covers only what the current call-site
inventory proves is in use; it is not the complete negation surface.

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
port. If a future assertion truly required that machinery it would be new, out-of-scope work — for now these
primitives belong on the known-unsupported list.

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

### No FatCat equivalent — known unsupported (deferred, §6.3)

The minimal surface intentionally omits the richer FluentAssertions exception assertions. These have **no**
FatCat equivalent yet and are on the known-unsupported list (Phase 07 finalises it):

- `ThrowExactly<T>()` — exact-type (non-derived) match.
- `WithInnerException<T>()` / `WithInnerExceptionExactly<T>()`.
- `Where(predicate)` — predicate over the caught exception.
- `WithParameterName(name)` — `ArgumentException.ParamName` assertion.
- `NotThrowAfter(...)` and other async-completion / timing helpers.
