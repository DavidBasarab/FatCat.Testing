# Test-Driven Development

## TDD Is Non-Negotiable
- All production code is written test-first. No exceptions.
- Tests define the contract. Implementation satisfies the tests.
- Tests are not written after the fact — they define behavior before implementation begins.
- This library **is** an assertion library, and it asserts on itself. Every comparer method, every failure
  message, and every `because` override has a test.

## Test Stack
- Framework: xUnit
- Assertions: this library's own `Should()` API for the passing path; `Assert.Throws<CompareException>` (via `BaseTest`) for the failing path
- There is **no** FluentAssertions, **no** FakeItEasy, and **no** mocking library. Do not add one.
- `FatCat.Fakes` is referenced but not currently used. Prefer explicit literal values — see "Test Data" below.

## BaseTest — The Shared Base
Every test class derives from `Tests.FatCat.Testing.BaseTest`. It provides the two helpers used to assert
failure behaviour:

```csharp
public abstract class BaseTest
{
    protected void RunCompareFailTest(Action testAction) { Assert.Throws<CompareException>(testAction); }

    protected void RunCompareFailTest(Action testAction, string message)
    {
        var exception = Assert.Throws<CompareException>(testAction);

        Assert.Equal(message, exception.Message);
    }
}
```

- Use the two-argument overload wherever a specific message is expected — the message text is part of the
  public contract and must be pinned.
- Use the one-argument overload only when the message is genuinely irrelevant to the test.
- Do not add setup fields, constructors, or fake configuration to `BaseTest`. It is deliberately tiny.

## Test Class Layout — One Class Per Assertion Method
There is no `<Class>Specs` folder and no abstract per-class base. The layout is flat and mechanical:

- The test project mirrors the source folder structure exactly: `FatCat.Testing/Booleans/` → `Tests.FatCat.Testing/Booleans/`.
- One test class per **assertion method**, named `<Subject><Method>Tests`: `BoolBeTests`, `BoolBeTrueTests`, `DateTimeBeCloseToTests`, `StringContainTests`.
- The nullable variant gets its own class, prefixed `Nullable`: `NullableBoolBeTests`, `NullableDateTimeBeAfterTests`.
- Every nullable comparer also has a `Nullable<Subject>BeNullTests` class covering the null path.
- The class derives from `BaseTest` and contains only `[Fact]` methods — no fields, no constructor, no setup.

```csharp
using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringContainTests : BaseTest
{
    [Fact]
    public void GoodContain() { "hello world".Should().Contain("hello"); }

    [Fact]
    public void BadContain() { RunCompareFailTest(() => "hello world".Should().Contain("xyz"), "hello world should contain xyz"); }
}
```

## Test Method Naming — Good/Bad Prefix
`[Fact]` methods are PascalCase with **no underscores**, prefixed by the path they exercise:

| Prefix | Covers |
|---|---|
| `Good<Method>` | The assertion passes — the call simply does not throw |
| `Bad<Method>` | The assertion fails — asserted via `RunCompareFailTest` |
| `Bad<Method>ShowsCorrectMessage` | The generated failure message is exactly as expected |
| `Bad<Method>WithBecause` | A supplied `because` replaces the generated message |
| `GoodNot<Method>` / `BadNot<Method>` | The same four cases through the `Not` comparer |

```csharp
[Fact] public void GoodBe() { true.Should().Be(true); }
[Fact] public void BadBe() { RunCompareFailTest(() => false.Should().Be(true)); }
[Fact] public void BadBeShowsCorrectMessage() { RunCompareFailTest(() => false.Should().Be(true), "False should be True"); }
[Fact] public void BadBeWithBecause() { RunCompareFailTest(() => false.Should().Be(true, "custom because"), "custom because"); }
[Fact] public void GoodNotBe() { false.Should().Not.Be(true); }
[Fact] public void BadNotBe() { RunCompareFailTest(() => true.Should().Not.Be(true)); }
```

Never use underscores in a test method name. No `Should` prefix, no Given/When/Then.

## Required Coverage For A New Assertion
When you add an assertion method, it is not done until all of these exist:

1. `Good<Method>` — passing case
2. `Bad<Method>` — failing case
3. `Bad<Method>ShowsCorrectMessage` — the exact generated message
4. `Bad<Method>WithBecause` — `because` overrides the message
5. The same four for the `Not` form
6. The same set again in the `Nullable<Subject><Method>Tests` class, if the type has a nullable comparer
7. A null-subject case, where null is a meaningful input

Missing any of these is an incomplete implementation, not a follow-up task.

## One Test, One Assertion
- Each test verifies exactly one thing.
- A failing test must tell you precisely what broke without investigation.
- `RunCompareFailTest(action, message)` counts as one assertion — it verifies "throws with this message" as a single behaviour.

## Test Data — Explicit Literals
Use concrete, readable literal values. The value in the test is the value in the expected failure message,
so a reader can verify the assertion by eye:

```csharp
// Correct — the literal and the message line up
RunCompareFailTest(() => "hello world".Should().Contain("xyz"), "hello world should contain xyz");

// Wrong — a random value cannot be matched against an expected message
var text = Faker.Create<string>();
RunCompareFailTest(() => text.Should().Contain("xyz"), $"{text} should contain xyz");
```

Random test data is a poor fit here: the failure message is the thing under test, and it is built from the
input. Generate data only when the specific value genuinely does not matter.

For time-based assertions, construct explicit `DateTime` values rather than reading the clock — a test that
calls `DateTime.Now` is non-deterministic.

## Global Usings
The test project has one `GlobalUsings.cs`. Add a namespace there only when it appears in nearly every test file in the project.

```csharp
// Tests.FatCat.Testing/GlobalUsings.cs
global using FatCat.Testing;
global using Xunit;
```

Per-folder namespaces (`FatCat.Testing.Strings`, `FatCat.Testing.Exceptions`) are imported per file, not globally.

## Expression-Bodied Members in Tests — BANNED
The expression-bodied member ban applies to test code too. All test methods must use block bodies — CSharpier will collapse a short one onto a single line, which is correct:

```csharp
// Wrong
[Fact]
public void GoodBe() => true.Should().Be(true);

// Correct — CSharpier formats it to one line, braces intact
[Fact]
public void GoodBe() { true.Should().Be(true); }
```

## OneOff Is Not A Test Project
`OneOff` is a scratch console for manual experimentation. Do not put assertions there in place of real tests,
and do not treat running it as verification. Run `dotnet test Fatcat.Testing.sln`.
