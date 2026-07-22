# Phase 06 — `Task<T>` Overloads: char / DateTime / TimeSpan / Guid / Enums

- **Work item:** `tier_2_gaps`
- **Gap:** **G7** (gaps.md §3, Tier 2)
- **Risk:** **medium.** Additive public overloads on an established pattern; the one genuine hazard is the
  generic enum overload, which interacts with the existing `Should<T>(this T) where T : struct, Enum`.
- **Depends on:** 05
- **Depended on by:** 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Phase 05 established the pattern in `FatCat.Testing/ShouldExtensions.cs`: a contiguous block of
`Should(this Task<…>)` overloads, each unwrapping through `TaskResultReader.Read` and handing the result to
the existing comparer.

```csharp
public static BoolComparer Should(this Task<bool> subject) { return new BoolComparer(TaskResultReader.Read(subject)); }
```

`TaskResultReader` lives in `FatCat.Testing/Tasks/TaskResultReader.cs`, is `internal static`, and holds the
library's **only** blocking call — `subject.GetAwaiter().GetResult()`, guarded by an `ArgumentNullException`
for a null task. Read that file before starting; do not duplicate its logic anywhere.

Phase 05 covered `bool`, `bool?`, `string`, the eleven `INumber<T>` types, `int?`, `double`, `float`. This
phase finishes the remaining families so `Task<T>` coverage matches the synchronous `Should()` surface
one-for-one.

Read before starting: `.claude/rules/csharp/async.md`, `naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, [00-overview.md](00-overview.md) (ADR-2), and
[05-task-overloads-core.md](05-task-overloads-core.md)'s Hand-off section.

---

## Scope

**In scope** — `Should(this Task<…>)` for `char`, `char?`, `DateTime`, `DateTime?`, `TimeSpan`,
`TimeSpan?`, `Guid`, `Guid?`, and the two enum generics.

**Out of scope**

- Anything phase 05 already shipped. Do not touch `TaskResultReader`.
- `Task<List<T>>`, `Task<SomeDto>`, non-generic `Task`, `ValueTask<T>` — Tier 1 or explicitly excluded
  (ADR-1, OQ-3).
- Families added later in this plan. `Task<DateTimeOffset>`, `Task<Uri>`, `Task<Stream>` and friends are
  **not** added here; see the note in Hand-off about who owns them.

---

## Design

```csharp
public static CharComparer Should(this Task<char> subject) { return new CharComparer(TaskResultReader.Read(subject)); }

public static NullableCharComparer Should(this Task<char?> subject) { return new NullableCharComparer(TaskResultReader.Read(subject)); }

public static DateTimeComparer Should(this Task<DateTime> subject) { return new DateTimeComparer(TaskResultReader.Read(subject)); }

public static NullableDateTimeComparer Should(this Task<DateTime?> subject) { return new NullableDateTimeComparer(TaskResultReader.Read(subject)); }

public static TimeSpanComparer Should(this Task<TimeSpan> subject) { return new TimeSpanComparer(TaskResultReader.Read(subject)); }

public static NullableTimeSpanComparer Should(this Task<TimeSpan?> subject) { return new NullableTimeSpanComparer(TaskResultReader.Read(subject)); }

public static GuidComparer Should(this Task<Guid> subject) { return new GuidComparer(TaskResultReader.Read(subject)); }

public static NullableGuidComparer Should(this Task<Guid?> subject) { return new NullableGuidComparer(TaskResultReader.Read(subject)); }

public static EnumComparer<T> Should<T>(this Task<T> subject)
	where T : struct, Enum
{
	return new EnumComparer<T>(TaskResultReader.Read(subject));
}

public static NullableEnumComparer<T> Should<T>(this Task<T?> subject)
	where T : struct, Enum
{
	return new NullableEnumComparer<T>(TaskResultReader.Read(subject));
}
```

All ten join the existing `Task<…>` block, kept in the same order as their synchronous counterparts.

### The enum overloads — the medium-risk part

The synchronous file already has `Should<T>(this T) where T : struct, Enum` and
`Should<T>(this T?) where T : struct, Enum`. Adding `Should<T>(this Task<T>) where T : struct, Enum` is
safe *because* the constraint is on `T`, not on the receiver — `Task<SomeEnum>` binds the new overload,
and a bare `SomeEnum` still binds the old one. But verify rather than assume:

1. `Task.FromResult(SomeEnum.Value).Should()` resolves to `EnumComparer<SomeEnum>` and compiles.
2. `Task.FromResult<SomeEnum?>(SomeEnum.Value).Should()` resolves to `NullableEnumComparer<SomeEnum>`.
3. `SomeEnum.Value.Should()` still resolves to the synchronous overload — no regression.
4. `Task.FromResult(true).Should()` still binds the concrete `Task<bool>` overload from phase 05, not the
   enum generic. (It cannot bind the generic — `bool` is not an `Enum` — but pin it anyway; this is the
   overload the compiler would silently redirect if a future constraint loosened.)

Each is a compiling test. If any of 1–4 fails to compile or resolves elsewhere, **stop** and report — do
not work around it with casts.

There is a pre-existing enum type in `Tests.FatCat.Testing/Enums/` used by the enum tests. Reuse it rather
than declaring a new one; open that folder and find it before writing any test.

---

## TDD Steps

Deterministic tasks only — `Task.FromResult` / `Task.FromException` / `Task.FromCanceled`. No `Task.Delay`,
no `Thread.Sleep`.

Construct explicit `DateTime` values (`new DateTime(2026, 7, 21, 10, 30, 0, DateTimeKind.Utc)`). **Never
`DateTime.Now` or `DateTime.UtcNow` in a test** — `not-allowed.md`.

1. **Red.** `Tests.FatCat.Testing/Tasks/TaskCharShouldTests.cs` — `GoodBe`, `BadBe`,
   `BadBeShowsCorrectMessage`, `BadBeWithBecause`, `GoodNotBe`, plus a `char?` fact and a
   `char?`-is-null fact.

2. **Red.** `Tests.FatCat.Testing/Tasks/TaskDateTimeShouldTests.cs` — the same shape using explicit
   `DateTime` values, plus `BeAfter` to prove the full comparer surface is reachable, plus the `DateTime?`
   and null cases.

3. **Red.** `Tests.FatCat.Testing/Tasks/TaskTimeSpanShouldTests.cs` — same shape with
   `TimeSpan.FromHours(1)` style values, plus the nullable and null cases.

4. **Red.** `Tests.FatCat.Testing/Tasks/TaskGuidShouldTests.cs` — same shape with an explicit
   `new Guid("…")` literal (not `Guid.NewGuid()`, which would make the pinned message non-deterministic),
   plus `BeEmpty`, the nullable case, and the null case.

5. **Red.** `Tests.FatCat.Testing/Tasks/TaskEnumShouldTests.cs` — the four overload-resolution proofs from
   the Design section, named `GoodBeForEnum`, `GoodBeForNullableEnum`,
   `GoodSynchronousEnumStillResolves`, `GoodTaskOfBoolStillResolvesToBoolComparer`, plus the standard
   `Bad…ShowsCorrectMessage` / `Bad…WithBecause` pair for the enum path.

6. **Green.** Add the ten overloads.

7. **Refactor.** None.

---

## Files

**Changed**

- `FatCat.Testing/ShouldExtensions.cs`
- `README.md`
- `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Tasks/TaskCharShouldTests.cs`
- `Tests.FatCat.Testing/Tasks/TaskDateTimeShouldTests.cs`
- `Tests.FatCat.Testing/Tasks/TaskTimeSpanShouldTests.cs`
- `Tests.FatCat.Testing/Tasks/TaskGuidShouldTests.cs`
- `Tests.FatCat.Testing/Tasks/TaskEnumShouldTests.cs`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog` → `### Task Results`** (created by phase 05) — extend the supported-
`T` list to the full set and remove the "the rest arrive in phase 06" caveat. Add one line: `Task<T>`
coverage now mirrors the synchronous `Should()` surface exactly, with the exception of objects and
collections, which arrive with G1/G4.

**`README.md` → `## Coverage Status`** — update the `Task<T>` row from partial to shipped, keeping the
object/collection caveat.

**`MIGRATION.md` → `## 3. Mapping Table`** — append:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `(await x).Should().Be(y)` on `Task<DateTime>` | `x.Should().Be(y)` | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskDateTimeShouldTests` |
| `(await x).Should().Be(y)` on `Task<Guid>` | `x.Should().Be(y)` | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskGuidShouldTests` |
| `(await x).Should().Be(y)` on `Task<TEnum>` | `x.Should().Be(y)` | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskEnumShouldTests` |
| `(await x).Should().Be(y)` on `Task<TimeSpan>` | `x.Should().Be(y)` | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskTimeSpanShouldTests` |
| `(await x).Should().Be(y)` on `Task<char>` | `x.Should().Be(y)` | ✅ supported | `Tests.FatCat.Testing.Tasks.TaskCharShouldTests` |

Awaiting remains correct and is still the recommendation where the test can be async — the row says the
`await` is now optional, not that it is wrong. Keep the deadlock warning phase 05 added; do not soften it.

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

The blocking-call grep from phase 05 must still return exactly one hit:

```pwsh
Select-String -Path FatCat.Testing\**\*.cs -Pattern '\.Result\b|\.Wait\(\)|GetAwaiter\(\)'
```

---

## Definition of Done

- [ ] Tests written before implementation; red state observed and recorded.
- [ ] All ten overloads added to the existing `Task<…>` block, ordered to match their synchronous twins.
- [ ] Enum overload-resolution proofs 1–4 all compile and pass.
- [ ] The synchronous `SomeEnum.Value.Should()` path is proven unregressed.
- [ ] No new blocking call — the grep still finds exactly one, in `TaskResultReader.cs`.
- [ ] No `DateTime.Now` / `DateTime.UtcNow` / `Guid.NewGuid()` in any pinned-message test.
- [ ] No `Task.Delay`, `Thread.Sleep`, `async void`, or `ConfigureAwait`.
- [ ] No new `string?`, no extension of the `#nullable enable` region (OQ-2).
- [ ] No new compiler warnings; namespaces match folders.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] README `### Task Results` and `## Coverage Status` updated; `MIGRATION.md` rows appended.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/06-task-overloads-remaining.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-06-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Reverts cleanly on its own — nothing depends on it except phase 14 (revert that first if it landed). It
**must** be reverted before phase 05, which owns `TaskResultReader`. Reverting 05 without reverting 06
breaks the build.

No manual steps.

---

## Hand-off

**Public surface added** — `namespace FatCat.Testing`:

```csharp
Should(this Task<char>)       -> CharComparer
Should(this Task<char?>)      -> NullableCharComparer
Should(this Task<DateTime>)   -> DateTimeComparer
Should(this Task<DateTime?>)  -> NullableDateTimeComparer
Should(this Task<TimeSpan>)   -> TimeSpanComparer
Should(this Task<TimeSpan?>)  -> NullableTimeSpanComparer
Should(this Task<Guid>)       -> GuidComparer
Should(this Task<Guid?>)      -> NullableGuidComparer
Should<T>(this Task<T>)  where T : struct, Enum  -> EnumComparer<T>
Should<T>(this Task<T?>) where T : struct, Enum  -> NullableEnumComparer<T>
```

`Task<T>` coverage now mirrors the synchronous surface one-for-one.

**Contracts for later phases**

- Phases 07–12 add new type families. Each **may** add a matching `Task<…>` overload, but this plan does
  **not** require it — the G7 obligation was defined against the families that existed when the plan was
  written. If a phase adds one, it goes in this same block and delegates through `TaskResultReader.Read`.
  If it does not, its README row must say so rather than leaving the reader to guess. Phase 14 audits this.
- The enum generic `Should<T>(this Task<T>) where T : struct, Enum` now exists. Any future generic
  `Should<T>(this Task<T>)` from G1 must be checked against it — an unconstrained generic would be
  ambiguous with this one for enum arguments. **Carry this into the G1 plan's collision inventory.**
