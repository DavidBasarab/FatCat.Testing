# Task

Add char methods

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This feature is adding support for car.

## Tasks


- [ ]  item.Should().Be(char expected)
- [ ]  item.Should().Not.Be(char expected)
- [ ]  item.Should().BeUpperCased() — char.IsUpper()
- [ ]  item.Should().BeLowerCased() — char.IsLower()
- [ ]  item.Should().BeLetter() — char.IsLetter()
- [ ]  item.Should().BeDigit() — char.IsDigit()
- [ ]  item.Should().BeLetterOrDigit() — char.IsLetterOrDigit()
- [ ]  item.Should().BeWhiteSpace() — char.IsWhiteSpace()
- [ ]  item.Should().BeControl() — char.IsControl()
- [ ]  item.Should().Not.BeUpperCased() — char.IsUpper()
- [ ]  item.Should().Not.BeLowerCased() — char.IsLower()
- [ ]  item.Should().Not.BeLetter() — char.IsLetter()
- [ ]  item.Should().Not.BeDigit() — char.IsDigit()
- [ ]  item.Should().Not.BeLetterOrDigit() — char.IsLetterOrDigit()
- [ ]  item.Should().Not.BeWhiteSpace() — char.IsWhiteSpace()
- [ ]  item.Should().Not.BeControl() — char.IsControl()
- [ ]  Make all char assertions work with nullable

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
