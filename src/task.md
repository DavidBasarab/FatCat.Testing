# Task

General Object Assertions (inherited by all types via Should())

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This first feature is to focus on implement the checks for the int.

## Tasks

- [ ] Add `3.Should().BeOneOf(2, 3, 4, 5)`
- [ ] Add `3.Should().BeOneOf(someIntList)`
- [ ] Add `3.Should().BeOfType<T>()`
- [ ] Add `3.Should().BeOfType(Type t)`
- [ ] Add `3.Should().BeAssignableTo<T>()`
- [ ] Add `3.Should().BeAssignableTo(Type t)`
- [ ] Add `3.Should().Not.BeOneOf(2, 3, 4, 5)`
- [ ] Add `3.Should().Not.BeOneOf(someIntList)`
- [ ] Add `3.Should().Not.BeOfType<T>()`
- [ ] Add `3.Should().Not.BeOfType(Type t)`
- [ ] Add `3.Should().Not.BeAssignableTo<T>()`
- [ ] Add `3.Should().Not.BeAssignableTo(Type t)`
- [ ] Satisfy(Action<int> inspector) — runs nested assertions
- [ ] Add all other valid methods to be used as a nullable int.
- [ ] Add a because reason to each added method

## Required Steps

1. Run `jb cleanupcode` on all created/modified `.cs` files using the `CineMassive_Default` profile

## Verification

- [ ] Tests written before implementation (TDD)
- [ ] All tests pass (`dotnet test`)
- [ ] `jb cleanupcode` run on all modified files
- [ ] No compiler warnings introduced
- [ ] Namespaces match folder paths exactly
- [ ] No banned patterns used (see `.claude/rules/not-allowed.md`)
- [ ] Report results before finishing

## References

- C:\Code\FatCat.Testing\src\Tests.FatCat.Testing\IntComparerTests.cs
- C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\IntComparer.cs
- C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\NotIntComparer.cs

## Notes

- If there is any logical abstraction or bases classes that should be added put them in.
