# Task

Add Nullability for int

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This first feature is to focus on implement the checks for the int.

## Tasks

- [ ] Add `3?.Should().Be(3)`
- [ ] Add `someValue?.Should().BeNull()`
- [ ] Add `someValue?.Should().HaveValue()`
- [ ] Add `someValue?.Should().Not.HaveValue()`
- [ ] Add all other valid methods to be used as a nullable int.  This would mean that you can do `3.Should().BePositive()` and `3?.Should().BePositive()`
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

- Nullable Int Comparing should be in a separate file than the IntCompare.
- If there is any logical abstraction or bases classes that should be added put them in.
