# Phase 10 — `Uri` Family

- **Work item:** `tier_2_gaps`
- **Gap:** **G10** (gaps.md §3, Tier 2 — missing type families)
- **Risk:** **medium.** First **reference-type** family in this plan. `Should(this Uri)` is a public entry
  point on a reference type, which is the same overload space G1's generic `Should<T>(this T)` will occupy
  — the Hand-off carries a note the G1 plan must consume.
- **Depends on:** 01
- **Depended on by:** 14
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Value-type families in this library ship four comparers. **Reference-type families ship two** — ADR-7 in
[00-overview.md](00-overview.md) — because the subject is already nullable and there is no separate
nullable form to model. The precedent is `FatCat.Testing/Strings/`: `NullableStringComparer` and
`NotNullableStringComparer` are the *only* string comparers, and both tolerate a null subject via a private
`SubjectDisplay` property that renders `"null"`.

For `Uri`, ADR-7 names the classes `UriComparer` / `NotUriComparer` — the `Nullable` prefix on the string
pair is a historical artifact, not the pattern to copy. What **is** copied from `Strings/` is the null
handling: the positive comparer fails every assertion when the subject is null, and `SubjectDisplay`
renders `"null"` in messages.

`Uri` has no comparer and no `Should()` overload today. FluentAssertions has no `Uri` assertions either, so
this surface is FatCat's own design; nothing constrains it except internal consistency. No consuming-repo
call site needs it (gaps.md A3).

Read before starting: `FatCat.Testing/Strings/NullableStringComparer.cs` and
`NotNullableStringComparer.cs` in full, plus `.claude/rules/csharp/naming-and-structure.md`, `types.md`,
`testing.md`, `not-allowed.md`, and [00-overview.md](00-overview.md).

---

## Scope

**In scope** — `Uris/` folder with `UriComparer` + `NotUriComparer`, one `ShouldExtensions` overload, full
test set, doc updates.

**Out of scope**

- `Should(this Task<Uri>)` — optional per phase 06's Hand-off; skip it and say so in the README.
- Any `NullableUriComparer` — there is no `Uri?`. Creating one is a rule violation, not a nicety.
- URI *building* or normalization helpers. This is an assertion library.
- `#nullable enable` in the new files. The `Strings/` folder is the library's only nullable region and must
  stay that way (OQ-2). Write `string because = null`.

---

## Design

**Folder and namespace** — `FatCat.Testing/Uris/`, namespace `FatCat.Testing.Uris`; tests in
`Tests.FatCat.Testing/Uris/`, namespace `Tests.FatCat.Testing.Uris` (ADR-10).

```csharp
public class UriComparer(Uri subject) : ComparerBase<Uri, UriComparer>(subject)
{
	public NotUriComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get { return Subject == null ? "null" : $"{Subject}"; }
	}
	…
}
```

Note the block-bodied property getter — `Strings/` uses an expression body (`get => …`) inside its
`#nullable enable` region, but expression-bodied members are banned (`not-allowed.md`) and this file is new
code with no excuse.

**Assertion surface**

| Assertion | Fails when | Message |
|---|---|---|
| `Be(expected)` | `Subject == null` or `Subject != expected` | `{SubjectDisplay} should be {expected}` |
| `BeNull()` | `Subject != null` | `{SubjectDisplay} should be null` |
| `HaveValue()` | `Subject == null` | `subject should have a value` |
| `BeAbsolute()` | null, or `!Subject.IsAbsoluteUri` | `{SubjectDisplay} should be absolute` |
| `BeRelative()` | null, or `Subject.IsAbsoluteUri` | `{SubjectDisplay} should be relative` |
| `HaveScheme(expected)` | null, not absolute, or scheme differs | `{SubjectDisplay} should have scheme {expected}` |
| `HaveHost(expected)` | null, not absolute, or host differs | `{SubjectDisplay} should have host {expected}` |
| `HavePort(expected)` | null, not absolute, or port differs | `{SubjectDisplay} should have port {expected}` |
| `HavePath(expected)` | null, not absolute, or `AbsolutePath` differs | `{SubjectDisplay} should have path {expected}` |
| `HaveQuery(expected)` | null, not absolute, or `Query` differs | `{SubjectDisplay} should have query {expected}` |
| `HaveFragment(expected)` | null, not absolute, or `Fragment` differs | `{SubjectDisplay} should have fragment {expected}` |

`HaveValue`'s message is copied verbatim from `NullableStringComparer.HaveValue` — `subject should have a
value`, lowercase, with no subject interpolation. Verify it in source before pinning.

**The relative-URI trap.** Reading `Scheme`, `Host`, `Port`, `AbsolutePath`, `Query`, or `Fragment` on a
relative `Uri` throws `InvalidOperationException`. Every `Have*` assertion must guard with
`Subject.IsAbsoluteUri` **before** touching those properties and fail as a `CompareException`, never let
the BCL exception escape. A relative subject failing `HaveScheme` is an assertion failure, not API misuse.
Pin this with a test per `Have*` method.

Scheme and host comparison is **case-insensitive** (`Uri` normalizes both to lowercase); path, query, and
fragment are **case-sensitive**. Document that in the README and pin it with tests.

`NotUriComparer` mirrors every assertion with `should not …`, derives from
`NotComparerBase<Uri, NotUriComparer>`, and has **no** `Not` property of its own.

**Entry point** — in `ShouldExtensions.cs`:

```csharp
public static UriComparer Should(this Uri subject) { return new UriComparer(subject); }
```

Overload resolution: no existing overload accepts `Uri`, and the enum generic is constrained to
`struct, Enum`, so this binds unambiguously today. It will interact with G1 — see Hand-off.

---

## TDD Steps

Explicit literals: `new Uri("https://example.com:8080/api/items?page=2#top")`,
`new Uri("/api/items", UriKind.Relative)`.

One test class per assertion method — `Uri<Method>Tests`, deriving `BaseTest`, no fields, no constructor.
Each carries `Good<Method>`, `Bad<Method>`, `Bad<Method>ShowsCorrectMessage`, `Bad<Method>WithBecause`, and
the four `Not` equivalents.

Additional required facts:

- `Bad<Method>WhenNull` on every assertion except `BeNull` — a null subject fails.
- `GoodBeNullWhenNull` and `BadBeNullWhenNotNull` in `UriBeNullTests`.
- `Bad<Method>WhenRelative` on each `Have*` — proves the guard produces a `CompareException` and not an
  `InvalidOperationException`. Use `RunCompareFailTest`, which asserts the exception type.
- `GoodHaveSchemeIgnoresCase` and `GoodHaveHostIgnoresCase`.
- `BadHavePathIsCaseSensitive`.

Declare the subject as `Uri value = null;` for the null cases so the overload resolves.

Work assertion by assertion: red, green, next.

---

## Files

**Added**

- `FatCat.Testing/Uris/UriComparer.cs`
- `FatCat.Testing/Uris/NotUriComparer.cs`
- `Tests.FatCat.Testing/Uris/` — one class per assertion method

**Changed** — `FatCat.Testing/ShouldExtensions.cs`, `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog`** — new `### Uris` subsection, alphabetically placed (after
`### TimeSpans`). Include the assertion table, and state:

- a null `Uri` fails every assertion except `BeNull`;
- `Have*` on a relative URI fails as an assertion rather than throwing;
- scheme and host compare case-insensitively; path, query, and fragment case-sensitively;
- there is no `Nullable` comparer for `Uri` because the subject is already nullable — the same shape the
  `string` family uses;
- whether the `Task<Uri>` form exists (it does not, unless you shipped it).

**`README.md` → `## Coverage Status`** — flip the `Uri` row to `✅ shipped`.

**`MIGRATION.md` → `## 3. Mapping Table`** — FluentAssertions has no `Uri` assertions, so the rows document
the **rewrite** rather than a one-to-one mapping. Lead with that sentence, then:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `uri.Should().Be(x)` *(object equality)* | `uri.Should().Be(x)` | ✅ supported | `Tests.FatCat.Testing.Uris.UriBeTests` |
| `uri.Should().NotBeNull()` | `uri.Should().Not.BeNull()` | ✅ supported | `Tests.FatCat.Testing.Uris.UriBeNullTests` |
| `uri.Scheme.Should().Be("https")` | `uri.Should().HaveScheme("https")` | ✅ supported | `Tests.FatCat.Testing.Uris.UriHaveSchemeTests` |
| `uri.Host.Should().Be("example.com")` | `uri.Should().HaveHost("example.com")` | ✅ supported | `Tests.FatCat.Testing.Uris.UriHaveHostTests` |
| … one row per remaining assertion … | | | |

**`MIGRATION.md` → `## 4. Type Coverage`** — mark `Uri` supported.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Uris"
```

Then run the standards review on the uncommitted change and resolve every finding.

---

## Definition of Done

- [ ] Tests written before implementation, assertion by assertion; red states observed.
- [ ] **Two** comparers only — `UriComparer` and `NotUriComparer`. No nullable pair.
- [ ] Every assertion guards a null subject and produces a `CompareException`.
- [ ] Every `Have*` guards `IsAbsoluteUri` and produces a `CompareException`, proven by a test —
      no `InvalidOperationException` escapes.
- [ ] Case sensitivity documented and pinned by tests.
- [ ] No `#nullable enable` and no `string?` in the new files.
- [ ] Block-bodied property getters — no expression bodies anywhere.
- [ ] One `Should()` overload added and unambiguous.
- [ ] No new compiler warnings; namespaces match folders.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` and `MIGRATION.md` updated.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/10-uri-family.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-10-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Self-contained. Only phase 14 depends on it — revert that first if it landed. No manual steps.

---

## Hand-off

```csharp
namespace FatCat.Testing.Uris;
UriComparer / NotUriComparer

namespace FatCat.Testing;
Should(this Uri) -> UriComparer
```

**Contracts for later phases**

- The reference-type shape is now established: two comparers, `SubjectDisplay` renders `"null"`, a null
  subject fails everything except `BeNull`. Phases 11 (`Type`) and 12 (`Stream`) follow it exactly.
- **For the G1 plan:** `Should(this Uri)` is a concrete overload on a reference type. When G1 adds
  `Should<T>(this T subject)`, the concrete overload wins for `Uri` — which is the intent, but it means a
  `Uri` never reaches `ObjectComparer<T>` and therefore never gets `BeEquivalentTo` or `BeSameAs` unless
  those are added to `UriComparer` too. **Carry this into the G1 plan's collision inventory**, along with
  the same note for `Type` (phase 11) and `Stream` (phase 12).
