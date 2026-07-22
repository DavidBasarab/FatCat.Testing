# Phase 12 — `Stream` Family

- **Work item:** `tier_2_gaps`
- **Gap:** **G10** (missing type families) — also closes **G19** (gaps.md §6.2)
- **Risk:** **medium.** Reference-type entry point sharing G1's overload space, plus the only family whose
  subject carries **mutable state** — reading `Length` or `Position` can throw on a disposed or
  non-seekable stream, and assertions must never consume the stream they inspect.
- **Depends on:** 01
- **Depended on by:** 14
- **Precondition:** **OQ-5 answered** (`HaveBufferSize` in or out — the proposal below is out). See
  [00-overview.md](00-overview.md).
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Reference-type families ship **two** comparers — `<Type>Comparer` and `Not<Type>Comparer` (ADR-7 in
[00-overview.md](00-overview.md)). If phases 10 (`Uris/`) or 11 (`Types/`) have landed, open one as the
template: private `SubjectDisplay` rendering `"null"`, null subject fails everything except `BeNull`,
`ArgumentException` for genuine API misuse, `CompareException` for failed assertions. If neither has
landed, use `FatCat.Testing/Strings/NullableStringComparer.cs` for the null-handling shape only — do not
copy its `Nullable` prefix or its `#nullable enable` region (OQ-2).

gaps.md G19 defines the target surface: `BeReadable`, `BeWritable`, `BeSeekable`, `BeReadOnly`,
`BeWriteOnly`, `HaveLength`, `HavePosition`, `HaveBufferSize`, plus the negated forms. It rates the family
"Cheap, self-contained". No consuming-repo call site needs it (gaps.md A3).

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`errors.md`, `not-allowed.md`, and [00-overview.md](00-overview.md).

---

## Scope

**In scope** — `Streams/` folder with `StreamComparer` + `NotStreamComparer`, one `ShouldExtensions`
overload, full test set, doc updates.

**Out of scope**

- `HaveBufferSize` — it only applies to `BufferedStream` and would need a type check plus a cast for a
  property no other stream exposes. Excluded by proposal; OQ-5 confirms.
- Content assertions (`HaveContent`, `BeEquivalentTo` over bytes). They would have to read the stream,
  which mutates `Position` — a class of assertion this phase deliberately does not open.
- `Should(this Task<Stream>)`; any `NullableStreamComparer`.
- Async stream APIs. The fluent surface stays synchronous (`async.md`).

---

## Design

**Folder and namespace** — `FatCat.Testing/Streams/`, namespace `FatCat.Testing.Streams`; tests in
`Tests.FatCat.Testing/Streams/`, namespace `Tests.FatCat.Testing.Streams` (ADR-10).

```csharp
public class StreamComparer(Stream subject) : ComparerBase<Stream, StreamComparer>(subject)
{
	public NotStreamComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get { return Subject == null ? "null" : Subject.GetType().Name; }
	}
	…
}
```

`SubjectDisplay` renders the **type name** (`MemoryStream`, `FileStream`), because `Stream.ToString()`
returns the type name anyway and a message reading `System.IO.MemoryStream should be readable` is noise.
Messages therefore read `MemoryStream should be readable`.

**Assertion surface**

| Assertion | Fails when | Message |
|---|---|---|
| `BeNull()` | `Subject != null` | `{SubjectDisplay} should be null` |
| `HaveValue()` | `Subject == null` | `subject should have a value` |
| `BeReadable()` | null, or `!Subject.CanRead` | `{SubjectDisplay} should be readable` |
| `BeWritable()` | null, or `!Subject.CanWrite` | `{SubjectDisplay} should be writable` |
| `BeSeekable()` | null, or `!Subject.CanSeek` | `{SubjectDisplay} should be seekable` |
| `BeReadOnly()` | null, or not (`CanRead && !CanWrite`) | `{SubjectDisplay} should be read only` |
| `BeWriteOnly()` | null, or not (`CanWrite && !CanRead`) | `{SubjectDisplay} should be write only` |
| `HaveLength(expected)` | null, not seekable, or `Length != expected` | `{SubjectDisplay} should have length {expected}` |
| `HavePosition(expected)` | null, not seekable, or `Position != expected` | `{SubjectDisplay} should have position {expected}` |
| `BeEmpty()` | null, not seekable, or `Length != 0` | `{SubjectDisplay} should be empty` |

Rules that must hold:

- **Never mutate the subject.** No `Read`, no `Seek`, no `Position` assignment, no `CopyTo`. Every
  assertion is a property read. A test that asserts on a stream must be able to assert again and get the
  same answer — pin that with a test that runs two assertions in sequence and checks `Position` is
  unchanged.
- **Non-seekable streams.** `Length` and `Position` throw `NotSupportedException` on a non-seekable stream.
  Guard with `CanSeek` **first** and fail as a `CompareException` — the BCL exception must never escape.
  Pin it for `HaveLength`, `HavePosition`, and `BeEmpty`.
- **Disposed streams.** Reading `CanRead` on a disposed stream returns `false` (it does not throw), but
  `Length` throws `ObjectDisposedException`. The `CanSeek` guard covers it, since a disposed stream reports
  `CanSeek == false`. Pin one test with a disposed `MemoryStream` per `Have*` assertion, and one proving
  `BeReadable` fails rather than throws.
- `BeReadOnly` / `BeWriteOnly` are about the *capability pair*, not about `FileAccess`. Say so in the
  README — a `MemoryStream` created with `new MemoryStream(bytes, writable: false)` is read-only;
  `new MemoryStream()` is neither read-only nor write-only.

`NotStreamComparer` mirrors every assertion with `should not …` and keeps the same guards.

**Entry point** — in `ShouldExtensions.cs`:

```csharp
public static StreamComparer Should(this Stream subject) { return new StreamComparer(subject); }
```

One overload on the `Stream` base class covers `MemoryStream`, `FileStream`, and every other derived type —
do not add per-derived-type overloads.

`System.IO` is available through `ImplicitUsings`; no new `using` is needed in the comparer files beyond
the `FatCat.Testing.Comparers` / `FatCat.Testing.Exceptions` pair every family uses.

---

## TDD Steps

`MemoryStream` is the workhorse — deterministic, disposable, and configurable in every dimension this
family asserts on:

```csharp
var readable = new MemoryStream([1, 2, 3]);                       // CanRead, CanSeek, CanWrite
var readOnly = new MemoryStream([1, 2, 3], writable: false);      // CanRead, CanSeek, !CanWrite
```

For non-seekable and write-only coverage you need a small fixture stream. Put it in
`Tests.FatCat.Testing/Streams/` as its own file, one class per file — a minimal `Stream` subclass
overriding `CanRead`/`CanWrite`/`CanSeek` and throwing `NotSupportedException` from the rest. Name it for
what it is (e.g. `NonSeekableStream`, `WriteOnlyStream`).

Dispose fixtures with `using var` where the test creates them. Do **not** add setup fields or a constructor
to the test class — `testing.md` forbids both.

One test class per assertion method — `Stream<Method>Tests`, deriving `BaseTest`: `Good<Method>`,
`Bad<Method>`, `Bad<Method>ShowsCorrectMessage`, `Bad<Method>WithBecause`, the four `Not` equivalents, and
`Bad<Method>WhenNull` (declare `Stream value = null;`).

Plus the behaviour pins:

- `BadHaveLengthWhenNotSeekable`, `BadHavePositionWhenNotSeekable`, `BadBeEmptyWhenNotSeekable` — all via
  `RunCompareFailTest`, proving a `CompareException` and not a `NotSupportedException`
- `BadHaveLengthWhenDisposed`, `BadBeReadableWhenDisposed`
- `GoodAssertionDoesNotMoveposition` — two assertions in sequence, `Position` unchanged (name it
  `GoodAssertionsDoNotMovePosition`)
- `GoodBeReadOnlyWhenWritableIsFalse`, `BadBeReadOnlyWhenAlsoWritable`

Work assertion by assertion: red, green, next.

---

## Files

**Added**

- `FatCat.Testing/Streams/StreamComparer.cs`
- `FatCat.Testing/Streams/NotStreamComparer.cs`
- `Tests.FatCat.Testing/Streams/` — one class per assertion method, plus one file per fixture stream

**Changed** — `FatCat.Testing/ShouldExtensions.cs`, `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog`** — new `### Streams` subsection, alphabetically placed (after
`### Strings`). Include the assertion table and state:

- assertions never read from or seek the stream — position and content are untouched;
- `HaveLength` / `HavePosition` / `BeEmpty` fail (as assertions) on a non-seekable or disposed stream
  rather than throwing;
- `BeReadOnly` / `BeWriteOnly` describe the `CanRead`/`CanWrite` pair, and a plain `MemoryStream` is
  neither;
- content assertions are deliberately absent, because reading would mutate the subject;
- whether `HaveBufferSize` exists (per OQ-5, it does not).

**`README.md` → `## Coverage Status`** — flip the `Stream` row to `✅ shipped`.

**`MIGRATION.md` → `## 3. Mapping Table`** — one row per assertion, positive and negated:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `stream.Should().BeReadable()` | same | ✅ supported | `Tests.FatCat.Testing.Streams.StreamBeReadableTests` |
| `stream.Should().NotBeWritable()` | `.Should().Not.BeWritable()` | ✅ supported | `Tests.FatCat.Testing.Streams.StreamBeWritableTests` |
| `stream.Should().HaveLength(n)` | same | ✅ supported | `Tests.FatCat.Testing.Streams.StreamHaveLengthTests` |
| … one row per remaining assertion … | | | |
| `stream.Should().HaveBufferSize(n)` | — | ❌ not supported | — |

**`MIGRATION.md` → `## 5. Known Unsupported`** — add `HaveBufferSize` with the recommended rewrite
(`((BufferedStream)stream).BufferSize.Should().Be(n)`).

**`MIGRATION.md` → `## 4. Type Coverage`** — mark `Stream` supported and note G19 closed.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Streams"
```

Manual check — the comparers must contain no mutating call:

```pwsh
Select-String -Path FatCat.Testing\Streams\*.cs -Pattern '\.Read|\.Seek|\.Write|\.CopyTo|Position\s*='
```

No hits. `Subject.Position` **reads** are fine; an assignment is not.

Then run the standards review on the uncommitted change and resolve every finding.

---

## Definition of Done

- [ ] OQ-5 answered before implementation; the decision recorded in the phase report.
- [ ] Tests written before implementation, assertion by assertion; red states observed.
- [ ] **Two** comparers only. No nullable pair.
- [ ] No mutating call anywhere in `Streams/`, verified by the grep above.
- [ ] Non-seekable and disposed subjects produce `CompareException`, never `NotSupportedException` or
      `ObjectDisposedException`, proven by tests.
- [ ] Null subject fails every assertion except `BeNull`.
- [ ] `GoodAssertionsDoNotMovePosition` passes.
- [ ] Fixture streams are one class per file and are disposed by the tests that create them.
- [ ] No new compiler warnings; no banned patterns; namespaces match folders.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` and `MIGRATION.md` updated, including the `HaveBufferSize` not-supported row.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/12-stream-family.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-12-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Self-contained. Only phase 14 depends on it — revert that first if it landed. No manual steps.

---

## Hand-off

```csharp
namespace FatCat.Testing.Streams;
StreamComparer / NotStreamComparer

namespace FatCat.Testing;
Should(this Stream) -> StreamComparer
```

**Contracts for later phases**

- Assertions never mutate their subject. This is now a stated property of the library and applies to every
  future family whose subject is stateful.
- A capability that cannot be inspected without side effects is not asserted on — it becomes a
  known-unsupported entry with a recommended rewrite instead.
- **For the G1 plan:** `Should(this Stream)` is the third concrete reference-type overload, after `Uri`
  (10) and `Type` (11). **All three belong in the G1 collision inventory.**
- G19 is closed by this phase. If a future task cites G19, point it here.
