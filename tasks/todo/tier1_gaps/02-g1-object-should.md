# Phase 02 — G1: `Should()` for reference types / objects

- **Gap:** G1 (`gaps.md` §3). Unblocks ~1,000 call sites: object `Be` (712), `NotBeNull` (216),
  `BeNull` (111), `BeSameAs`/`NotBeSameAs` (10), object `BeOfType`.
- **Depends on:** Phase 01 (formatting).
- **Depended on by:** Phase 03 (G3 adds `BeEquivalentTo` to this comparer), Phase 04 (G4 overload interplay),
  Phase 05 (G5 documents this comparer's base).
- **Risk:** **HIGH** — changes the **public API contract** (`ShouldExtensions`) and hinges on overload
  resolution against the existing enum generic. Flagged for extra human review per `task.md`.

## Context (complete handoff)

`ShouldExtensions` has 27 concrete overloads plus `Should<T>(this T subject) where T : struct, Enum`
(`FatCat.Testing/ShouldExtensions.cs:68`). There is no entry point for a reference type. The hard part —
and the reason `gaps.md` says "prototype overload resolution first" — is that **a second generic
`Should<T>(this T)` cannot exist**: constraints are not part of the signature, so it is a CS0111 duplicate
member against the enum generic. **ADR-001 in `00-overview.md` resolves this: the object entry point is the
non-generic `Should(this object subject)`.** Read ADR-001 before writing any code.

## Deliverable

New folder `FatCat.Testing/Objects/` mirroring the comparer symmetry (ADR-003):

- `ObjectComparer` — `Subject` typed `object`; primary constructor forwards to
  `ComparerBase<object, ObjectComparer>`. Exposes `Not` → `NotObjectComparer`.
- `NotObjectComparer` — negated form, `NotComparerBase<object, NotObjectComparer>`.

Assertions on `ObjectComparer` (each returns `this`, each takes trailing `string because = null`, each
formats values via `ValueFormatter.Format`):

| Method | Passes when | Message (proposed — pinned by test) |
|---|---|---|
| `Be(object expected)` | `Equals(Subject, expected)` (see OQ-1) | `{Format(Subject)} should be {Format(expected)}` |
| `BeNull()` | `Subject is null` | `{Format(Subject)} should be null` |
| `BeSameAs(object expected)` | `ReferenceEquals(Subject, expected)` | `{Format(Subject)} should be the same instance as {Format(expected)}` |
| `Satisfy(Action<object>)` | inspector does not throw | (delegates; inherited shape) |

`NotObjectComparer` gets `Be`, `BeNull` (i.e. `NotBeNull` → `Not.BeNull()`), `BeSameAs` — the negated forms
needed by the call-site inventory. **`BeEquivalentTo` is intentionally NOT in this phase — it is added to
`ObjectComparer`/`NotObjectComparer` by Phase 03** (deep equality needs the G3 machinery). Do not stub it.

The `Should(this object subject)` overload goes in `ShouldExtensions.cs`. **Guard the overload interplay
with tests** (this is where G1 breaks if wrong):

- `"text".Should()` still returns `NullableStringComparer`, not `ObjectComparer` (string overload wins).
- `5.Should()` still returns `NumericComparer<int>`; `SomeEnum.X.Should()` still returns `EnumComparer<T>`.
- `new SomeDto().Should()` returns `ObjectComparer`.
- A `null` reference-typed subject reaches `ObjectComparer` and `Not.BeNull()` fails, `BeNull()` passes —
  extension methods are the one place a null subject is expected (`types.md`).

## OQ-1 to resolve here

`Be` uses `object.Equals`. Prototype against a sample of real Fog/Toolkit `Be` sites to confirm value
equality (not reference) is intended for the 712 sites. **If a material fraction rely on reference identity,
stop and ask** before finalising — it changes what `Be` means. Assumed answer: `Equals`.

## TDD — tests first

`Tests.FatCat.Testing/Objects/` mirrors the source folder. One class per assertion method
(`testing.md` layout), `BaseTest`-derived, `[Fact]` only, no underscores. Required per method (full set):
`Good<M>`, `Bad<M>`, `Bad<M>ShowsCorrectMessage`, `Bad<M>WithBecause`, and the `Not` equivalents.

- `ObjectBeTests`, `ObjectBeNullTests`, `ObjectBeSameAsTests`, `ObjectSatisfyTests`
- Overload-resolution guard tests (a small `ObjectShouldOverloadTests` or in each type's existing tests):
  the five binding cases above.
- **Migration-proof tests** (per §5.5): the FluentAssertions-shaped call rewritten per §5.2 must compile and
  pass — `subject.Should().Not.BeNull()`, `subject.Should().Not.BeSameAs(other)`, `subject.Should().Be(x)`.

## Migration obligation

Create `MIGRATION.md` at repo root (first phase to need it). Add the G1 rows from `gaps.md` §5.2:
`NotBeNull` → `Not.BeNull()` (216), `NotBeSameAs` → `Not.BeSameAs(x)` (6), `NotBe` → `Not.Be(x)` (1),
object `Be`/`BeNull`. Seed the file with the §5.1 intro and the mapping-table header so later phases append.

## Verification

`dotnet build`, `dotnet test`, `dotnet format style --verify-no-changes`, `dotnet csharpier .` (from repo
root). Confirm the full library still compiles — this phase touches the shared `ShouldExtensions`.

## Definition of Done

- [ ] ADR-001 honoured: non-generic `Should(this object)`; no second `Should<T>(this T)`.
- [ ] `ObjectComparer`/`NotObjectComparer` with `Be`, `BeNull`, `BeSameAs`, `Satisfy` (+ `Not` forms), full
      test set each, all green.
- [ ] Five overload-resolution guard tests green.
- [ ] `BeEquivalentTo` NOT present (deferred to 03).
- [ ] `MIGRATION.md` created with G1 rows + table header.
- [ ] OQ-1 resolved and recorded in the phase report.
- [ ] All `00-overview.md` DoD gates met; one commit `[tier1_gaps 02] G1 object Should()`.

## Rollback Procedure

`git revert <phase-02-commit>`. Manual: remove the G1 section from `MIGRATION.md` if the revert leaves it
orphaned (or revert the doc edit in the same revert). Reverting 02 **requires reverting 03, 04, 05 first**
(they build on `ObjectComparer` / the object entry point).

## Hand-off (contract exposed to later phases)

- `FatCat.Testing.Objects.ObjectComparer` / `NotObjectComparer` — the reference-type comparers. Phase 03
  **extends** these with `BeEquivalentTo`; do not create a new comparer for it.
- `ShouldExtensions.Should(this object subject) : ObjectComparer` — the reference-type entry point. Phase 04
  must not let its `IEnumerable<T>` overload shadow this for plain objects (verified by 04's tests).
- `MIGRATION.md` exists at repo root with the §5.2 table header for later phases to append to.
