# Phase 02 — Value formatting engine (G15)

- **Work item:** `final_gaps`
- **Gap:** **G15** (`remaining_gaps.md` §4, prerequisite tier · `gaps.md` §6.1)
- **Risk:** **medium.** No new public assertion, but it rewires how *every* failure message renders its
  subject. The whole suite pins message text, so a mistake here is loud rather than silent — which is the
  reason this phase leads.
- **Depends on:** 01
- **Depended on by:** 04, 05, 06, 07, 08, 09, 10, 13
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Every failure message in the library is built with plain interpolation — `$"{Subject} should be {expected}"`
— which is `ToString()`. That is correct for `int`, `bool`, `Guid`, `DateTime`. It produces garbage the
moment objects and collections arrive in phases 04 and 06: a `List<string>` renders as

```
System.Collections.Generic.List`1[System.String]
```

and a DTO renders as its type name. A `BeEquivalentTo` failure (phase 07) that cannot name the member that
differed is not a usable assertion. This phase exists so the messages those phases produce are readable on
the day they land, not retrofitted afterwards.

There is already duplication to collapse. `FatCat.Testing/Comparers/ComparerBase.cs` and
`NotComparerBase.cs` each end with an identical **private** method:

```csharp
private string FormatSubject()
{
	var boxed = (object)Subject;

	return boxed == null ? "null" : $"{boxed}";
}
```

Read before starting: [00-overview.md](00-overview.md) — especially **ADR-7** (the formatter is public API)
and **ADR-13** (top-level strings render bare, nested strings render quoted) — plus
`.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`, `not-allowed.md`.

---

## Scope

**In scope** — a public `ValueFormatter`, its tests, and rewiring the two `FormatSubject` methods onto it.

**Out of scope**

- Changing any existing failure message. **The whole suite must stay green with zero test edits.** If a
  pinned message changes, the formatter is wrong, not the test.
- `ComparerBase.BeOneOf`, which builds its list with `string.Join(", ", valuesList)`. Leave it exactly as
  is — routing it through the formatter would change pinned messages. Note it in the hand-off for a later
  phase to reconsider.
- Registering custom formatters (`Formatter.AddFormatter`), `FormattedObjectGraph`, indentation,
  `MaxLinesExceededException`. FluentAssertions has all of it; nothing here needs it.
- Touching the `#nullable enable` region in `Strings/` (OQ-2).

---

## Design

New folder `FatCat.Testing/Formatting/`, namespace `FatCat.Testing.Formatting`, one public class:

```csharp
namespace FatCat.Testing.Formatting;

public static class ValueFormatter
{
	public static string Format(object value) { ... }
}
```

### Rendering rules

Position matters — see **ADR-13**. `Format` is the top-level entry; nested rendering is a private overload
carrying depth and a visited set.

| Value | Top level | Nested (inside a collection or object dump) |
|---|---|---|
| `null` | `null` | `null` |
| `string` | the string, bare | `"the string"` |
| `char` | the char, bare | `'c'` |
| `IEnumerable` (not `string`) | `[a, b, c]` | `[a, b, c]` |
| anything overriding `ToString()` | `ToString()` | `ToString()` |
| any other object | `TypeName { Member = value, ... }` | same |

Details that are part of the contract and must be pinned by tests:

- **Collection cap.** At most **32** elements, then `, …and 5 more` — the count of the remainder, not an
  ellipsis alone. 32 is FluentAssertions' number; matching it means a reader coming from there is not
  surprised. An empty collection renders `[]`.
- **Member dump.** Public readable **instance properties** only, in declaration order. No fields, no static
  members, no indexers. A type with no such properties renders `TypeName { }`.
- **Depth cap.** Nesting deeper than **5** renders `{ … }`. Depth is counted from the top-level call.
- **Cycle detection.** A reference already on the current path renders `{ cyclic reference to TypeName }`.
  Use reference equality (`ReferenceEqualityComparer.Instance`), never `Equals` — the object being formatted
  is frequently one whose `Equals` is the thing under test.
- **A property getter that throws** renders `Member = <threw TypeName>`. Catch it, do not let a formatter
  fail an assertion for the wrong reason. This is one of the rare deliberate empty-ish catches
  `errors.md` allows; comment it.
- **"Overrides `ToString()`"** means `value.GetType().GetMethod("ToString", Type.EmptyTypes).DeclaringType`
  is not `typeof(object)`. This is what keeps `Guid`, `DateTime`, `TimeSpan`, and every enum rendering
  exactly as they do today.

### Rewiring

`ComparerBase.FormatSubject()` and `NotComparerBase.FormatSubject()` both become:

```csharp
private string FormatSubject() { return ValueFormatter.Format(Subject); }
```

Keep them private and keep the name — every call site inside those files stays unchanged, and the two
one-line bodies are now the same one line rather than the same six.

---

## TDD Steps

Tests first. Red before green.

1. **Red.** `Tests.FatCat.Testing/Formatting/ValueFormatterTests.cs`, namespace
   `Tests.FatCat.Testing.Formatting`, deriving `BaseTest`. This class asserts on returned strings rather
   than on thrown exceptions, so it uses `Should().Be(...)` directly — `BaseTest` is still the base for
   consistency with every other test class. Facts, one behaviour each:

   - `GoodFormatNull` → `"null"`
   - `GoodFormatStringIsBare` → `ValueFormatter.Format("hello")` is `"hello"` *(ADR-13)*
   - `GoodFormatIntUsesToString` → `"42"`
   - `GoodFormatGuidUsesToString`, `GoodFormatDateTimeUsesToString`, `GoodFormatEnumUsesToString`
   - `GoodFormatEmptyCollection` → `"[]"`
   - `GoodFormatStringCollectionQuotesElements` → `["a", "b"]` *(ADR-13 — the other half)*
   - `GoodFormatIntCollection` → `"[1, 2, 3]"`
   - `GoodFormatCollectionCapsAtThirtyTwo` → 40 elements renders 32 then `, …and 8 more`
   - `GoodFormatObjectDumpsProperties` → `Dto { Name = "Bob", Age = 42 }`
   - `GoodFormatObjectWithNoPropertiesRendersEmptyBraces`
   - `GoodFormatNestedObject`
   - `GoodFormatCyclicReference` — two objects referencing each other; must return, not stack-overflow
   - `GoodFormatDepthCapped`
   - `GoodFormatThrowingPropertyIsCaught`
   - `GoodFormatCharIsBareAtTopLevel` and `GoodFormatCharIsQuotedInCollection`
   - `GoodFormatTypeOverridingToStringUsesIt` — a test type whose `ToString()` returns a known string

   Test fixture types (`Dto`, the cyclic pair, the throwing-property type, the `ToString`-overriding type)
   go in the same folder as small `internal` classes, one per file, per the one-class-per-file rule.

2. **Red → green.** Implement `ValueFormatter`. Expect several iterations here — the ordering of the checks
   (null → string → char → overrides-`ToString` → `IEnumerable` → member dump) is what makes `Guid` and
   `DateTime` keep their current rendering. Get that order wrong and the existing suite goes red, which is
   the signal.

3. **Green, no edits.** Rewire both `FormatSubject` methods. **Run the full suite. Every existing test must
   pass with zero test-file edits.** If any message test fails, the formatter is wrong. Do not update the
   test.

4. **Refactor.** The private recursive overload should be small and obvious. Do not add an interface or a
   registry — nothing has two implementations (`not-allowed.md`).

---

## Files

**Added**

- `FatCat.Testing/Formatting/ValueFormatter.cs`
- `Tests.FatCat.Testing/Formatting/ValueFormatterTests.cs`
- Test fixture types under `Tests.FatCat.Testing/Formatting/` — one class per file

**Changed**

- `FatCat.Testing/Comparers/ComparerBase.cs`
- `FatCat.Testing/Comparers/NotComparerBase.cs`
- `README.md`

---

## Documentation Updates

**`README.md` → `## Value Formatting`** — replace the phase-02 placeholder with: what the formatter does,
the rendering table above, the 32-element cap, the depth cap, the cycle-reference rendering, and the
top-level-bare / nested-quoted rule with an example of each. State that `ValueFormatter.Format` is public
and supported so custom comparers (phase 10) can render subjects identically.

**`MIGRATION.md`** — no mapping rows. Formatting is not a call-site construct. Add nothing.

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
```

Targeted:

```pwsh
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~ValueFormatter"
```

Then run the standards review on the uncommitted change and resolve every finding before committing.

---

## Definition of Done

- [ ] Tests written before implementation; the red state was observed and is recorded in the phase report.
- [ ] `ValueFormatter.Format` handles: null, string, char, `IEnumerable`, `ToString()`-overriding types,
      plain objects.
- [ ] Collection output capped at 32 with an `…and N more` remainder; empty renders `[]`.
- [ ] Cycle detection by reference equality; a cyclic graph formats and returns.
- [ ] Depth cap at 5.
- [ ] A throwing property getter is caught and rendered, with a comment saying why the catch is there.
- [ ] Top-level string bare, nested string quoted — both pinned by tests (**ADR-13**).
- [ ] Both `FormatSubject` methods delegate to `ValueFormatter`; the duplicated six-line bodies are gone.
- [ ] **The full existing suite passes with zero edits to any existing test file.**
- [ ] No new compiler warnings (five pre-existing IDE1006 `TestGuid` warnings are baseline).
- [ ] Namespaces match folder paths. No expression bodies, no braceless `if`, no `string?`, no underscores
      in test names, no `+` concatenation, no `new List<T>()`.
- [ ] `dotnet format style` and `dotnet format analyzers` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` → `## Value Formatting` written.
- [ ] Standards review clean.
- [ ] Exactly one commit, message referencing `tasks/todo/final_gaps/02-value-formatting-engine.md`.
- [ ] No file outside `c:\Code\FatCat.Testing` touched.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-02-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Reverting restores the two duplicated `FormatSubject` bodies, so the build stays green. **Phases 04–10 and
13 all render subjects through the formatter** — revert them first, or their messages lose the type they
were written against.

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing.Formatting;

public static class ValueFormatter
{
	public static string Format(object value)
}
```

**Every later phase formats subjects and expected values with `ValueFormatter.Format`** — never with bare
`$"{subject}"` — whenever the value can be an object or a collection. Scalar comparers keep their existing
interpolation; their messages are pinned and must not move.

For phase 07, the equivalency engine reuses two things from here and must not reimplement them: the cycle
detection approach (reference equality, path-scoped visited set) and the member-selection rule (public
readable instance properties, declaration order). If phase 07's answer to **OQ-4** differs from the dump's
member rule, say so out loud in that phase's report — two different notions of "a type's members" in one
library is a defect, not a detail.

Deliberately left alone, for a later phase to decide: `ComparerBase.BeOneOf` still builds its list with
`string.Join(", ", valuesList)` rather than the formatter, because routing it through would change a pinned
message.
