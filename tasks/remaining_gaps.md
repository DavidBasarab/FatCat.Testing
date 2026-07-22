# FatCat.Testing — Remaining Gaps

Companion to [gaps.md](gaps.md). That file is the full audit of both consumer repos. **This file is the
subset that still has no plan**, written so each entry can be turned directly into a phased plan under
`tasks/todo/`.

Goal unchanged: FatCat.Testing replaces FluentAssertions in `C:\Code\FatCat.Toolkit` and `C:\Code\Fog`.
FluentAssertions 7.0.0 is the last Apache-2.0 release; 8.x is commercially licensed and both repos are
pinned at 7.0.0.

Gap IDs are carried over from `gaps.md` unchanged so the two files cross-reference. `G26` is new.

---

## 1. Scope

**In scope here — no plan exists yet:**

| ID | Gap | Tier |
|---|---|---|
| G15 | Value formatting engine | prerequisite |
| G1 | `Should()` for reference types / objects | 1 |
| G5 | Custom-assertion extension point | 1 |
| G4 | Collection assertions | 1 |
| G3 | Structural `BeEquivalentTo` | 1 |
| G6 | Exception assertions | 1 |
| G2 | The negation codemod + `MIGRATION.md` | migration workstream |
| G17 | Custom-assertion authoring primitives | optional |
| G16 | Test-framework coupling (xUnit lock) | decision |
| G26 | Method-level completeness in families already covered | closing |

**Already planned — do not duplicate.** `tasks/todo/tier_2_gaps/` covers G7 (`Task<T>` overloads), G8
(numeric `…OrEqualTo`), G9 (string `MatchEquivalentOf`), G10 (`DateTimeOffset`, `DateOnly`, `TimeOnly`,
`Uri`, `Type`, `Stream`, and `IDictionary<K,V>` — the last blocked on G4 here).

**Decided out of scope.** G11 `AssertionScope`, G12 `ExecutionTime`, G13 `.And` chaining, G14
`.Which`/`.Subject`, and G20–G25 (reflection/architecture tests, event monitoring, XML, JSON,
serializability). All have zero or ≤3 usages across both repos. See `gaps.md` §3 Tier 3 and §6.2.

---

## 2. Baseline — re-audited 2026-07-22

**2,580 `.Should()` call sites** — Fog 2,289, Toolkit 291. What the gaps below are worth:

| Gap | Call sites blocked |
|---|---|
| G1 — objects | ~1,100 (object `Be`, `NotBeNull` 261, `BeNull` 131, `BeSameAs`/`NotBeSameAs` 10, object `BeOfType`) |
| G5 — extension point | 367 (`BeOk` 152, `BeNotFound` 47, `BeGet` 45, `BeSuccessful` 37, `BePost` 35, `BeBadRequest` 27, `BeUnauthorized` 11, `BeDelete` 9, `BeUnsuccessful` 4) plus an unknown share of `Be`/`BeEquivalentTo` |
| G4 — collections | ≤344 (`Contain` 223, of which 159 pass a string literal and are already covered) |
| G3 — `BeEquivalentTo` | 241 |
| G6 — exceptions | 9 |
| G2 — negations to rewrite | 304 |

---

## 3. Settled decisions — do not relitigate

**ADR-A — negation stays `.Not.`** `x.Should().Not.Be(y)`, never `NotBe(y)`. No alias methods, not even
`[Obsolete]` shims. This makes G2 a migration-tooling workstream rather than a library gap.

**ADR-B — `BeEquivalentTo` ships default options only.** `Excluding`, `Including`,
`RespectingRuntimeType`, `WithStrictOrdering`, and the other ~45 option methods have **zero** usages in
either repo. Only `Using<T>().WhenTypeIs<T>()` is used (2 sites) and only that hook gets built.

**ADR-C — overload architecture for G1 + G4.** This was prototyped, not reasoned about. Four results:

1. Two `Should<T>(this T)` overloads differing only by constraint **cannot live in the same static class** —
   `CS0111`, because constraints are not part of a signature. The object entry point must go in its own
   static class (e.g. `ObjectShouldExtensions`), separate from the enum-constrained generic in
   `ShouldExtensions`.
2. Across separate static classes, constraint-based filtering resolves cleanly: `where T : struct, Enum`
   and `where T : class` are disjoint, so enums and objects each bind correctly.
3. **A generic `Should<T>(this T) where T : class` silently swallows every collection.** For a
   `List<string>`, binding `T = List<string>` is an *identity* conversion while `IEnumerable<T>` requires a
   reference conversion — identity wins, so `List<T>` and `T[]` both resolve to the object comparer. This
   is the trap in this design and it fails silently, not at compile time.
4. **The fix: G4 must declare concrete-shape overloads** — `Should<T>(this IEnumerable<T>)`,
   `Should<T>(this List<T>)`, `Should<T>(this T[])`. A constructed type beats a bare type parameter in
   overload resolution, so these win. Verified: `Dto` → object, `List<string>` → collection, `string[]` →
   collection, `IEnumerable<string>` → collection, `string` → the existing concrete string overload,
   `int` → the existing numeric overload, an interface-typed reference → object.

**Consequence for sequencing:** `ObjectComparer<T>` can stay generic (`where T : class`), but **G1 must not
ship before G4's overloads exist**, or every collection call site binds to the object comparer and compiles
green while asserting the wrong thing. Either land them together or gate G1's merge on G4.

`Dictionary<K, V>` currently falls to `IEnumerable<KeyValuePair<K, V>>`. Whether a dedicated
`Should<K, V>(this IDictionary<K, V>)` wins over that is **unverified** — probe it in the dictionary phase
(`tier_2_gaps/13`), which is already blocked on G4.

---

## 4. The Gaps

Every gap below inherits the standing obligations from `.claude/rules/csharp/testing.md` and `gaps.md` §5.5:
TDD, the full `Good` / `Bad` / `BadShowsCorrectMessage` / `BadWithBecause` set plus the `Not` equivalents
and the nullable variant, a `README.md` catalog entry, and a `MIGRATION.md` row per replaced
FluentAssertions call naming the test class that proves it.

---

### G15 — Value formatting engine · *sequence first*

**Why it leads.** The library builds every failure message with `$"{subject}"` — plain `ToString()`. That
is fine for `int`, `bool`, `Guid`. It produces garbage the moment G1/G3/G4 land: a `List<string>` renders
as ``System.Collections.Generic.List`1[System.String]`` and a DTO renders as its type name. A
`BeEquivalentTo` failure that cannot name the member that differed is not a usable assertion.

**Build.** A shared formatter used everywhere the library currently interpolates a subject. Minimum viable:
`null` → `"null"`; `string` → quoted; `IEnumerable` → bounded elementwise (FluentAssertions caps at 32);
object → member dump. Today `ComparerBase.FormatSubject()` and `NotComparerBase.FormatSubject()` are
**private and duplicated** — collapse them onto the shared implementation as the first move.

**Hard parts.** Cycle detection (shared with G3). Bounding output so a large collection does not produce an
unreadable message. Deciding whether formatting is public API (it becomes so the moment a consumer writes a
custom comparer — see G17).

**Depends on.** Nothing. **Blocks.** G3, G4 (usefully), G1.

**Done when.** Every existing failure message is unchanged for scalar types (the message text is pinned by
tests — a regression here breaks the suite loudly, which is the point), and collections/objects format
readably.

---

### G1 — `Should()` for reference types

**Why it blocks.** The only generic entry point is `Should<T>(this T) where T : struct, Enum`. There is no
overload for `object`, a DTO, an interface, or any reference type. `result.Should()` on a class does not
compile. This is the single largest gap and G3, G4, and G5 all build on it.

**Build.** `Objects/ObjectComparer.cs` and `Objects/NotObjectComparer.cs`, generic on `T` constrained
`where T : class`, following the `<Type>Comparer` / `Not<Type>Comparer` scheme. Entry point in a **separate
static class** per ADR-C. Assertions: `Be` (via `Equals`), `BeNull`, `BeSameAs`, `Satisfy`, and `BeOfType` /
`BeAssignableTo` / `BeOneOf` inherited from `ComparerBase`. `BeEquivalentTo` arrives with G3.

**Hard parts.** Overload resolution — settled by ADR-C, but re-verify against the real 27-overload
`ShouldExtensions` rather than the prototype. `Be` semantics on reference types: reference equality or
`Equals`? FluentAssertions uses `Equals`, with `BeSameAs` for reference identity — match that.

**Depends on.** G15 (messages), and must land with or after G4 (ADR-C consequence).

**Done when.** `new Dto().Should().Not.BeNull()` compiles and both repos' object call sites bind to
`ObjectComparer<T>` — spot-check that no collection site binds here.

---

### G5 — Custom-assertion extension point

**Why it blocks.** 367 call sites go through **project-defined** assertions. Both repos build them on
`ReferenceTypeAssertions<TSubject, TAssertions>` returning `AndConstraint<T>`:

- Toolkit, in **production** projects: `ToolKit\Testing\WebResultAssertions.cs`,
  `WebResultClosedOverAssertions.cs`, `FatResultAssertions.cs`, and
  `Toolkit.WebServer\Testing\{WebResultAssertions,WebResultClosedOverAssertions,EndpointTestExtensions}.cs`
- Fog: `EndToEndTests\Helpers\Assertions\{WebResultAssertion,UserServiceModelAssertion,LokrServiceModelAssertion}.cs`

**This is smaller than it looks.** Those assertions delegate to inner `.Should()` calls — there is **zero**
dependency on FluentAssertions' `Execute.Assertion` / `AssertionChain` engine. They need only: a public
base class with an accessible `Subject`, and a chainable return. `ComparerBase<TSubject, TComparer>` already
provides both. So G5 is mostly *deciding and documenting* that `ComparerBase` is the supported extension
point — plus resolving the two mismatches below.

**Mismatch 1 — `becauseArgs`.** FluentAssertions takes `string because, params object[] becauseArgs` and
formats. FatCat takes `string because = null` and *replaces* the message. Toolkit's helpers use the args
form: `HaveStatusCode(code, "you cannot test for content from an unsuccessful status code: {0}", Subject.StatusCode)`.
**Recommendation: do not add `becauseArgs`.** Rewrite those call sites to string interpolation, which
`naming-and-structure.md` requires anyway. Needs a decision before the port — see §7.

**Mismatch 2 — `because` semantics.** FluentAssertions *appends* the reason to a generated message; FatCat
*replaces* it. Sites like `.BeTrue(Subject.Content)` still produce a useful message, but the text differs.
Document in `MIGRATION.md` as a known behavioral difference.

**Also.** The Toolkit and Toolkit.WebServer copies are near-duplicates (`FatWebResponse` vs `WebResult`) —
consolidate during the port rather than porting the same code twice.

**Depends on.** G1. **Done when.** A worked example of a custom comparer ships in `README.md`, and one of
the Toolkit assertion classes is ported to it as proof.

---

### G4 — Collection assertions

**Why it blocks.** No `Collections/` folder exists.

**Build.** `Collections/CollectionComparer.cs` + `NotCollectionComparer.cs`, and — per ADR-C, mandatory —
the concrete-shape entry points `Should<T>(this IEnumerable<T>)`, `Should<T>(this List<T>)`,
`Should<T>(this T[])`.

Assertions, by call volume: `Contain` (223 total, 159 of them string-literal and already covered by the
string comparer — the collection form is the remainder), `BeEmpty` (50), `NotContain` (18 → `.Not.Contain`),
`HaveCount` (16), `NotBeEmpty` (9), `ContainSingle` (8), `ContainEquivalentOf` (8), `OnlyContain` (5),
`Equal` (2), `OnlyHaveUniqueItems` (2), `NotContainEquivalentOf` (1), `ContainInOrder` (1),
`BeInDescendingOrder` (1).

**Hard parts.** `string` is `IEnumerable<char>` — the existing concrete `Should(this string)` must keep
winning (verified in the ADR-C probe, but re-verify in place). `ContainEquivalentOf` depends on G3.
Multiple-enumeration of an `IEnumerable<T>` that is a lazy query.

**Depends on.** G15, G1 (ADR-C: land together). **Blocks.** `tier_2_gaps/13` (dictionaries).

---

### G3 — Structural `BeEquivalentTo`

**Why it blocks.** 241 call sites. Today `BeEquivalentTo` exists only on strings, meaning
case-insensitive compare. Objects and collections need recursive member-by-member comparison with cycle
detection and a diff in the failure message naming the member that differed.

**Build.** Default-options path only (ADR-B), plus **the global configuration hook** — Fog registers a
`DateTime` closeness rule in two places and those suites break without an equivalent:

```
Brume\Tests.Brume\BrumeTests.cs:13              options.Using<DateTime>(...).WhenTypeIs<DateTime>()   // 1s
EndToEndTests\Infrastructure\EndToEndTest.cs:28 options.Using<DateTime>(...).WhenTypeIs<DateTime>()   // 10s
```

**Hard parts.** This is the hardest gap in the library. Cycle detection, depth limiting, collection
equivalency (order-insensitive by default in FluentAssertions), and a diff message that is actually
readable — which is why G15 comes first, not after.

**Depends on.** G15, G1, G4 (for collection equivalency).

---

### G6 — Exception assertions

**Why it blocks.** Only 9 call sites, but nothing substitutes for them: `Should().Throw<T>()`,
`ThrowAsync<T>()`, `NotThrow()`, `WithMessage()`.

**Build.** `Should(this Action)` and `Should(this Func<Task>)` overloads plus an exception comparer. Per
`.claude/rules/csharp/async.md` the fluent surface **stays synchronous** — the comparer runs the delegate.

**Depends on.** Nothing. Fully independent — good warm-up or parallel work.

---

### G2 — Negation codemod + `MIGRATION.md` · *migration workstream*

**304 negated call sites** fail to compile as written under ADR-A. The rewrite is mechanical:

```
find:     \.Should\(\)\.Not([A-Z]\w*)\(
replace:  .Should().Not.$1(
```

Deliverables and the cases the regex will not catch are specified in `gaps.md` §5.1–§5.3 — chained
negations, line-broken chains, project-defined names beginning with `Not`, and negations on subjects whose
gap has not landed yet. The script must **report** those rather than skip them silently.

**Per-repo order is forced** (`gaps.md` §5.4): Toolkit first — it references FluentAssertions from
production projects, so replacing it changes the public contract of the Toolkit NuGet package and is its
own release step — then the custom assertion layer (G5), then Fog, which only gets FluentAssertions
transitively through the FatCat.* packages.

**Depends on.** Runs per-repo after G1/G3/G4 land. Not a library gap.

---

### G17 — Custom-assertion authoring primitives · *optional*

FluentAssertions gives extension authors `AssertionChain.GetOrCreate()` with
`ForCondition`/`FailWith`/`BecauseOf`/`Given`, the `[CustomAssertion]` attribute for correct caller
identification in stack traces, and message templating. FatCat offers `CompareException.New(string)`.

Both repos' custom assertions delegate to inner `.Should()` calls, so **G5 alone unblocks them**. The one
thing genuinely missed is `[CustomAssertion]`-equivalent stack-trace handling — what makes a failing custom
assertion point at the *test* line instead of the library line. Judge that on developer experience after
G5 lands, not before.

---

### G16 — Test-framework coupling · *decision, not a task*

`CompareException` derives from `XunitException` and `xunit.assert` is the library's one package reference.
**FatCat.Testing is xUnit-locked.** FluentAssertions auto-detects xUnit2/3, NUnit, MSTest, MSTest4, TUnit,
and MSpec.

Irrelevant to Toolkit and Fog — both are xUnit. It is decisive for the *package claim*: "a free replacement
for FluentAssertions" that only works on xUnit is a narrower promise. If the answer is that it should be
framework-agnostic, the fix is a detection shim that drops the hard `xunit.assert` dependency — and it is
cheaper to do that before the surface grows. See §7.

---

### G26 — Method-level completeness in families already covered · *closing work*

Not migration-blocking — zero call sites in either repo. This is what makes the "replacement" claim true
for a third party. Full list in `gaps.md` §6.3. Highlights:

| Family | Missing |
|---|---|
| DateTimes | The **fluent difference chains** — `BeLessThan(2.Hours()).Before(x)`, `BeWithin(...).After(x)`, `BeMoreThan`, `BeAtLeast`, `BeExactly`. Genuinely nice API and the largest single family gap. Also `BeSameDateAs`, `BeIn(DateTimeKind)`, `BeOneOf` |
| Collections | Beyond G4: `BeSubsetOf`, `IntersectWith`, `AllSatisfy`, `AllBeOfType`, `SatisfyRespectively`, `HaveCountGreaterThan`/`LessThan`, `HaveSameCount`, `ContainNulls`, `ContainMatch`, `HaveElementAt` |
| Exceptions | Beyond G6: `ThrowExactly<T>`, `WithInnerException<T>`, `Where(predicate)`, `WithParameterName`, and the async set incl. `CompleteWithinAsync` |
| Strings | `ContainEquivalentOf`, `NotContainAll`, `NotContainAny` |
| Numerics | `NotBeInRange`, `NotBeApproximately` |
| Enums | `HaveSameNameAs`, `HaveSameValueAs`, `NotBeDefined` |
| Booleans | `Imply` |
| Guids | `Be(string)` overload |

Sequence last. Split by family — each is an independent leaf.

---

## 5. Dependency graph

```
G15 (formatting)
  │
  ├──► G1 (objects) ══╗   ADR-C: G1 and G4 land together, or G1 gates on G4.
  │                   ║   Shipping G1 alone makes every collection call site
  ├──► G4 (collections) ══╝  bind to the object comparer — silently.
  │      │      │
  │      │      └──► tier_2_gaps/13 (dictionaries — currently blocked)
  │      │
  │      └──► G3 (BeEquivalentTo) ◄── G1
  │                  │
  └──────────────────┴──► G5 (extension point) ──► G17 (authoring primitives, optional)
                                │
                                └──► G2 codemod: Toolkit ──► Fog

G6 (exceptions)   independent — any time
G26 (completeness) last, one phase per family
G16 (framework coupling) — a decision, cheapest to make before the surface grows
```

**Suggested phase breakdown for `tasks/todo/`:** G15 alone · G1 + G4 as one plan with a shared
overload-resolution phase · G3 alone (largest) · G5 + the Toolkit port · G6 anytime · G2 as a per-repo
migration plan · G26 as one leaf phase per family.

---

## 6. Exit criteria

The replacement claim is proven when, and only when:

1. `FatCat.Toolkit` builds with **no** `FluentAssertions` `PackageReference` in `ToolKit.csproj`,
   `Toolkit.WebServer.csproj`, or `Tests.ToolKit.csproj`, and its 291 call sites pass.
2. `Fog` builds against that Toolkit with no FluentAssertions anywhere — 6 `GlobalUsings.cs` files swapped,
   the ~27 per-file `using FluentAssertions;` in Fog removed — and its 2,289 call sites pass.
3. Every row in `MIGRATION.md` §5.2 is backed by a test class that compiles the rewritten form
   (`gaps.md` §5.5). A mapping row with no test behind it is a claim, not a guarantee.

---

## 7. Open questions — need a decision before planning

1. **`becauseArgs`** (G5) — add `params object[] becauseArgs` to every assertion, or rewrite the Toolkit
   call sites to string interpolation? *Recommendation: interpolation.* It is what the project rules
   require, and adding the args form doubles the parameter surface of every assertion in the library.
2. **Framework coupling** (G16) — is xUnit-only the intended scope of the shipped package? If not, drop the
   hard `xunit.assert` dependency now rather than after G1/G3/G4 multiply the surface.
3. **Object `Be` semantics** (G1) — confirm `Equals` for `Be` and reference identity for `BeSameAs`,
   matching FluentAssertions.
4. **Does FatCat ship the `Task<T>` overloads, or do the consumers keep theirs?** Open from G7 in
   `tier_2_gaps/05`; the answer depends on the G5 extension point existing.
