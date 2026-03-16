# Task

Add `DateTime` methods

## Feature Context

This will be a replacement for FluentAssertions.  The syntax will be slightly different and the intention is to use it with doing unit tests in C#.  This feature is adding support for `DateTime`.

## Tasks


- [ ]  Be(DateTime expected)
- [ ]  NotBe(DateTime expected)
- [ ]  BeAfter(DateTime expected)
- [ ]  BeOnOrAfter(DateTime expected)
- [ ]  BeBefore(DateTime expected)
- [ ]  BeOnOrBefore(DateTime expected)
- [ ]  BeCloseTo(DateTime expected, TimeSpan precision) — within a time window
- [ ]  NotBeCloseTo(DateTime expected, TimeSpan precision)
- [ ]  HaveYear(int expected)
- [ ]  NotHaveYear(int expected)
- [ ]  HaveMonth(int expected)
- [ ]  NotHaveMonth(int expected)
- [ ]  HaveDay(int expected)
- [ ]  NotHaveDay(int expected)
- [ ]  HaveHour(int expected)
- [ ]  NotHaveHour(int expected)
- [ ]  HaveMinute(int expected)
- [ ]  NotHaveMinute(int expected)
- [ ]  HaveSecond(int expected)
- [ ]  NotHaveSecond(int expected)
- [ ]  HaveMillisecond(int expected)
- [ ]  NotHaveMillisecond(int expected)
- [ ]  BeUtc() — DateTimeKind.Utc
- [ ]  BeLocal() — DateTimeKind.Local
- [ ]  HaveKind(DateTimeKind expected)
- [ ]  HaveOffset(TimeSpan expected)
- [ ]  NotHaveOffset(TimeSpan expected)
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
