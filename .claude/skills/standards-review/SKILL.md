---
name: standards-review
description: Review code against the FatCat.Testing coding standards in .claude/rules. Use when the user says "review all uncommitted changes", "review the library", "review this directory", "review the last commit", "review commit <hash>", or similar. Loads the relevant language rule files, finds every violation (especially the C# one-class-per-file rule), and — only when violations exist — writes a human-readable, session-actionable markdown report to .reviews/ (gitignored). This skill NEVER edits source code and NEVER deletes report files.
---

# Standards Review

Review code in a requested scope against the FatCat.Testing coding standards and produce a report another session can act on. This skill **only reads and reports** — it does not fix code and does not delete report files.

## Step 1 — Resolve the scope

Map the user's phrasing to a concrete set of files to review. Run all `git` commands from the repo root (`C:/Code/FatCat.Testing`). Note that the solution and all source live under `src/`.

| User says | Scope |
|---|---|
| "review all uncommitted changes" / "review my changes" | Staged + unstaged + untracked files: `git status --porcelain` (each entry is in scope). Review the **current working-tree content** of each file. |
| "review the library" / "review the tests" | All source files under `src/FatCat.Testing/` or `src/Tests.FatCat.Testing/` respectively. |
| "review this directory" / "review `<path>`" | All source files under the given directory (or the current working directory if none named). |
| "review the last commit" | Files changed in `HEAD`: `git show --stat --name-only HEAD`. Review the **content as of that commit** via `git show HEAD:<path>`. |
| "review commit `<hash>`" | Files changed in `<hash>`: `git show --stat --name-only <hash>`. Review content via `git show <hash>:<path>`. |

Rules:
- Only review source files: `*.cs`, `*.ps1`. Skip `bin/`, `obj/`, `*.csproj`, `*.sln`, `*.json`, and other non-source files.
- For commit-scoped reviews, read the file content **at that commit**, not the working tree.
- If the resolved scope is empty (e.g. no uncommitted changes), say so and stop — do not write a report.
- If the phrasing is ambiguous about which scope, pick the most likely one and state your assumption before proceeding.

## Step 2 — Load the relevant rules

Read only the rule files for the languages present in the scope. The rules live in `C:/Code/FatCat.Testing/src/.claude/rules/`.

- **C# (`*.cs`)** — read all of: `csharp/naming-and-structure.md`, `csharp/types.md`, `csharp/toolchain.md`, `csharp/async.md`, `csharp/errors.md`, `csharp/testing.md`, `csharp/not-allowed.md`.
- **PowerShell (`*.ps1`)** — read `powershell/powershell.md`.

These rule files are the source of truth. The standard is "indistinguishable from code written by a senior member of this team." Treat the rules as a checklist — do not rely on memory.

## Step 3 — Review each file

Go file by file. For every file, check it against every applicable rule. Pay special attention to these high-signal violations:

### C# — emphasized checks
- **One class per file.** A `.cs` file must contain exactly one class. The *only* acceptable second type in the file is the single interface that the class directly implements (per `naming-and-structure.md`). Two classes, two unrelated interfaces, or an enum + class in one file are all violations. Flag every extra top-level type and name it.
- File named after the class, never the interface.
- File-scoped namespaces only; namespace matches folder path; production namespaces start with `FatCat.Testing.*`, tests with `Tests.FatCat.Testing.*`.
- No expression-bodied members (any access level, including tests).
- No records. No nullable **reference** annotations (`string?`) — `Nullable` is disabled; nullable value types are fine.
- Primary constructors, not explicit constructor bodies. No `new` for dependencies.
- **Braces on every `if`**, including single-statement bodies. A CSharpier-collapsed one-liner keeps its braces.
- Collection expressions (`[]`) not `new List<T>()`; switch expressions with a discard arm over if/else chains.
- **String interpolation, never `+` concatenation.** Require `$"{Subject} should be {expected}"`; flag any `"..." + value`. No analyzer catches this — it is a review-only rule, so check it carefully.
- No logging, no `Console` writes outside `OneOff`, no new package references on `FatCat.Testing`, no DI/mocking/mapping infrastructure.
- Assertion failures throw via `CompareException.New(...)`; `because ?? $"..."` so a supplied reason always wins; API misuse throws a BCL exception, not `CompareException`.
- Comparer naming symmetry intact (`<Type>Comparer` / `Not<Type>Comparer` / `Nullable<Type>Comparer` / `NotNullable<Type>Comparer`); assertion methods return the concrete comparer for chaining.
- TDD: every assertion method has its full test set — `Good`, `Bad`, `BadShowsCorrectMessage`, `BadWithBecause`, the `Not` equivalents, and the nullable variants. Test classes are `<Subject><Method>Tests : BaseTest` in a folder mirroring the source. **No underscores in test method names.**

### PowerShell — emphasized checks
- `Verb-Noun` names with approved verbs; one function per file; no aliases; typed params; `[switch]` not `[bool]`.

Be precise. For each violation capture: the file, the line (or line range), what rule it breaks, and the concrete change required to satisfy the rule. Reference the rule file by name. Do not invent rules that are not in `.claude/rules/`.

## Step 4 — Report

**If there are no violations:** report a clean pass inline in the session (briefly list what was reviewed). **Do not write a file.**

**If there are violations:** write one markdown report to `.reviews/`.

- Ensure the folder exists (`.reviews/` at repo root — it is gitignored).
- Filename: `.reviews/<YYYY-MM-DD-HHmmss>-<scope-slug>.md`, where the scope slug describes the target (`uncommitted`, `library`, `tests`, `last-commit`, `commit-<shorthash>`, a directory name, etc.). Generate the timestamp with `pwsh -Command '. $PROFILE; (Get-Date).ToString("yyyy-MM-dd-HHmmss")'`.
- Use the template below. It must be **actionable by another session** (precise file/line/change) and **readable by a human** (grouped, plain language, no raw tool dumps).
- After writing, tell the user the report path and give a one-line summary of how many violations were found.

### Report template

```markdown
# Standards Review — <scope description>

- **Reviewed:** <what was in scope, e.g. "uncommitted changes (7 files)">
- **Generated:** <timestamp>
- **Result:** <N> violation(s) across <M> file(s)

> This report was generated by `/standards-review`. To resolve it, point a session at this
> file and ask it to fix the listed violations. **Do not delete this file** — use
> `/clean-reviews` when you want to remove reports.

---

## <relative/path/to/File.cs>

### 1. <short title of the violation>
- **Lines:** <line or range>
- **Rule:** <rule file>, <which rule>
- **Problem:** <plain-language description of what is wrong>
- **Required change:** <the concrete edit needed to comply>

### 2. <next violation in this file>
...

---

## <relative/path/to/Next.cs>
...

---

## Summary checklist
- [ ] <File.cs> — <one-line of what to fix>
- [ ] <Next.cs> — <one-line of what to fix>
```

## Hard rules for this skill
- **Never edit source files.** This skill reviews and reports only. Fixing is a separate, explicit action a session does when pointed at the report.
- **Never delete a report.** If the user asks you to "review the markdown" (i.e. read a report and resolve its issues), fix the *source code* the report points to but leave the report file in place. Removing reports is exclusively `/clean-reviews`.
- **Do not commit anything.** `.reviews/` is gitignored by design.
- Only flag violations that trace to a rule in `.claude/rules/`. If something looks off but no rule covers it, you may add it under an "Observations (not rule violations)" section, clearly separated.
