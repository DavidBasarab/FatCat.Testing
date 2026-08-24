# Fog Migration Support — make FatCat.Testing ready to replace FluentAssertions in Fog

**Owner:** David (you). Do this **before** asking any Fog session to start the companion task
(`C:\Code\Fog\tasks\todo\fluentassertions-to-fatcat-testing`). That task has a hard gate that will
refuse to run until the version you publish here is available to Fog.

## Why this exists

Fog's ~5,000 `.Should()` call sites will move from FluentAssertions onto **native FatCat.Testing
idioms** — no FA-compatibility aliases. We deliberately chose `.Not.Xxx()` over flat `NotXxx()`, and
Fog rewrites its own call sites to match. That means **FatCat.Testing needs almost no new code** — this
todo is mostly *verification that the native surface Fog will target already exists*, plus publishing a
stamped version Fog can pin.

The full gap analysis lives in the Fog task's `00-overview.md`. This file is only the FatCat.Testing
side.

---

## 1. Verify the native surface Fog will target (add only what's missing)

Fog's migration rewrites every FA-only idiom to one of these. Each must exist and behave as described.
Where one is missing, that — and only that — is the code change for this todo. Add it test-first per
this repo's rules; do **not** add FA-style aliases beyond the list.

| Native idiom Fog will use | Replaces (FA) | Expected to already exist? | Verify by |
|---|---|---|---|
| `.Not.Be(x)`, `.Not.BeNull()`, `.Not.Contain(x)`, `.Not.BeEmpty()`, `.Not.BeNullOrEmpty()`, `.Not.BeNullOrWhiteSpace()`, `.Not.BeSameAs(x)`, `.Not.ContainAny(x)`, `.Not.ContainEquivalentOf(x)` | `NotBeNull`, `NotContain`, `NotBe`, … (flat names) | **Yes** — `.Not` property on every comparer | Grep the `Not*Comparer` classes for each method; add a unit test asserting `.Not.BeNullOrWhiteSpace()` on a string and `.Not.ContainAny(...)` if either is absent |
| Self-chaining: `s.Should().Contain("a").Not.Contain("b")` | `.And.NotContain(...)` | **Yes** — positive comparers return `this` and expose `.Not` | Unit test: chain a positive then a `.Not.` on one `.Should()` |
| `col.Single()` then a fresh `.Should()` on the element | `.ContainSingle().Which.X.Should()` | N/A — `Single()` is LINQ; Fog splits into two statements | Nothing to do (confirm `ContainSingle()` exists — it does) |
| `.Throw<T>().Where(e => e.Message.Contains("x"))` | `.Throw<T>().WithMessage("*x*")` (wildcard) | **Yes** — `ThrownExceptionComparer.Where(Func<Exception,bool>)` | Unit test that `.Where(...)` filters on message. **If** you'd rather Fog keep `.WithMessage`, make `WithMessage` accept `*`/`?` wildcards — but the plan assumes `.Where`, so leaving `WithMessage` exact-match is fine |
| `Action act = () => x.Do(); act.Should().Throw<T>()` | `x.Invoking(y => y.Do()).Should().Throw<T>()` | **Yes** — `Should(this Action)` exists | Confirm the `Action` entry point compiles |
| `.BeGreaterThanOrEqualTo(n)` | `.BeGreaterOrEqualTo(n)` | **Yes** | Confirm the numeric comparer has `BeGreaterThanOrEqualTo` (it does) |
| `date.Should().BeCloseTo(other, 3.Seconds(), "because")` | same | Verify the **3-arg** (`because`) overload | Unit test `BeCloseTo(DateTime, TimeSpan, string)` — one Fog site passes a `because` |
| `Equivalency.Using<DateTime>((a, b) => (a - b).Duration() <= 1.Seconds())` registered globally | `AssertionOptions.AssertEquivalencyUsing(o => o.Using<DateTime>(...).WhenTypeIs<DateTime>())` | **Yes** — `Equivalency.Using<T>` global registry | Unit test: register a DateTime tolerance rule, then `BeEquivalentTo` two objects whose DateTimes differ by < tolerance and assert they're equivalent |
| `ComparerBase<TSubject, TComparer>` as a base for **custom** endpoint/service-model comparers, with a paired `Should(this TSubject)` extension | subclassing `FluentAssertions.Primitives.ReferenceTypeAssertions<,>` | **Yes** — see `Tests.FatCat.Testing/Extensibility/FakeWebResponseComparer.cs` | Already covered by the extensibility test; Fog copies that template for its 5 EndToEnd assertion classes |

**Bottom line:** if all rows verify (they should, per the current inventory), the only *code* you may
need is a tiny gap-fill (e.g. a missing `Not.` string method or the 3-arg `BeCloseTo`). Do not
speculatively add anything Fog doesn't use.

## 2. Prerequisite you must confirm (blocks the whole Fog task)

Fog's ~480 `BeOk()` / `BePost()` / `BeBadRequest()` / `BeNotFound()` endpoint assertions come from
**`FatCat.Toolkit.WebServer.Testing`** (the `FatCat.Toolkit` package), **not** this repo. If that
package still pulls FluentAssertions in transitively, Fog cannot drop FA no matter what we do here.

- [x] Confirm `FatCat.Toolkit` (the version Fog references — currently **1.0.344**) has **no**
  transitive dependency on `FluentAssertions`, and that its WebResult/Endpoint `.Should()` assertions
  are built on `FatCat.Testing` (or self-contained). If it still needs FA, that is a **`FatCat.Toolkit`**
  fix and must land there first — note it here and tell Fog to wait.

Result: `FatCat.Toolkit 1.0.344 FA-free? YES  (if not, blocking issue: n/a)`

Verified 2026-08-24 by inspecting the cached package at
`~/.nuget/packages/fatcat.toolkit/1.0.344`:
- `fatcat.toolkit.nuspec` declares **no** `FluentAssertions` dependency. Its deps are Autofac,
  DebounceThrottle, FakeItEasy, FatCat.Fakes, **FatCat.Testing 1.0.2**, Humanizer.Core, Konscious
  Argon2, ASP.NET SignalR.Client, MongoDB.Driver (+ diagnostics), System.IO.Abstractions, fasterflect,
  xunit.extensibility.core — none of which pull FA transitively.
- `lib/net10.0/FatCat.Toolkit.dll` contains **no** `FluentAssertions` string reference; the
  `BeOk`/`BePost` (`WebServer.Testing`) endpoint assertions live in that same DLL and it depends on
  `FatCat.Testing`. Endpoint assertions are built on FatCat.Testing / self-contained — no FA.

No blocking `FatCat.Toolkit` issue. Fog is clear to proceed once §3 publishes the stamped version.

## 3. Publish a stamped version for Fog to gate on

- [x] Bump `FatCat.Testing.csproj` `<VersionPrefix>` (from `1.0.2`) and publish to the feed Fog
  restores from. **Fog has no `nuget.config`** — it uses the machine/global default feed, so publish
  where that resolves (nuget.org, or your local/GitHub feed added to Fog's machine config). If Fog can't
  `dotnet restore` the new `FatCat.Testing`, the migration can't start.
  - Bumped to `1.0.3`. Packed `-c Release` → `FatCat.Testing.1.0.3.nupkg`, pushed to
    **nuget.org** (`https://api.nuget.org/v3/index.json`) — the machine default feed Fog restores from.
    Push returned "Your package was pushed." No `§1` code changed; this release is functionally
    identical to `1.0.2` and exists purely as the coordination marker Fog gates on.
- [x] Record the published version so Fog's gate can check it:

  **Published FatCat.Testing version for the Fog migration: `1.0.3`**
  **Published on: `2026-08-24`**

- [x] Write that same version into the Fog task gate: open
  `C:\Code\Fog\tasks\todo\fluentassertions-to-fatcat-testing\00-overview.md`, find the
  `FATCAT_TESTING_VERSION` line, and fill it in. Only then hand the Fog task to a session.
  - Set `FATCAT_TESTING_VERSION = 1.0.3` in the Fog gate.

## Definition of done

- [x] Every row in §1 verified; any missing native method added test-first (no FA-style aliases).
      All eight rows verified — every native method already exists in `1.0.2` with test coverage
      (`.Not.*` string methods, `BeSameAs` on `NotObjectComparer`, `ContainSingle`,
      `ThrownExceptionComparer.Where`, `Should(this Action)`+`.Throw<T>()`, `BeGreaterThanOrEqualTo`,
      3-arg `BeCloseTo`, `Equivalency.Using<T>` consulted by `EquivalencyComparer`, and the
      `Extensibility` custom-comparer template). No code change was needed; no aliases added.
- [x] §2 prerequisite confirmed (or the blocking `FatCat.Toolkit` issue is written down and Fog is told
      to hold). — `FatCat.Toolkit 1.0.344` is FA-free (see §2 result above). No blocker.
- [x] New version published to a feed Fog can restore, version + date recorded above. — `1.0.3` to
      nuget.org on 2026-08-24.
- [x] `FATCAT_TESTING_VERSION` filled into the Fog overview. — set to `1.0.3`.
- [x] `dotnet test Fatcat.Testing.sln` green. — 1921 passed, 0 failed.
