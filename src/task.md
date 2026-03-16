# Task

Add `IEnumerable<T>` methods

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This feature is adding support for `IEnumerable<T>`.

## Tasks


- [ ]  Empty / Null

BeEmpty()
NotBeEmpty()
BeNullOrEmpty()
NotBeNullOrEmpty()

Count

HaveCount(int expected)
NotHaveCount(int expected)
HaveCountGreaterThan(int expected)
HaveCountGreaterThanOrEqualTo(int expected)
HaveCountLessThan(int expected)
HaveCountLessThanOrEqualTo(int expected)
HaveCount(Expression<Func<int, bool>> countPredicate) — e.g. c => c > 3
HaveSameCount(IEnumerable<T> expected)
NotHaveSameCount(IEnumerable<T> expected)
ContainSingle() — exactly one element
ContainSingle(Expression<Func<T, bool>> predicate) — exactly one matching element

Equality

Equal(IEnumerable<T> expected) — order sensitive, uses Equals()
Equal(params T[] expected) — params overload
Equal(IEnumerable<T> expected, Func<T,T,bool> comparer) — custom equality
NotEqual(IEnumerable<T> expected)
BeEquivalentTo(IEnumerable<T> expected) — order insensitive, deep comparison
NotBeEquivalentTo(IEnumerable<T> expected)

Contains

Contain(T expected)
Contain(Expression<Func<T, bool>> predicate)
Contain(IEnumerable<T> expected) — contains all items (subset)
NotContain(T expected)
NotContain(Expression<Func<T, bool>> predicate)
NotContain(IEnumerable<T> expected)
ContainEquivalentOf(T expected) — deep equality match
NotContainEquivalentOf(T expected)
ContainItemsAssignableTo<TExpected>() — all items assignable to type
OnlyContain(Expression<Func<T, bool>> predicate) — all items match predicate

Ordering

BeInAscendingOrder()
BeInAscendingOrder(IComparer<T> comparer)
BeInAscendingOrder(Expression<Func<T, object>> propertyExpression)
BeInAscendingOrder(Expression<Func<T, object>> propertyExpression, IComparer<T> comparer)
BeInDescendingOrder()
BeInDescendingOrder(IComparer<T> comparer)
BeInDescendingOrder(Expression<Func<T, object>> propertyExpression)
BeInDescendingOrder(Expression<Func<T, object>> propertyExpression, IComparer<T> comparer)
NotBeInAscendingOrder()
NotBeInDescendingOrder()
ContainInOrder(IEnumerable<T> expected) — items appear in order (not necessarily consecutive)
ContainInConsecutiveOrder(IEnumerable<T> expected) — items appear consecutively
NotContainInConsecutiveOrder(IEnumerable<T> expected)

Uniqueness

OnlyHaveUniqueItems()
OnlyHaveUniqueItems(Expression<Func<T, object>> propertyExpression)

Type Checking

AllBeAssignableTo<TExpected>()
AllBeAssignableTo(Type expected)
AllBeOfType<TExpected>()
AllBeOfType(Type expected)
ContainItemsAssignableTo<TExpected>()

Null Elements

NotContainNulls()
NotContainNulls(Expression<Func<T, object>> propertyExpression)

Predicates / Inspection

AllSatisfy(Action<T> inspector) — every element passes nested assertions
SatisfyRespectively(params Action<T>[] inspectors) — one inspector per element in order
Satisfy(Expression<Func<IEnumerable<T>, bool>> predicate)

Start / End

StartWith(IEnumerable<T> expected)
StartWith(IEnumerable<T> expected, Func<T,T,bool> comparer)
EndWith(IEnumerable<T> expected)
EndWith(IEnumerable<T> expected, Func<T,T,bool> comparer)
- [ ]  Make all enum assertions work with nullable

## Required Steps

1. Run `jb cleanupcode` on all created/modified `.cs` files using the `Toolkit_Default` profile

## Verification

- [ ] Tests written before implementation (TDD)
- [ ] All tests pass (`dotnet test`)
- [ ] `jb cleanupcode` run on all modified files
- [ ] Run `dotnet csharpier .` after `jb cleanupcode`
- [ ] No compiler warnings introduced
- [ ] Namespaces match folder paths exactly
- [ ] No banned patterns used (see `.claude/rules/not-allowed.md`)
- [ ] Report results before finishing

## References 


## Notes

- The general object assertions will be used for all valid types.
- Keep the types in there own logical files
- Use Templates to keep the code cleaner and less of it
- If there is any logical abstraction or bases classes that should be added put them in.
- Understand the complete architecture and follow the pattern we have for the Numeric Compares
- Keep test files in a logical group to prevent too many test method in the same test file
