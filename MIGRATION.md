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
| `.Should().Be(x)` *(objects)* | `.Should().Be(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeTests` |
| `.Should().NotBe(x)` *(objects)* | `.Should().Not.Be(x)` | ✅ supported | `Tests.FatCat.Testing.Objects.ObjectBeTests` |
| `.Should().NotBe(x)` *(value types)* | `.Should().Not.Be(x)` | ✅ supported | `BoolBeTests` |
| `.Should().BeTrue()` / `.Should().BeFalse()` | `.Should().BeTrue()` / `.Should().BeFalse()` | ✅ supported | `BoolBeTrueTests`, `BoolBeFalseTests` |
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
| `.Should().BeGreaterOrEqualTo(x)` | `.Should().BeGreaterThanOrEqualTo(x)` | ⬜ pending (phase 16) | — |
| `.Should().BeLessOrEqualTo(x)` | `.Should().BeLessThanOrEqualTo(x)` | ⬜ pending (phase 16) | — |
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
| `.Should().MatchEquivalentOf(pattern)` | `.Should().MatchEquivalentOf(pattern)` | ⬜ pending (phase 15) | — |
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

_Ships in phase 11 — see `tasks/todo/final_gaps/11-migration-codemod.md`._
