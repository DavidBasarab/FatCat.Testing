# Async / Await

This library is synchronous by design — assertions execute inline and throw inline. There is currently no
async code in `FatCat.Testing`, and none should be added without a genuine need. These rules apply if and
when async code is introduced.

- Async all the way up. If anything in a call chain is async, the whole chain is async.
- Never block on a Task with `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`.
- `async void` is banned. All async methods return `Task` or `Task<T>`.
- Do NOT use `ConfigureAwait(false)`. We do not use it in this codebase.
- Do NOT use `Task.Delay`, `Thread.Sleep`, or `new Thread(...)` in the library project.

## Blocking calls — very limited exceptions only
Blocking on a Task is only acceptable in these specific scenarios:
- Top-level synchronous entry points where converting to async is genuinely infeasible
- Interfacing with a third-party API that exposes only synchronous entry points and cannot be changed
- Properties (which cannot be awaited) — isolate and keep minimal

When a blocking call is unavoidable: isolate it, document in a comment why async was not possible, and consider `Task.Run` with caution.

## Asserting On Async Code
An assertion that needs to observe a `Task` (e.g. "this should throw") takes the delegate and runs it —
it does not make the assertion API itself async. Keep the fluent `Should()` surface synchronous.
