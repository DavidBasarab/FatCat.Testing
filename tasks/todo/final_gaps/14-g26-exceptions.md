# Phase 14 — G26 completeness: Exceptions

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **medium.** Additive on phase 03's exception comparers, but the async members reuse the library's
  one deliberate blocking site, and `Where(predicate)` / `WithInnerException` drill into a captured
  exception — the same chaining shape phase 03 established.
- **Depends on:** 03 (exception comparers)
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Phase 03 shipped `Should(this Action)`, `Should(this Func<Task>)`, `ActionComparer`, `AsyncActionComparer`,
their `Not` twins, and `ThrownExceptionComparer` (returned by `Throw<T>` so `WithMessage` can chain). It also
established the library's **single blocking site** in `AsyncActionComparer` — the documented exception to
`async.md`. This phase adds the rest of the FluentAssertions exception surface (`gaps.md` §6.3 Exceptions).

**Read `FatCat.Testing/Exceptions/ActionComparer.cs`, `AsyncActionComparer.cs`, and
`ThrownExceptionComparer.cs` before writing.** Reuse the blocking helper — do not add a second blocking site.
Any new async member runs its delegate through the existing helper.

Read before starting: [00-overview.md](00-overview.md), phase 03's hand-off (the chaining shape and the
blocking-site rule), `.claude/rules/csharp/async.md`, `errors.md`, `naming-and-structure.md`, `testing.md`,
`not-allowed.md`.

---

## Scope

**In scope** (`gaps.md` §6.3 Exceptions, beyond phase 03's `Throw`/`ThrowAsync`/`NotThrow`/`WithMessage`):

- `ThrowExactly<T>()` — the exact type, not derived (contrast `Throw<T>`, which matches derived). On the
  action comparers, returns `ThrownExceptionComparer`.
- On `ThrownExceptionComparer` (drilling into the captured exception):
  - `WithInnerException<TInner>()` — returns a `ThrownExceptionComparer` scoped to the inner exception so it
    chains further
  - `WithInnerExceptionExactly<TInner>()`
  - `Where(Func<TException, bool> predicate)` — but the captured exception is stored as `Exception`; expose
    `Where(Func<Exception, bool>)` and document the cast, or make `ThrownExceptionComparer` generic — see
    Design
  - `WithParameterName(string)` — for `ArgumentException`-derived; reads `.ParamName`
- `NotThrowAfter` / the async completion members — **decide by value**, see Design. At minimum ship
  `CompleteWithinAsync(TimeSpan)` and `NotThrowAsync` (03 shipped `NotThrowAsync`; confirm).

**Out of scope**

- The `Awaiting` / `Invoking` / `Enumerating` wrapper surface — these are FluentAssertions' way of adapting a
  subject into a delegate; FatCat's delegate overloads (phase 03) already cover the need. Note as a
  deliberate omission.
- `WithResult` (asserting an async result value) — no consumer, marginal; omit and note.
- Anything time-based that would need `Task.Delay`/`Thread.Sleep` in the **library** — banned (`async.md`).
  `CompleteWithinAsync` measures elapsed time of the awaited task; it must not itself sleep. If it cannot be
  implemented without a banned primitive, **omit it and record why** rather than breaking the rule.

---

## Design

### `ThrowExactly<T>`

On `ActionComparer` and `AsyncActionComparer`: fails if nothing thrown, or the thrown type is not **exactly**
`T` (a derived type fails, unlike `Throw<T>`). Returns `ThrownExceptionComparer`. Message: `should throw
exactly ArgumentException but threw ArgumentNullException`.

### Drilling into the captured exception

`ThrownExceptionComparer` currently holds the caught `Exception`. The new members need the caught type. Two
options — **choose and record**:

- **A (recommended):** keep `ThrownExceptionComparer` non-generic, holding `Exception`. `Where` takes
  `Func<Exception, bool>`; `WithInnerException<TInner>` checks `caught.InnerException is TInner` and returns
  a new `ThrownExceptionComparer` wrapping that inner exception. `WithParameterName` casts to
  `ArgumentException` and reads `ParamName`, throwing `CompareException` with a clear message if the caught
  type is not an `ArgumentException`. Simple, matches phase 03's shape.
- **B:** make `Throw<T>` return `ThrownExceptionComparer<T>` so `Where(Func<T, bool>)` is strongly typed.
  Cleaner call sites, but changes phase 03's return type — a public-surface change to a shipped phase. Only do
  this if OQ discussion in the report justifies it; default to A.

Default to **A**. Record the choice.

`Where(predicate)` fails when the predicate returns false: `thrown exception should match the predicate but
did not`. `because` makes it specific.

`WithInnerException<TInner>` fails when there is no inner exception or it is not a `TInner`: `thrown
InvalidOperationException should have inner exception ArgumentException but had none` / `but had
FormatException`.

### Message consistency

Phase 03's exception messages lead with `should` (there is no useful subject). Match that — every new message
here leads with `should` or `thrown`, never with an interpolated `Action`.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Exceptions/`, one class per method. Use real
exceptions with real inner exceptions; **no `Task.Delay`, no `Thread.Sleep`** — construct tasks with
`Task.FromException`, `Task.CompletedTask`, or a completed `Task.Yield()`-based delegate.

1. `ActionThrowExactlyTests.cs` — passes on exact type, **fails on a derived type** (the distinguishing test
   vs `Throw<T>`), full set.
2. `ThrownExceptionWithInnerExceptionTests.cs` — throw an outer wrapping an inner; assert the inner type;
   include no-inner and wrong-inner failure cases; assert it chains (`.WithInnerException<X>().WithMessage`).
3. `ThrownExceptionWhereTests.cs` — predicate pass/fail, `because`.
4. `ThrownExceptionWithParameterNameTests.cs` — `ArgumentNullException("param")`; pass, wrong-name fail, and
   the not-an-ArgumentException misuse case.
5. `AsyncActionThrowExactlyAsyncTests.cs` — the async counterpart, via the existing blocking helper.
6. `CompleteWithinAsync` tests **only if** it ships without a banned primitive.
7. **Green.** Implement across the exception comparer files, reusing the single blocking helper.
8. Run the whole suite — phase 03's tests untouched.

---

## Files

**Changed**

- `FatCat.Testing/Exceptions/ActionComparer.cs`, `AsyncActionComparer.cs`, `NotActionComparer.cs`,
  `NotAsyncActionComparer.cs`, `ThrownExceptionComparer.cs`
- `README.md`, `MIGRATION.md`

**Added**

- One test class per method under `Tests.FatCat.Testing/Exceptions/`

---

## Documentation Updates

**`README.md` → `### Exceptions`** — append `ThrowExactly`, `WithInnerException`, `Where`,
`WithParameterName`, and any async completion member shipped. State the `Throw` (derived) vs `ThrowExactly`
(exact) distinction plainly. List the deliberate omissions (`Awaiting`/`Invoking`/`Enumerating`, `WithResult`,
and `CompleteWithinAsync` if omitted) so the family does not read as fully complete.

**`MIGRATION.md` → `## 3. Mapping Table`** — a `✅ supported` coverage row per method naming its test class.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Exceptions"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `ThrowExactly<T>` passes on exact and **fails on derived** — pinned.
- [ ] `WithInnerException<TInner>`, `Where`, `WithParameterName` implemented on `ThrownExceptionComparer`
      with full sets; chaining proven.
- [ ] Async members reuse the **single** blocking helper — no second blocking site added.
- [ ] No `Task.Delay`, no `Thread.Sleep`, no `async void`, no `ConfigureAwait(false)`.
- [ ] The A/B choice for the captured-exception type recorded in the report.
- [ ] Deliberate omissions listed in the report and README.
- [ ] No new warnings; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION coverage rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/14-g26-exceptions.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-14-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (revert phase 20 first if landed).

---

## Hand-off

Public surface added on the exception comparers per the scope. The single blocking site in
`AsyncActionComparer` remains the only one. G26 exceptions closed except the listed omissions. Phase 20
counts it toward the audit.
