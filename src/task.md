# Task

String Contains assertions

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This feature is adding support for strings.

## Tasks

- [ ]  item.Should().Contain(string expected) — case-sensitive
- [ ]  item.Should().Contain(string expected, OccurrenceConstraint occurrence) — with occurrence count e.g. Exactly.Once(), AtLeast.Twice(), AtMost.Times(5), MoreThan.Thrice(), LessThan.Twice()
- [ ]  item.Should().Not.Contain(string expected)
- [ ]  item.Should().ContainAll(params string[] expected) — must contain every substring
- [ ]  item.Should().Not.ContainAll(params string[] expected) — must not contain ALL of them (can contain some)
- [ ]  item.Should().ContainAny(params string[] expected) — must contain at least one
- [ ]  item.Should().Not.ContainAny(params string[] expected) — must contain none
- [ ]  Add Options options = Options.CaseSensitive to all methods for strings
- [ ]  Ensure all compares have a because reason

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

- C:\Code\FatCat.Testing\src\FatCat.Testing\Strings\StringComparer.cs


## Notes

- The general object assertions will be used for all valid types.
- Keep the types in there own logical files
- Use Templates to keep the code cleaner and less of it
- If there is any logical abstraction or bases classes that should be added put them in.
- Understand the complete architecture and follow the pattern we have for the Numeric Compares
