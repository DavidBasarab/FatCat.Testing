# FatCat.Testing

A small, self-contained C# assertion library — a free, Apache-licensed alternative to FluentAssertions for
value-type and scalar assertions.

```csharp
using FatCat.Testing;

result.Should().BeTrue();
name.Should().Be("Sir Fluffington");
count.Should().BeGreaterThan(0).BeLessThan(10);
whiskerLength.Should().BeApproximately(3.2, 0.05);
DateTime.Now.Should().BeCloseTo(expected, TimeSpan.FromSeconds(1));
```

Every assertion returns its comparer, so calls chain. Every assertion takes a trailing
`string because = null` that replaces the generated message when supplied. Negation is a `Not` property —
`value.Should().Not.Be(x)` — not a `NotBe` method.

## Status & Scope

This library covers **value types, scalars, strings, and enums**. That surface is complete and, in places,
richer than FluentAssertions (`BeAround`, `ContainAll`/`ContainAny`, `BeControl`, a full `TimeSpan`
component set).

It does **not yet** cover object-graph equality, collections, exceptions, dictionaries, or async — those
are planned. See [tasks/gaps.md](tasks/gaps.md) for the full gap analysis against FluentAssertions.

Failure assertions throw `CompareException`, which derives from xUnit's `XunitException`, so a failed
assertion registers as a normal xUnit test failure. The library currently targets xUnit; its one package
reference is `xunit.assert`.

## Installation

Target framework is `net10.0`.

```
dotnet add package FatCat.Testing
```

Then bring the extension methods into scope:

```csharp
using FatCat.Testing;
```

## The `Should()` Entry Points

`Should()` is an extension method with an overload per supported type. Each returns a comparer for that
type. There is a nullable-value-type overload wherever a non-nullable one exists.

| Subject type | Comparer |
|---|---|
| `bool`, `bool?` | `BoolComparer`, `NullableBoolComparer` |
| `char`, `char?` | `CharComparer`, `NullableCharComparer` |
| `string` | `NullableStringComparer` |
| all integral & floating types (`int`, `long`, `short`, `byte`, `decimal`, `nint`, … and their unsigned forms), `int?` | `NumericComparer<T>`, `NullableIntComparer` |
| `double` | `DoubleComparer` |
| `float` | `FloatComparer` |
| `DateTime`, `DateTime?` | `DateTimeComparer`, `NullableDateTimeComparer` |
| `TimeSpan`, `TimeSpan?` | `TimeSpanComparer`, `NullableTimeSpanComparer` |
| `Guid`, `Guid?` | `GuidComparer`, `NullableGuidComparer` |
| any `enum`, nullable enum | `EnumComparer<T>`, `NullableEnumComparer<T>` |

## Assertions By Type

### Common to every comparer
Available on any subject via the shared base:

- `BeOfType<T>()` / `BeOfType(Type)`
- `BeAssignableTo<T>()` / `BeAssignableTo(Type)`
- `BeOneOf(params T[])` / `BeOneOf(IEnumerable<T>)`
- `Satisfy(Action<T> inspector)` — run an arbitrary inspection against the subject

### Booleans
`Be`, `BeTrue`, `BeFalse`. Nullable adds `BeNull`, `HaveValue`.

### Characters
`Be`, `BeDigit`, `BeLetter`, `BeLetterOrDigit`, `BeLowerCased`, `BeUpperCased`, `BeWhiteSpace`, `BeControl`.
Nullable adds `BeNull`, `HaveValue`.

### Numbers (`NumericComparer<T> where T : INumber<T>`)
`Be`, `BeGreaterThan`, `BeLessThan`, `BeInRange`, `BeAround(center, tolerance)`, `BeNegative`, `BePositive`,
`BeZero`, `Match(Func<T, bool>)`. Works for any `INumber<T>` — every built-in numeric type. `int?` has its
own `NullableIntComparer` with the same set plus `BeAround`, `BeNull`, `HaveValue`.

### Doubles & Floats
`BeApproximately(expected, tolerance)`, `BeGreaterThan`, `BeLessThan`, `BeInRange`, `BeNaN`, `BeNegative`,
`BePositive`, `BeZero`, `Match(Func<T, bool>)`.

### DateTimes
`Be`, `BeAfter`, `BeBefore`, `BeOnOrAfter`, `BeOnOrBefore`, `BeCloseTo(expected, precision)`, `BeUtc`,
`BeLocal`, `HaveYear`, `HaveMonth`, `HaveDay`, `HaveHour`, `HaveMinute`, `HaveSecond`, `HaveMillisecond`,
`HaveKind`, `HaveOffset`. Nullable adds `BeNull`, `HaveValue`.

### TimeSpans
`Be`, `BeCloseTo(expected, precision)`, `BeGreaterThan`, `BeGreaterThanOrEqualTo`, `BeLessThan`,
`BeLessThanOrEqualTo`, `BeNegative`, `BePositive`, `HaveDays`, `HaveHours`, `HaveMinutes`, `HaveSeconds`,
`HaveMilliseconds`. Nullable adds `BeNull`, `HaveValue`.

### Guids
`Be`, `BeEmpty`. Nullable adds `BeNull`, `HaveValue`.

### Enums
`Be`, `BeDefined`, `HaveFlag`. Nullable adds `BeNull`, `HaveValue`.

### Strings
Equality: `Be`, `BeEquivalentTo`. Content: `Contain`, `ContainAll`, `ContainAny`. Affixes: `StartWith`,
`EndWith`, `StartWithEquivalentOf`, `EndWithEquivalentOf`. Patterns: `Match` (wildcard), `MatchRegex`
(string or `Regex`). Shape: `HaveLength`, `BeEmpty`, `BeNull`, `BeNullOrEmpty`, `BeNullOrWhiteSpace`,
`BeLowerCased`, `BeUpperCased`, `HaveValue`.

Most string assertions take an `Options` argument — `Options.CaseSensitive` (default) or
`Options.IgnoreCase`:

```csharp
"Hello World".Should().Contain("hello", Options.IgnoreCase);
```

`Contain` also has an overload that asserts on the number of occurrences, using the fluent occurrence
builders `AtLeast`, `AtMost`, `Exactly`, `MoreThan`, `LessThan` — each with `.Once()`, `.Twice()`,
`.Thrice()`, and `.Times(n)`:

```csharp
"na na na na".Should().Contain("na", AtLeast.Twice());
"one two three".Should().Contain("four", Exactly.Times(0));
```

## Negation

Negation is the `Not` property, which returns the negated comparer for the same subject:

```csharp
value.Should().Not.Be(5);
name.Should().Not.BeNullOrEmpty();
when.Should().Not.BeAfter(deadline);
```

This is a deliberate design choice — `.Should().Not.Be(x)` reads better than FluentAssertions'
`.Should().NotBe(x)`. There are no `NotXxx` methods.

## The `because` Override

Every assertion's final parameter is `string because = null`. When supplied, it replaces the generated
failure message verbatim:

```csharp
age.Should().BeGreaterThan(18, "the customer must be an adult to check out");
```

## Migrating From FluentAssertions

FatCat.Testing is intentionally not a source-compatible clone. The main differences a migration must
account for:

- Negation: `NotBe(x)` → `Not.Be(x)` (uniformly `NotXxx(` → `Not.Xxx(`).
- Object, collection, exception, and async assertions are not implemented yet.

The full mapping and migration plan lives in [tasks/gaps.md](tasks/gaps.md).

## Building

The solution is `Fatcat.Testing.sln`.

```
dotnet build Fatcat.Testing.sln
dotnet test  Fatcat.Testing.sln
```

CSharpier owns all formatting and runs on build. Run `dotnet csharpier .` as the final step of any change.

## License

Apache 2.0 — see [LICENSE](LICENSE).
