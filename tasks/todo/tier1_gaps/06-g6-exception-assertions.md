# Phase 06 — G6: exception assertions

- **Gap:** G6 (`gaps.md` §3). 8 call sites (`Throw<T>`, `ThrowAsync<T>`), but nothing substitutes for it.
- **Depends on:** nothing (fully independent — `gaps.md` §4 lists G6 as an independent warm-up track). Only
  needs Phase 01 if its failure messages format non-trivial values; they format exception type names, so
  **01 is not a hard dependency** — 06 may run any time, including first / in parallel with 01–05.
- **Depended on by:** Phase 07 (migration guide adds the exception mappings).
- **Risk:** **medium** — adds new `Should` entry points for delegates (public API) but small, self-contained
  surface with no overload collision against existing value/reference/collection overloads.

## Context (complete handoff)

There is no way to assert that a delegate throws. Per `async.md`, **the fluent surface stays synchronous** —
the comparer *runs* the delegate; the `Should()` API itself does not become async. New entry points take the
delegate:

- `Should(this Action subject)` → `ActionComparer` (for sync throwing code)
- `Should(this Func<Task> subject)` → `AsyncActionComparer` (for async throwing code — the comparer blocks
  on the task internally in the one place a blocking call is permitted, per `async.md`'s limited-exception
  rule; isolate and comment why)

These do not collide with existing overloads: `Action`/`Func<Task>` are distinct delegate types with no
more-specific competing overload.

## Deliverable

New folder `FatCat.Testing/Exceptions/` already exists (`CompareException` lives there); add the comparers in
a sibling folder that matches the namespace — put them under `FatCat.Testing/Delegates/` (new type family →
new folder, `naming-and-structure.md`) to avoid mixing with the exception type. Confirm the folder/namespace
choice in the phase; `Delegates/` is the proposal.

- `ActionComparer` : `ComparerBase<Action, ActionComparer>`, `Not` → `NotActionComparer`.
- `AsyncActionComparer` : `ComparerBase<Func<Task>, AsyncActionComparer>`, `Not` → `NotAsyncActionComparer`.

Assertions:

| Method | Passes when | Message (proposed) |
|---|---|---|
| `Throw<TException>(because)` | running the delegate throws a `TException` (or derived) | `Expected {typeof(TException).Name} but {Format(actual)} was thrown` / `...but no exception was thrown` |
| `NotThrow(because)` | running the delegate throws nothing | `Expected no exception but {Format(actual)} was thrown` |
| `WithMessage(string expected, because)` | the thrown exception's message matches (chained after `Throw`) | returns `this` so `Throw<T>().WithMessage(...)` chains |

- `Throw<T>` returns the comparer so `WithMessage` can chain (mirror the existing chainable style — return
  `this`, keep the caught exception on the comparer for `WithMessage` to inspect). Keep it minimal: no
  `ThrowExactly`, `WithInnerException`, `Where`, `WithParameterName`, async-completion helpers — those are
  §6.3 (out of scope).
- `AsyncActionComparer.Throw<T>` runs `subject().GetAwaiter().GetResult()` inside a try/catch — the **one**
  permitted blocking call (`async.md` limited exceptions: top-level sync entry point interfacing an
  unavoidably-deferred delegate). Isolate it in a single private method with a comment stating why async is
  not possible here (the `Should()` surface is synchronous by design).
- `Not` forms: `Not.Throw<T>()` = "should not throw a T". Provide what the inventory needs.

## TDD — tests first

`Tests.FatCat.Testing/Delegates/`. One class per assertion method, full
`Good/Bad/BadShowsCorrectMessage/BadWithBecause` + `Not` set, no underscores:

- `ActionThrowTests`, `ActionNotThrowTests`, `ActionThrowWithMessageTests`
- `AsyncActionThrowTests`, `AsyncActionNotThrowTests`
- Construct delegates inline: `Action act = () => throw new InvalidOperationException("boom");` and assert
  `act.Should().Throw<InvalidOperationException>()`. For the async set,
  `Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));`.
- Migration-proof: `act.Should().Throw<T>()`, `asyncAct.Should().Throw<T>()` (FluentAssertions
  `ThrowAsync<T>` maps to the sync-surface `Should(Func<Task>).Throw<T>`).

## Migration obligation

Append to `MIGRATION.md`: `Should().Throw<T>()` (unchanged shape), `ThrowAsync<T>()` →
`Should(Func<Task>).Throw<T>()`, `NotThrow()` → `Not.Throw()` (or `NotThrow()` — decide and record; prefer
`Not.Throw()` for ADR-003 consistency), `WithMessage`. Note the deferred exception methods
(`ThrowExactly`, `WithInnerException`, …) in the known-unsupported list (finalised Phase 07).

## Verification

`dotnet build`, `dotnet test`, `dotnet format style --verify-no-changes`, `dotnet csharpier .` from repo root.

## Definition of Done

- [ ] `Should(this Action)` and `Should(this Func<Task>)` entry points; `ActionComparer` /
      `AsyncActionComparer` (+ `Not` forms) with `Throw<T>`, `NotThrow`, `WithMessage`, full test set.
- [ ] Async path blocks in exactly one isolated, commented place; no `async void`, no `ConfigureAwait`,
      no `Task.Delay`/`Thread.Sleep` (`async.md` / `not-allowed.md`).
- [ ] Fluent surface stays synchronous.
- [ ] `MIGRATION.md` appended with G6 rows.
- [ ] All `00-overview.md` DoD gates met; one commit `[tier1_gaps 06] G6 exception assertions`.

## Rollback Procedure

`git revert <phase-06-commit>`. No dependents except Phase 07's doc rows (remove them if orphaned). Fully
independent — reverting 06 does not cascade to any other phase.

## Hand-off (contract exposed to later phases)

- `ShouldExtensions.Should(this Action) : ActionComparer` and `Should(this Func<Task>) : AsyncActionComparer`
  — the delegate entry points, with `Throw<T>` / `NotThrow` / `WithMessage`. Phase 07 maps FluentAssertions
  `Throw`/`ThrowAsync` to these.
