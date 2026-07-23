# FatCat.Testing

A fluent assertion library for .NET — a free, Apache-2.0 replacement for FluentAssertions.

## What It Is

FatCat.Testing is a small, self-contained assertion library. You write assertions as fluent, chainable
sentences:

```csharp
result.Should().Be(expected);
name.Should().StartWith("Fat").EndWith("Cat"); // assertions chain by returning the comparer
count.Should().BeGreaterThan(0);
```

It exists because FluentAssertions stopped being free: `7.0.0` is the last Apache-2.0 release, and every
`8.x` release is commercially licensed. FatCat.Testing is Apache-2.0 and stays that way. It is a
replacement, not a source-compatible clone — see [`MIGRATION.md`](MIGRATION.md) for the differences and how
to move across.

## Installing

```bash
dotnet add package FatCat.Testing
```

Then bring the extension methods into scope, typically through a `GlobalUsings.cs`:

```csharp
global using FatCat.Testing;
```

## Quick Start

Every assertion starts with `.Should()` on the subject and reads like a sentence:

```csharp
using FatCat.Testing;

true.Should().BeTrue();
"hello world".Should().Contain("world");
42.Should().BeGreaterThan(10).BeLessThan(100);
Guid.Empty.Should().BeEmpty();

// Negation is a property, not a method — see ## Negation
response.IsSuccess.Should().Not.BeFalse();
```

A failing assertion throws `CompareException` with a plain-English message describing what was expected. An
optional trailing `because` argument replaces that message:

```csharp
age.Should().BeGreaterThan(18, "a member must be an adult");
```

## Requirements

**FatCat.Testing requires xUnit.** This is not a soft preference — it is a hard coupling:

- `CompareException` derives from xUnit's `XunitException`.
- `xunit.assert` is the library's only package reference.

NUnit, MSTest, TUnit, and MSpec are **not supported**. A failure raised by FatCat.Testing is an
`XunitException`, so it is only recognised as a test failure by an xUnit test runner. If your test project
does not use xUnit, this library will not work for you today.

## Assertion Catalog

One table per subject family. Every assertion below ships today and is covered by tests. Negated forms are
reached through the `Not` property (`x.Should().Not.Be(y)`) and are not listed separately.

Assertions marked *(nullable only)* live on the `Nullable<Type>Comparer` and are reached when the subject is
the nullable value type (`bool?`, `Guid?`, and so on).

### Booleans

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The value equals `expected`. |
| `BeTrue()` | The value is `true`. |
| `BeFalse()` | The value is `false`. |
| `BeNull()` | *(nullable only)* The nullable has no value. |
| `HaveValue()` | *(nullable only)* The nullable has a value. |

### Characters

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The character equals `expected`. |
| `BeDigit()` | The character is a decimal digit. |
| `BeLetter()` | The character is a letter. |
| `BeLetterOrDigit()` | The character is a letter or a digit. |
| `BeLowerCased()` | The character is lower case. |
| `BeUpperCased()` | The character is upper case. |
| `BeWhiteSpace()` | The character is white space. |
| `BeControl()` | The character is a control character. |
| `BeNull()` | *(nullable only)* The nullable has no value. |
| `HaveValue()` | *(nullable only)* The nullable has a value. |

### Collections

Entry points exist for `IEnumerable<T>`, `List<T>`, and `T[]`. Most assertions match elements by **element
equality** (`Equals`, via `EqualityComparer<T>.Default`); `BeEquivalentTo` and `ContainEquivalentOf` match
**structurally** instead (see below). A lazy `IEnumerable<T>` is **snapshotted once** when `.Should()` is
called, so a side-effecting query is enumerated a single time and every chained assertion reads that
snapshot.

| Assertion | What it asserts |
|---|---|
| `Contain(expected)` | The collection contains an element equal to `expected`. |
| `BeEmpty()` | The collection has no elements. |
| `HaveCount(count)` | The collection has exactly `count` elements. |
| `ContainSingle()` | The collection has exactly one element. |
| `Equal(expected)` | The collection equals `expected` element-by-element, **in order**. |
| `BeEquivalentTo(expected)` | The collection has the same elements as `expected`, compared structurally, **ignoring order**. |
| `ContainEquivalentOf(expected)` | The collection contains an element structurally equivalent to `expected`. |
| `OnlyContain(predicate)` | Every element satisfies `predicate`. |
| `OnlyHaveUniqueItems()` | The collection has no duplicate elements. |
| `ContainInOrder(expected)` | The `expected` elements appear in this relative order (not necessarily contiguous). |
| `BeInDescendingOrder()` | The elements are in descending order (`Comparer<T>.Default`). |
| `ContainSingle(predicate)` | Exactly one element satisfies `predicate`. |
| `BeInAscendingOrder()` | The elements are in ascending order (`Comparer<T>.Default`). |
| `BeSubsetOf(superset)` | Every element is contained in `superset`. |
| `IntersectWith(other)` | The collection and `other` share at least one element. |
| `AllSatisfy(inspector)` | Every element passes the `inspector` without throwing. |
| `SatisfyRespectively(inspectors)` | Element _i_ passes inspector _i_; the counts must match. |
| `AllBeOfType<T>()` / `AllBeOfType(type)` | Every element's runtime type is exactly `T`. |
| `AllBeEquivalentTo(expected)` | Every element is structurally equivalent to `expected`. |
| `HaveCountGreaterThan(n)` | The collection has more than `n` elements. |
| `HaveCountLessThan(n)` | The collection has fewer than `n` elements. |
| `HaveSameCount(other)` | The collection has the same number of elements as `other`. |
| `HaveElementAt(index, expected)` | The element at `index` equals `expected`. |
| `ContainNulls()` | At least one element is `null`. |
| `ContainMatch(wildcard)` | At least one `string` element matches the `wildcard` (`*`, `?`). |

`Equal` is **order-sensitive** — `[1, 2, 3].Should().Equal([3, 2, 1])` fails. `BeEquivalentTo` is its
order-**insensitive** counterpart — `[1, 2, 3].Should().BeEquivalentTo([3, 2, 1])` **passes**. This is the
single most surprising default in the library and it matches FluentAssertions; there is no
`WithStrictOrdering` opt-out — reach for `Equal` when order matters.

`BeEquivalentTo` and `ContainEquivalentOf` compare elements **structurally**, reusing the same object
equivalency engine as object `BeEquivalentTo` (public readable properties, recursively), so elements match by
value without needing an `Equals` override. Matching is **greedy**: each element is paired with the first
still-unmatched candidate it is equivalent to. Greedy pairing can theoretically mis-pair when several
elements are ambiguously equivalent to one another; this matches FluentAssertions and is accepted rather than
running a full bipartite matcher. Failure messages render at most **32** elements (the value formatter's cap),
so a large mismatch does not dump the whole collection.

Predicate-based assertions (`OnlyContain`, `ContainSingle(predicate)`) cannot describe the predicate in the
generated message, which reads "matching the predicate". Supply `because` to make the failure specific.

Negated forms are reached through `Not`: `Not.Contain(x)`, `Not.BeEmpty()` (assert the collection is not
empty), `Not.HaveCount(n)`, `Not.IntersectWith(other)` (share no element), and `Not.ContainNulls()`. The
remaining assertions have no negated form because it would not read naturally.

`AllSatisfy` and `SatisfyRespectively` take inspectors that assert on each element and signal failure by
throwing (typically a nested `Should()` call). `SatisfyRespectively` uses `params` for its inspectors, so it
has no trailing `because`. `ContainMatch` and `ContainNulls` are meaningful only on the element types they
name — `ContainMatch` inspects `string` elements, `ContainNulls` a collection whose elements can be `null`.

Deliberately **not** shipped from FluentAssertions' collection surface: `ContainInConsecutiveOrder`,
`HaveElementPreceding` / `HaveElementSucceeding`, and `ContainItemsAssignableTo`. They have no consumer and
marginal value; they are a possible future top-up, not part of the current catalog.

### DateTimes

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The value equals `expected`. |
| `BeAfter(other)` | The value is strictly after `other`. |
| `BeBefore(other)` | The value is strictly before `other`. |
| `BeOnOrAfter(other)` | The value is at or after `other`. |
| `BeOnOrBefore(other)` | The value is at or before `other`. |
| `BeCloseTo(other, tolerance)` | The value is within `tolerance` of `other`. |
| `BeUtc()` | The value's `Kind` is `Utc`. |
| `BeLocal()` | The value's `Kind` is `Local`. |
| `HaveYear(year)` | The year component equals `year`. |
| `HaveMonth(month)` | The month component equals `month`. |
| `HaveDay(day)` | The day component equals `day`. |
| `HaveHour(hour)` | The hour component equals `hour`. |
| `HaveMinute(minute)` | The minute component equals `minute`. |
| `HaveSecond(second)` | The second component equals `second`. |
| `HaveMillisecond(millisecond)` | The millisecond component equals `millisecond`. |
| `HaveKind(kind)` | The `DateTimeKind` equals `kind`. |
| `HaveOffset(offset)` | The value's offset equals `offset`. |
| `BeIn(kind)` | The value's `Kind` equals `kind`. Alias of `HaveKind` under the FluentAssertions name. |
| `BeSameDateAs(other)` | The value falls on the same calendar date as `other`, ignoring the time of day. |
| `BeLessThan(tolerance).Before(other)` / `.After(other)` | The gap between the value and `other` is less than `tolerance`. |
| `BeMoreThan(tolerance).Before(other)` / `.After(other)` | The gap between the value and `other` is more than `tolerance`. |
| `BeAtLeast(tolerance).Before(other)` / `.After(other)` | The gap between the value and `other` is at least `tolerance`. |
| `BeWithin(tolerance).Before(other)` / `.After(other)` | The gap between the value and `other` is at most `tolerance`. |
| `BeExactly(tolerance).Before(other)` / `.After(other)` | The gap between the value and `other` is exactly `tolerance`. |
| `BeNull()` | *(nullable only)* The nullable has no value. |
| `HaveValue()` | *(nullable only)* The nullable has a value. |

`BeIn` overlaps `HaveKind` — both assert on `Subject.Kind`. `BeIn` exists so the FluentAssertions name is
available; `HaveKind` remains the native spelling. Either reads the same failure through the same check.

#### Difference chains

The five difference builders are a two-step assertion. The builder captures the tolerance; `.Before(other)`
or `.After(other)` names the direction and runs the check. `.Before` requires the value to be earlier than
`other` and measures `other - value`; `.After` requires it to be later and measures `value - other`. Calling
the wrong direction is a failure, not a silent pass. Each direction rejoins the normal fluent surface by
returning the `DateTimeComparer`.

```csharp
var start = new DateTime(2026, 1, 1, 10, 0, 0);
var end = new DateTime(2026, 1, 1, 11, 30, 0);

start.Should().BeLessThan(2.Hours()).Before(end);
end.Should().BeMoreThan(1.Hours()).After(start);
```

#### Numeric time extensions

`Hours()`, `Minutes()`, `Seconds()`, `Days()`, and `Milliseconds()` are extension methods on `int` that return
a `TimeSpan`, so a tolerance reads as `2.Hours()` instead of `TimeSpan.FromHours(2)`. They exist primarily to
make the difference chains read well, but they are general-purpose and usable anywhere a `TimeSpan` is wanted.

```csharp
2.Hours(); // TimeSpan.FromHours(2)
30.Minutes(); // TimeSpan.FromMinutes(30)
250.Milliseconds(); // TimeSpan.FromMilliseconds(250)
```

### Doubles And Floats

Both `double` and `float` share the same assertion set.

| Assertion | What it asserts |
|---|---|
| `BeApproximately(expected, tolerance)` | The value is within `tolerance` of `expected`. |
| `BeGreaterThan(expected)` | The value is strictly greater than `expected`. |
| `BeLessThan(expected)` | The value is strictly less than `expected`. |
| `BeInRange(lower, upper)` | The value is within `[lower, upper]`. |
| `BeNaN()` | The value is `NaN`. |
| `BeNegative()` | The value is less than zero. |
| `BePositive()` | The value is greater than zero. |
| `BeZero()` | The value is zero. |
| `Match(predicate)` | The value satisfies `predicate`. |

### Enums

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The value equals `expected`. |
| `BeDefined()` | The value is a defined member of the enum. |
| `HaveFlag(flag)` | The value has `flag` set. |
| `BeNull()` | *(nullable only)* The nullable has no value. |
| `HaveValue()` | *(nullable only)* The nullable has a value. |

### Exceptions

Assertions on a delegate — an `Action` for synchronous code, a `Func<Task>` for asynchronous code. This is
the only family whose failure messages do not lead with the subject: an `Action`'s `ToString()` is just its
type name, so there is nothing useful to render, and the messages start with `should` instead.

| Assertion | What it asserts |
|---|---|
| `Throw<TException>()` | Invoking the `Action` throws a `TException` (or a derived type). Returns a `ThrownExceptionComparer`. |
| `ThrowExactly<TException>()` | Invoking the `Action` throws a `TException` *exactly* — a derived type fails. Returns a `ThrownExceptionComparer`. |
| `NotThrow()` | Invoking the `Action` throws nothing. |
| `ThrowAsync<TException>()` | Awaiting the `Func<Task>` throws a `TException` (or a derived type). Returns a `ThrownExceptionComparer`. |
| `ThrowExactlyAsync<TException>()` | Awaiting the `Func<Task>` throws a `TException` *exactly* — a derived type fails. Returns a `ThrownExceptionComparer`. |
| `NotThrowAsync()` | Awaiting the `Func<Task>` throws nothing. |
| `WithMessage(expected)` | *(on `ThrownExceptionComparer`)* The caught exception's `Message` equals `expected` exactly. |
| `WithInnerException<TInner>()` | *(on `ThrownExceptionComparer`)* The caught exception has an inner exception of `TInner` (or a derived type). Returns a `ThrownExceptionComparer` scoped to the inner exception, so it chains further. |
| `WithInnerExceptionExactly<TInner>()` | *(on `ThrownExceptionComparer`)* The inner exception is `TInner` *exactly*. Returns a `ThrownExceptionComparer` scoped to the inner exception. |
| `Where(predicate)` | *(on `ThrownExceptionComparer`)* The caught exception satisfies `predicate` (`Func<Exception, bool>`). |
| `WithParameterName(expected)` | *(on `ThrownExceptionComparer`)* The caught `ArgumentException`'s `ParamName` equals `expected`. |

Three things worth knowing before you use these:

1. **The delegate must be a typed variable.** A bare lambda has no type and cannot receive an extension
   method, so you cannot write `(() => Foo()).Should()`. Declare the delegate first:

   ```csharp
   Action action = () => service.Charge(card);
   action.Should().Throw<InvalidOperationException>();
   ```

2. **`Throw<T>` returns a `ThrownExceptionComparer`, not the action comparer**, so `WithMessage` can chain
   onto the caught exception. This is the one place in the library where an assertion changes comparer type —
   the subject genuinely changes from "the delegate" to "the exception it threw":

   ```csharp
   action.Should().Throw<InvalidOperationException>().WithMessage("card declined");
   ```

3. **`Throw<T>` matches derived exception types; `ThrowExactly<T>` does not.**
   `Throw<ArgumentException>()` is satisfied by an `ArgumentNullException`; `ThrowExactly<ArgumentException>()`
   fails on it, requiring the thrown type to be `ArgumentException` exactly. The same distinction holds for
   `WithInnerException<T>` (derived) versus `WithInnerExceptionExactly<T>` (exact).

`WithMessage` compares the message with exact string equality — FluentAssertions supports `*` wildcards
there and FatCat.Testing does not. Use `Where(e => e.Message.Contains("..."))` for a substring check. See
[`MIGRATION.md`](MIGRATION.md) §5.

Asserting that code throws does **not** make the assertion API asynchronous: `ThrowAsync<T>` and
`ThrowExactlyAsync<T>` observe the `Task` synchronously inside the comparer, so the fluent `Should()` surface
stays synchronous and is never awaited.

**Deliberate omissions.** FluentAssertions' `Awaiting` / `Invoking` / `Enumerating` wrappers are not
provided — the `Action` and `Func<Task>` overloads above already adapt a subject into a delegate. `WithResult`
(asserting an async result value) is omitted as it has no consumer here. `CompleteWithinAsync(TimeSpan)` is
also omitted: enforcing a timeout requires either a banned primitive (`Task.Delay` / `Thread.Sleep`) or a
second blocking site, both disallowed, so this family is intentionally not a complete port.

### Guids

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The value equals `expected`. |
| `BeEmpty()` | The value is `Guid.Empty`. |
| `BeNull()` | *(nullable only)* The nullable has no value. |
| `HaveValue()` | *(nullable only)* The nullable has a value. |

### Numbers

Applies to every integral numeric type (`int`, `long`, `short`, `byte`, `decimal`, and their signed and
unsigned relatives).

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The value equals `expected`. |
| `BeGreaterThan(expected)` | The value is strictly greater than `expected`. |
| `BeLessThan(expected)` | The value is strictly less than `expected`. |
| `BeInRange(lower, upper)` | The value is within `[lower, upper]`. |
| `BeAround(center, tolerance)` | The value is within `tolerance` of `center`. |
| `BeNegative()` | The value is less than zero. |
| `BePositive()` | The value is greater than zero. |
| `BeZero()` | The value is zero. |
| `Match(predicate)` | The value satisfies `predicate`. |
| `BeNull()` | *(nullable only)* The nullable has no value. |
| `HaveValue()` | *(nullable only)* The nullable has a value. |

### Objects

Any reference type has a `.Should()` entry point through `Should<T>(this T) where T : class`. Concrete
collection shapes (`IEnumerable<T>`, `List<T>`, `T[]`) keep binding to their own comparer, so a `List<string>`
never lands here by accident.

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The subject equals `expected` by **value** — `Equals`, so a type that overrides `Equals` compares by value. |
| `BeEquivalentTo(expected)` | The subject is **structurally equivalent** to `expected` — a recursive, member-by-member comparison. |
| `BeNull()` | The subject is `null`. |
| `BeSameAs(expected)` | The subject is the **same instance** as `expected` — reference identity (`ReferenceEquals`). |
| `Satisfy(inspector)` | The subject passes the `inspector` action without throwing. |
| `BeOfType(type)` | *(inherited)* The subject is exactly `type`. |
| `BeAssignableTo(type)` | *(inherited)* The subject is assignable to `type`. |
| `BeOneOf(values)` | *(inherited)* The subject equals one of `values`. |

`Be` and `BeSameAs` are different questions. `Be` is **value** equality: two distinct instances that are
`Equals` pass `Be` and **fail** `BeSameAs`. `BeSameAs` is **reference identity**: only the very same object
passes. `Be` is *not* structural comparison — comparing the members of a type that does not override
`Equals` is `BeEquivalentTo`. When two objects differ but render the same text (because
they override `ToString`), the `Be` failure message cannot explain *why* they differ; `BeEquivalentTo` is the
assertion that does.

`BeEquivalentTo` compares two objects **structurally**, member by member, without either type having to
override `Equals`. It:

- compares **public readable instance properties only** — fields are **not** compared, so two objects that
  differ only in a public field are still equivalent;
- uses the type's own `Equals` for any type that **overrides** it (so `string`, primitives, `Guid`,
  `DateTime`, and enums compare by value and terminate the recursion) — this is the base case that keeps the
  engine from trying to dump the members of an `int`;
- recurses into nested objects and into collection-typed members (collection members match
  **order-insensitively**), and is **cycle-safe** — a reference already on the current path is treated as
  equivalent rather than recursing forever;
- **names the differing member** in the failure message and the full path to it, for example
  `... but Address.City differs: expected "York" but found "Leeds"`.

This is a separate question from `Be`. `Be` uses the type's own `Equals`; `BeEquivalentTo` walks members
regardless of whether `Equals` is overridden. Recursion is depth-limited (10) and collection members are
capped (32 elements) as safety valves for pathological graphs.

```csharp
var actual = new Person { Name = "Alice", Address = new Address { City = "Leeds" } };
var expected = new Person { Name = "Alice", Address = new Address { City = "Leeds" } };
actual.Should().BeEquivalentTo(expected);       // passes — same members, no Equals override needed
actual.Should().Not.BeEquivalentTo(other);      // negation is the Not property, as everywhere
```

#### Per-type equivalency rules — `Equivalency.Using<T>`

`BeEquivalentTo` ships one configuration hook: a global per-type rule that replaces the default comparison
for a single type wherever that type appears in a graph. The canonical use is a `DateTime` closeness rule —
two timestamps within a tolerance count as equivalent even though they are not exactly equal. Register the
rule with `Equivalency.Using<T>` and it fires for every `T` node, short-circuiting the recursion for that
type (a `DateTime` with a closeness rule never recurses into `DateTime`'s own properties):

```csharp
Equivalency.Using<DateTime>((subject, expected) => (subject - expected).Duration() <= TimeSpan.FromSeconds(1));

actual.Should().BeEquivalentTo(expected); // Timestamp members within one second now match
```

Registration is a **single `<T>`** — there is no `WhenTypeIs<T>()` chain to repeat the type. Registering
twice for the same type is **last-registration-wins**.

> **This registry is process-wide, mutable state.** A rule registered by one test affects **every later test
> in the run**, so registration belongs in a **fixture** (a collection fixture, a static constructor, or a
> test class's constructor), **never inside a `[Fact]`**. Tear it down with `Equivalency.Reset()` in the
> fixture's `Dispose`, so no rule leaks into unrelated tests and test-run order stays irrelevant:

```csharp
public class EquivalencyFixture : IDisposable
{
	public EquivalencyFixture()
	{
		Equivalency.Using<DateTime>((subject, expected) => (subject - expected).Duration() <= TimeSpan.FromSeconds(1));
	}

	public void Dispose() { Equivalency.Reset(); }
}
```

This is a deliberate divergence from FluentAssertions, where `options.Using<T>(...).WhenTypeIs<T>()` is a
**per-call** option. FatCat.Testing registers the rule **globally** instead — matched to the only way real
call sites use it, and without growing every `BeEquivalentTo` signature with an options lambda.

A bare `null` literal cannot receive `Should()` — the compiler has no type to bind `T` to. Type the variable
first:

```csharp
Dto result = null;
result.Should().BeNull();
result.Should().Not.BeNull(); // fails with "null should not be null"
```

### Strings

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The string equals `expected` (case-sensitive). |
| `BeEquivalentTo(expected)` | The string equals `expected`, ignoring case. |
| `Contain(expected)` | The string contains `expected`. |
| `ContainAll(values)` | The string contains every value in `values`. |
| `ContainAny(values)` | The string contains at least one value in `values`. |
| `StartWith(expected)` | The string starts with `expected`. |
| `EndWith(expected)` | The string ends with `expected`. |
| `StartWithEquivalentOf(expected)` | The string starts with `expected`, ignoring case. |
| `EndWithEquivalentOf(expected)` | The string ends with `expected`, ignoring case. |
| `Match(pattern)` | The string matches the wildcard `pattern`. |
| `MatchRegex(pattern)` | The string matches the regular expression `pattern`. |
| `HaveLength(length)` | The string has exactly `length` characters. |
| `BeEmpty()` | The string is empty. |
| `BeNull()` | The string is `null`. |
| `BeNullOrEmpty()` | The string is `null` or empty. |
| `BeNullOrWhiteSpace()` | The string is `null`, empty, or white space. |
| `BeLowerCased()` | The string is entirely lower case. |
| `BeUpperCased()` | The string is entirely upper case. |
| `HaveValue()` | The string is not `null`. |

### TimeSpans

| Assertion | What it asserts |
|---|---|
| `Be(expected)` | The value equals `expected`. |
| `BeCloseTo(other, tolerance)` | The value is within `tolerance` of `other`. |
| `BeGreaterThan(expected)` | The value is strictly greater than `expected`. |
| `BeGreaterThanOrEqualTo(expected)` | The value is greater than or equal to `expected`. |
| `BeLessThan(expected)` | The value is strictly less than `expected`. |
| `BeLessThanOrEqualTo(expected)` | The value is less than or equal to `expected`. |
| `BeNegative()` | The value is negative. |
| `BePositive()` | The value is positive. |
| `HaveDays(days)` | The days component equals `days`. |
| `HaveHours(hours)` | The hours component equals `hours`. |
| `HaveMinutes(minutes)` | The minutes component equals `minutes`. |
| `HaveSeconds(seconds)` | The seconds component equals `seconds`. |
| `HaveMilliseconds(milliseconds)` | The milliseconds component equals `milliseconds`. |
| `BeNull()` | *(nullable only)* The nullable has no value. |
| `HaveValue()` | *(nullable only)* The nullable has a value. |

### Shared (all comparers)

Every comparer inherits these from the shared base, so they are available on every subject type above.

| Assertion | What it asserts |
|---|---|
| `BeOfType(type)` | The subject is exactly `type`. |
| `BeAssignableTo(type)` | The subject is assignable to `type`. |
| `BeOneOf(values)` | The subject equals one of `values`. |
| `Satisfy(inspector)` | The subject passes the `inspector` action without throwing. |

## Negation

Negation is a property, not a family of methods. Every positive assertion has a negated form reached through
`Not`:

```csharp
value.Should().Not.Be(expected);
name.Should().Not.BeNullOrEmpty();
```

**There are no `NotXxx` methods, and there never will be.** FatCat.Testing does not expose `NotBe`,
`NotBeNull`, or any other prefixed negation — not even as obsolete shims. The single negation form is the
`Not` property. This is a deliberate, permanent API decision.

Because FluentAssertions uses the prefixed shape (`NotBe`, `NotBeNull`, …), moving from it is a mechanical
rewrite of `.Should().NotXxx(` to `.Should().Not.Xxx(`. That rewrite, and a codemod to perform it, are
described in [`MIGRATION.md`](MIGRATION.md).

## Custom Comparers

FatCat.Testing is extensible: you write your own comparer for your own subject type, reusing the same base
class, failure exception, and value formatter the built-in comparers use. There is no separate authoring API
to learn — a custom comparer is an ordinary class deriving from `ComparerBase<TSubject, TComparer>`, and it
gets `BeOfType`, `BeAssignableTo`, `BeOneOf`, and `Satisfy` for free.

The recipe:

1. **Derive from `ComparerBase<TSubject, TComparer>`**, forwarding the subject through the primary
   constructor. `TComparer` is your own comparer type, so chained assertions return it and keep chaining.
2. **Expose a `Not` property** returning a twin that derives from `NotComparerBase<TSubject, TComparer>`
   (recommended — it gives you `.Not.BeOk()`; there are no `NotXxx` methods).
3. **Return `this`** from every assertion method so calls chain.
4. **Throw failures with `CompareException.New(because ?? $"...")`** — `because` *replaces* the generated
   message, and messages are built with string interpolation, never `+` concatenation.
5. **Add a `Should(this TSubject)` extension** in your own static class to reach the comparer.
6. **Read `Subject`** (public on the base) and format object or collection values with
   `FatCat.Testing.Formatting.ValueFormatter.Format` so your messages render subjects the way the built-in
   comparers do.

`Subject` is **public** on both base classes, so a composing helper can read it from *outside* the comparer —
which is exactly what a real assertion does when it delegates to an inner `.Should()`.

### The direct form

The comparer inspects `Subject` and throws:

```csharp
using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

public class WebResponse
{
	public int StatusCode { get; set; }
}

public class WebResponseComparer(WebResponse subject) : ComparerBase<WebResponse, WebResponseComparer>(subject)
{
	public WebResponseComparer BeOk(string because = null)
	{
		if (Subject.StatusCode != 200) { CompareException.New(because ?? $"status code {Subject.StatusCode} should be 200 (OK)"); }

		return this;
	}
}

public static class WebResponseShouldExtensions
{
	public static WebResponseComparer Should(this WebResponse subject) { return new WebResponseComparer(subject); }
}
```

```csharp
response.Should().BeOk();                 // passes when StatusCode is 200
response.Should().BeOk().BeOk();          // chains — every assertion returns the comparer
response.Should().BeOk("service is up");  // because replaces the generated message on failure
```

### The delegating form

Because `Subject` is public, an assertion can delegate to an inner `.Should()` on a property of the subject
rather than re-implementing the comparison — this is how the real consumer assertions are written:

```csharp
public WebResponseComparer BeOk(string because = null)
{
	Subject.StatusCode.Should().Be(200, because);

	return this;
}
```

Here the inner `Subject.StatusCode.Should().Be(200)` produces its own `CompareException` if the status code
is not `200`, and `BeOk` simply returns `this` to keep the chain going. Both forms are proven in
`Tests.FatCat.Testing.Extensibility.CustomComparerTests`.

### Negation

Give your comparer a `Not` twin deriving from `NotComparerBase<TSubject, TComparer>` so negation reads
`.Not.BeOk()`, matching the rest of the library:

```csharp
public class NotWebResponseComparer(WebResponse subject)
	: NotComparerBase<WebResponse, NotWebResponseComparer>(subject)
{
	public NotWebResponseComparer BeOk(string because = null)
	{
		if (Subject.StatusCode == 200) { CompareException.New(because ?? $"status code {Subject.StatusCode} should not be 200 (OK)"); }

		return this;
	}
}
```

Then expose it from the positive comparer with `public NotWebResponseComparer Not { get; } = new(subject);`.

### One caveat — failure stack traces

FatCat.Testing has no `[CustomAssertion]` equivalent. When a custom assertion fails, the top of the stack
trace points at `CompareException.New` inside your comparer method, not at the test line that called it — the
test line is the bottom-most frame. The failure message is unaffected; only the stack-trace attribution
differs from a hand-written `Assert` in the test body.

## Value Formatting

When an assertion fails, FatCat.Testing renders the subject into the failure message with
`FatCat.Testing.Formatting.ValueFormatter.Format`. `ToString()` alone is not enough: a `List<string>`
renders as ``System.Collections.Generic.List`1[System.String]`` and a plain DTO renders as its type name.
`ValueFormatter` turns both into something a reader can act on.

`ValueFormatter.Format` is **public and supported**. Custom comparers (see `## Custom Comparers`) call it so
their failure messages render subjects exactly the way the built-in comparers do.

### Rendering rules

Position matters — a string at the top level renders differently from a string nested inside a collection or
an object dump (see the top-level-bare / nested-quoted rule below).

| Value | Top level | Nested (inside a collection or object dump) |
|---|---|---|
| `null` | `null` | `null` |
| `string` | the string, bare | `"the string"` |
| `char` | the char, bare | `'c'` |
| `IEnumerable` (not `string`) | `[a, b, c]` | `[a, b, c]` |
| anything overriding `ToString()` | `ToString()` | `ToString()` |
| any other object | `TypeName { Member = value, ... }` | same |

Because `Guid`, `DateTime`, `TimeSpan`, and every enum override `ToString()`, they render exactly as they
always have.

### Top-level bare, nested quoted

A string passed straight to `Format` renders without quotes; a string found inside a collection or an object
dump renders quoted, so `[a, b]` is never ambiguous against a collection of some other type:

```csharp
ValueFormatter.Format("hello");                       // hello
ValueFormatter.Format(new[] { "a", "b" });            // ["a", "b"]
ValueFormatter.Format(new Person { Name = "Bob" });   // Person { Name = "Bob" }
```

### Collections

An empty collection renders `[]`. At most **32** elements are shown; the rest are summarised as the count of
the remainder:

```csharp
ValueFormatter.Format(Enumerable.Range(1, 40));       // [1, 2, ... 32, …and 8 more]
```

### Object dumps

A plain object that does not override `ToString()` renders its public, readable, instance properties in
declaration order (no fields, no static members, no indexers). A type with no such properties renders
`TypeName { }`.

- **Depth cap.** Nesting deeper than **5** levels renders `{ … }`.
- **Cycle detection.** A reference already on the current path renders `{ cyclic reference to TypeName }`, so
  a graph that references itself formats and returns instead of overflowing the stack. Detection is by
  reference identity, never `Equals` — the object being formatted is frequently one whose `Equals` is the
  thing under test.
- **A property getter that throws** renders `Member = <threw TypeName>` rather than failing the assertion for
  the wrong reason.

## Coming From FluentAssertions

FatCat.Testing is a replacement for FluentAssertions, but it is not source-compatible. The two most common
differences you will hit are:

- **Negation** is `.Should().Not.Xxx(...)`, not `.Should().NotXxx(...)`.
- **`because`** replaces the generated failure message rather than being appended to it, and there is no
  `becauseArgs` — use string interpolation.

The full mapping table, the behavioural differences, the known-unsupported list, and the codemod that
automates the mechanical parts all live in [`MIGRATION.md`](MIGRATION.md).

## Known Limitations

- **xUnit only.** `CompareException` derives from `XunitException`; there is no framework-detection shim. See
  `## Requirements`.
- **`because` replaces the message.** A supplied `because` becomes the whole failure message. It is not
  appended to the generated one, and there is no `params object[] becauseArgs`.
- **No `AssertionScope`.** Assertions throw on the first failure; there are no soft assertions that collect
  multiple failures.
- **No `.And` / `.Which`.** Assertions chain by returning the comparer, but there is no `.And` property and no
  `.Which` drill-down onto a matched item.
- **`BeEquivalentTo` ships default options plus one hook.** The only configuration point is the global
  `Equivalency.Using<T>` per-type rule (register it in a fixture, reset it after). There is no `Excluding`,
  `Including`, `WithStrictOrdering`, or the wider option-method surface, and no per-call options lambda.
