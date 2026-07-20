# FatCat.Testing — Gap Analysis vs FluentAssertions

Goal: FatCat.Testing becomes a drop-in free replacement for FluentAssertions in `C:\Code\FatCat.Toolkit`
and `C:\Code\Fog`.

Motivation: FluentAssertions 7.0.0 is the last Apache-2.0 release. 8.x is commercially licensed. Both
consuming repos are pinned at 7.0.0.

Each gap below has an ID (`G1`…`G14`). Create one phased plan per gap.

---

## 1. What Exists Today

`FatCat.Testing` ships 46 source files, one folder per value-type family, with a strictly symmetrical
`<Type>Comparer` / `Not<Type>Comparer` / `Nullable<Type>Comparer` / `NotNullable<Type>Comparer` scheme.
Its only package reference is `xunit.assert`; `CompareException` derives from `XunitException`.

| Family | Comparers | Assertions |
|---|---|---|
| `Booleans/` | 4 | `Be`, `BeTrue`, `BeFalse`, `BeNull`, `HaveValue` |
| `Characters/` | 4 | `Be`, `BeDigit`, `BeLetter`, `BeLetterOrDigit`, `BeLowerCased`, `BeUpperCased`, `BeWhiteSpace`, `BeControl`, `BeNull`, `HaveValue` |
| `DateTimes/` | 4 | `Be`, `BeAfter`, `BeBefore`, `BeOnOrAfter`, `BeOnOrBefore`, `BeCloseTo`, `BeUtc`, `BeLocal`, `HaveYear`…`HaveMillisecond`, `HaveKind`, `HaveOffset` |
| `Doubles/`, `Floats/` | 4 | `BeApproximately`, `BeGreaterThan`, `BeLessThan`, `BeInRange`, `BeNaN`, `BeNegative`, `BePositive`, `BeZero`, `Match` |
| `Enums/` | 4 | `Be`, `BeDefined`, `HaveFlag`, `BeNull`, `HaveValue` |
| `Guids/` | 4 | `Be`, `BeEmpty`, `BeNull`, `HaveValue` |
| `Numbers/` | 4 | `Be`, `BeGreaterThan`, `BeLessThan`, `BeInRange`, `BeAround`, `BeNegative`, `BePositive`, `BeZero`, `Match` |
| `Strings/` | 2 (+5 occurrence helpers) | `Be`, `BeEquivalentTo`, `Contain`, `ContainAll`, `ContainAny`, `StartWith`, `EndWith`, `StartWithEquivalentOf`, `EndWithEquivalentOf`, `Match`, `MatchRegex`, `HaveLength`, `BeEmpty`, `BeNull`, `BeNullOrEmpty`, `BeNullOrWhiteSpace`, `BeLowerCased`, `BeUpperCased`, `HaveValue` |
| `TimeSpans/` | 4 | `Be`, `BeCloseTo`, `BeGreaterThan(OrEqualTo)`, `BeLessThan(OrEqualTo)`, `BeNegative`, `BePositive`, `HaveDays`…`HaveMilliseconds` |
| `Comparers/` (base, all types) | 2 | `BeOfType`, `BeAssignableTo`, `BeOneOf`, `Satisfy` |

`ShouldExtensions` has 27 overloads — every primitive, `string`, `DateTime`, `TimeSpan`, `Guid`, and a
generic `Should<T>` constrained `where T : struct, Enum`.

**The scalar/value-type story is essentially complete and in several places richer than FluentAssertions
(`BeAround`, `ContainAll`, `ContainAny`, `BeControl`, `HaveOffset`).**

---

## 2. What The Consumers Actually Use

Combined: **~2,360 `.Should()` call sites** (Fog 2,068 / Toolkit 291) across 7 test projects plus two
production projects that ship test helpers.

Ranked by combined call count, mapped against what FatCat.Testing supports today:

| FluentAssertions method | Fog | Toolkit | Total | FatCat status |
|---|---|---|---|---|
| `Be` | 645 | 67 | **712** | Value types ✅ · **objects ❌** |
| `BeEquivalentTo` | 185 | 48 | **233** | Strings only · **object graphs ❌** |
| `BeTrue` / `BeFalse` | 239 | 66 | **305** | ✅ |
| `NotBeNull` | 159 | 57 | **216** | **❌** (no object `Should()`, and no `NotXxx` shape) |
| `Contain` | 169 | 1 | **170** | Strings ✅ · **collections ❌** |
| `BeOk`/`BePost`/`BeGet`/`BeNotFound`/`BeSuccessful`/`BeBadRequest`/`BeUnauthorized`/`BeDelete`/`BeUnsuccessful`/`HaveStatusCode` | ~406 | ~4 | **~410** | **Custom project assertions — need an extension point ❌** |
| `BeNull` | 107 | 4 | **111** | Nullable value types & string ✅ · **objects ❌** |
| `BeEmpty` | 35 | 11 | **46** | Strings & Guid ✅ · **collections ❌** |
| `BeCloseTo` | 18 | 0 | **18** | ✅ |
| `HaveCount` | 15 | 1 | **16** | **❌** |
| `NotContain` | 12 | 0 | **12** | **❌** |
| `ContainSingle` | 11 | 0 | **11** | **❌** |
| `ContainEquivalentOf` | 8 | 4 | **12** | **❌** |
| `NotBeEmpty` | 9 | 0 | **9** | **❌** |
| `Throw<T>` / `ThrowAsync<T>` | 7 | 1 | **8** | **❌** |
| `BeSameAs` / `NotBeSameAs` | 4 | 6 | **10** | **❌** |
| `BeOfType<T>` | 3 | 3 | **6** | ✅ (on base) |
| `BeOneOf` | 0 | 4 | **4** | ✅ (on base) |
| `OnlyContain` | 5 | 0 | **5** | **❌** |
| `MatchEquivalentOf` | 0 | 4 | **4** | **❌** (wildcard match) |
| `Equal` (collections) | 4 | 0 | **4** | **❌** |
| `BeApproximately` | 4 | 0 | **4** | ✅ |
| `NotBeNullOrEmpty` / `NotBeNullOrWhiteSpace` | 7 | 0 | **7** | Positive form ✅ · **`NotXxx` shape ❌** |
| `BeGreaterThan(OrEqualTo)` / `BeLessThan(OrEqualTo)` | 8 | 4 | **12** | `BeGreaterThan`/`BeLessThan` ✅ · **`…OrEqualTo` missing on numerics ❌** |
| `NotBeEquivalentTo` | 0 | 1 | **1** | **❌** |
| `OnlyHaveUniqueItems` | 1 | 1 | **2** | **❌** |
| `BeInDescendingOrder` | 1 | 0 | **1** | **❌** |
| `NotContainEquivalentOf` | 1 | 0 | **1** | **❌** |
| `StartWith` / `EndWith` | 2 | 0 | **2** | ✅ |
| `AssertionScope` | 0 | 0 | **0** | Not needed |
| `ExecutionTime` | 0 | 0 | **0** | Not needed |
| `.And.` chaining | 1 | 0 | **1** | Not needed |
| `.Which` | 3 | 0 | **3** | Low priority |

---

## 3. The Gaps

### Tier 1 — Blocking. Migration cannot start without these.

#### G1 — No `Should()` for reference types / objects
The generic overload is `Should<T>(this T subject) where T : struct, Enum`. There is **no** entry point for
`object`, a DTO, an interface, or any reference type. This single gap blocks ~1,000 call sites: object
`Be`, `NotBeNull` (216), `BeNull` (111), `BeSameAs`/`NotBeSameAs` (10), and object `BeOfType`.

Needs: an `Objects/` folder with `ObjectComparer<T>` / `NotObjectComparer<T>`, a `Should<T>(this T)`
overload that does not collide with the existing enum-constrained generic, and `Be` (via `Equals`),
`BeNull`, `BeSameAs`, `BeEquivalentTo`, `Satisfy`. Overload-resolution against the enum generic and
against the existing 27 concrete overloads is the hard part and should be prototyped first.

#### G2 — The `Not` API shape differs — **DECIDED: keep `.Not.`**
FatCat exposes negation as a `Not` property (`x.Should().Not.Be(y)`). FluentAssertions uses prefixed
methods (`x.Should().NotBe(y)`). Every negated call site in both repos — **~260** (`NotBeNull` 216,
`NotContain` 12, `NotBeEmpty` 9, `NotBeNullOrEmpty`/`NotBeNullOrWhiteSpace` 7, `NotBeSameAs` 6, others) —
fails to compile as written.

**Decision: `.Should().Not.Be(x)` is the API. It reads better and it is the shape the library already
uses.** No `NotXxx` alias methods will be added — not even as `[Obsolete]` shims. The `Not` property stays
the single negation form, and the naming symmetry in `naming-and-structure.md` is preserved.

The consequence is that this stops being a library gap and becomes a **migration-tooling workstream** —
see §5. The rewrite is mechanical and regex-clean (FluentAssertions negations are always `Not` + a
PascalCase method name), so it is a codemod, not hand-editing. It no longer gates the other gaps.

#### G3 — No structural (deep) equality — `BeEquivalentTo` for object graphs
233 call sites. Today `BeEquivalentTo` exists only on strings (case-insensitive compare). Objects and
collections need recursive member-by-member comparison with cycle detection and a readable diff in the
failure message.

Also required: the **global configuration hook**. Fog registers a `DateTime` closeness rule in two places
(`EndToEndTests\Infrastructure\EndToEndTest.cs` at 10s, `Brume\Tests.Brume\BrumeTests.cs` at 1s) via
`options.Using<DateTime>(...).WhenTypeIs<DateTime>()`. Some equivalent must exist or those suites break.

Not required: `Excluding`, `Including`, `RespectingRuntimeType`, `WithStrictOrdering` — zero usages in
either repo. Ship the default-options path only.

#### G4 — No collection assertions at all
There is no `Collections/` folder. ~270 call sites: `Contain` (170 combined with strings), `BeEmpty`,
`HaveCount`, `NotContain`, `ContainSingle`, `ContainEquivalentOf`, `NotBeEmpty`, `OnlyContain`, `Equal`,
`OnlyHaveUniqueItems`, `NotContainEquivalentOf`, `BeInDescendingOrder`.

Needs `Should<T>(this IEnumerable<T>)` and a `CollectionComparer<T>` family. Watch the overload collision
with `string` (which is `IEnumerable<char>`) and with G1's object `Should<T>`.

#### G5 — No custom-assertion extension point
~410 call sites in the two repos go through **project-defined** assertions, not FluentAssertions built-ins.
Both repos build them on `ReferenceTypeAssertions<TSubject, TAssertions>` and return `AndConstraint<T>`:

- Toolkit: `WebResultAssertions.cs`, `WebResultClosedOverAssertions.cs`, `FatResultAssertions.cs`,
  `EndpointTestExtensions.cs` (×2, near-duplicated between `ToolKit` and `Toolkit.WebServer`)
- Fog: `EndToEndTests\Helpers\Assertions\` — `WebResultAssertion.cs`, `UserServiceModelAssertion.cs`,
  `LokrServiceModelAssertion.cs`

FatCat.Testing has no public base a consumer can derive from — `ComparerBase` is usable but undocumented
as an extension point, and `Subject` is `protected`. Define and document the supported way to write a
custom comparer. This is a hard requirement, not a nice-to-have.

Mitigating factor: those custom assertions are implemented by delegating to inner `.Should()` calls, not
by using `Execute.Assertion` / `AssertionChain`. There is **zero** dependency on FluentAssertions' internal
assertion engine — only on the base class and `AndConstraint`.

#### G6 — No exception assertions
`Should().Throw<T>()`, `ThrowAsync<T>()`, `NotThrow()`, `WithMessage()`. Only 8 call sites, but nothing
substitutes for it. Needs `Should(this Action)` / `Should(this Func<Task>)` overloads. Per `async.md`, the
fluent surface stays synchronous — the comparer runs the delegate.

### Tier 2 — Needed for a clean migration.

#### G7 — `Task<T>` assertion overloads
Both repos ship sync-over-async shims so tests assert on async calls without `await`:
- Toolkit: `ToolKit\Testing\TaskTestExtensions.cs` — `Should<T>(this Task<T>)` with `Be`,
  `BeEquivalentTo`, `BeTrue`, `BeFalse`
- Fog: `UserServiceModelAssertion.cs` — `Should(Task<T>)` and `Should(Task<List<T>>)`

FluentAssertions itself does not provide these, so they can stay consumer-side — but they will need a
FatCat base to build on (depends on G5). Decide whether FatCat ships them or the consumers keep them.

#### G8 — Missing numeric `…OrEqualTo` comparisons
`NumericComparer<T>` has `BeGreaterThan` / `BeLessThan` but **not** `BeGreaterThanOrEqualTo` /
`BeLessThanOrEqualTo`. `TimeSpanComparer` has them; numerics do not. 12 call sites. Small, self-contained.

Also flag the two deprecated aliases in Fog — `BeGreaterOrEqualTo` and `BeLessOrEqualTo` (1 each) — which
should be rewritten to the modern names during migration rather than reproduced.

#### G9 — String: `MatchEquivalentOf` and `NotBeNullOrEmpty` family
`MatchEquivalentOf` (case-insensitive wildcard match) has 4 call sites and no equivalent. `Match` exists
but is a `Func<string, bool>` predicate on some comparers and wildcard on others — verify and reconcile.
The `NotBeNullOrEmpty` / `NotBeNullOrWhiteSpace` shape is covered by G2.

#### G10 — Missing type families
No comparer exists for: `DateTimeOffset`, `DateOnly`, `TimeOnly`, `Type`, `Uri`, `Stream`,
`IDictionary<K,V>`. None appear in the current call-site inventory, so this is coverage-completeness for
the "replacement" claim, not migration-blocking. Sequence it last.

### Tier 3 — Explicitly out of scope.

#### G11 — `AssertionScope` — **skip**
Zero usages in either repo. Soft assertions require an ambient-state assertion engine, which would be the
single largest architectural change to the library. Do not build it until something needs it.

#### G12 — `ExecutionTime` — **skip**
Zero usages. Also conflicts with the `Task.Delay`/`Thread.Sleep` ban in `async.md`.

#### G13 — `.And` chaining — **defer**
One call site in Fog, zero in Toolkit. FatCat's assertions already return `this`, so
`x.Should().Be(1).BeGreaterThan(0)` chains without an `And` property. Adding `And` as a self-returning
property is trivial if it turns out to be wanted; it is not needed to migrate.

#### G14 — `.Which` / `.Subject` drill-down — **defer**
3 usages, all `ContainSingle().Which.X.Should()...` in one Fog file. Rewrite those 3 sites rather than
building the `AndWhichConstraint` machinery. Revisit only if `ContainSingle` (G4) proves awkward without it.

---

## 4. Suggested Sequence

```
G1 (object Should)  ──►  G3 (BeEquivalentTo)
        │                       │
        └──►  G4 (collections) ◄┘
                     │
     G5 (extension point) ◄──┘  ──►  G7 (Task overloads)

G6 (exceptions) · G8 (numerics) · G9 (strings)   [independent, any order]
                     │
                     └──►  G10 (remaining type families)
```

G1 leads because object `Should()` is what G3, G4, and G5 all build on. G6, G8, and G9 are independent
and are good candidates to run in parallel or to use as warm-up work.

G2 is no longer in the graph — the decision is made, and the codemod that implements it (§5) runs at
migration time, once per consuming repo, after the library gaps it touches are closed.

**Every gap task carries a migration obligation.** A gap is not done until its FluentAssertions → FatCat
mappings are appended to the migration table in §5.2, including any case the codemod cannot handle. The
migration guide is a living document built up gap by gap — not something written at the end.

## 5. Migration Plan

FatCat.Testing is deliberately **not** a source-compatible clone of FluentAssertions. The negation shape
differs by design (G2). So the library ships with a migration path, and that path is a product
deliverable — anybody coming from FluentAssertions must be able to follow it, not just these two repos.

### 5.1 Deliverables

1. **`MIGRATION.md` at the repo root** — the public, shipped migration guide. Grows one section per gap as
   gaps close. Contains the mapping table (§5.2), the codemod (§5.3), and the known-unsupported list.
2. **A codemod script** — `tools/Migrate-FluentAssertions.ps1`, following `.claude/rules/powershell/`.
   Idempotent, `-WhatIf`-able, operates on a directory tree.
3. **A "known unsupported" list** — every FluentAssertions construct with no FatCat equivalent and the
   recommended rewrite. `AssertionScope` (G11), `ExecutionTime` (G12), `.Which` (G14), and the
   `BeEquivalentTo` option methods all live here.

### 5.2 Mapping Table

The rule is uniform: **`NotXxx(` → `Not.Xxx(`**. FluentAssertions negations are always the literal `Not`
followed by a PascalCase method name, which makes this mechanical.

| FluentAssertions | FatCat.Testing | Sites | Gap |
|---|---|---|---|
| `.Should().NotBeNull()` | `.Should().Not.BeNull()` | 216 | G1 |
| `.Should().NotContain(x)` | `.Should().Not.Contain(x)` | 12 | G4 |
| `.Should().NotBeEmpty()` | `.Should().Not.BeEmpty()` | 9 | G4 |
| `.Should().NotBeNullOrEmpty()` | `.Should().Not.BeNullOrEmpty()` | 4 | — (exists) |
| `.Should().NotBeSameAs(x)` | `.Should().Not.BeSameAs(x)` | 6 | G1 |
| `.Should().NotBeNullOrWhiteSpace()` | `.Should().Not.BeNullOrWhiteSpace()` | 3 | — (exists) |
| `.Should().NotBeEquivalentTo(x)` | `.Should().Not.BeEquivalentTo(x)` | 1 | G3 |
| `.Should().NotContainEquivalentOf(x)` | `.Should().Not.ContainEquivalentOf(x)` | 1 | G4 |
| `.Should().NotBe(x)` | `.Should().Not.Be(x)` | 1 | G1 |
| `.Should().BeGreaterOrEqualTo(x)` | `.Should().BeGreaterThanOrEqualTo(x)` | 1 | G8 |
| `.Should().BeLessOrEqualTo(x)` | `.Should().BeLessThanOrEqualTo(x)` | 1 | G8 |

The last two are FluentAssertions' own deprecated aliases — normalize to the modern names rather than
reproducing them.

**Each gap task appends its rows here as it lands.** The table above covers only what the current call-site
inventory proves is in use; it is not the complete negation surface.

### 5.3 The Codemod

The core transform is one regex:

```
find:     \.Should\(\)\.Not([A-Z]\w*)\(
replace:  .Should().Not.$1(
```

Cases the regex will **not** catch — the script must report these for manual review rather than silently
skipping them:

- **Chained negations** — `.And.NotContain(x)`. One known site in Fog. `.And` itself is deferred (G13), so
  these get hand-rewritten into separate statements.
- **Line-broken chains** where `.Should()` and `.NotXxx(` are on different lines.
- **Custom assertions** whose names begin with `Not` but are project-defined, not FluentAssertions.
- **`Not` on a subject the library does not yet cover** — the rewrite compiles only once the matching gap
  has landed, so run the codemod per-repo *after* G1/G3/G4.

### 5.4 Per-Repo Sequence

FluentAssertions is a *transitive* dependency in Fog, arriving through the FatCat.* packages — so the
order is forced:

1. **`FatCat.Toolkit` first.** It references FluentAssertions from **production** projects (`ToolKit`,
   `Toolkit.WebServer`), not just tests, because it ships test helpers under `ToolKit\Testing\`. Replacing
   it there changes the public contract of the Toolkit NuGet package — treat that as its own release step,
   not a side effect of a test cleanup.
2. **Port the custom assertion layer** (G5) — `WebResultAssertions`, `WebResultClosedOverAssertions`,
   `FatResultAssertions`, `EndpointTestExtensions`. The Toolkit and Toolkit.WebServer copies are
   near-duplicates (`FatWebResponse` vs `WebResult`); consolidate during the port rather than porting the
   same code twice.
3. **`Fog` last**, once it can pick up a FluentAssertions-free Toolkit.

Per repo, the mechanical steps are small: swap `global using FluentAssertions;` → `global using
FatCat.Testing;` in the 6 `GlobalUsings.cs` files, drop the ~27 per-file `using FluentAssertions;` in Fog,
run the codemod, then build and fix the residue.

### 5.5 Migration Is Testable

The migration guide is only credible if it is verified. Each gap's test set should include the
FluentAssertions-shaped call it replaces, rewritten per §5.2, so the mapping table is proven by compiling
tests rather than asserted in prose. A mapping row with no test behind it is a claim, not a guarantee.

## 6. Complete-Surface Comparison (beyond what the consumers use)

§2–§3 are scoped to what Toolkit and Fog actually call. This section is the full FluentAssertions surface
(v8.10.0), for judging the "replacement" claim rather than the migration. Nothing here blocks migration —
sequence it all after §3.

### 6.1 Cross-cutting infrastructure — the real differentiators

These are not assertion methods, and they matter more than any method list.

#### G15 — Value formatting engine
FatCat builds failure messages with `$"{boxed}"` — plain `ToString()`. That reads fine for `int`, `bool`,
`Guid`. It produces garbage the moment G1/G3/G4 land: a `List<string>` formats as
`System.Collections.Generic.List\`1[System.String]`, and a DTO as its type name.

FluentAssertions has a whole subsystem: `IValueFormatter` (`CanHandle`/`Format`), `Formatter.AddFormatter`,
`DefaultValueFormatter`, `EnumerableValueFormatter` (32-item cap), `FormattedObjectGraph` with indentation
and `MaxLinesExceededException`, plus scoped per-`AssertionScope` formatter registration.

**This is a prerequisite for G3 and G4, not a follow-up.** A `BeEquivalentTo` failure whose message cannot
show *which member differed* is not a usable assertion. Minimum viable: a `Format(object)` helper handling
null, string (quoted), `IEnumerable` (bounded, elementwise), and object (member dump), used everywhere the
library currently interpolates a subject.

#### G16 — Test-framework coupling
`CompareException` derives from `XunitException`, and `xunit.assert` is the library's one package
reference. **FatCat.Testing is xUnit-locked.** FluentAssertions auto-detects xUnit2, xUnit3, NUnit, MSTest,
MSTest4, TUnit, and MSpec, throwing each framework's native failure exception.

Irrelevant for Toolkit and Fog (both xUnit). It is decisive for anyone else adopting the package — a
"free replacement for FluentAssertions" that only works on xUnit is a narrower claim. Decide whether that
is the intended scope, and if not, the fix is a framework-detection shim that drops the hard `xunit.assert`
dependency.

#### G17 — Custom assertion authoring primitives
Beyond the extension point in G5, FluentAssertions gives extension authors: `AssertionChain.GetOrCreate()`
with `ForCondition` / `FailWith` / `BecauseOf` / `Given` (lazy), the `[CustomAssertion]` attribute for
correct caller identification in stack traces, and message templating (`{context}`, `{reason}`, numbered
placeholders). FatCat offers `CompareException.New(string)` and nothing else.

Both consuming repos' custom assertions delegate to inner `.Should()` calls rather than using this
machinery, so a minimal extension point (G5) is enough for them — but `[CustomAssertion]`-equivalent
stack-trace handling is what makes a custom assertion point at the *test* line instead of the library line.

### 6.2 Whole feature areas absent

| Gap | Area | FluentAssertions surface | Verdict |
|---|---|---|---|
| G18 | **Dictionaries** | `ContainKey(s)`, `ContainValue(s)`, `HaveCount`, `HaveSameCount`, `Equal`, `Contain(KeyValuePair)` + `Not*` | Worth doing — common type, small surface |
| G19 | **Streams** | `BeReadable`, `BeWritable`, `BeSeekable`, `BeReadOnly`, `BeWriteOnly`, `HaveLength`, `HavePosition`, `HaveBufferSize` + `Not*` | Cheap, self-contained |
| G20 | **Types / assemblies / reflection** | Type: `BeDecoratedWith<T>`, `BeAbstract`, `BeStatic`, `BeSealed`, `BeDerivedFrom<T>`, `Implement<T>`, `BeInNamespace`, `HaveAccessModifier`. Method: `BeVirtual`, `BeAsync`, `Return<T>`, `ReturnVoid`. Property: `BeReadable`, `BeWritable`. Assembly: `Reference`, `HavePublicKey`, `BeUnsigned`, `DefineType`. Plus the whole `Types.InAssembly(...).ThatAre*` selection DSL | Large. Architecture-test territory. Skip unless wanted |
| G21 | **Event monitoring** | `Monitor<T>()`, `Should().Raise("X")`, `NotRaise`, `WithSender`, `WithArgs<T>`, `RaisePropertyChangeFor(x => x.P)` | Large, needs an event-recording subsystem. Skip unless wanted |
| G22 | **XML** | `XDocument`: `HaveRoot`, `HaveElement`, `BeEquivalentTo`. `XElement`: `HaveValue`, `HaveAttribute`, `HaveElement`, `HaveInnerText`. `XAttribute`: `HaveValue`, `Be` | Skip |
| G23 | **JSON** (`System.Text.Json`) | `HaveProperty`, `BeAnArray`, `BeString`, `BeNumeric`, `BeBool`, `BeLocalDate`, `BeUtcDate` + `Not*` | Skip |
| G24 | **Serializability** | `BeXmlSerializable`, `BeDataContractSerializable`, `BeBinarySerializable` | Skip |
| G25 | **Analyzers package** | `FluentAssertions.Analyzers` — Roslyn rules + code fixes, incl. auto-conversion from MSTest/NUnit/xUnit assertions | Interesting long-term: an analyzer that rewrites FluentAssertions → FatCat would *be* the §5 codemod |

Not gaps — do not build: **Data/DataSet/DataTable** (moved out of core into the separate
`FluentAssertions.DataSets` package) and **`HttpResponseMessage`** (removed entirely in v8; delegated to
the third-party `FluentAssertions.Web`). Note that both consuming repos hand-rolled their own HTTP result
assertions anyway, which is exactly what G5 exists to support.

### 6.3 Method-level gaps inside areas FatCat already covers

| Family | Missing |
|---|---|
| Booleans | `Imply` |
| Enums | `HaveSameNameAs`, `HaveSameValueAs`, `NotBeDefined` |
| Guids | `Be(string)` overload |
| DateTimes | `BeSameDateAs`, `BeIn(DateTimeKind)`, `BeOneOf`, and the **fluent difference chains** — `BeLessThan(2.Hours()).Before(x)`, `BeWithin(...).After(x)`, `BeMoreThan`, `BeAtLeast`, `BeExactly`. The chains are a genuinely nice API and the largest DateTime gap |
| Numerics | `BeGreaterThanOrEqualTo`, `BeLessThanOrEqualTo` (G8), `NotBeInRange`, `NotBeApproximately` |
| Strings | `ContainEquivalentOf`, `MatchEquivalentOf` (G9), `NotContainAll`, `NotContainAny` |
| Objects | `Match(predicate)`, `As<T>()` — plus everything in G1 |
| Collections | Beyond G4: `ContainInOrder`, `ContainInConsecutiveOrder`, `BeInAscendingOrder`, `BeSubsetOf`, `IntersectWith`, `HaveElementAt`, `HaveElementPreceding`/`Succeeding`, `ContainItemsAssignableTo`, `ContainNulls`, `ContainMatch`, `AllBe`, `AllBeOfType`, `AllBeEquivalentTo`, `AllSatisfy`, `SatisfyRespectively`, `HaveCountGreaterThan`/`LessThan`(`OrEqualTo`), `HaveSameCount`, `BeNullOrEmpty` |
| Exceptions | Beyond G6: `ThrowExactly<T>`, `WithInnerException<T>`, `WithInnerExceptionExactly<T>`, `Where(predicate)`, `WithParameterName`, `NotThrowAfter`, and the async set `ThrowAsync`, `NotThrowAsync`, `ThrowExactlyAsync`, `NotThrowAfterAsync`, `CompleteWithinAsync`, `NotCompleteWithinAsync`, `ThrowWithinAsync`, `WithResult`, plus the `Awaiting`/`Invoking`/`Enumerating` wrappers |
| `BeEquivalentTo` options | FatCat will ship default-options only (G3). FluentAssertions has ~45 option methods — `Excluding`, `Including`, `WithStrictOrdering`, `ComparingByMembers<T>`, `ComparingEnumsByName`, `AllowingInfiniteRecursion`, `IgnoringCase`, `WithMapping`, `WithTracing`, … Only `Using<T>().WhenTypeIs<T>()` is used by the consumers |

### 6.4 Where FatCat is already ahead

Not everything is a deficit. FatCat has assertions FluentAssertions lacks: `BeAround(center, tolerance)`
on numerics, `ContainAll` / `ContainAny` on strings, `BeControl` on chars, and a fuller `Have*` component
set on `TimeSpan` (`HaveDays` … `HaveMilliseconds`). The nullable story is also more systematic — a
dedicated `Nullable*Comparer` per family rather than nullable overloads bolted onto the base assertions.

---

## 7. Observations Outside The Gap Scope

- `NullableStringComparer` and `NotNullableStringComparer` declare parameters as `string?` (e.g.
  `string? because = null`) even though `<Nullable>disable</Nullable>` is set and `not-allowed.md`
  explicitly bans it. Worth a cleanup pass — it is the only place in the library that does this.
- `NotComparerBase` lacks `Satisfy`, which `ComparerBase` has. Minor asymmetry.
- `ComparerBase.BeOneOf(params TSubject[] values)` drops the `because` parameter — the `params` overload
  cannot forward one. Every other assertion in the library accepts `because`.
