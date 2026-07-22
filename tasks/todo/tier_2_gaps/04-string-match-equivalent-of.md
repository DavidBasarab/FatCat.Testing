# Phase 04 — String `MatchEquivalentOf` And The `Match` Reconciliation

- **Work item:** `tier_2_gaps`
- **Gap:** **G9** (gaps.md §3, Tier 2)
- **Risk:** **low.** One new method per string comparer plus documentation. No existing behaviour changes.
  Carries one forced, recorded deviation from `not-allowed.md` (see "The `string?` problem" below).
- **Depends on:** 01
- **Depended on by:** 14
- **Precondition:** **OQ-1 answered** (see [00-overview.md](00-overview.md)). If the answer is "fix
  `StartWithEquivalentOf` / `EndWithEquivalentOf` too", that is a **separate phase** — do not fold it in.
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

`FatCat.Testing/Strings/` contains **two** comparers, not four — `string` is a reference type, so the
nullable form is the only form:

| File | Type |
|---|---|
| `NullableStringComparer.cs` | `NullableStringComparer(string? subject) : ComparerBase<string?, NullableStringComparer>` |
| `NotNullableStringComparer.cs` | the negated twin, exposed as the `Not` property |

Plus helpers: `Options` (`CaseSensitive` / `IgnoreCase`), `StringEqualityHelper`, and five occurrence
constraint types (`Exactly`, `AtLeast`, `AtMost`, `MoreThan`, `LessThan`).

The relevant existing method:

```csharp
public NullableStringComparer Match(
	string pattern,
	Options options = Options.CaseSensitive,
	string? because = null
)
{
	if (Subject == null || !StringEqualityHelper.MatchesWildcard(Subject, pattern, options)) { CompareException.New(because ?? $"{SubjectDisplay} should match {pattern}"); }

	return this;
}
```

`StringEqualityHelper.MatchesWildcard` escapes the pattern, then maps `*` → `.*` and `?` → `.`, anchors it,
and applies `RegexOptions.IgnoreCase` when `options == Options.IgnoreCase`.

`MatchEquivalentOf` is FluentAssertions' case-insensitive wildcard match. It has 4 call sites in the
consuming repos and no FatCat equivalent today (gaps.md §2, §3 G9).

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, and [00-overview.md](00-overview.md) — **ADR-4 and ADR-5 govern this phase.**

---

## Scope

**In scope**

- `MatchEquivalentOf` on `NullableStringComparer` and `NotNullableStringComparer`.
- The G9 "verify and reconcile `Match`" obligation, discharged as **documentation** — ADR-4.

**Out of scope**

- Changing `Match` on any comparer. String `Match` stays a wildcard, numeric `Match` stays a predicate.
  ADR-4 decided this; do not "unify" them.
- `StartWithEquivalentOf` / `EndWithEquivalentOf` default-casing fix — OQ-1, separate phase.
- `ContainEquivalentOf`, `NotContainAll`, `NotContainAny` (gaps.md §6.3). Not G9, not this phase.
- The `NotBeNullOrEmpty` / `NotBeNullOrWhiteSpace` shape — already covered by the `.Not.` property; the
  mapping rows were seeded in phase 01.
- Cleaning up `#nullable enable` / `string?` in this folder — OQ-2, separate phase.

---

## Design

```csharp
public NullableStringComparer MatchEquivalentOf(string pattern, string? because = null)
public NotNullableStringComparer MatchEquivalentOf(string pattern, string? because = null)
```

**No `Options` parameter** (ADR-5). "EquivalentOf" means ignore case; an `Options.CaseSensitive` argument
would contradict the name. Internally both delegate to
`StringEqualityHelper.MatchesWildcard(Subject, pattern, Options.IgnoreCase)`.

| Comparer | Fails when | Message |
|---|---|---|
| `NullableStringComparer.MatchEquivalentOf` | `Subject == null` or the pattern does not match case-insensitively | `{SubjectDisplay} should match equivalent of {pattern}` |
| `NotNullableStringComparer.MatchEquivalentOf` | the pattern **does** match case-insensitively | `{SubjectDisplay} should not match equivalent of {pattern}` |

Null handling mirrors `Match` exactly: in the positive comparer a null subject **fails**; in the negated
comparer mirror whatever `NotNullableStringComparer.Match` already does with null — open that file and copy
its guard rather than inventing one. `SubjectDisplay` (private, renders `null` for a null subject) already
exists in both files; reuse it.

Place the method alphabetically among its neighbours — after `Match` and before `MatchRegex`.

### The `string?` problem — a forced, recorded deviation

`not-allowed.md` bans `string?` parameters. But `NullableStringComparer.cs` and
`NotNullableStringComparer.cs` both open with `#nullable enable`, and every existing `because` parameter in
them is already `string?` (gaps.md §7 flags this as the only place in the library that does it). Inside
that region, writing `string because = null` raises **CS8625** — a new compiler warning, which the
Definition of Done forbids.

**Decision for this phase:** match the surrounding file and write `string? because = null`. Do not remove
`#nullable enable`, do not convert the file. Record it in the phase report's deviation log with a pointer
to OQ-2. Fixing the folder is its own phase, and it will convert these two new methods along with the
thirty existing ones.

### The `Match` reconciliation (documentation only)

`Match` means two different things in this library, and G9 requires that be settled rather than left
ambiguous:

| Family | `Match` signature | Semantics |
|---|---|---|
| `Strings/` | `Match(string pattern, Options options, string? because)` | wildcard — `*` and `?` |
| `Numbers/`, `Doubles/`, `Floats/` | `Match(Func<T, bool> predicate, string because)` | predicate |

Both stay. The string form is FluentAssertions-compatible and has live call sites; the predicate form is a
FatCat extra with no FluentAssertions counterpart. This phase makes the split explicit in the README (phase
01 already created those subsections — verify the wording is there and sharpen it if it is vague) and adds
a note to `MIGRATION.md` §5 so a migrating reader is never surprised by it.

---

## TDD Steps

1. **Red.** `Tests.FatCat.Testing/Strings/StringMatchEquivalentOfTests.cs`, deriving `BaseTest`, namespace
   `Tests.FatCat.Testing.Strings`. Follow the shape of the existing `StringMatchTests.cs`. Facts:

   - `GoodMatchEquivalentOf` — `"Hello World".Should().MatchEquivalentOf("hello*")`
   - `GoodMatchEquivalentOfWithQuestionMark` — `"Hello".Should().MatchEquivalentOf("h?llo")`
   - `GoodMatchEquivalentOfWhenExactCase` — case-sensitive input still matches
   - `BadMatchEquivalentOf`
   - `BadMatchEquivalentOfShowsCorrectMessage` →
     `"Hello World should match equivalent of goodbye*"`
   - `BadMatchEquivalentOfWithBecause` → `"custom because"`
   - `GoodNotMatchEquivalentOf`
   - `BadNotMatchEquivalentOf`
   - `BadNotMatchEquivalentOfShowsCorrectMessage` →
     `"Hello World should not match equivalent of hello*"`
   - `BadNotMatchEquivalentOfWithBecause`
   - `BadMatchEquivalentOfIsCaseInsensitive` — the discriminator against plain `Match`: a pattern that
     `Match` would reject on casing is accepted here. Assert the *positive* path passes.

2. **Red.** `Tests.FatCat.Testing/Strings/NullableStringMatchEquivalentOfTests.cs` — the null-subject set,
   following `NullableStringMatchTests.cs`:

   - `BadMatchEquivalentOfWhenNull` → `"null should match equivalent of hello*"`
   - `BadMatchEquivalentOfWhenNullWithBecause`
   - the `Not` counterpart, matching whatever `NotNullableStringComparer.Match` does with null

   Declare the subject as `string value = null;` and call `value.Should().MatchEquivalentOf(...)`.

3. **Green.** Implement both methods.

4. **Refactor.** None — a two-line delegation to `StringEqualityHelper` needs no helper of its own.

Verify the exact generated message from the first red run before pinning it; do not assume spacing.

---

## Files

**Changed**

- `FatCat.Testing/Strings/NullableStringComparer.cs`
- `FatCat.Testing/Strings/NotNullableStringComparer.cs`
- `README.md`
- `MIGRATION.md`

**Added**

- `Tests.FatCat.Testing/Strings/StringMatchEquivalentOfTests.cs`
- `Tests.FatCat.Testing/Strings/NullableStringMatchEquivalentOfTests.cs`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog` → `### Strings`** — add:

| Assertion | What it asserts |
|---|---|
| `MatchEquivalentOf(pattern)` | the value matches the wildcard `pattern`, ignoring case |

And confirm/sharpen the ADR-4 note in that subsection: `Match` is a **wildcard** match (`*` = any run of
characters, `?` = any single character) and takes an `Options` argument; `MatchEquivalentOf` is the same
match, always case-insensitive; `MatchRegex` takes a real regular expression; and on numeric families
`Match` takes a **predicate** instead. Cross-link the `### Numbers` subsection.

**`MIGRATION.md` → `## 3. Mapping Table`** — append (and flip the `⬜ pending G9` row seeded in phase 01):

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `.Should().MatchEquivalentOf(p)` | `.Should().MatchEquivalentOf(p)` | ✅ supported | `Tests.FatCat.Testing.Strings.StringMatchEquivalentOfTests` |
| `.Should().NotMatchEquivalentOf(p)` | `.Should().Not.MatchEquivalentOf(p)` | ✅ supported | `Tests.FatCat.Testing.Strings.StringMatchEquivalentOfTests` |
| `.Should().Match(p)` *(wildcard)* | `.Should().Match(p)` | ✅ supported | `Tests.FatCat.Testing.Strings.StringMatchTests` |
| `.Should().NotMatch(p)` | `.Should().Not.Match(p)` | ✅ supported | `Tests.FatCat.Testing.Strings.StringMatchTests` |

Verify `StringMatchTests` actually covers the `.Not.Match` path before citing it for those last two rows.
If it does not, either add the missing facts to that class in this phase or mark the row `⬜ pending` —
do not cite a test that does not prove the claim.

**`MIGRATION.md` → `## 5. Known Unsupported`** — add a short "`Match` means two things" note carrying the
table from the Design section above, so a reader porting numeric `Match` calls is not misled.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~MatchEquivalentOf"
```

Then run the standards review on the uncommitted change and resolve every finding — **except** the
`string?` finding, which is the recorded deviation above. Note it in the report; do not "fix" it by
introducing CS8625.

---

## Definition of Done

- [ ] Tests written before implementation; red state observed and recorded.
- [ ] `MatchEquivalentOf` on both string comparers, no `Options` parameter (ADR-5).
- [ ] Case-insensitivity proven by a test that distinguishes it from `Match`.
- [ ] Null-subject behaviour tested on both the positive and negated forms.
- [ ] `Match` semantics documented in README (both string and numeric) and in `MIGRATION.md` §5.
- [ ] No new compiler warnings — in particular no CS8625 from a non-nullable `because` inside the
      `#nullable enable` region.
- [ ] No banned patterns other than the recorded `string?` deviation.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` and `MIGRATION.md` updated as specified.
- [ ] Deviation log records the `string?` decision with a pointer to OQ-2.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/04-string-match-equivalent-of.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-04-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Independent of every phase except 01 (which it appends to) and 14 (which audits it). If 14 has landed,
revert it first. No manual steps.

---

## Hand-off

Public surface added:

```csharp
namespace FatCat.Testing.Strings;

NullableStringComparer.MatchEquivalentOf(string pattern, string? because = null)
NotNullableStringComparer.MatchEquivalentOf(string pattern, string? because = null)
```

Reachable from `"any string".Should()` and `((string)null).Should()`.

Contracts later phases must respect:

- `Match` is wildcard on strings and predicate on numerics, permanently (ADR-4). A future phase adding
  `Match` to a new family must pick one and say which in the README.
- `…EquivalentOf` in a method name means case-insensitive with no `Options` parameter (ADR-5). Note that
  the pre-existing `StartWithEquivalentOf` / `EndWithEquivalentOf` violate that convention — OQ-1.
- The `Strings/` folder remains the library's only `#nullable enable` region. Do not spread it.
