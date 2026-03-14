# Task

Adding Null / Empty / Whitespace assertions for strings

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This feature is adding support for strings.

## Tasks

- [ ]  Adding new folder for the Strings comparers
- [ ]  `"item".Should().BeNull()` if the string is empty then this would be false
- [ ]  `"item".Should().Not.BeNull()`
- [ ]  `"item".Should().BeEmpty()`  — string.Empty only
- [ ]  `"item".Should().Not.BeEmpty()` 
- [ ]  `"item".Should().BeNullOrEmpty()` — null or ""
- [ ]  `"item".Should().Not.BeNullOrEmpty()` 
- [ ]  `"item".Should().BeNullOrWhiteSpace()`  — null, empty, or all whitespace
- [ ]  `"item".Should().Not.BeNullOrWhiteSpace()` 
- [ ]  Add methods for nullable strings as well
- [ ]  Ensure all compares have a because reason

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

- C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\NumericComparer.cs
- C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\NullableIntComparer.cs
- C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\NotNullableIntComparer.cs


## Notes

- The general object assertions will be used for all valid types.
- Keep the types in there own logical files
- Use Templates to keep the code cleaner and less of it
- If there is any logical abstraction or bases classes that should be added put them in.
- Understand the complete architecture and follow the pattern we have for the Numeric Compares
