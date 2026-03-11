# Task

Add full int assertion suite to FatCat.Testing.

## Feature Context

We are writing an alternative to Fluent Assertions for use in personal and professional projects.

The API follows this pattern: `3.Should().BeGreaterThan(2)` — extension methods off the int value that throw on failure with a clear message.

For the negative cases I want to do a .Not property rather than a method an example is `4.Should().Not.Be(5)`

For each positive case we write we should add an equivalent negative case.

The existing `BeAround` implementation in `NumberComparer` is the pattern to follow for all new assertions.

## Tasks

- [ ] Add `BeGreaterThan(int expected)`
- [ ] Add `BeLessThan(int expected)`
- [ ] Add `BeInRange(int min, int max)`
- [ ] Add `BePositive()`
- [ ] Add `BeNegative()`
- [ ] Add `BeZero()`
- [ ] Add `Not.BeGreaterThan`, `Not.BeLessThan`, `Not.BeInRange`, `Not.BePositive`, `Not.BeNegative`, `Not.BeZero`
- [ ] Add optional `string because` parameter to each assertion for a custom failure message

## Required Steps

1. Read the existing `NumberComparer.cs` and `NumberTests.cs` to understand the current pattern before writing anything
2. Rename any generic "Number" naming to be Int-specific where appropriate
3. For each assertion: write the failing test first, then implement, then write the success test
4. Every assertion must have a failure case test that verifies the failure message is clear and correct
5. Custom message: when `because` is provided, print it instead of the default message
6. Run `jb cleanupcode` on all created/modified `.cs` files using the `Toolkit_Default` profile

## Verification

- [ ] Tests written before implementation (TDD)
- [ ] All tests pass (`dotnet test`)
- [ ] Every assertion has a failure case test with message verification
- [ ] `jb cleanupcode` run on all modified files
- [ ] No compiler warnings introduced
- [ ] Namespaces match folder paths exactly
- [ ] No banned patterns used (see `.claude/rules/not-allowed.md`)
- [ ] If any single unit test takes longer than 100ms, flag it before finishing
- [ ] Report results before finishing

## References

- `C:\Code\FatCat.Testing\src\Tests.FatCat.Testing\NumberTests.cs`
- `C:\Code\FatCat.Testing\src\FatCat.Testing\Numbers\NumberComparer.cs`

## Notes

- Follow the exact same structural pattern as `BeAround` — do not introduce new patterns
- The `Not` chain should mirror each positive assertion exactly