# Phase 17 — G26 completeness: Enums

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **low.** Additive methods on the existing enum comparers.
- **Depends on:** 01
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/Enums/` has `EnumComparer<T>` (`where T : struct, Enum`), `NotEnumComparer<T>`,
`NullableEnumComparer<T>`, `NotNullableEnumComparer<T>`. Existing assertions: `Be`, `BeDefined`, `HaveFlag`,
`BeNull`, `HaveValue`. The entry points are the two generic `Should<T>` overloads in `ShouldExtensions.cs`
(`where T : struct, Enum`).

This phase adds the G26 enum methods `gaps.md` §6.3 names: `HaveSameNameAs`, `HaveSameValueAs`,
`NotBeDefined`.

Read before starting: [00-overview.md](00-overview.md), `gaps.md` §6.3 (Enums row),
`FatCat.Testing/Enums/EnumComparer.cs` and `NotEnumComparer.cs`,
`.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** on `EnumComparer<T>` / `NotEnumComparer<T>` (and the nullable variants where meaningful):

- `HaveSameNameAs<TOther>(TOther other)` — the two enum values have the same member name
- `HaveSameValueAs<TOther>(TOther other)` — the two enum values have the same underlying numeric value
- `NotBeDefined()` — via `.Not.BeDefined()`; fails when the value **is** a defined member

**Out of scope** — any non-enum family; a `Be(string)` parse overload (not in the §6.3 enum row).

`NotBeDefined` — `BeDefined` is on the positive comparer, so its negation is `BeDefined` on
`NotEnumComparer<T>` (fails when defined). **Check whether `NotEnumComparer` already has `BeDefined`**; if so,
record it and this half is done.

`HaveSameNameAs` / `HaveSameValueAs` compare across enum types, so the argument is a second enum type
`TOther : struct, Enum`. Both go on the positive comparer (and a natural `Not` form if it reads well).

---

## Design

| Method | Fails when | Message |
|---|---|---|
| `HaveSameNameAs(other)` | `Enum.GetName(Subject) != Enum.GetName(other)` | `{Subject} should have the same name as {other}` |
| `HaveSameValueAs(other)` | `Convert.ToInt64(Subject) != Convert.ToInt64(other)` | `{Subject} should have the same value as {other}` |
| `BeDefined()` on `NotEnumComparer` | `Enum.IsDefined(typeof(T), Subject)` | `{Subject} should not be defined` |

Use `Convert.ToInt64` for the underlying value comparison so it works across differing underlying types.
Match the existing enum message style (interpolate the enum value directly — enums override `ToString`, so
the current `$"{Subject}"` is fine and no `ValueFormatter` is needed).

Signatures use generics: `public EnumComparer<T> HaveSameNameAs<TOther>(TOther other, string because = null)
where TOther : struct, Enum`.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Enums/`, one class per method. Define two small
test enums with overlapping names/values as fixtures (one class per file).

1. `EnumHaveSameNameAsTests.cs` — two enums with a shared member name pass; differing names fail; full set +
   `Not` if shipped.
2. `EnumHaveSameValueAsTests.cs` — two enums with a shared underlying value pass; differing values fail;
   full set.
3. `EnumNotBeDefinedTests.cs` — `.Not.BeDefined()`: an undefined cast value passes, a defined value fails;
   full set.
4. **Green.** Implement on the enum comparers.
5. Run the whole suite.

---

## Files

**Changed**

- `FatCat.Testing/Enums/EnumComparer.cs`, `NotEnumComparer.cs` (and nullable variants if a method applies)
- `README.md`, `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Enums/EnumHaveSameNameAsTests.cs`
- `Tests.FatCat.Testing/Enums/EnumHaveSameValueAsTests.cs`
- `Tests.FatCat.Testing/Enums/EnumNotBeDefinedTests.cs`
- Fixture enums — one per file

---

## Documentation Updates

**`README.md` → `### Enums`** — append `HaveSameNameAs`, `HaveSameValueAs`, and note `BeDefined` is negatable.

**`MIGRATION.md` → `## 3. Mapping Table`** — coverage rows:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().HaveSameNameAs(x)` | same | `Tests.FatCat.Testing.Enums.EnumHaveSameNameAsTests` |
| `.Should().HaveSameValueAs(x)` | same | `Tests.FatCat.Testing.Enums.EnumHaveSameValueAsTests` |
| `.Should().NotBeDefined()` | `.Should().Not.BeDefined()` | `Tests.FatCat.Testing.Enums.EnumNotBeDefinedTests` |

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Enum"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `HaveSameNameAs`, `HaveSameValueAs`, `.Not.BeDefined()` shipped with full sets; cross-type comparison
      proven.
- [ ] Pre-existing negated `BeDefined` checked; recorded if already present.
- [ ] No new warnings; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/17-g26-enums.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-17-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (revert phase 20 first if landed).

---

## Hand-off

Enum G26 methods shipped. Phase 20 counts this toward the audit.
