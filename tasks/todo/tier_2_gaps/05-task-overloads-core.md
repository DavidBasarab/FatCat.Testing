# Phase 05 — `Task<T>` Overloads: Unwrap Helper + bool / string / Numerics

- **Work item:** `tier_2_gaps`
- **Gap:** **G7** (gaps.md §3, Tier 2)
- **Risk:** **HIGH — flagged for extra human review.** Two reasons: (1) it introduces the library's only
  blocking call, a deliberate documented exception to `.claude/rules/csharp/async.md`; (2) it adds a new
  family of public extension-method overloads, which is a public API contract change with
  overload-resolution consequences for future phases and for G1.
- **Depends on:** 01
- **Depended on by:** 06, 14
- **Precondition:** **ADR-2 in [00-overview.md](00-overview.md) has been confirmed by a human reviewer.**
  Do not start this phase on the strength of the plan alone.
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/ShouldExtensions.cs` is the library's single public entry point: 27 `Should()` extension
overloads, one per supported subject type, each returning that type's comparer. Excerpt:

```csharp
public static BoolComparer Should(this bool subject) { return new BoolComparer(subject); }
public static NumericComparer<int> Should(this int subject) { return new NumericComparer<int>(subject); }
public static NullableStringComparer Should(this string? subject) { return new NullableStringComparer(subject); }

public static EnumComparer<T> Should<T>(this T subject)
	where T : struct, Enum
{
	return new EnumComparer<T>(subject);
}
```

The file opens with `#nullable enable` (see OQ-2 — do not extend that region, do not remove it).

**The gap.** Both consuming repos ship sync-over-async shims so tests can assert on async calls without
`await` (gaps.md §3 G7):

- Toolkit — `ToolKit\Testing\TaskTestExtensions.cs`: `Should<T>(this Task<T>)` with `Be`,
  `BeEquivalentTo`, `BeTrue`, `BeFalse`
- Fog — `UserServiceModelAssertion.cs`: `Should(Task<T>)` and `Should(Task<List<T>>)`

ADR-2 decided FatCat absorbs these. This phase ships the unwrap helper and the overloads for the families
whose comparers exist and are simple: `bool`, `bool?`, `string`, the eleven `INumber<T>` types, `int?`,
`double`, `float`. Phase 06 finishes the rest.

Read before starting: `.claude/rules/csharp/async.md` **in full**, plus `naming-and-structure.md`,
`types.md`, `errors.md`, `testing.md`, `not-allowed.md`, and [00-overview.md](00-overview.md).

---

## Scope

**In scope**

- `FatCat.Testing/Tasks/TaskResultReader.cs` — one internal helper holding the library's only blocking call.
- `Should(this Task<…>)` overloads in `ShouldExtensions.cs` for: `bool`, `bool?`, `string`, `byte`,
  `decimal`, `int`, `int?`, `long`, `nint`, `nuint`, `sbyte`, `short`, `uint`, `ulong`, `ushort`, `double`,
  `float`.
- Tests for the helper's failure modes and a representative subset of the overloads.

**Out of scope**

- `char`, `DateTime`, `TimeSpan`, `Guid`, enums and their nullable forms — **phase 06**.
- Non-generic `Should(this Task)`. Only meaningful for exception assertions (G6, Tier 1) — OQ-3.
- A generic `Should<T>(this Task<T>)` for arbitrary reference types. It needs `ObjectComparer<T>` from G1
  and belongs to the G1 plan, not here (ADR-1).
- `Task<List<T>>` (Fog's shim). Needs collections (G4, Tier 1).
- `ValueTask<T>`. No call sites; not in gaps.md. Do not add it.

---

## Design

### The unwrap helper — the entire blocking surface, in one place

New file `FatCat.Testing/Tasks/TaskResultReader.cs`, namespace `FatCat.Testing.Tasks`:

```csharp
namespace FatCat.Testing.Tasks;

internal static class TaskResultReader
{
	// The fluent Should() surface is synchronous by design (.claude/rules/csharp/async.md): an
	// assertion entry point cannot be awaited, so unwrapping the task here is the one place the
	// library blocks. Nothing else in FatCat.Testing may block on a Task.
	internal static T Read<T>(Task<T> subject)
	{
		if (subject == null) { throw new ArgumentNullException(nameof(subject)); }

		return subject.GetAwaiter().GetResult();
	}
}
```

Why this shape:

- `GetAwaiter().GetResult()` rather than `.Result` — it rethrows the original exception instead of wrapping
  it in an `AggregateException`, so a test that awaits a throwing service sees the real exception.
- `ArgumentNullException` for a null task, not `CompareException` — a null task is API misuse, not a failed
  assertion (`errors.md`).
- `internal`, not `public`. It is not an extension point, and G5 has not defined one yet.
- Comment retained: this is one of the few places `errors.md` and `async.md` permit a comment, because it
  documents *why*, not *what*.

### The overloads

Add to `ShouldExtensions.cs`, each a one-liner delegating through the helper:

```csharp
public static BoolComparer Should(this Task<bool> subject) { return new BoolComparer(TaskResultReader.Read(subject)); }

public static NullableBoolComparer Should(this Task<bool?> subject) { return new NullableBoolComparer(TaskResultReader.Read(subject)); }

public static NumericComparer<int> Should(this Task<int> subject) { return new NumericComparer<int>(TaskResultReader.Read(subject)); }

public static NullableIntComparer Should(this Task<int?> subject) { return new NullableIntComparer(TaskResultReader.Read(subject)); }

public static NullableStringComparer Should(this Task<string> subject) { return new NullableStringComparer(TaskResultReader.Read(subject)); }

public static DoubleComparer Should(this Task<double> subject) { return new DoubleComparer(TaskResultReader.Read(subject)); }

public static FloatComparer Should(this Task<float> subject) { return new FloatComparer(TaskResultReader.Read(subject)); }

// …and the remaining INumber<T> types: byte, decimal, long, nint, nuint, sbyte, short, uint, ulong, ushort
```

Placement: keep `ShouldExtensions.cs` readable — put all `Task<…>` overloads together in one contiguous
block **after** the existing synchronous overloads, not interleaved. That block grows in phase 06.

`using FatCat.Testing.Tasks;` joins the existing using list.

### Overload resolution — the reason this phase is high risk

Verify each of these explicitly; they are assertions about the compiler, not hopes:

1. `Task<bool>` is a reference type, so the existing `Should<T>(this T) where T : struct, Enum` cannot
   apply. No ambiguity.
2. A concrete `Should(this Task<int>)` beats any future generic `Should<T>(this Task<T>)`. When G1 adds a
   generic reference-type entry point, these concrete overloads still win for these types — that is the
   intent, and the G1 plan must be told (see Hand-off).
3. Inside `#nullable enable`, `Task<string>` and `Task<string?>` are the same type for overload
   resolution. One overload covers both. Do **not** add a second.
4. `Task<int>` does not implicitly convert to `Task<int?>`; both overloads are needed and neither shadows
   the other.

Prove 1–4 with tests that compile, not by reasoning alone. A test that calls `.Should()` on each shape and
compiles *is* the proof.

### Known hazard — sync-over-async deadlock

Blocking on a task whose continuation needs the calling context can deadlock. This is inherent to
sync-over-async and is exactly the risk the consumers' existing shims already carry, so this phase does not
make anything worse — but it must be **documented, not hidden**:

- README notes that `Should(Task<T>)` blocks until the task completes, and that a task requiring the
  calling synchronization context to finish will deadlock. The recommended alternative is
  `(await service.Get()).Should()`.
- `MIGRATION.md` repeats the warning where the Task rows land.
- The phase report calls it out as a discovered risk.

Do **not** attempt a `Task.Run` / custom `TaskScheduler` workaround. The extension receives an
already-started task, so rescheduling cannot help, and the complexity would be unjustified.

---

## TDD Steps

Deterministic tasks only. `Task.FromResult`, `Task.FromException`, `Task.FromCanceled`, and
`Task.CompletedTask` are the vocabulary. **No `Task.Delay`, no `Thread.Sleep`, no `new Thread(...)`** —
banned by `not-allowed.md`, and they would make the suite non-deterministic.

1. **Red.** `Tests.FatCat.Testing/Tasks/TaskResultReaderTests.cs`, namespace
   `Tests.FatCat.Testing.Tasks`, deriving `BaseTest`. The helper is `internal`, so exercise it **through
   the public overloads** rather than adding `InternalsVisibleTo` — do not change the library's assembly
   attributes for a test. Facts:

   - `GoodReadReturnsCompletedValue` — `Task.FromResult(true).Should().BeTrue()`
   - `BadReadWhenTaskIsNull` — `Task<bool> task = null;` →
     `Assert.Throws<ArgumentNullException>(() => task.Should())`
   - `BadReadRethrowsOriginalException` — `Task.FromException<bool>(new InvalidOperationException("boom"))`
     → `Assert.Throws<InvalidOperationException>(() => task.Should())`, and assert the message is `boom`
     (proves it is not wrapped in an `AggregateException`)
   - `BadReadWhenTaskIsCanceled` — `Task.FromCanceled<bool>(new CancellationToken(true))` →
     `Assert.Throws<TaskCanceledException>(() => task.Should())`

   These four use raw `Assert.Throws`, not `RunCompareFailTest` — they are not assertion failures.

2. **Red.** `Tests.FatCat.Testing/Tasks/TaskBoolShouldTests.cs`:

   - `GoodBeTrue` — `Task.FromResult(true).Should().BeTrue()`
   - `BadBeTrue` — `RunCompareFailTest(() => Task.FromResult(false).Should().BeTrue())`
   - `BadBeTrueShowsCorrectMessage` → `"False should be True"` (confirm against `BoolComparer`'s actual
     message before pinning)
   - `BadBeTrueWithBecause`
   - `GoodNotBeTrue`
   - `GoodBeWhenNullableHasValue` — `Task.FromResult<bool?>(true).Should().Be(true)`
   - `BadBeWhenNullableIsNull` — `Task.FromResult<bool?>(null).Should().Be(true)` fails

3. **Red.** `Tests.FatCat.Testing/Tasks/TaskStringShouldTests.cs` — `Be`, its failure message, `Contain`,
   and the null case (`Task.FromResult<string>(null).Should().BeNull()`).

4. **Red.** `Tests.FatCat.Testing/Tasks/TaskNumericShouldTests.cs` — one fact per numeric overload proving
   it compiles and resolves to `NumericComparer<T>`: `Task.FromResult((byte)3).Should().Be(3)`,
   `Task.FromResult(3L).Should().Be(3L)`, … through all eleven, plus `int?`, `double`, `float`. Name them
   `GoodBeForByte`, `GoodBeForDecimal`, … one assertion each. This class is the overload-resolution proof
   from the Design section.

5. **Green.** Add `TaskResultReader.cs`, then the overloads.

6. **Refactor.** None. Every overload is one line; do not introduce a shared generic factory.

---

## Files

**Added**

- `FatCat.Testing/Tasks/TaskResultReader.cs`
- `Tests.FatCat.Testing/Tasks/TaskResultReaderTests.cs`
- `Tests.FatCat.Testing/Tasks/TaskBoolShouldTests.cs`
- `Tests.FatCat.Testing/Tasks/TaskStringShouldTests.cs`
- `Tests.FatCat.Testing/Tasks/TaskNumericShouldTests.cs`

**Changed**

- `FatCat.Testing/ShouldExtensions.cs`
- `README.md`
- `MIGRATION.md`

---

## Documentation Updates

**`README.md`** — new `### Task Results` subsection inside `## Assertion Catalog`, placed alphabetically
(after `### Strings`, before `### TimeSpans`). It must state:

- `Task<T>.Should()` unwraps the task and returns the comparer for `T`: `service.GetCount().Should().Be(3)`.
- It **blocks**. A task that needs the calling synchronization context to complete will deadlock; prefer
  `(await service.GetCount()).Should().Be(3)` where you can await.
- A null task throws `ArgumentNullException`; a faulted task rethrows its original exception; a canceled
  task throws `TaskCanceledException`. None of these is a `CompareException` — they are not assertion
  failures.
- Which `T`s are supported today, and that the rest arrive in phase 06.

**`README.md` → `## Coverage Status`** — add a `Task<T>` row.

**`MIGRATION.md` → `## 3. Mapping Table`** — append:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| Toolkit `TaskTestExtensions.Should<T>(Task<T>).Be(x)` | `.Should().Be(x)` (built in) | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskNumericShouldTests` |
| Toolkit `TaskTestExtensions.Should(Task<bool>).BeTrue()` | `.Should().BeTrue()` (built in) | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskBoolShouldTests` |
| Toolkit `TaskTestExtensions.Should(Task<bool>).BeFalse()` | `.Should().BeFalse()` (built in) | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskBoolShouldTests` |
| Fog `Should(Task<T>)` shim | `.Should()` (built in) | ⬜ partial — reference-type `T` needs G1 | `Tests.FatCat.Testing.Tasks.TaskStringShouldTests` |
| Fog `Should(Task<List<T>>)` shim | — | ⬜ pending G4 | — |

**`MIGRATION.md` → `## 7. Per-Repo Sequence`** — note that `ToolKit\Testing\TaskTestExtensions.cs` is
**deleted** during the Toolkit migration rather than ported, and repeat the blocking/deadlock warning.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Tests.FatCat.Testing.Tasks"
```

Then run the standards review on the uncommitted change and resolve every finding.

Manual check — grep the library for any blocking call other than the one in `TaskResultReader`:

```pwsh
Select-String -Path FatCat.Testing\**\*.cs -Pattern '\.Result\b|\.Wait\(\)|GetAwaiter\(\)'
```

Exactly one hit, in `TaskResultReader.cs`. Any other hit fails the phase.

---

## Definition of Done

- [ ] Tests written before implementation; red state observed and recorded.
- [ ] `TaskResultReader` is the **only** place in the library that blocks, verified by the grep above.
- [ ] Its comment explains why the blocking call exists and cites `async.md`.
- [ ] All 17 overloads added, grouped in one contiguous block in `ShouldExtensions.cs`.
- [ ] Null task → `ArgumentNullException`; faulted task → original exception unwrapped; canceled task →
      `TaskCanceledException`. All three proven by tests.
- [ ] Overload-resolution claims 1–4 proven by compiling tests.
- [ ] No `Task.Delay`, `Thread.Sleep`, `new Thread`, `async void`, or `ConfigureAwait` anywhere.
- [ ] No new `string?` parameters, no extension of the `#nullable enable` region (OQ-2).
- [ ] No new compiler warnings; namespaces match folders (`FatCat.Testing.Tasks`,
      `Tests.FatCat.Testing.Tasks`).
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] README `### Task Results` documents the blocking behaviour and the deadlock hazard.
- [ ] `MIGRATION.md` rows added, including the two honest `⬜ pending` rows.
- [ ] Deviation log records the `async.md` exception explicitly, with the reviewer who confirmed ADR-2.
- [ ] Deadlock hazard recorded as a discovered risk in the phase report.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/05-task-overloads-core.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-06-commit>   # if phase 06 has landed — it depends on this one
git revert --no-edit <phase-05-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

**Cascades to phase 06**, which adds overloads calling `TaskResultReader`. Revert 06 first or the build
breaks. Phase 14 (if landed) reverts before both.

No manual steps — nothing published, no data or config. If the package has been released with these
overloads, removing them is a breaking change for consumers; that is a release decision, not a revert.

---

## Hand-off

**Public surface added** — `namespace FatCat.Testing` (extension methods on `Task<T>`):

```csharp
Should(this Task<bool>)     -> BoolComparer
Should(this Task<bool?>)    -> NullableBoolComparer
Should(this Task<string>)   -> NullableStringComparer
Should(this Task<int>)      -> NumericComparer<int>
Should(this Task<int?>)     -> NullableIntComparer
Should(this Task<double>)   -> DoubleComparer
Should(this Task<float>)    -> FloatComparer
Should(this Task<byte|decimal|long|nint|nuint|sbyte|short|uint|ulong|ushort>) -> NumericComparer<T>
```

**Internal surface** — `namespace FatCat.Testing.Tasks`: `internal static T TaskResultReader.Read<T>(Task<T>)`.
Phase 06 calls it. It stays `internal` until G5 defines a public extension point.

**Contracts for later phases**

- Every new `Task<…>` overload goes in the same contiguous block and delegates through
  `TaskResultReader.Read`. No other file blocks.
- **For the G1 plan:** when a generic `Should<T>(this T subject)` for reference types lands, `Task<T>` is
  itself a reference type and would otherwise bind to it. The concrete overloads above win for their
  types, but a `Task<SomeDto>` would bind to the generic and produce a comparer over the *task*, not its
  result. G1 must either add `Should<T>(this Task<T>) -> ObjectComparer<T>` in the same commit or
  explicitly document the trap. **Carry this into the G1 plan's collision inventory.**
- The deadlock hazard is now a documented, public property of the library. Any future async surface must
  keep the same story.
