# Phase 15 — G26 completeness: Strings

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **low.** Additive methods on the existing string comparers.
- **Depends on:** 01
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/Strings/` holds `NullableStringComparer` and `NotNullableStringComparer` (string has no
non-nullable comparer — `NullableStringComparer` is the only one, per the `Strings/` precedent). It already
has `Be`, `BeEquivalentTo`, `Contain`, `ContainAll`, `ContainAny`, `StartWith`, `EndWith`,
`StartWithEquivalentOf`, `EndWithEquivalentOf`, `Match`, `MatchRegex`, `HaveLength`, `BeEmpty`, `BeNull`,
`BeNullOrEmpty`, `BeNullOrWhiteSpace`, `BeLowerCased`, `BeUpperCased`, `HaveValue`, and a
`StringEqualityHelper` with wildcard/equality helpers.

**`MatchEquivalentOf` is claimed by `tier_2_gaps` phase 04 (G9), a different plan.** Do **not** ship it here —
if that plan has run it already exists; if not, it is that plan's job. This phase ships only the G26 string
methods `gaps.md` §6.3 names that no other plan owns.

**These files carry `#nullable enable` and use `string?` (OQ-2).** New methods added **in these files** must
match the existing `string?` style or raise CS8625 — that is the established local convention here and this
phase follows it (the cleanup is a separate, out-of-scope effort). Note: `NullableStringComparer` also has an
expression-bodied `SubjectDisplay` getter that violates the rules; do not copy that pattern into new members,
and do not fix it here.

Read before starting: [00-overview.md](00-overview.md) (**OQ-2**), `gaps.md` §6.3 (Strings row),
`FatCat.Testing/Strings/NullableStringComparer.cs`, `StringEqualityHelper.cs`,
`.claude/rules/csharp/naming-and-structure.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** (`gaps.md` §6.3 Strings, minus `MatchEquivalentOf`/G9 which `tier_2_gaps` owns):

- `ContainEquivalentOf(string expected)` — case-insensitive substring containment
- `NotContainAll(params string[])` — via `Not`; fails only if the subject contains **every** listed value
- `NotContainAny(params string[])` — via `Not`; fails if the subject contains **any** listed value

**Out of scope**

- `MatchEquivalentOf` (G9, `tier_2_gaps/04`).
- Fixing the `string?` region or the expression-bodied `SubjectDisplay` (OQ-2 cleanup — separate effort).
- Any non-string family.

The `NotContainAll` / `NotContainAny` naming: FluentAssertions exposes them as `NotContainAll` /
`NotContainAny`. Under ADR-A these become `.Not.ContainAll(...)` / `.Not.ContainAny(...)`. Since `ContainAll`
and `ContainAny` already exist on the positive comparer, the negations belong on `NotNullableStringComparer`
as `ContainAll` / `ContainAny`. Confirm those two negated methods do not already exist before adding them; if
they do, this phase is just `ContainEquivalentOf` plus tests — record that.

---

## Design

| Method | Comparer | Fails when | Message |
|---|---|---|---|
| `ContainEquivalentOf(expected)` | `NullableStringComparer` | subject does not contain `expected` case-insensitively | `{SubjectDisplay} should contain equivalent of {expected}` |
| `ContainAll(params values)` | `NotNullableStringComparer` | subject contains **all** of `values` | `{SubjectDisplay} should not contain all of [{values}]` |
| `ContainAny(params values)` | `NotNullableStringComparer` | subject contains **any** of `values` | `{SubjectDisplay} should not contain any of [{values}]` |

`ContainEquivalentOf` uses `StringEqualityHelper` — check for a case-insensitive `IndexOf`/`Contains` helper
there; add one to the helper if absent rather than inlining `StringComparison.OrdinalIgnoreCase` at the call
site (keep the equality logic in the helper). Match the existing `SubjectDisplay` message style.

`params string[]` argument list rendered with the existing `string.Join(", ", ...)` style already used in the
comparers (do not pull in `ValueFormatter` for strings — the string family predates it and its messages are
pinned in that style).

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Strings/`, one class per method, namespace
`Tests.FatCat.Testing.Strings`.

1. `StringContainEquivalentOfTests.cs` — `GoodContainEquivalentOf` (`"Hello World".Should().ContainEquivalentOf("hello")`),
   `BadContainEquivalentOf`, `BadContainEquivalentOfShowsCorrectMessage`, `BadContainEquivalentOfWithBecause`,
   plus the `Not` set.
2. `StringNotContainAllTests.cs` — via `.Not.ContainAll(...)`: passes when the subject lacks at least one,
   fails when it contains all; full set.
3. `StringNotContainAnyTests.cs` — via `.Not.ContainAny(...)`: passes when the subject contains none, fails
   when it contains any; full set.
4. **Green.** Implement across the two string comparer files (+ helper method if needed).
5. Run the whole suite — existing string tests untouched.

Explicit literals — the values appear in the failure message.

---

## Files

**Changed**

- `FatCat.Testing/Strings/NullableStringComparer.cs`, `NotNullableStringComparer.cs`
- `FatCat.Testing/Strings/StringEqualityHelper.cs` (if a case-insensitive contains helper is added)
- `README.md`, `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Strings/StringContainEquivalentOfTests.cs`
- `Tests.FatCat.Testing/Strings/StringNotContainAllTests.cs`
- `Tests.FatCat.Testing/Strings/StringNotContainAnyTests.cs`

---

## Documentation Updates

**`README.md` → `### Strings`** — append `ContainEquivalentOf`, and note `ContainAll`/`ContainAny` are
negatable via `.Not.`.

**`MIGRATION.md` → `## 3. Mapping Table`** — coverage rows:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().ContainEquivalentOf(x)` | `.Should().ContainEquivalentOf(x)` | `Tests.FatCat.Testing.Strings.StringContainEquivalentOfTests` |
| `.Should().NotContainAll(...)` | `.Should().Not.ContainAll(...)` | `Tests.FatCat.Testing.Strings.StringNotContainAllTests` |
| `.Should().NotContainAny(...)` | `.Should().Not.ContainAny(...)` | `Tests.FatCat.Testing.Strings.StringNotContainAnyTests` |

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~String"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `ContainEquivalentOf`, `Not.ContainAll`, `Not.ContainAny` shipped with full sets.
- [ ] `MatchEquivalentOf` was **not** added (it belongs to `tier_2_gaps/04`).
- [ ] Case-insensitive containment logic lives in `StringEqualityHelper`, not inlined.
- [ ] New members follow the existing `string?` region convention; the expression-bodied `SubjectDisplay`
      pattern was not copied and not fixed.
- [ ] No new warnings; no banned patterns in new code.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION rows written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/15-g26-strings.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-15-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (revert phase 20 first if landed).

---

## Hand-off

String G26 methods shipped. G9 (`MatchEquivalentOf`) remains `tier_2_gaps`'s to deliver. Phase 20 counts this
toward the audit.
