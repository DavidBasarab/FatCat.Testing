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
- `⬜ out of scope — tier_2_gaps` — deliberately **not** shipped by the `final_gaps` plan. These belong to the
  independent `tier_2_gaps` plan (G7–G10) and are owned by the named phase there; they are listed here only so
  the mapping is complete and the ownership is explicit. As of this close-out those methods are genuinely
  absent from the library (verified by grep), so the row is not a claim this plan makes.

Where a method is supported for some subject types but pending for others (for example `Be` on value types
versus objects), the row reflects what ships today and §4 records the type coverage.

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `.Should().Be(x)` *(value types & string)* | `.Should().Be(x)` | ✅ supported | `BoolBeTests`, `IntBeTests` |
| `.Should().Be(x)` *(objects)* | `.Should().Be(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeTests` |
| `.Should().NotBe(x)` *(objects)* | `.Should().Not.Be(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeTests` |
| `.Should().NotBe(x)` *(value types)* | `.Should().Not.Be(x)` | ✅ supported | `BoolBeTests` |
| `.Should().Be("guid-string")` *(Guid)* | `.Should().Be("guid-string")` | ✅ supported | `Tests.FatCat.Testing.Guids.GuidBeStringTests` |
| `.Should().BeTrue()` / `.Should().BeFalse()` | `.Should().BeTrue()` / `.Should().BeFalse()` | ✅ supported | `BoolBeTrueTests`, `BoolBeFalseTests` |
| `Imply(x)` *(FatCat-native — no FluentAssertions equivalent)* | `.Should().Imply(x)` | ✅ supported | `Tests.FatCat.Testing.Booleans.BoolImplyTests` |
| `.Should().BeNull()` *(value types & string)* | `.Should().BeNull()` | ✅ supported | `StringBeNullTests`, `NullableGuidBeNullTests` |
| `.Should().BeNull()` *(objects)* | `.Should().BeNull()` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeNullTests` |
| `.Should().NotBeNull()` *(objects)* | `.Should().Not.BeNull()` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeNullTests` |
| `.Should().BeEmpty()` *(string & Guid)* | `.Should().BeEmpty()` | ✅ supported | `StringBeEmptyTests`, `GuidBeEmptyTests` |
| `.Should().BeEmpty()` *(collections)* | `.Should().BeEmpty()` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionBeEmptyTests` |
| `.Should().NotBeEmpty()` *(collections)* | `.Should().Not.BeEmpty()` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionNotBeEmptyTests` |
| `.Should().BeNullOrEmpty()` | `.Should().BeNullOrEmpty()` | ✅ supported | `StringBeNullOrEmptyTests` |
| `.Should().NotBeNullOrEmpty()` | `.Should().Not.BeNullOrEmpty()` | ✅ supported | `StringBeNullOrEmptyTests` |
| `.Should().NotBeNullOrWhiteSpace()` | `.Should().Not.BeNullOrWhiteSpace()` | ✅ supported | `StringBeNullOrWhiteSpaceTests` |
| `.Should().Contain(x)` *(string)* | `.Should().Contain(x)` | ✅ supported | `StringContainTests` |
| `.Should().ContainEquivalentOf(x)` *(string)* | `.Should().ContainEquivalentOf(x)` | ✅ supported | `Tests.FatCat.Testing.Strings.StringContainEquivalentOfTests` |
| `.Should().NotContainAll(...)` *(string)* | `.Should().Not.ContainAll(...)` | ✅ supported | `StringContainTests` |
| `.Should().NotContainAny(...)` *(string)* | `.Should().Not.ContainAny(...)` | ✅ supported | `StringContainTests` |
| `.Should().Contain(x)` *(collections)* | `.Should().Contain(x)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionContainTests` |
| `.Should().NotContain(x)` *(collections)* | `.Should().Not.Contain(x)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionContainTests` |
| `.Should().EndWith(x)` | `.Should().EndWith(x)` | ✅ supported | `StringEndWithTests` |
| `.Should().BeEquivalentTo(x)` *(string)* | `.Should().BeEquivalentTo(x)` | ✅ supported | `StringBeEquivalentToTests` |
| `.Should().BeEquivalentTo(x)` *(object graphs)* | `.Should().BeEquivalentTo(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeEquivalentToTests` |
| `.Should().BeEquivalentTo(coll)` *(collections)* | `.Should().BeEquivalentTo(coll)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionBeEquivalentToTests` |
| `.Should().NotBeEquivalentTo(x)` | `.Should().Not.BeEquivalentTo(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeEquivalentToTests` |
| `options.Using<T>(...).WhenTypeIs<T>()` | `Equivalency.Using<T>((s, e) => ...)` | ✅ supported | `Tests.FatCat.Testing.Equivalency.EquivalencyUsingTests` |
| `.Should().BeCloseTo(x, tolerance)` | `.Should().BeCloseTo(x, tolerance)` | ✅ supported | `DateTimeBeCloseToTests`, `TimeSpanBeCloseToTests` |
| `.Should().BeSameDateAs(x)` *(DateTime)* | `.Should().BeSameDateAs(x)` | ✅ supported *(coverage — no call site)* | `DateTimeBeSameDateAsTests` |
| `.Should().BeIn(kind)` *(DateTime)* | `.Should().BeIn(kind)` | ✅ supported *(coverage — no call site)* | `DateTimeBeInTests` |
| `.Should().BeLessThan(t).Before(x)` / `.After(x)` *(DateTime)* | same | ✅ supported *(coverage — no call site)* | `DateTimeBeLessThanBeforeTests`, `DateTimeBeLessThanAfterTests` |
| `.Should().BeMoreThan(t).Before(x)` / `.After(x)` *(DateTime)* | same | ✅ supported *(coverage — no call site)* | `DateTimeBeMoreThanBeforeTests`, `DateTimeBeMoreThanAfterTests` |
| `.Should().BeAtLeast(t).Before(x)` / `.After(x)` *(DateTime)* | same | ✅ supported *(coverage — no call site)* | `DateTimeBeAtLeastBeforeTests`, `DateTimeBeAtLeastAfterTests` |
| `.Should().BeWithin(t).Before(x)` / `.After(x)` *(DateTime)* | same | ✅ supported *(coverage — no call site)* | `DateTimeBeWithinBeforeTests`, `DateTimeBeWithinAfterTests` |
| `.Should().BeExactly(t).Before(x)` / `.After(x)` *(DateTime)* | same | ✅ supported *(coverage — no call site)* | `DateTimeBeExactlyBeforeTests`, `DateTimeBeExactlyAfterTests` |
| `2.Hours()` / `2.Minutes()` *(FluentAssertions.Extensions)* | `2.Hours()` / `2.Minutes()` | ✅ supported *(coverage — no call site)* | `NumericTimeExtensionsTests` |
| `.Should().BeApproximately(x, tolerance)` | `.Should().BeApproximately(x, tolerance)` | ✅ supported | `DoubleBeApproximatelyTests` |
| `.Should().NotBeApproximately(v, t)` | `.Should().Not.BeApproximately(v, t)` | ✅ supported | `DoubleBeApproximatelyTests`, `FloatBeApproximatelyTests` |
| `.Should().NotBeInRange(lo, hi)` | `.Should().Not.BeInRange(lo, hi)` | ✅ supported | `IntBeInRangeTests`, `DoubleNotBeInRangeTests`, `FloatNotBeInRangeTests` |
| `.Should().BeGreaterThan(x)` / `.Should().BeLessThan(x)` | same | ✅ supported | `IntBeGreaterThanTests`, `IntBeLessThanTests` |
| `.Should().BeGreaterOrEqualTo(x)` *(numerics)* | `.Should().BeGreaterThanOrEqualTo(x)` | ⬜ out of scope — `tier_2_gaps` (phases 02/03, G8) | — |
| `.Should().BeLessOrEqualTo(x)` *(numerics)* | `.Should().BeLessThanOrEqualTo(x)` | ⬜ out of scope — `tier_2_gaps` (phases 02/03, G8) | — |
| `.Should().BeOfType<T>()` | `.Should().BeOfType(typeof(T))` | ✅ supported | `ComparerBaseTests` |
| `.Should().BeOneOf(...)` | `.Should().BeOneOf(...)` | ✅ supported | `ComparerBaseTests` |
| `.Should().BeSameAs(x)` | `.Should().BeSameAs(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeSameAsTests` |
| `.Should().NotBeSameAs(x)` | `.Should().Not.BeSameAs(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeSameAsTests` |
| `.Should().HaveCount(n)` | `.Should().HaveCount(n)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionHaveCountTests` |
| `.Should().ContainSingle()` | `.Should().ContainSingle()` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionContainSingleTests` |
| `.Should().ContainSingle(p)` | `.Should().ContainSingle(p)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionContainSinglePredicateTests` |
| `.Should().ContainEquivalentOf(x)` | `.Should().ContainEquivalentOf(x)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionContainEquivalentOfTests` |
| `.Should().NotContainEquivalentOf(x)` | `.Should().Not.ContainEquivalentOf(x)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionContainEquivalentOfTests` |
| `.Should().OnlyContain(pred)` | `.Should().OnlyContain(pred)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionOnlyContainTests` |
| `.Should().OnlyHaveUniqueItems()` | `.Should().OnlyHaveUniqueItems()` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionOnlyHaveUniqueItemsTests` |
| `.Should().Equal(...)` *(collections)* | `.Should().Equal(...)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionEqualTests` |
| `.Should().ContainInOrder(...)` | `.Should().ContainInOrder(...)` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionContainInOrderTests` |
| `.Should().BeInDescendingOrder()` | `.Should().BeInDescendingOrder()` | ✅ supported | `Tests.FatCat.Testing.Collections.CollectionBeInDescendingOrderTests` |
| `.Should().BeInAscendingOrder()` | `.Should().BeInAscendingOrder()` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionBeInAscendingOrderTests` |
| `.Should().BeSubsetOf(super)` | `.Should().BeSubsetOf(super)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionBeSubsetOfTests` |
| `.Should().IntersectWith(other)` | `.Should().IntersectWith(other)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionIntersectWithTests` |
| `.Should().NotIntersectWith(other)` | `.Should().Not.IntersectWith(other)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionIntersectWithTests` |
| `.Should().AllSatisfy(inspector)` | `.Should().AllSatisfy(inspector)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionAllSatisfyTests` |
| `.Should().SatisfyRespectively(...)` | `.Should().SatisfyRespectively(...)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionSatisfyRespectivelyTests` |
| `.Should().AllBeOfType<T>()` | `.Should().AllBeOfType<T>()` / `.Should().AllBeOfType(type)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionAllBeOfTypeTests` |
| `.Should().AllBeEquivalentTo(x)` | `.Should().AllBeEquivalentTo(x)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionAllBeEquivalentToTests` |
| `.Should().HaveCountGreaterThan(n)` | `.Should().HaveCountGreaterThan(n)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionHaveCountGreaterThanTests` |
| `.Should().HaveCountLessThan(n)` | `.Should().HaveCountLessThan(n)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionHaveCountLessThanTests` |
| `.Should().HaveSameCount(other)` | `.Should().HaveSameCount(other)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionHaveSameCountTests` |
| `.Should().HaveElementAt(i, x)` | `.Should().HaveElementAt(i, x)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionHaveElementAtTests` |
| `.Should().ContainNulls()` | `.Should().ContainNulls()` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionContainNullsTests` |
| `.Should().NotContainNulls()` | `.Should().Not.ContainNulls()` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionContainNullsTests` |
| `.Should().ContainMatch(wildcard)` | `.Should().ContainMatch(wildcard)` | ✅ supported *(coverage — no call site)* | `Tests.FatCat.Testing.Collections.CollectionContainMatchTests` |
| `.Should().MatchEquivalentOf(pattern)` *(string)* | `.Should().MatchEquivalentOf(pattern)` | ⬜ out of scope — `tier_2_gaps` (phase 04, G9) | — |
| `.Should().Throw<T>()` | `.Should().Throw<T>()` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ActionThrowTests` |
| `.Should().ThrowAsync<T>()` | `.Should().ThrowAsync<T>()` | ✅ supported | `Tests.FatCat.Testing.Exceptions.AsyncActionThrowAsyncTests` |
| `.Should().NotThrow()` | `.Should().NotThrow()` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ActionNotThrowTests` |
| `.Should().Throw<T>().WithMessage(m)` | `.Should().Throw<T>().WithMessage(m)` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ActionThrowWithMessageTests` |
| `.Should().ThrowExactly<T>()` | `.Should().ThrowExactly<T>()` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ActionThrowExactlyTests` |
| `.Should().ThrowExactlyAsync<T>()` | `.Should().ThrowExactlyAsync<T>()` | ✅ supported | `Tests.FatCat.Testing.Exceptions.AsyncActionThrowExactlyAsyncTests` |
| `.Should().Throw<T>().WithInnerException<TInner>()` | `.Should().Throw<T>().WithInnerException<TInner>()` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ThrownExceptionWithInnerExceptionTests` |
| `.Should().Throw<T>().WithInnerExceptionExactly<TInner>()` | `.Should().Throw<T>().WithInnerExceptionExactly<TInner>()` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ThrownExceptionWithInnerExceptionExactlyTests` |
| `.Should().Throw<T>().Where(predicate)` | `.Should().Throw<T>().Where(predicate)` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ThrownExceptionWhereTests` |
| `.Should().Throw<T>().WithParameterName(name)` | `.Should().Throw<T>().WithParameterName(name)` | ✅ supported | `Tests.FatCat.Testing.Exceptions.ThrownExceptionWithParameterNameTests` |
| `.Should().HaveSameNameAs(x)` | `.Should().HaveSameNameAs(x)` | ✅ supported | `Tests.FatCat.Testing.Enums.EnumHaveSameNameAsTests` |
| `.Should().HaveSameValueAs(x)` | `.Should().HaveSameValueAs(x)` | ✅ supported | `Tests.FatCat.Testing.Enums.EnumHaveSameValueAsTests` |
| `.Should().NotBeDefined()` | `.Should().Not.BeDefined()` | ✅ supported | `Tests.FatCat.Testing.Enums.EnumBeDefinedTests` |
| `ReferenceTypeAssertions<T, TAssertions>` *(custom-assertion base)* | `ComparerBase<TSubject, TComparer>` | ✅ supported | `Tests.FatCat.Testing.Extensibility.CustomComparerTests` |
| `AndConstraint<T>` *(custom-assertion return)* | return `this` (the comparer) | ✅ supported | `Tests.FatCat.Testing.Extensibility.CustomComparerTests` |

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
| `object` / reference types | ✅ | `Be` (value), `BeEquivalentTo` (structural), `BeNull`, `BeSameAs` (reference identity) ship. |
| Collections (`IEnumerable<T>`, `List<T>`, `T[]`) | ✅ | Core methods (`Contain`, `BeEmpty`, `HaveCount`, `ContainSingle`), order-sensitive `Equal`, and the structural `BeEquivalentTo` / `ContainEquivalentOf` all ship. |
| `Action` | ✅ | Exception assertions — `Throw<T>`, `ThrowExactly<T>`, `NotThrow`, plus `WithInnerException`/`Where`/`WithParameterName` on the caught exception. |
| `Func<Task>` | ✅ | Async exception assertions — `ThrowAsync<T>`, `ThrowExactlyAsync<T>`, `NotThrowAsync`. |

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
   This is now backed by a shipped assertion (`CollectionBeEquivalentToTests`). It matches FluentAssertions'
   default, but FluentAssertions lets you opt into strict ordering and FatCat.Testing does not. If you need
   order-sensitive comparison, use `Equal` instead. Elements are compared **structurally** (reusing the object
   equivalency engine) and paired **greedily** — each element takes the first still-unmatched equivalent
   candidate, rather than solving a full bipartite matching. *(from phase 08)*

6. **Object `BeEquivalentTo` compares public readable instance properties only — fields are excluded.**
   Two objects that differ only in a public **field** are still equivalent. This matches FluentAssertions'
   default (properties only), but FluentAssertions can be configured to include fields
   (`ComparingByMembers`, `IncludingFields`) and FatCat.Testing ships no such option (ADR-6). If a field
   carries state that must be compared, expose it as a property or assert on it directly.

5. **`WithMessage` compares exactly; FluentAssertions supports `*` wildcards.**
   `Throw<T>().WithMessage(m)` requires the caught exception's message to equal `m` exactly. FluentAssertions'
   wildcard form (`WithMessage("card *")`) has no equivalent. Rewrite a wildcard assertion as
   `Throw<T>().Where(e => e.Message.Contains("..."))`, or assert on the message directly. *(from phase 03;
   `Where` shipped in phase 14)*

7. **Per-type equivalency rules are registered globally, not per `BeEquivalentTo` call.** FluentAssertions'
   `options.Using<T>(...).WhenTypeIs<T>()` is a per-call option on one assertion. FatCat.Testing ships a
   single **global** registration instead — `Equivalency.Using<T>((subject, expected) => ...)` — with no
   `WhenTypeIs<T>()` chain (the `<T>` already carries the type) and no per-call options lambda. Rewrite each
   `Using().WhenTypeIs()` chain to one `Equivalency.Using<T>` call in a **fixture** (a collection fixture or
   a static/instance constructor), and add `Equivalency.Reset()` to the fixture teardown — the registry is
   process-wide mutable state, so leaving a rule registered makes every later test in the run see it.
   Registering twice for the same type is last-registration-wins. *(from phase 09)*

   ```csharp
   // FluentAssertions — per call
   actual.Should().BeEquivalentTo(expected, options =>
       options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds())).WhenTypeIs<DateTime>());

   // FatCat.Testing — once, in a fixture
   Equivalency.Using<DateTime>((subject, expected) => (subject - expected).Duration() <= TimeSpan.FromSeconds(1));
   actual.Should().BeEquivalentTo(expected);
   ```

8. **A ported custom assertion drops `becauseArgs` and uses `because` as a replacement.** FluentAssertions
   custom assertions built on `ReferenceTypeAssertions<T, TAssertions>` return `AndConstraint<T>`; the
   FatCat.Testing equivalent derives from `ComparerBase<TSubject, TComparer>` and returns `this` (see
   `## Custom Comparers` in [`README.md`](README.md)). When porting one, rewrite any
   `("expected {0}", value)` reason into string interpolation — `($"expected {value}")` — because there is no
   `params object[] becauseArgs`, and remember `because` **replaces** the generated message rather than being
   appended to it. Proven by `Tests.FatCat.Testing.Extensibility.CustomComparerTests`. *(from phase 10)*

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

The rewrite in §2 is mechanical, so the repository ships a codemod that performs it across a tree of C#
source: `tools/Convert-FluentAssertions.ps1`. It is a PowerShell 7 (`pwsh`) script; it is **not** part of the
NuGet package — it lives in the GitHub repository only, since packaging a script into a library `.nupkg`
changes the package layout for no consumer benefit.

> The file and function are named `Convert-FluentAssertions` rather than `Migrate-FluentAssertions`, because
> the repository's PowerShell rules require an approved `Verb-Noun` verb and `Migrate` is not one (`Get-Verb`);
> `Convert` is.

### The transform

The core is a single regular expression:

```
find:     \.Should\(\)\.Not([A-Z]\w*)\(
replace:  .Should().Not.$1(
```

FluentAssertions negations are always the literal `Not` followed by a PascalCase method name, so
`.Should().NotBeNull()` becomes `.Should().Not.BeNull()` uniformly. The script only rewrites a `Not<Method>`
whose `<Method>` is a **known FluentAssertions negation**; a `.Should().Not<Method>(` whose method it does not
recognise is left untouched and **reported** (case 3 below), so a project-defined assertion that merely starts
with `Not` is never silently broken.

The rewrite is **idempotent**: an already-migrated `.Should().Not.BeNull()` no longer matches the find pattern
(`Not` is followed by `.`, not an uppercase letter), so running the script a second time changes nothing.

### Running it

Preview every change without touching a file:

```pwsh
pwsh ./tools/Convert-FluentAssertions.ps1 -Path <directory> -WhatIf
```

Then apply the rewrite:

```pwsh
pwsh ./tools/Convert-FluentAssertions.ps1 -Path <directory>
```

`-Path` is the root to search; every `*.cs` file beneath it is scanned, and `bin/` and `obj/` are excluded.
The script prints how many negations it rewrote, in which files, followed by the four cases below.

### The four cases the regex cannot catch

The script **reports** each of these rather than silently skipping or mangling it, so nothing is lost to a
blind rewrite.

1. **Chained negations through `.And`** — `x.Should().Contain(a).And.NotContain(b)`. `.And` is not supported
   (there is no `.And` property; see §6). The regex leaves the `.And.NotContain(` untouched. **Fix:** split
   the chain into separate statements — `x.Should().Contain(a); x.Should().Not.Contain(b);`.
2. **Line-broken chains** — `.Should()` and `.NotXxx(` on different lines. The single-line regex does not
   match across the break, so nothing is rewritten. **Fix:** rewrite by hand, inserting the `Not.` before the
   method — for example a `.NotBeNull()` continuation line becomes `.Not.BeNull()`.
3. **Project-defined negations** — a method whose name begins with `Not` but is defined by your project, not
   FluentAssertions. The script rewrites only method names it recognises as FluentAssertions negations; an
   unrecognised `.Should().Not<Method>(` is left in place and listed for review. **Fix:** confirm whether it is
   a FluentAssertions negation (rewrite it) or your own assertion (leave it, or migrate it per the custom
   comparer guidance in [`README.md`](README.md)).
4. **Negations on a subject the target build does not yet cover** — a rewritten `.Should().Not.Xxx(` compiles
   only once FatCat.Testing ships the matching assertion for that subject type. This is not detectable from
   source and does not arise for the in-repo fixtures, but in a consumer repository it means the rewrite must
   follow the per-repo order below. **Fix:** run the codemod per repository in the sequence in §5.4, then build
   and resolve any residue the compiler flags.

### Per-repo sequence

FluentAssertions is a transitive dependency for downstream consumers, so migrate in dependency order:

1. **`FatCat.Toolkit` first.** It references FluentAssertions from **production** projects (not only tests)
   because it ships test helpers, so replacing it changes the public contract of the Toolkit NuGet package —
   treat it as its own release step. Swap the `global using FluentAssertions;` to `global using
   FatCat.Testing;`, run the codemod, then build and fix the residue.
2. **Port the custom assertion layer next** — the FluentAssertions custom assertions built on
   `ReferenceTypeAssertions<T, TAssertions>` become comparers deriving from `ComparerBase<TSubject, TComparer>`
   (see the custom-assertion note in §5 and the `## Custom Comparers` recipe in [`README.md`](README.md)).
3. **`Fog` last**, once it can pick up a FluentAssertions-free Toolkit.

The script reports what it cannot rewrite; those sites are hand-edited in the consuming repository.
