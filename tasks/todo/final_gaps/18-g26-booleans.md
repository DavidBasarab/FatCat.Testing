# Phase 18 — G26 completeness: Booleans

- **Work item:** `final_gaps`
- **Gap:** **G26** (`remaining_gaps.md` §4 · `gaps.md` §6.3)
- **Risk:** **low.** One method pair on the existing boolean comparers.
- **Depends on:** 01
- **Depended on by:** 20
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/Booleans/` has `BoolComparer`, `NotBoolComparer`, `NullableBoolComparer`,
`NotNullableBoolComparer`. Existing assertions: `Be`, `BeTrue`, `BeFalse`, `BeNull`, `HaveValue`.

This phase adds the one G26 boolean method `gaps.md` §6.3 names: `Imply`.

Read before starting: [00-overview.md](00-overview.md), `gaps.md` §6.3 (Booleans row),
`FatCat.Testing/Booleans/BoolComparer.cs` and `NotBoolComparer.cs`,
`.claude/rules/csharp/naming-and-structure.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** — `Imply(bool consequent)` on `BoolComparer` and its `Not` twin (and the nullable variant if it
reads naturally — logical implication on a nullable is awkward; **ship it on the non-nullable comparer
first**, and only add the nullable form if the semantics are unambiguous; otherwise record the omission).

**Out of scope** — any non-boolean family.

`Imply` is material implication: `subject ⇒ consequent` is false **only** when `subject` is true and
`consequent` is false. That is the single failing case.

---

## Design

| Method | Comparer | Fails when | Message |
|---|---|---|---|
| `Imply(consequent)` | `BoolComparer` | `Subject && !consequent` | `{Subject} should imply {consequent}` |
| `Imply(consequent)` | `NotBoolComparer` | `!(Subject && !consequent)` i.e. the implication holds | `{Subject} should not imply {consequent}` |

Follow the existing `BoolComparer` message style (interpolate `Subject` directly — `True`/`False`).

---

## TDD Steps

Tests first. Red before green. Under `Tests.FatCat.Testing/Booleans/`.

1. `BoolImplyTests.cs`:
   - `GoodImplyWhenAntecedentFalse` — `false.Should().Imply(false)` passes (false implies anything)
   - `GoodImplyWhenBothTrue` — `true.Should().Imply(true)` passes
   - `BadImply` — `true.Should().Imply(false)` fails (the only failing case)
   - `BadImplyShowsCorrectMessage` → `"True should imply False"`
   - `BadImplyWithBecause`
   - `GoodNotImply` / `BadNotImply` / `BadNotImplyShowsCorrectMessage` / `BadNotImplyWithBecause`
2. **Green.** Implement on `BoolComparer` and `NotBoolComparer`.
3. Run the whole suite.

---

## Files

**Changed**

- `FatCat.Testing/Booleans/BoolComparer.cs`, `NotBoolComparer.cs`
- `README.md`, `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Booleans/BoolImplyTests.cs`

---

## Documentation Updates

**`README.md` → `### Booleans`** — append `Imply`, with a one-line note that it is material implication
(fails only when the subject is true and the consequent is false).

**`MIGRATION.md` → `## 3. Mapping Table`** — coverage row:

| FluentAssertions | FatCat.Testing | Proven by |
|---|---|---|
| `.Should().Imply(x)` | `.Should().Imply(x)` | `Tests.FatCat.Testing.Booleans.BoolImplyTests` |

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Imply"
```

Standards review; resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; red observed and recorded.
- [ ] `Imply` on `BoolComparer` and `NotBoolComparer`, full set; all four truth-table cases covered.
- [ ] Nullable form shipped or its omission recorded with a reason.
- [ ] No new warnings; no banned patterns.
- [ ] `dotnet format` / `csharpier` in order, csharpier last.
- [ ] README + MIGRATION row written.
- [ ] Standards review clean.
- [ ] Exactly one commit referencing `tasks/todo/final_gaps/18-g26-booleans.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-18-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Leaf phase. Reverts alone (revert phase 20 first if landed).

---

## Hand-off

Boolean G26 method shipped. Phase 20 counts this toward the audit.
