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
