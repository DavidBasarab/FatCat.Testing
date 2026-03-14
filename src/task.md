# Task

Adding equality assertions for strings

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This feature is adding support for strings.

## Tasks

- [ ]  Adding new folder for the Strings comparers
- [ ]  `"item".Should().Be(string expected)` this will be case sensitive
- [ ]  `"item".Should().Be(string expected, Options.IgnoreCase)` this will IgnoreCase valid Options are `CaseSensitive`, `IgnoreCase` and will default to `CaseSensitive`
- [ ]  `"item".Should().Not.Be(string expected)` this will be case sensitive
- [ ]  `"item".Should().Not.Be(string expected, Options.IgnoreCase)` this will IgnoreCase valid Options are `CaseSensitive`, `IgnoreCase` and will default to `CaseSensitive`
- [ ]  `"item".Should().Be(string expected)` this will be case sensitive
- [ ]  `"item".Should().Be(string expected, Options.IgnoreCase)` this will IgnoreCase valid Options are `CaseSensitive`, `IgnoreCase` and will default to `CaseSensitive`
- [ ]  `"item".Should().Not.Be(string expected)` this will be case sensitive
- [ ]  `"item".Should().Not.Be(string expected, Options.IgnoreCase)` this will IgnoreCase valid Options are `CaseSensitive`, `IgnoreCase` and will default to `CaseSensitive`
- [ ]  `"item".Should().BeEquivalentTo(string expected)` this will be case sensitive
- [ ]  `"item".Should().BeEquivalentTo(string expected, Options.IgnoreCase)` this will IgnoreCase valid Options are `CaseSensitive`, `IgnoreCase` and will default to `CaseSensitive`
- [ ]  `"item".Should().Not.BeEquivalentTo(string expected)` this will be case sensitive
- [ ]  `"item".Should().Not.BeEquivalentTo(string expected, Options.IgnoreCase)` this will IgnoreCase valid Options are `CaseSensitive`, `IgnoreCase` and will default to `CaseSensitive`
- [ ]  Add methods for nullable strings as well

## Required Steps

1. Run `jb cleanupcode` on all created/modified `.cs` files using the `Toolkit_Default` profile

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
