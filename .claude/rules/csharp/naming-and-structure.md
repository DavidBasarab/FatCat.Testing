# Naming & Structure

## Core Philosophy
- Follow Clean Code principles (Robert C. Martin) and SOLID.
- Methods do one thing. Classes have one responsibility.
- Code reads like prose. Names make intent obvious without reading the implementation.
- Prefer interfaces and polymorphism over if/switch chains.
- Do NOT over-engineer. Do NOT introduce abstractions that do not already exist in this codebase.
- Match the abstraction level and style of the surrounding code.

## Naming Rules
- Avoid abbreviations. Prefer full words so readers never have to guess meaning.
- Acceptable abbreviations: widely recognized acronyms (e.g. `HTTP`, `URL`, `ID`) and any abbreviation that appears among the top 3 Google results for that term. When in doubt, use the full word.
- Names reveal intent. A method name makes it unnecessary to read the body.
- No comments explaining what code does — rename until it is obvious.
- PascalCase: classes, interfaces, methods, properties, constants
- camelCase: local variables, parameters, private fields — no leading underscore
- Private fields prefer `readonly` for dependencies where applicable
- Boolean names read as questions or states: `isReady`, `hasOutputs`, `canRestore`
- String interpolation required — never string concatenation with `+`
- Do NOT suffix method names with `Async` just because they return a `Task`. Name the method after what it does: `Save`, not `SaveAsync`. Only use the `Async` suffix when a non-async overload with the same name already exists and both must coexist.

## Discards
- Use `_` to discard outputs you intentionally do not need — `out _` for ignored out parameters, `using var _ = ...` for disposables acquired only for their side effect.

## Method Size
- Methods should be as short as possible.
- ~10 lines is a signal to evaluate refactoring — not an automatic rule.
- No method should require a comment to explain what it does. Refactor or rename instead.

## Spacing
- Leave a blank line between method definitions.
- Leave a blank line after variable declarations in a method before logic begins.
- Leave a blank line before return statements.

## Control Flow
- Avoid deep if/else nesting. Prefer guard clauses and early returns to keep the main flow readable.
- Avoid complex nested ternary expressions — prefer clear `if` statements or extract into a well-named method.
- Always use braces on `if` statements, including single-statement bodies. CSharpier expands a braced body with content onto its own lines — write the block form (`if (Subject != expected)\n{\n\tCompareException.New(because);\n}`); never the one-line braced form and never braceless.
- If you need to explain what code does with a comment, first ask whether a better name makes the comment unnecessary.
- Use switch expressions (not if/else chains) when branching on an enum or type. Always include a discard arm `_` that throws `ArgumentOutOfRangeException` for unhandled cases:

```csharp
// Correct — switch expression
var message = constraint switch
{
    Exactly exactly => exactly.BuildMessage(subject),
    AtLeast atLeast => atLeast.BuildMessage(subject),
    _ => throw new ArgumentOutOfRangeException(nameof(constraint)),
};

// Wrong — if/else chain
if (constraint is Exactly exactly) message = exactly.BuildMessage(subject);
else if (constraint is AtLeast atLeast) message = atLeast.BuildMessage(subject);
```

## Files & Namespaces
- One class per file. File named after the class, never the interface.
- When a class directly implements a single interface, the interface and class live in the same file — named after the class. Do not create a separate file for the interface.
- Only create a standalone interface file when the interface has multiple implementations or is consumed without a single obvious implementation.
- Namespace must exactly match the folder path within the project. No exceptions.
- All production namespaces start with `FatCat.Testing.*` (e.g. `FatCat.Testing.Booleans`, `FatCat.Testing.Comparers`, `FatCat.Testing.Strings`).
- Test project mirrors source project: same folder structure, same namespace with `Tests.` prepended — `FatCat.Testing.Booleans` → `Tests.FatCat.Testing.Booleans`.
- Always use file-scoped namespaces (C# 10+). Never use block-style `namespace X { }`.

```csharp
// Correct — file-scoped
namespace FatCat.Testing.Booleans;

public class BoolComparer { }

// Wrong — block-scoped
namespace FatCat.Testing.Booleans
{
    public class BoolComparer { }
}
```

## Project Layout

The solution is `Fatcat.Testing.sln` with three projects:

| Project | Purpose |
|---|---|
| `FatCat.Testing` | The assertion library itself — the shipped NuGet package. |
| `Tests.FatCat.Testing` | xUnit tests for the library. Mirrors the source folder structure exactly. |
| `OneOff` | Scratch console for manual experimentation. Not shipped, not tested. |

Inside `FatCat.Testing`, code is grouped into one folder per type family being asserted on:
`Booleans/`, `Characters/`, `DateTimes/`, `Doubles/`, `Enums/`, `Floats/`, `Guids/`, `Numbers/`,
`Strings/`, `TimeSpans/`, plus `Comparers/` for the shared base classes and `Exceptions/` for
`CompareException`.

New assertions go in the folder for the type they assert on. Create a new folder only when adding
support for a genuinely new type family, and mirror it in the test project.

## Comparer Naming Conventions

The library follows a strict, symmetrical naming scheme. Follow it exactly — do not invent variations.

| Pattern | Role |
|---|---|
| `<Type>Comparer` | Assertions on a non-nullable value — `BoolComparer`, `GuidComparer`, `TimeSpanComparer` |
| `Not<Type>Comparer` | The negated form, exposed via the `Not` property — `NotBoolComparer` |
| `Nullable<Type>Comparer` | Assertions on `<Type>?` — `NullableGuidComparer`, `NullableEnumComparer` |
| `NotNullable<Type>Comparer` | Negated nullable form — `NotNullableGuidComparer` |
| `ComparerBase<TSubject, TComparer>` | Shared base for positive comparers |
| `NotComparerBase<TSubject, TComparer>` | Shared base for negated comparers |

- Every comparer takes its subject through a primary constructor and passes it to the base.
- Every positive comparer exposes a `Not` property returning the matching negated comparer.
- Assertion methods return the comparer (`this`) so calls chain.
- Every assertion method takes a trailing `string because = null` parameter and uses it in place of the
  generated message when supplied.

```csharp
public class BoolComparer(bool subject) : ComparerBase<bool, BoolComparer>(subject)
{
    public NotBoolComparer Not { get; } = new(subject);

    public BoolComparer BeTrue(string because = null)
    {
        if (!Subject)
        {
            CompareException.New(because ?? $"{Subject} should be True");
        }

        return this;
    }
}
```

New assertion entry points are added as `Should()` overloads in `ShouldExtensions.cs`.

## Assertion Failure Messages
Failure messages are plain English sentences built with string interpolation, describing what was expected:

```csharp
CompareException.New($"{Subject} should be {expected}");
CompareException.New($"{Subject} should not be {expected}");
```

- The message reads `<actual> should [not] <expectation>`.
- Never inline a raw message when the caller supplied `because` — `because` always wins: `because ?? $"..."`.
- Message text is asserted directly in the tests, so changing a message means changing its test.

## Interfaces
- All interfaces use the `I` prefix.
- Interface names describe a capability or action, not what something is.
- Default to narrow, single-purpose interfaces. One interface = one capability.
- This library is largely interface-free by design — it is a concrete, chainable assertion API. Do not add
  interfaces speculatively; add one only when there is genuinely more than one implementation.
