# Phase 03 — Exception assertions (G6)

- **Work item:** `final_gaps`
- **Gap:** **G6** (`remaining_gaps.md` §4 · `gaps.md` §3 Tier 1)
- **Risk:** **medium.** Small surface, 9 call sites — but it adds `Should(this Action)` and
  `Should(this Func<Task>)`, the first delegate overloads in the library, and it runs a `Task` synchronously,
  which `async.md` bans except as a documented exception.
- **Depends on:** 01
- **Depended on by:** 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

The library has no way to assert that code throws. `Should().Throw<T>()`, `ThrowAsync<T>()`, `NotThrow()`,
and `WithMessage()` have 9 call sites across the consuming repos and nothing substitutes for them —
`Assert.Throws` exists but is not the fluent surface this library ships.

`ShouldExtensions.cs` currently has 27 overloads and carries `#nullable enable` at the top of the file
(OQ-2). Adding overloads there means any `because` parameter you declare **in that file** must be written
`string?` or it raises CS8625, which the Definition of Done forbids. New comparer files must **not** carry
`#nullable enable`.

`.claude/rules/csharp/async.md` says the fluent surface stays synchronous — the comparer runs the delegate.
It also bans blocking on a `Task`. Both are true at once, and reconciling them is the design decision of
this phase.

Read before starting: [00-overview.md](00-overview.md), `.claude/rules/csharp/async.md`, `errors.md`,
`naming-and-structure.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** — `Should(this Action)`, `Should(this Func<Task>)`, an `Exceptions/` comparer pair, and
`Throw<T>`, `ThrowAsync<T>`, `NotThrow`, `NotThrowAsync`, `WithMessage`.

**Out of scope** — everything in G26's exception list, which is **phase 14**: `ThrowExactly<T>`,
`WithInnerException<T>`, `Where(predicate)`, `WithParameterName`, `NotThrowAfter`, `CompleteWithinAsync`,
`ThrowWithinAsync`, `WithResult`, and the `Awaiting`/`Invoking`/`Enumerating` wrappers. Do not add them here
— phase 14 is where they belong and it depends on the shape this phase establishes.

Also out of scope: `Should(this Func<T>)` for a value-returning delegate. No call site needs it and it would
collide with the generic overloads phase 06 adds.

---

## Design

New folder `FatCat.Testing/Exceptions/` **already exists** — it holds `CompareException.cs`. The comparers go
there, namespace `FatCat.Testing.Exceptions`.

### Files

| File | Type |
|---|---|
| `Exceptions/ActionComparer.cs` | `ActionComparer(Action subject) : ComparerBase<Action, ActionComparer>(subject)` |
| `Exceptions/NotActionComparer.cs` | `NotActionComparer(Action subject) : NotComparerBase<Action, NotActionComparer>(subject)` |
| `Exceptions/AsyncActionComparer.cs` | `AsyncActionComparer(Func<Task> subject) : ComparerBase<Func<Task>, AsyncActionComparer>(subject)` |
| `Exceptions/NotAsyncActionComparer.cs` | `NotAsyncActionComparer(Func<Task> subject) : NotComparerBase<...>(subject)` |
| `Exceptions/ThrownExceptionComparer.cs` | returned by `Throw<T>` so `WithMessage` can chain |

`ActionComparer` exposes `Not { get; } = new(subject);` per the standard scheme.

### The chaining problem — `WithMessage`

`Should().Throw<InvalidOperationException>().WithMessage("boom")` requires `Throw<T>` to return something
that knows about the caught exception. Returning `this` (the library's normal rule) loses it.

**Decision:** `Throw<T>` returns `ThrownExceptionComparer`, which holds the caught exception and exposes
`WithMessage(string expected, string because = null)` returning `this`. This is the one place in the library
where an assertion returns a different comparer, and it is deliberate — the subject genuinely changes from
"the delegate" to "the exception it threw". Document it in `README.md` so it reads as a decision rather than
an inconsistency.

`NotThrow` returns the comparer (`this`) as normal — there is nothing to drill into.

### Signatures

```csharp
// ActionComparer
public ThrownExceptionComparer Throw<TException>(string because = null) where TException : Exception
public ActionComparer NotThrow(string because = null)

// NotActionComparer
public NotActionComparer Throw<TException>(string because = null) where TException : Exception

// AsyncActionComparer
public ThrownExceptionComparer ThrowAsync<TException>(string because = null) where TException : Exception
public AsyncActionComparer NotThrowAsync(string because = null)

// NotAsyncActionComparer
public NotAsyncActionComparer ThrowAsync<TException>(string because = null) where TException : Exception

// ThrownExceptionComparer
public ThrownExceptionComparer WithMessage(string expected, string because = null)
```

`Throw<T>` succeeds when the delegate throws a `TException` **or a type derived from it** — matching
FluentAssertions, where `ThrowExactly<T>` (phase 14) is the exact-type form.

### Messages

| Case | Message |
|---|---|
| Nothing thrown | `should throw InvalidOperationException but no exception was thrown` |
| Wrong type thrown | `should throw InvalidOperationException but threw ArgumentException` |
| `NotThrow` and it threw | `should not throw but threw ArgumentException: boom` |
| `Not.Throw<T>` and it threw a `T` | `should not throw InvalidOperationException but did` |
| `WithMessage` mismatch | `exception message boom should be bang` |

There is no useful subject to interpolate — an `Action`'s `ToString()` is its type name — so these messages
deliberately start with `should`. Note that in `README.md`; it is the only family whose messages do not lead
with the subject.

`WithMessage` compares with **exact string equality**. FluentAssertions supports wildcards there; no call
site uses one. Documented in `MIGRATION.md` §5 as a behavioural difference.

### Running the async delegate — the `async.md` exception

`ThrowAsync<T>` must observe a `Task` from a synchronous method. `async.md` bans `.Result`, `.Wait()`, and
`.GetAwaiter().GetResult()`, and permits blocking only at a top-level synchronous entry point that cannot be
made async, isolated and commented.

**Decision:** the fluent surface stays synchronous (an assertion API that must be awaited is not this
library's shape, and `async.md` says so explicitly). The block is isolated in **exactly one private method**
in `AsyncActionComparer`, with a comment stating why:

```csharp
// The fluent assertion surface is synchronous by design (async.md). Observing the task here is the
// single, deliberate blocking call in the library; no other file may block.
private static Exception RunAndCaptureException(Func<Task> subject)
{
	try
	{
		subject().GetAwaiter().GetResult();

		return null;
	}
	catch (Exception exception)
	{
		return exception;
	}
}
```

`GetAwaiter().GetResult()` rather than `.Result` — it unwraps rather than wrapping in
`AggregateException`, so the caught type is the type the test author expects.

`NotAsyncActionComparer` calls the same helper. Do not write a second one.

**This makes `AsyncActionComparer.cs` the only file in the library permitted to block.** Say so in the
comment, and in the hand-off, so a later phase does not read it as precedent.

`tier_2_gaps/05-task-overloads-core.md` (a different plan) introduces `TaskResultReader.Read` for the same
reason. If that plan has already landed, **reuse it** rather than adding a second blocking site, and record
the reuse in the phase report. If it has not, this phase's helper stands alone and that plan reconciles.

---

## TDD Steps

Tests first. Red before green. All under `Tests.FatCat.Testing/Exceptions/`, namespace
`Tests.FatCat.Testing.Exceptions`, one class per assertion method.

1. **Red.** `ActionThrowTests.cs`:
   - `GoodThrow` — `Action action = () => throw new InvalidOperationException("boom");`
     then `action.Should().Throw<InvalidOperationException>();`
   - `GoodThrowMatchesDerivedType` — throwing `ArgumentNullException`, asserting `Throw<ArgumentException>()`
   - `BadThrow` — an action that does not throw
   - `BadThrowShowsCorrectMessage` →
     `"should throw InvalidOperationException but no exception was thrown"`
   - `BadThrowWhenWrongTypeShowsCorrectMessage` →
     `"should throw InvalidOperationException but threw ArgumentException"`
   - `BadThrowWithBecause` → `"custom because"`
   - `GoodNotThrow` / `BadNotThrow` / `BadNotThrowShowsCorrectMessage` / `BadNotThrowWithBecause`

2. **Red.** `ActionNotThrowTests.cs` — the `NotThrow()` method (distinct from `Not.Throw<T>()`; both exist
   and mean different things, and the tests must show that difference).

3. **Red.** `ActionThrowWithMessageTests.cs` — `GoodThrowWithMessage`, `BadThrowWithMessage`,
   `BadThrowWithMessageShowsCorrectMessage`, `BadThrowWithMessageWithBecause`.

4. **Red.** `AsyncActionThrowAsyncTests.cs` and `AsyncActionNotThrowAsyncTests.cs` — the same sets against
   `Func<Task>`. Include:
   - `GoodThrowAsyncFromAsyncMethod` — a delegate that awaits before throwing, so the exception surfaces
     from the awaited continuation rather than synchronously
   - `GoodThrowAsyncUnwrapsAggregateException` — proves `GetAwaiter().GetResult()` gives the inner type
   Use a real `Task.Yield()` or a completed task; **no `Task.Delay`, no `Thread.Sleep`** (`not-allowed.md`).

5. **Green.** Implement the five comparer files.

6. **Red.** `Tests.FatCat.Testing/Exceptions/ExceptionOverloadResolutionTests.cs` — proves the delegate
   overloads bind as intended and nothing existing moved:
   - `Action` binds to `ActionComparer`
   - `Func<Task>` binds to `AsyncActionComparer`
   - a lambda assigned to `Action` first, then `.Should()` — **a bare lambda has no type and cannot be an
     extension-method receiver**, so every test must declare the delegate variable first. Pin that in a
     comment; it is the first thing a consumer will trip over and it belongs in `README.md`.

---

## Files

**Added**

- `FatCat.Testing/Exceptions/ActionComparer.cs`
- `FatCat.Testing/Exceptions/NotActionComparer.cs`
- `FatCat.Testing/Exceptions/AsyncActionComparer.cs`
- `FatCat.Testing/Exceptions/NotAsyncActionComparer.cs`
- `FatCat.Testing/Exceptions/ThrownExceptionComparer.cs`
- `Tests.FatCat.Testing/Exceptions/ActionThrowTests.cs`
- `Tests.FatCat.Testing/Exceptions/ActionNotThrowTests.cs`
- `Tests.FatCat.Testing/Exceptions/ActionThrowWithMessageTests.cs`
- `Tests.FatCat.Testing/Exceptions/AsyncActionThrowAsyncTests.cs`
- `Tests.FatCat.Testing/Exceptions/AsyncActionNotThrowAsyncTests.cs`
- `Tests.FatCat.Testing/Exceptions/ExceptionOverloadResolutionTests.cs`

**Changed**

- `FatCat.Testing/ShouldExtensions.cs` — two overloads
- `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `### Exceptions`** — replace the phase-03 placeholder. Table of the five methods, plus three
notes that will otherwise be asked as questions:

1. The delegate must be a typed variable — `Action action = () => ...; action.Should().Throw<T>();`. A bare
   lambda cannot receive an extension method.
2. `Throw<T>` returns a `ThrownExceptionComparer`, not the action comparer, so `WithMessage` can chain. It
   is the only assertion in the library that changes comparer type.
3. `Throw<T>` matches derived exception types; `ThrowExactly<T>` (phase 14) is the exact form.

**`MIGRATION.md` → `## 3. Mapping Table`** — flip these rows to `✅ supported`:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().Throw<T>()` | `.Should().Throw<T>()` | `Tests.FatCat.Testing.Exceptions.ActionThrowTests` |
| `.Should().ThrowAsync<T>()` | `.Should().ThrowAsync<T>()` | `Tests.FatCat.Testing.Exceptions.AsyncActionThrowAsyncTests` |
| `.Should().NotThrow()` | `.Should().NotThrow()` | `Tests.FatCat.Testing.Exceptions.ActionNotThrowTests` |
| `.Should().Throw<T>().WithMessage(m)` | same | `Tests.FatCat.Testing.Exceptions.ActionThrowWithMessageTests` |

**`MIGRATION.md` → `## 5. Behavioural Differences`** — add: `WithMessage` compares exactly; FluentAssertions
supports `*` wildcards. Rewrite a wildcard assertion as `Throw<T>().Where(e => e.Message.Contains("..."))`
once phase 14 ships `Where`, or assert on the message directly until then.

**`MIGRATION.md` → `## 4. Type Coverage`** — `Action` and `Func<Task>` move from pending to supported.

---

## Verification

```pwsh
. $PROFILE
Set-Location C:\Code\FatCat.Testing
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
dotnet format style Fatcat.Testing.sln
dotnet format analyzers Fatcat.Testing.sln
dotnet csharpier .
dotnet build Fatcat.Testing.sln
```

Targeted:

```pwsh
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Exceptions"
```

Then run the standards review on the uncommitted change and resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; the red state was observed and recorded.
- [ ] Five comparer files; `ActionComparer` and `AsyncActionComparer` each expose `Not`.
- [ ] `Throw<T>`, `ThrowAsync<T>`, `NotThrow`, `NotThrowAsync`, `WithMessage` all implemented, each with the
      full `Good` / `Bad` / `BadShowsCorrectMessage` / `BadWithBecause` set and its `Not` equivalent.
- [ ] `Throw<T>` matches derived exception types, proven by a test.
- [ ] Exactly **one** blocking call in the library, in `AsyncActionComparer`, commented with why. No other
      file blocks. (Or: `tier_2_gaps` phase 05's `TaskResultReader` reused, and the reuse recorded.)
- [ ] No `Task.Delay`, no `Thread.Sleep`, no `async void`, no `ConfigureAwait(false)`.
- [ ] Overload-resolution test class proves `Action` and `Func<Task>` bind correctly and nothing else moved.
- [ ] No new compiler warnings. `ShouldExtensions.cs` additions respect its `#nullable enable` region
      (OQ-2); new comparer files do not carry `#nullable enable`.
- [ ] No banned patterns: no expression bodies, no braceless `if`, no `string?` outside the existing region,
      no underscores in test names, no `+` concatenation.
- [ ] `dotnet test` green; total is baseline + the new facts.
- [ ] `dotnet format style` / `analyzers` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` → `### Exceptions` and the four `MIGRATION.md` rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit, message referencing `tasks/todo/final_gaps/03-exception-assertions.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-03-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Removes two `Should()` overloads. Nothing bound to them before this phase, so nothing else breaks. **Phase
14 extends these comparers** — revert it first if it has landed.

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing;
ActionComparer Should(this Action subject)
AsyncActionComparer Should(this Func<Task> subject)

namespace FatCat.Testing.Exceptions;
ActionComparer.Throw<TException>(string because = null)          // returns ThrownExceptionComparer
ActionComparer.NotThrow(string because = null)
ActionComparer.Not                                                // NotActionComparer
NotActionComparer.Throw<TException>(string because = null)
AsyncActionComparer.ThrowAsync<TException>(string because = null) // returns ThrownExceptionComparer
AsyncActionComparer.NotThrowAsync(string because = null)
AsyncActionComparer.Not                                           // NotAsyncActionComparer
ThrownExceptionComparer.WithMessage(string expected, string because = null)
```

For **phase 14** (G26 exceptions): `ThrownExceptionComparer` is the extension point for `WithInnerException`,
`Where`, and `WithParameterName` — all drill into the already-captured exception, so they are methods on that
type and return `this`. `ThrowExactly<T>` is a method on the *action* comparers alongside `Throw<T>` and
returns `ThrownExceptionComparer` too. The single blocking helper is where every new async assertion runs its
delegate; do not add a second.

For **phase 06** (objects): `Action` and `Func<Task>` are reference types, so `Should<T>(this T) where T :
class` would otherwise capture them. These concrete overloads beat it — a constructed type beats a bare type
parameter. Phase 06's overload-resolution suite must include both, and this phase's
`ExceptionOverloadResolutionTests` is where those cases already live.

**The library's one blocking call is in `AsyncActionComparer`.** It is a documented exception to `async.md`,
not a precedent. No other file may block.
