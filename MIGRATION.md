# Migrating From FluentAssertions

This guide moves a codebase from FluentAssertions (7.0.0, the last Apache-2.0 release) to FatCat.Testing.

FatCat.Testing is deliberately **not** a source-compatible clone of FluentAssertions. Most calls translate
by a single mechanical rule (§2); a handful compile unchanged but behave differently (§5); a few constructs
have no equivalent and must be rewritten (§6).

This is a living document. Each assertion family added to the library appends its rows to the mapping table
(§3). A row is only marked supported once a test class in this repository proves the rewritten form
compiles and passes.

## 1. Why

FluentAssertions `7.0.0` is the last release under the Apache-2.0 licence. Every `8.x` release is
commercially licensed. FatCat.Testing is Apache-2.0 and provides a free replacement for the assertions a
typical test suite actually uses.

The trade-off for staying free is that FatCat.Testing does not reproduce the entire FluentAssertions
surface, and it does not match its API shape everywhere. This guide is how you cross that gap.

## 2. The One Rule

FluentAssertions spells negation as a prefixed method: `NotBe`, `NotBeNull`, `NotContain`. FatCat.Testing
spells it as a `Not` property followed by the positive method:

```
.Should().NotXxx(   ->   .Should().Not.Xxx(
```

This is uniform. FluentAssertions negations are always the literal `Not` followed by a PascalCase method
name, so the rewrite is mechanical and regex-clean:

```csharp
// FluentAssertions
value.Should().NotBeNull();
name.Should().NotBeNullOrEmpty();
list.Should().NotContain(x);

// FatCat.Testing
value.Should().Not.BeNull();
name.Should().Not.BeNullOrEmpty();
list.Should().Not.Contain(x);
```

There are no `NotXxx` methods in FatCat.Testing and there never will be — the `Not` property is the single
negation form. See §7 for the codemod that performs this rewrite.

## 3. Mapping Table

Columns: the FluentAssertions call, its FatCat.Testing form, whether it is supported today, and the test
class that proves it.

- `✅ supported` — the rewritten form compiles and is covered by the named test class in this repository.
- `⬜ pending (phase NN)` — not shipped yet; the phase number is the one that delivers it.

Where a method is supported for some subject types but pending for others (for example `Be` on value types
versus objects), the row reflects what ships today and §4 records the type coverage.

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `.Should().Be(x)` *(value types & string)* | `.Should().Be(x)` | ✅ supported | `BoolBeTests`, `IntBeTests` |
| `.Should().Be(x)` *(objects)* | `.Should().Be(x)` | ⬜ pending (phase 06) | — |
| `.Should().NotBe(x)` *(value types)* | `.Should().Not.Be(x)` | ✅ supported | `BoolBeTests` |
| `.Should().BeTrue()` / `.Should().BeFalse()` | `.Should().BeTrue()` / `.Should().BeFalse()` | ✅ supported | `BoolBeTrueTests`, `BoolBeFalseTests` |
| `.Should().BeNull()` *(value types & string)* | `.Should().BeNull()` | ✅ supported | `StringBeNullTests`, `NullableGuidBeNullTests` |
| `.Should().BeNull()` *(objects)* | `.Should().BeNull()` | ⬜ pending (phase 06) | — |
| `.Should().NotBeNull()` *(objects)* | `.Should().Not.BeNull()` | ⬜ pending (phase 06) | — |
| `.Should().BeEmpty()` *(string & Guid)* | `.Should().BeEmpty()` | ✅ supported | `StringBeEmptyTests`, `GuidBeEmptyTests` |
| `.Should().BeEmpty()` *(collections)* | `.Should().BeEmpty()` | ⬜ pending (phase 04) | — |
| `.Should().NotBeEmpty()` *(collections)* | `.Should().Not.BeEmpty()` | ⬜ pending (phase 04) | — |
| `.Should().BeNullOrEmpty()` | `.Should().BeNullOrEmpty()` | ✅ supported | `StringBeNullOrEmptyTests` |
| `.Should().NotBeNullOrEmpty()` | `.Should().Not.BeNullOrEmpty()` | ✅ supported | `StringBeNullOrEmptyTests` |
| `.Should().NotBeNullOrWhiteSpace()` | `.Should().Not.BeNullOrWhiteSpace()` | ✅ supported | `StringBeNullOrWhiteSpaceTests` |
| `.Should().Contain(x)` *(string)* | `.Should().Contain(x)` | ✅ supported | `StringContainTests` |
| `.Should().Contain(x)` *(collections)* | `.Should().Contain(x)` | ⬜ pending (phase 04) | — |
| `.Should().NotContain(x)` *(collections)* | `.Should().Not.Contain(x)` | ⬜ pending (phase 04) | — |
| `.Should().EndWith(x)` | `.Should().EndWith(x)` | ✅ supported | `StringEndWithTests` |
| `.Should().BeEquivalentTo(x)` *(string)* | `.Should().BeEquivalentTo(x)` | ✅ supported | `StringBeEquivalentToTests` |
| `.Should().BeEquivalentTo(x)` *(object graphs)* | `.Should().BeEquivalentTo(x)` | ⬜ pending (phase 07) | — |
| `.Should().NotBeEquivalentTo(x)` | `.Should().Not.BeEquivalentTo(x)` | ⬜ pending (phase 07) | — |
| `.Should().BeCloseTo(x, tolerance)` | `.Should().BeCloseTo(x, tolerance)` | ✅ supported | `DateTimeBeCloseToTests`, `TimeSpanBeCloseToTests` |
| `.Should().BeApproximately(x, tolerance)` | `.Should().BeApproximately(x, tolerance)` | ✅ supported | `DoubleBeApproximatelyTests` |
| `.Should().BeGreaterThan(x)` / `.Should().BeLessThan(x)` | same | ✅ supported | `IntBeGreaterThanTests`, `IntBeLessThanTests` |
| `.Should().BeGreaterOrEqualTo(x)` | `.Should().BeGreaterThanOrEqualTo(x)` | ⬜ pending (phase 16) | — |
| `.Should().BeLessOrEqualTo(x)` | `.Should().BeLessThanOrEqualTo(x)` | ⬜ pending (phase 16) | — |
| `.Should().BeOfType<T>()` | `.Should().BeOfType(typeof(T))` | ✅ supported | `ComparerBaseTests` |
| `.Should().BeOneOf(...)` | `.Should().BeOneOf(...)` | ✅ supported | `ComparerBaseTests` |
| `.Should().BeSameAs(x)` / `.Should().NotBeSameAs(x)` | `.Should().BeSameAs(x)` / `.Should().Not.BeSameAs(x)` | ⬜ pending (phase 06) | — |
| `.Should().HaveCount(n)` | `.Should().HaveCount(n)` | ⬜ pending (phase 04) | — |
| `.Should().ContainSingle()` | `.Should().ContainSingle()` | ⬜ pending (phase 04) | — |
| `.Should().ContainEquivalentOf(x)` | `.Should().ContainEquivalentOf(x)` | ⬜ pending (phase 08) | — |
| `.Should().NotContainEquivalentOf(x)` | `.Should().Not.ContainEquivalentOf(x)` | ⬜ pending (phase 08) | — |
| `.Should().OnlyContain(pred)` | `.Should().OnlyContain(pred)` | ⬜ pending (phase 05) | — |
| `.Should().OnlyHaveUniqueItems()` | `.Should().OnlyHaveUniqueItems()` | ⬜ pending (phase 05) | — |
| `.Should().Equal(...)` *(collections)* | `.Should().Equal(...)` | ⬜ pending (phase 05) | — |
| `.Should().ContainInOrder(...)` | `.Should().ContainInOrder(...)` | ⬜ pending (phase 05) | — |
| `.Should().BeInDescendingOrder()` | `.Should().BeInDescendingOrder()` | ⬜ pending (phase 13) | — |
| `.Should().MatchEquivalentOf(pattern)` | `.Should().MatchEquivalentOf(pattern)` | ⬜ pending (phase 15) | — |
| `.Should().Throw<T>()` / `.Should().ThrowAsync<T>()` | `.Should().Throw<T>()` / `.Should().ThrowAsync<T>()` | ⬜ pending (phase 03) | — |

Every gap phase appends its rows here as it lands, flipping the matching `⬜ pending` row to `✅ supported`
and naming the test class that proves it. The table covers what the audited call-site inventory shows is in
use; it is not the complete assertion surface.

## 4. Type Coverage

Which subject types have a `.Should()` entry point today.

| Subject type | Entry point today | Notes |
|---|---|---|
| `bool`, `bool?` | ✅ | |
| `char`, `char?` | ✅ | |
| `DateTime`, `DateTime?` | ✅ | |
| `double`, `float` | ✅ | |
| Integral numerics (`int`, `long`, `short`, `byte`, `decimal`, unsigned & native variants), `int?` | ✅ | |
| Enums (`where T : struct, Enum`) | ✅ | |
| `Guid`, `Guid?` | ✅ | |
| `string` | ✅ | |
| `TimeSpan`, `TimeSpan?` | ✅ | |
| `object` / reference types | ⬜ pending (phase 06) | Blocks object `Be`, `BeNull`, `BeSameAs`, `BeEquivalentTo`. |
| Collections (`IEnumerable<T>`) | ⬜ pending (phase 04) | Blocks `Contain`, `HaveCount`, `BeEmpty`, and the collection family. |
| `Action` | ⬜ pending (phase 03) | Exception assertions — `Throw<T>`, `NotThrow`. |
| `Func<Task>` | ⬜ pending (phase 03) | Async exception assertions — `ThrowAsync<T>`. |

## 5. Behavioural Differences

These compile fine and behave differently — the dangerous category. Read them before migrating.

1. **`because` replaces the message; FluentAssertions appends a reason.** In FatCat.Testing a supplied
   `because` becomes the entire failure message. In FluentAssertions the reason is appended to the generated
   message. Migrate the intent, not the text.

2. **No `params object[] becauseArgs`.** FatCat.Testing assertions take exactly `string because = null`.
   FluentAssertions' `("expected {0} items", count)` form does not exist — rewrite it as string
   interpolation:

   ```csharp
   // FluentAssertions
   list.Should().HaveCount(3, "expected {0} items", count);

   // FatCat.Testing
   list.Should().HaveCount(3, $"expected {count} items");
   ```

3. **Negation is the `Not` property, not a prefixed method.** `.Should().NotBe(x)` becomes
   `.Should().Not.Be(x)`. See §2.

4. **Collection `BeEquivalentTo` is order-insensitive by default, with no `WithStrictOrdering` opt-out.**
   This matches FluentAssertions' default, but FluentAssertions lets you opt into strict ordering and
   FatCat.Testing does not. If you need order-sensitive comparison, use `Equal` instead. *(from phase 08)*

## 6. Known Unsupported

These FluentAssertions constructs have no FatCat.Testing equivalent. Each row gives the recommended rewrite.

| FluentAssertions construct | Recommended rewrite |
|---|---|
| `AssertionScope` (soft assertions) | Assert each condition separately; the first failure throws. There is no ambient scope that collects failures. |
| `ExecutionTime` | Not supported. Measure timing outside the assertion library if you must. |
| `.And` chaining | Chain by continuing off the returned comparer (`x.Should().Be(1).BeGreaterThan(0)`); there is no `.And` property. |
| `.Which` / `.Subject` drill-down | Capture the item in a local and assert on it directly instead of drilling through the assertion chain. |
| XML assertions (`XDocument` / `XElement` / `XAttribute`) | Not supported. Assert on parsed values with the standard assertions. |
| JSON assertions (`System.Text.Json` nodes) | Not supported. Deserialize and assert on the resulting objects. |
| Serializability (`BeXmlSerializable`, `BeDataContractSerializable`, `BeBinarySerializable`) | Not supported. Round-trip in a test manually and assert on the result. |
| Event monitoring (`Monitor<T>`, `Should().Raise(...)`, `WithArgs<T>`, …) | Not supported. Subscribe to the event in the test and assert on captured arguments. |
| Reflection / architecture DSL (`Types.InAssembly(...).ThatAre*`, `BeDecoratedWith<T>`, `Implement<T>`, …) | Not supported. Use reflection directly and assert on the results. |
| `BeEquivalentTo` option methods other than `Using` / `WhenTypeIs` (the ~45 including `Excluding`, `Including`, `WithStrictOrdering`, `ComparingByMembers<T>`, …) | Not supported. FatCat.Testing ships default options plus the `Using<T>()` / `WhenTypeIs<T>()` global registration only. Restructure the expected value, or assert on the specific members. |

## 7. The Codemod

_Ships in phase 11 — see `tasks/todo/final_gaps/11-migration-codemod.md`._
