# Phase 01 — Minimal value formatting (enabling slice of G15)

- **Gap:** Enabling work for G3/G4 (the minimum-viable slice of `gaps.md` §G15). Not full G15.
- **Depends on:** nothing.
- **Depended on by:** Phase 02 (G1), Phase 03 (G3), Phase 04 (G4).
- **Risk:** **low** — purely additive internal helper; no public API change; no behaviour change to existing
  assertions beyond how a subject is rendered inside a failure message.

## Context (complete handoff)

The library builds failure messages by interpolating the subject directly. `ComparerBase.FormatSubject()`
(`FatCat.Testing/Comparers/ComparerBase.cs:55`) is the only central helper and it just does
`boxed == null ? "null" : $"{boxed}"`. That is fine for `int`/`bool`/`Guid` but produces garbage the moment
objects and collections arrive in Phases 02–04: a `List<string>` renders as
`System.Collections.Generic.List\`1[System.String]`, a DTO as its type name. Per ADR-004, this phase adds
**only** the minimal formatter those phases need — not the full FluentAssertions formatting subsystem.

## Deliverable

New internal static helper `FatCat.Testing.Formatting.ValueFormatter` with one entry point:

```csharp
namespace FatCat.Testing.Formatting;

public static class ValueFormatter
{
    public static string Format(object value) { ... }
}
```

Behaviour (bounded and readable — pin each with a test):

| Input | Rendering |
|---|---|
| `null` | `null` |
| `string s` | `"s"` (double-quoted) |
| `char c` | `'c'` (single-quoted) |
| `bool` / numeric / `Guid` / `DateTime` / `TimeSpan` / `enum` | current behaviour — `$"{value}"` (do not regress existing messages) |
| `IEnumerable` (non-string) | `{ a, b, c }`, each element recursively `Format`-ed, capped at **10** items then `, …` (bounded to keep messages short — full 32-item cap is G15) |
| any other object | `TypeName { Prop1 = v1, Prop2 = v2 }` — public readable instance properties, each value recursively `Format`-ed, bounded |

- Guard against cycles in the object/enumerable dump with a small visited-set + max depth (e.g. depth 3);
  on limit, render `…`. (Full cycle handling is G3's concern; this is only for message rendering safety.)
- Public because it is consumed across folders and may be exercised directly by tests; keep it in a
  `Formatting/` folder so the namespace matches (`FatCat.Testing.Formatting`).

Route `ComparerBase.FormatSubject()` through `ValueFormatter.Format(Subject)` so existing comparers gain the
better rendering for free — **but only where it does not change an already-tested message**. Existing
value-type messages (`False should be True`, `Guid` values, numbers) must render byte-identical to today;
if routing changes any existing pinned message, either preserve the old rendering for that type or update
that message's test and log the deviation. Prefer preserving existing messages.

## TDD — tests first (`Tests.FatCat.Testing/Formatting/ValueFormatterTests.cs`)

Because this is not a `Should()` assertion, tests assert `ValueFormatter.Format(x)` directly with
`.Should().Be("...")` on the resulting string. One `[Fact]` per rendering rule, no underscores in names:

- `FormatsNullAsNull`, `FormatsStringQuoted`, `FormatsCharQuoted`
- `FormatsIntUnchanged`, `FormatsBoolUnchanged`, `FormatsGuidUnchanged` (regression pins)
- `FormatsEmptyEnumerable`, `FormatsEnumerableElementwise`, `CapsLongEnumerable`
- `FormatsObjectWithMembers`, `BoundsObjectDepth`, `HandlesCycleWithoutStackOverflow`

Add a regression test asserting an existing comparer message is unchanged, e.g.
`RunCompareFailTest(() => false.Should().Be(true), "False should be True")` still passes.

## Verification

```
dotnet build   Fatcat.Testing.sln
dotnet test    Fatcat.Testing.sln
dotnet format style Fatcat.Testing.sln --verify-no-changes
dotnet csharpier .
```

## Definition of Done

- [ ] `ValueFormatter.Format` exists with every row above covered by a passing test.
- [ ] `ComparerBase.FormatSubject` routes through it with **no** change to any existing pinned message
      (regression test green).
- [ ] No new package reference; no public API on existing comparers changed.
- [ ] All DoD gates from `00-overview.md` met; one commit `[tier1_gaps 01] minimal value formatting`.

## Rollback Procedure

`git revert <phase-01-commit>`. No data/config/flags. Because 02/03/04 depend on `ValueFormatter`, reverting
01 requires reverting 02, 03, 04 first (revert cascades downward — see graph in `00-overview.md`).

## Hand-off (contract exposed to later phases)

- `FatCat.Testing.Formatting.ValueFormatter.Format(object) : string` — the single approved way to render a
  subject or expected value into a failure message. Phases 02–04 use it for every object/collection value
  they interpolate; do not hand-roll `$"{value}"` for reference types or collections.
