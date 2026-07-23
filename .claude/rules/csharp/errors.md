# Error Handling

## Error Handling
- Exceptions are for unplanned, unexpected failures only (hardware failures, network timeouts, corrupted state).
- Never throw an exception for a predictable outcome (validation failure, value out of range, known bad state).
- For known failure modes, return a value — an enum is preferred.
- Let exceptions bubble to the boundary where they can be meaningfully handled.
- Do not catch and swallow exceptions silently. The one exception: if a failure is genuinely non-actionable (e.g. a reflection comparison on an incompatible type), an empty catch with a `// ignored` comment is acceptable. This must be rare and deliberate — never use it to hide logic errors.

```csharp
// Preferred for known failures:
public enum ParseResult { Success, NotANumber, OutOfRange }

public ParseResult TryParse(string input)
{
    if (!IsNumeric(input))   return ParseResult.NotANumber;
    if (!IsInRange(input))   return ParseResult.OutOfRange;
    return ParseResult.Success;
}
```

## CompareException — The One Deliberate Exception
This library is the exception to the rule above, and it is intentional: a failed assertion **is** an exceptional outcome, because throwing is how a test framework learns the test failed.

- `CompareException` (`FatCat.Testing.Exceptions`) is the only exception type this library throws for assertion failures.
- Throw it via `CompareException.New(message)` — never `throw new CompareException(...)` at the call site.
- Every assertion failure path produces exactly one `CompareException` with a message describing the failure.
- Do not introduce additional exception types for assertion failures. A caller catching `CompareException` must catch every failure this library can produce.

```csharp
public BoolComparer Be(bool expected, string because = null)
{
    if (Subject != expected)
    {
        CompareException.New(because ?? $"{Subject} should be {expected}");
    }

    return this;
}
```

- Guard clauses for genuine misuse of the API (a null constraint, an unsupported enum value) throw the ordinary BCL exception — `ArgumentNullException`, `ArgumentOutOfRangeException` — not `CompareException`. `CompareException` means "the assertion failed", not "you called this wrong".

## Logging — None
There is no logging framework in this codebase and none should be added.

- Do NOT add Serilog, `Microsoft.Extensions.Logging`, or any other logging package.
- Do NOT write to `Console` from the library project. `OneOff` is the scratch console — console output belongs there and nowhere else.
- If you add a temporary trace while diagnosing something, remove it before committing.
