# Types

## var
- Use `var` as the default for local variable declarations.
- Small methods and good naming make the type obvious from context.
- Use explicit types only when the type is not clear from the right-hand side.

## Nullable Reference Types
Both projects build with `<Nullable>disable</Nullable>`. Nullable reference annotations are not in play.

- Do NOT add `?` to reference types (`string?`, `object?`) — it is meaningless with the feature disabled and adds noise.
- `?` on **value** types is a real nullable and is used heavily — `bool?`, `Guid?`, `TimeSpan?`, `DateTime?` — since the library ships a full set of `Nullable*Comparer` types for them.
- Optional `because` parameters are declared `string because = null`. That is the established form; do not change it to `string?`.

## Collection Initialization
- Use collection expressions (`[]`) to initialize collections. Do not use `new List<T>()`, `new T[0]`, or `new Dictionary<K, V>()` when an empty or inline-populated collection is needed.
- The target type drives the actual collection — `List<string> Names { get; set; } = [];` produces an empty list, just like `new List<string>()`, but is shorter and consistent.

```csharp
// Correct — collection expressions
public List<string> Names { get; set; } = [];
public string[] Values { get; set; } = [];

// Wrong — explicit constructor calls
public List<string> Names { get; set; } = new List<string>();
public string[] Values { get; set; } = new string[0];
```

This applies to property initializers, field initializers, local variables, and method arguments. The exception is when you need a specific concrete type that the target cannot infer (e.g. assigning to `IEnumerable<T>` and needing a `HashSet<T>` specifically) — in that case, name the type explicitly.

## Thread-Safe Collections
- Use `ConcurrentDictionary<TKey, TValue>` for shared mutable state that is accessed across threads.
- Never use a plain `Dictionary` with manual locking for this purpose.

## Lazy Initialization
- The default lazy pattern uses the C# `field` keyword with null-coalescing assignment in a property getter. This is preferred over `Lazy<T>` for ordinary deferred initialization:

```csharp
public IReadOnlyList<string> AllValues
{
    get { return field ??= LoadValues(); }
}
```

- Use `Lazy<T>` only when you genuinely need its thread-safety guarantees (e.g. a value that may be initialized concurrently and must run the factory exactly once). When you do, use the factory constructor overload: `new Lazy<T>(() => ...)`.

## Records — BANNED
- Records are banned. Use classes only.

## Access Modifiers
- Public is the default. Do not add access modifiers to restrict visibility unless there is a specific reason.
- `dotnet format` (via `.editorconfig`) enforces readonly and auto-properties — follow its guidance.

## Primary Constructors
- Use primary constructors (C# 12+) as the standard form for all new code. Do not write explicit constructor bodies with `this.field = param` assignments.
- Comparers take their subject via the primary constructor and forward it to the base type.

```csharp
// Correct — primary constructor forwarding to the base
public class GuidComparer(Guid subject) : ComparerBase<Guid, GuidComparer>(subject)
{
    public NotGuidComparer Not { get; } = new(subject);
}

// Wrong — traditional explicit constructor
public class GuidComparer : ComparerBase<Guid, GuidComparer>
{
    public GuidComparer(Guid subject) : base(subject) { }
}
```

## Extension Methods
The public entry point of the library is a set of `Should()` extension methods in `ShouldExtensions.cs`, each returning the comparer for the subject type.

- Add new entry points there rather than creating a second extensions class.
- One overload per subject type, including a separate overload for the nullable value type where one exists.
- Extension methods are the only place in this library where a null subject is expected and handled by design.

## LINQ
- Use LINQ for querying and transforming collections. Prefer it over imperative loops.
- Always use method chaining syntax. Never use query syntax (`from x in y where...`).
- CSharpier handles formatting — write readable code and let it format.

## Dependencies — Keep Them Out
`FatCat.Testing` ships as a NuGet package and deliberately carries exactly one package reference: `xunit.assert`.

- Do NOT add a package reference to the library project to solve a problem that plain BCL code can solve.
- Do NOT introduce a DI container, a logging framework, a mapping library, or a mocking library into the library project.
- The test project may reference test-only packages (`xunit`, `FatCat.Fakes`) — that is where test infrastructure belongs.

## Generics
The comparer hierarchy is generic over both the subject type and the concrete comparer (`ComparerBase<TSubject, TComparer>`) so that chained calls return the derived type rather than the base. Preserve that shape when adding a new comparer — do not return `ComparerBase` from an assertion method.

## Global Usings
Each project may have a single `GlobalUsings.cs` at the project root declaring `global using` directives used throughout the project. Keep it short.

```csharp
// Tests.FatCat.Testing/GlobalUsings.cs
global using FatCat.Testing;
global using Xunit;
```

`ImplicitUsings` is enabled in both projects — do not re-declare `System`, `System.Linq`, or other implicit namespaces.

## C# 14 Features
- The `field` keyword is accepted in property getters for backing-field initialization (`field ??= ...`) and in computed properties that need to cache.
- Extension blocks (`extension(TargetType target) { ... }`) are accepted for grouping multiple extension methods on the same type.
