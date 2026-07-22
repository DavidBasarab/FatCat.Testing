# Phase 13 — `IDictionary<K,V>` Family — **BLOCKED, DO NOT RUN**

- **Work item:** `tier_2_gaps`
- **Gap:** **G10** (missing type families) — also closes **G18** (gaps.md §6.2)
- **Risk:** **medium** when it eventually runs — it shares an overload space with collections and depends
  on decisions that phase has not made yet.
- **Depends on:** 01 **and G4 (Tier 1 — collections), which is not planned or built**
- **Depended on by:** 14 *(only if it has landed; phase 14 treats it as optional)*
- **Status:** **BLOCKED.** The orchestrator does not execute this phase. It exists so the G10 scope is
  complete on paper and so the work is ready to start the moment its precondition clears.

---

## Why This Is Blocked

`IDictionary<K,V>` is `IEnumerable<KeyValuePair<K,V>>`. Any `Should(this IDictionary<K, V>)` overload
therefore competes with the `Should<T>(this IEnumerable<T>)` overload that **G4 will introduce**, and the
resolution between them is a decision G4 owns (gaps.md §3 G4 explicitly flags the `string`-is-
`IEnumerable<char>` collision as the hard part of that gap).

Shipping dictionaries first would mean either:

- guessing at G4's overload strategy and probably having to redo it, or
- freezing part of G4's design from a Tier 2 phase, which inverts the dependency the gap analysis set out.

ADR-1 in [00-overview.md](00-overview.md) resolved this: Tier 2 stays Tier-1-independent, and the pieces
that genuinely cannot be are carved out and declared. This is the only such piece.

**Precondition to unblock:** G4 has landed, with `Collections/` in place and its overload-resolution
strategy documented in the G4 plan's Hand-off. Re-read this file then — several decisions below may need
revising against what G4 actually shipped.

---

## Intended Scope (for when it unblocks)

**In scope** — a `Dictionaries/` folder with `DictionaryComparer<TKey, TValue>` and
`NotDictionaryComparer<TKey, TValue>`, one `ShouldExtensions` overload, the full test set, and the usual
README/MIGRATION updates.

**Out of scope** — `IReadOnlyDictionary<K,V>` overloads (decide with G4), `BeEquivalentTo` over dictionary
graphs (that is G3), and anything already covered by the collection comparer.

## Intended Surface (gaps.md G18)

| Assertion | Fails when |
|---|---|
| `ContainKey(key)` | the key is absent |
| `ContainKeys(params keys)` | any key is absent — message names the missing ones |
| `ContainValue(value)` | no entry has that value |
| `ContainValues(params values)` | any value is absent |
| `Contain(KeyValuePair<TKey, TValue>)` | the pair is absent or the value differs |
| `Contain(key, value)` | same, spelled as two arguments |
| `HaveCount(expected)` | `Count != expected` |
| `HaveSameCount(other)` | counts differ |
| `BeEmpty()` | `Count != 0` |
| `Equal(other)` | the dictionaries differ as key/value sets |

Plus every negated form on `NotDictionaryComparer<TKey, TValue>`.

Design points to settle **at that time**, not now:

- Whether `Equal` is order-insensitive (it should be — dictionaries have no order) and how the failure
  message renders a diff. That message depends on the value-formatting story (G15), which is also unbuilt.
- Whether `HaveSameCount` accepts an `IDictionary` only, or any `IEnumerable`.
- How a null subject is handled — presumably the reference-type shape from phases 10–12: null fails
  everything except `BeNull`.
- Whether the entry point is `Should(this IDictionary<TKey, TValue>)`, `IReadOnlyDictionary`, or both, and
  how it resolves against G4's `IEnumerable<T>` overload. **This is the blocking question.**

## When It Runs

Follow the structure of [10-uri-family.md](10-uri-family.md) — reference-type shape, two comparers,
`SubjectDisplay`, full `Good`/`Bad`/`BadShowsCorrectMessage`/`BadWithBecause` test set per assertion plus
the `Not` equivalents, README catalog entry, `MIGRATION.md` mapping rows with a `Proven by` test class for
each, and a `## Rollback Procedure` naming its own commit.

Its `Depends on:` line becomes `01, G4`. Its `Depended on by:` stays `14`.

---

## Definition of Done — for this file, today

- [ ] The phase file exists and states the block, its cause, and its precondition. ✅ (this file)
- [ ] `README.md` `## Coverage Status` carries an `IDictionary<K,V>` row marked blocked with the reason —
      **created by phase 01, verify it is there**.
- [ ] `MIGRATION.md` `## 4. Type Coverage` says dictionaries are pending G4.
- [ ] No `.cs` file is added. **No commit is made for this phase.**

## Rollback Procedure

Nothing to roll back — this phase produces no commit. If a future session runs it anyway, revert that
commit and reopen the block:

```pwsh
git revert --no-edit <that-commit>
```

## Hand-off

Nothing is exposed. G18 remains open. The G4 plan should link here so whoever closes collections knows a
dictionary phase is queued behind it and inherits its overload-resolution decision.
