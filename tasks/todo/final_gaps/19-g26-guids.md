# Phase 19 — G26 completeness: Guids

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **low.** One overload on the existing Guid comparers.
- **Depends on:** 01
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/Guids/` has `GuidComparer`, `NotGuidComparer`, `NullableGuidComparer`,
`NotNullableGuidComparer`. Existing assertions: `Be`, `BeEmpty`, `BeNull`, `HaveValue`. `Be` today takes a
`Guid`.

This phase adds the one G26 Guid item `gaps.md` §6.3 names: a **`Be(string)` overload** — assert a Guid equals
the value parsed from a string, matching FluentAssertions' `Be(string)`.

Read before starting: [00-overview.md](00-overview.md), `gaps.md` §6.3 (Guids row),
`FatCat.Testing/Guids/GuidComparer.cs` and `NotGuidComparer.cs`,
`.claude/rules/csharp/naming-and-structure.md`, `errors.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** — `Be(string expected, string because = null)` on `GuidComparer`, `NotGuidComparer`, and the
nullable variants.

**Out of scope** — any non-Guid family.

The subtlety: what if the string is not a valid Guid? Per `errors.md`, an unparseable expected string is
**API misuse**, not an assertion failure — the caller passed nonsense. Throw `ArgumentException` (a BCL
exception), **not** `CompareException`. `CompareException` means "the assertion failed"; a malformed expected
value means "you called this wrong". Pin this distinction in a test.

---

## Design

| Method | Comparer | Behaviour |
|---|---|---|
| `Be(string expected, ...)` | `GuidComparer` | parse `expected` to `Guid`; fail (`CompareException`) if `Subject != parsed`; message `{Subject} should be {parsed}` |
| `Be(string expected, ...)` | `NotGuidComparer` | fail if `Subject == parsed`; message `{Subject} should not be {parsed}` |
| unparseable `expected` | both | `throw new ArgumentException(...)` — not `CompareException` |

Use `Guid.TryParse`; on failure throw `ArgumentException($"'{expected}' is not a valid Guid")`. On success,
delegate to the existing `Be(Guid)` logic if practical (call it, or share a private helper) so the two
overloads cannot drift.

Match the existing `GuidComparer` message style.

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Guids/`.

1. `GuidBeStringTests.cs`:
   - `GoodBeString` — `var id = new Guid("..."); id.Should().Be("the-same-guid-string");`
   - `BadBeString` — different Guid string
   - `BadBeStringShowsCorrectMessage`
   - `BadBeStringWithBecause`
   - `GoodNotBeString` / `BadNotBeString` / `BadNotBeStringShowsCorrectMessage` / `BadNotBeStringWithBecause`
   - `BadBeStringWithInvalidGuidThrowsArgumentException` — asserts `Assert.Throws<ArgumentException>(...)`,
     **not** `CompareException` (the misuse-vs-failure distinction)
2. `NullableGuidBeStringTests.cs` — same, plus null-subject case.
3. **Green.** Implement on the four Guid comparers.
4. Run the whole suite — existing `Be(Guid)` tests untouched (the new overload must not shadow them; a
   `Guid` argument still binds to `Be(Guid)`, a `string` argument to `Be(string)` — confirm with a test that
   `id.Should().Be(otherGuid)` still compiles and behaves).

Use explicit literal Guid strings — they appear in the messages.

---

## Files

**Changed**

- `FatCat.Testing/Guids/GuidComparer.cs`, `NotGuidComparer.cs`, `NullableGuidComparer.cs`,
  `NotNullableGuidComparer.cs`
- `README.md`, `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Guids/GuidBeStringTests.cs`
- `Tests.FatCat.Testing/Guids/NullableGuidBeStringTests.cs`

---

## Documentation Updates

**`README.md` → `### Guids`** — note `Be` has a `string` overload that parses the expected value, and that an
unparseable string is a usage error (`ArgumentException`), not an assertion failure.

**`MIGRATION.md` → `## 3. Mapping Table`** — coverage row:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().Be("guid-string")` | `.Should().Be("guid-string")` | `Tests.FatCat.Testing.Guids.GuidBeStringTests` |

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~GuidBeString"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `Be(string)` on all four Guid comparers, full sets, null-subject case on nullables.
- [ ] Unparseable expected string throws `ArgumentException`, **not** `CompareException` — pinned by a test
      (`errors.md`).
- [ ] Existing `Be(Guid)` still binds and behaves; a `Guid` argument does not resolve to `Be(string)`.
- [ ] The two overloads share logic so they cannot drift.
- [ ] No new warnings; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION row written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/19-g26-guids.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-19-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (revert phase 20 first if landed).

---

## Hand-off

Guid G26 method shipped. This is the last G26 family leaf; phase 20 audits the whole replacement claim.
