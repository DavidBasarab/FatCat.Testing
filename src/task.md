# Task

Using the int as a base we are going to create other assertions for the other C# number primitives

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This first feature is to focus on implement the checks for the number primitives.

## Tasks

- [ ]  Assertion for `byte`
- [ ]  Assertion for `sbyte`
- [ ]  Assertion for `short`
- [ ]  Assertion for `ushort`
- [ ]  Assertion for `uint`
- [ ]  Assertion for `long`
- [ ]  Assertion for `ulong`
- [ ]  Assertion for `nint`
- [ ]  Assertion for `unit`
- [ ]  Assertion for `float`
- [ ]  Assertion for `double`
- [ ]  Assertion for `decimal`

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

- C:\Code\FatCat.Testing\src\Tests.FatCat.Testing\IntComparerTests.cs
- C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\IntComparer.cs
- C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\NotIntComparer.cs

## Notes

- The general object assertions will be used for all valid types.
- Keep the types in there own logical files
- Use Templates to keep the code cleaner and less of it
- If there is any logical abstraction or bases classes that should be added put them in.
