# Phase 11 — `Type` Family

- **Work item:** `tier_2_gaps`
- **Gap:** **G10** (gaps.md §3, Tier 2 — missing type families); touches the edge of **G20**
- **Risk:** **medium.** Reference-type entry point in the same overload space as G1's future generic
  `Should<T>(this T)`, and `Type` is the one family where scope creep is a real danger — G20 is a whole
  architecture-testing DSL that this phase must not start building.
- **Depends on:** 01
- **Depended on by:** 14
- **Precondition:** **OQ-4 answered** — the surface below is a proposal until a human confirms it. See
  [00-overview.md](00-overview.md).
- **Branch:** the task branch (see [orchestrator.md](orchestrator.md))
- **Commit:** exactly one

---

## Context — read this first

You have no state from any prior session.

Reference-type families in this library ship **two** comparers — `<Type>Comparer` and `Not<Type>Comparer`
(ADR-7 in [00-overview.md](00-overview.md)) — because the subject is already nullable. If phase 10
(`Uris/`) has landed, open it: it is the reference-type template, including the private `SubjectDisplay`
property and the rule that a null subject fails every assertion except `BeNull`. If it has not landed, use
`FatCat.Testing/Strings/NullableStringComparer.cs` for the null-handling shape and ignore its `Nullable`
prefix (a historical artifact) and its `#nullable enable` region (OQ-2 — do not copy either).

`ComparerBase` already provides `BeOfType`, `BeAssignableTo`, `BeOneOf`, and `Satisfy` to every comparer.
Those assert on the *runtime type of the subject* — for a `Type` subject that means `System.RuntimeType`,
which is almost never what a caller wants. This phase adds assertions **about the type the subject
represents**. Document the distinction in the README; it is the single most confusing thing about this
family.

FluentAssertions' `Type` surface is large (gaps.md §6.2, G20): `BeDecoratedWith<T>`, `BeAbstract`,
`BeStatic`, `BeSealed`, `BeDerivedFrom<T>`, `Implement<T>`, `BeInNamespace`, `HaveAccessModifier`, plus
method/property/assembly assertions and the whole `Types.InAssembly(...).ThatAre*` selection DSL. gaps.md
rates G20 "Large. Architecture-test territory. Skip unless wanted." **This phase ships the small core and
nothing else.**

Read before starting: `.claude/rules/csharp/naming-and-structure.md`, `types.md`, `testing.md`,
`not-allowed.md`, `errors.md`, and [00-overview.md](00-overview.md).

---

## Scope

**In scope** — `Types/` folder with `TypeComparer` + `NotTypeComparer`, one `ShouldExtensions` overload,
full test set, doc updates.

**Out of scope — do not build any of this**

- The `Types.InAssembly(...)` selection DSL, method assertions, property assertions, assembly assertions.
- `HaveAccessModifier` — it needs an access-modifier enum and a public/protected/internal decision matrix.
  Excluded by OQ-4's proposal; if the human wants it, it is a follow-up phase.
- `Should(this Task<Type>)`.
- Any `NullableTypeComparer`.

---

## Design

**Folder and namespace** — `FatCat.Testing/Types/`, namespace `FatCat.Testing.Types`; tests in
`Tests.FatCat.Testing/Types/`, namespace `Tests.FatCat.Testing.Types` (ADR-10).

The namespace `FatCat.Testing.Types` is close to the `types.md` rule file and to `System.Type` itself — but
ADR-10's plural-of-the-type-name convention is what every other family uses, and inventing
`Reflection/` here would break it. Keep `Types/` and add `using FatCat.Testing.Types;` to
`ShouldExtensions.cs` like any other family.

```csharp
public class TypeComparer(Type subject) : ComparerBase<Type, TypeComparer>(subject)
{
	public NotTypeComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get { return Subject == null ? "null" : Subject.FullName; }
	}
	…
}
```

`SubjectDisplay` uses `FullName`, not `Name` — `MyApp.Models.User` beats `User` in a failure message. Note
that `FullName` is null for open generic parameters; fall back to `Subject.Name` when it is. Pin that with
a test.

**Assertion surface** (the OQ-4 proposal)

| Assertion | Fails when | Message |
|---|---|---|
| `Be(expected)` | null, or `Subject != expected` | `{SubjectDisplay} should be {expected}` |
| `Be<T>()` | as above with `typeof(T)` | same |
| `BeNull()` | `Subject != null` | `{SubjectDisplay} should be null` |
| `HaveValue()` | `Subject == null` | `subject should have a value` |
| `BeAbstract()` | null, or `!Subject.IsAbstract` or the type is static | `{SubjectDisplay} should be abstract` |
| `BeSealed()` | null, or `!Subject.IsSealed` or the type is static | `{SubjectDisplay} should be sealed` |
| `BeStatic()` | null, or not (`IsAbstract && IsSealed`) | `{SubjectDisplay} should be static` |
| `BeDerivedFrom(expected)` / `BeDerivedFrom<T>()` | null, or `expected` is not a **strict** base class | `{SubjectDisplay} should be derived from {expected}` |
| `Implement(expected)` / `Implement<T>()` | null, or the interface is not implemented | `{SubjectDisplay} should implement {expected}` |
| `BeInNamespace(expected)` | null, or `Subject.Namespace != expected` | `{SubjectDisplay} should be in namespace {expected}` |
| `BeDecoratedWith<T>()` | null, or no `T` attribute present | `{SubjectDisplay} should be decorated with {typeof(T).Name}` |

Semantics that need deciding once and pinning with tests:

- **Static classes** are compiled as `abstract sealed`. So `BeStatic()` is `IsAbstract && IsSealed`, and
  `BeAbstract()` / `BeSealed()` must **exclude** static classes, or `typeof(SomeStaticClass).Should()
  .BeAbstract()` passes and reads as a lie. Three tests pin this: a static class fails `BeAbstract`, fails
  `BeSealed`, and passes `BeStatic`.
- **`BeDerivedFrom` is strict.** `typeof(User).Should().BeDerivedFrom<User>()` **fails** — a type is not
  derived from itself. Use `IsAssignableFrom` plus an inequality check, or `IsSubclassOf`, and pin it.
- **`BeDerivedFrom` rejects interfaces.** Passing an interface type is API misuse, not an assertion
  failure: throw `ArgumentException` with a message pointing at `Implement` (`errors.md` — BCL exceptions
  for misuse, `CompareException` only for failed assertions). Pin it with `Assert.Throws<ArgumentException>`.
- **`Implement` requires an interface.** Same rule: a non-interface argument throws `ArgumentException`.
- **`BeDecoratedWith<T>` uses `Attribute.IsDefined(Subject, typeof(T))`** and does **not** walk the
  inheritance chain by default. Pin both the present and inherited cases so the choice is explicit.

`NotTypeComparer` mirrors every assertion with `should not …`. The `ArgumentException` guards apply there
too — misuse is misuse regardless of direction.

**Entry point** — in `ShouldExtensions.cs`:

```csharp
public static TypeComparer Should(this Type subject) { return new TypeComparer(subject); }
```

This is unambiguous today. Note that `typeof(X).Should()` now returns `TypeComparer` rather than binding
any generic — and that `someObject.GetType().Should()` does too, which is usually what a caller wants.

---

## TDD Steps

Test fixtures: the test project needs a handful of purpose-built types to assert against. Put them in
`Tests.FatCat.Testing/Types/` as separate files, one class per file — an abstract class, a sealed class, a
static class, an interface, a class implementing it, a derived class, and a class carrying an attribute.
Name them for what they are (`AbstractFixture`, `SealedFixture`, `StaticFixture`, `MarkerInterface`,
`MarkerAttribute`, …). Do not cram them into one file; the one-class-per-file rule has no test exemption.

One test class per assertion method — `Type<Method>Tests`, deriving `BaseTest`, no fields, no constructor:
`Good<Method>`, `Bad<Method>`, `Bad<Method>ShowsCorrectMessage`, `Bad<Method>WithBecause`, plus the four
`Not` equivalents, plus `Bad<Method>WhenNull` (declare `Type value = null;`).

Plus the semantic pins listed in Design:

- `BadBeAbstractWhenStatic`, `BadBeSealedWhenStatic`, `GoodBeStaticWhenStatic`
- `BadBeDerivedFromWhenSameType`
- `BadBeDerivedFromWhenInterface` → `Assert.Throws<ArgumentException>`
- `BadImplementWhenNotAnInterface` → `Assert.Throws<ArgumentException>`
- `BadBeDecoratedWithWhenInherited` (or `Good…`, whichever the chosen semantics dictate — pin the decision)
- `GoodBeShowsFullNameInMessage` / the open-generic `FullName`-is-null fallback

Work assertion by assertion: red, green, next.

---

## Files

**Added**

- `FatCat.Testing/Types/TypeComparer.cs`
- `FatCat.Testing/Types/NotTypeComparer.cs`
- `Tests.FatCat.Testing/Types/` — one class per assertion method, plus one file per test fixture type

**Changed** — `FatCat.Testing/ShouldExtensions.cs`, `README.md`, `MIGRATION.md`

---

## Documentation Updates

**`README.md` → `## Assertion Catalog`** — new `### Types` subsection, alphabetically placed (after
`### TimeSpans`, before `### Uris`). Include the assertion table and, prominently:

- `BeOfType` / `BeAssignableTo` from the shared base assert on the *runtime type of the subject* — for a
  `Type` subject that is `System.RuntimeType`, which is not what you want. Use `Be` / `BeDerivedFrom` /
  `Implement` instead. Show a one-line example of the trap.
- static classes: `BeStatic` passes, `BeAbstract` and `BeSealed` fail.
- `BeDerivedFrom` is strict and rejects interfaces (throws `ArgumentException`); `Implement` requires one.
- `BeDecoratedWith<T>` does not walk the inheritance chain (or does — state whichever was implemented).
- what is deliberately **not** here: the assembly/method/property surface and the `Types.InAssembly` DSL
  (G20), and `HaveAccessModifier`.

**`README.md` → `## Coverage Status`** — flip the `Type` row to `✅ shipped`, with a note that it is the
small core of G20, not all of it.

**`MIGRATION.md` → `## 3. Mapping Table`** — one row per assertion:

| FluentAssertions | FatCat.Testing | Status | Proven by |
|---|---|---|---|
| `type.Should().BeDerivedFrom<T>()` | same | ✅ supported | `Tests.FatCat.Testing.Types.TypeBeDerivedFromTests` |
| `type.Should().Implement<T>()` | same | ✅ supported | `Tests.FatCat.Testing.Types.TypeImplementTests` |
| `type.Should().NotBeAbstract()` | `.Should().Not.BeAbstract()` | ✅ supported | `Tests.FatCat.Testing.Types.TypeBeAbstractTests` |
| … one row per remaining assertion … | | | |
| `type.Should().HaveAccessModifier(x)` | — | ❌ not supported | — |
| `Types.InAssembly(a).ThatAre…` | — | ❌ not supported | — |

**`MIGRATION.md` → `## 5. Known Unsupported`** — add the G20 surface this phase does not cover, with the
recommended rewrite (assert on the specific types you care about, one at a time).

**`MIGRATION.md` → `## 4. Type Coverage`** — mark `Type` partially supported and say what is missing.

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
dotnet test Fatcat.Testing.sln --filter "FullyQualifiedName~Tests.FatCat.Testing.Types"
```

Then run the standards review on the uncommitted change and resolve every finding — the fixture types make
the one-class-per-file rule the likeliest trip hazard here.

---

## Definition of Done

- [ ] OQ-4 answered before implementation started; the confirmed surface is recorded in the phase report.
- [ ] Tests written before implementation, assertion by assertion; red states observed.
- [ ] **Two** comparers only. No nullable pair.
- [ ] Every assertion guards a null subject and produces a `CompareException`.
- [ ] `ArgumentException` (not `CompareException`) for `BeDerivedFrom(interface)` and
      `Implement(non-interface)`, on both the positive and negated comparers, proven by tests.
- [ ] Static-class semantics pinned by three tests.
- [ ] `BeDerivedFrom` strictness pinned by a test.
- [ ] `BeDecoratedWith<T>` inheritance semantics pinned by a test and documented.
- [ ] `SubjectDisplay` uses `FullName` with a `Name` fallback, pinned by a test.
- [ ] Fixture types are one class per file.
- [ ] No new compiler warnings; no banned patterns; namespaces match folders.
- [ ] `dotnet test` green.
- [ ] `dotnet format` run; `dotnet csharpier .` run **last**.
- [ ] `README.md` (including the `BeOfType` trap) and `MIGRATION.md` (including the not-supported rows)
      updated.
- [ ] Exactly one commit referencing `tasks/todo/tier_2_gaps/11-type-family.md`.

---

## Rollback Procedure

```pwsh
git revert --no-edit <phase-11-commit>
dotnet build Fatcat.Testing.sln
dotnet test Fatcat.Testing.sln
```

Self-contained. Only phase 14 depends on it — revert that first if it landed. No manual steps.

---

## Hand-off

```csharp
namespace FatCat.Testing.Types;
TypeComparer / NotTypeComparer

namespace FatCat.Testing;
Should(this Type) -> TypeComparer
```

**Contracts for later phases**

- `typeof(X).Should()` and `instance.GetType().Should()` both now return `TypeComparer`. Any future
  assertion that wants the *runtime* type of a `Type` object must say so explicitly.
- The `ArgumentException`-for-misuse precedent (`errors.md`) is now applied in a comparer. Later families
  facing the same "wrong kind of argument" situation follow it rather than inventing a `CompareException`.
- **For the G1 plan:** `Should(this Type)` is a concrete reference-type overload that will shadow the
  generic `Should<T>(this T)`. Same note as `Uri` (phase 10) and `Stream` (phase 12) — **carry all three
  into the G1 collision inventory.**
- **For any future G20 work:** this phase is the foundation. Assembly, method, and property assertions
  extend `Types/` rather than replacing it.
