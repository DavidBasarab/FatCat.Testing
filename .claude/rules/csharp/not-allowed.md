# What NOT to Do

These are hard stops. Do not do any of the following under any circumstances.

## Type System
- Do NOT add nullable reference annotations (`string?`, `object?`) — `<Nullable>disable</Nullable>` is set, so they are meaningless noise. Nullable **value** types (`bool?`, `Guid?`) are used deliberately and are fine.
- Do NOT change `string because = null` parameters to `string?`
- Do NOT use records — use classes only

## Async
- Do NOT use `async void` — always return `Task` or `Task<T>`
- Do NOT use `ConfigureAwait(false)` — we do not use it
- Do NOT block on tasks with `.Result` or `.Wait()`
- Do NOT use `Task.Delay`, `Thread.Sleep`, or `new Thread(...)` in the library project
- Do NOT call `DateTime.Now` or `DateTime.UtcNow` in a test — construct explicit `DateTime` values so the test is deterministic

## Code Style
- Do NOT use expression-bodied members (`=>` syntax for methods or properties) — this applies to ALL access levels (public, private, protected, internal) and BOTH projects including tests
- Do NOT omit braces on an `if`, even for a single-statement body. CSharpier collapsing the body onto one line is not the same as dropping the braces
- Do NOT use query syntax LINQ (`from x in y where...`) — method chaining only
- Do NOT use string concatenation with `+` — use string interpolation. Write `$"{Subject} should be {expected}"`, never `Subject + " should be " + expected`. (No analyzer enforces this; it is caught by code review.)
- Do NOT abbreviate names — write them out fully
- Do NOT write comments explaining what code does — rename until obvious
- Do NOT use `new List<T>()`, `new T[0]`, or `new Dictionary<K, V>()` for empty or inline-populated collections — use collection expressions (`[]`)

## Architecture
- Do NOT add a DI container, service locator, or any dependency-injection infrastructure — this library has none and needs none
- Do NOT add a logging framework, a mapping library, a mocking library, or a database client
- Do NOT add a package reference to `FatCat.Testing` without a clear, stated need — it ships as a NuGet package and its dependency list is part of its contract
- Do NOT use `new` inside a class to instantiate a dependency — take it through the primary constructor
- Do NOT name a file after an interface — always name after the class
- Do NOT add an interface speculatively — add one only when there is genuinely more than one implementation
- Do NOT add abstractions or patterns that do not exist in the surrounding codebase
- Do NOT introduce over-engineering — match the abstraction level of the existing code
- Do NOT break the comparer naming symmetry (`<Type>Comparer` / `Not<Type>Comparer` / `Nullable<Type>Comparer` / `NotNullable<Type>Comparer`) — see `naming-and-structure.md`
- Do NOT return `ComparerBase` from an assertion method — return the concrete comparer so chaining keeps its type

## Errors
- Do NOT throw exceptions for predictable, known failure states in ordinary code — return an enum
- Do NOT introduce a second exception type for assertion failures — `CompareException` is the only one
- Do NOT write `throw new CompareException(...)` at a call site — use `CompareException.New(message)`
- Do NOT throw `CompareException` for API misuse (null argument, unhandled enum) — use the matching BCL exception
- Do NOT swallow exceptions silently
- Do NOT add logging or write to `Console` from the library project — `OneOff` is the only place console output belongs

## Testing
- Do NOT use underscores in test method names — PascalCase only
- Do NOT add FluentAssertions, FakeItEasy, or any mocking library to the test project
- Do NOT ship an assertion method without its full test set: `Good`, `Bad`, `BadShowsCorrectMessage`, `BadWithBecause`, the `Not` equivalents, and the nullable variants
- Do NOT use randomly generated data when the test asserts on a failure message built from that data
- Do NOT treat running `OneOff` as verification — run `dotnet test`

## Formatting
- Do NOT manually fight CSharpier formatting — it is the final authority
- Do NOT suppress `dotnet format` / analyzer warnings without a comment explaining why
- Do NOT finish a task that touched C# without running `dotnet csharpier .` as the final step
