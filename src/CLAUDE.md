# Coding Standards

This file defines the coding standards for the FatCat.Testing codebase.
All code you generate — in any context — must follow the rules for the relevant language below.
The goal is that AI-generated code is indistinguishable from code written by a senior member of this team.

## What This Project Is

FatCat.Testing is a standalone assertion library — a replacement for FluentAssertions. It has no web layer,
no database, no dependency-injection container, and no frontend. It is a small, self-contained C# library
(`FatCat.Testing`) plus its test project (`Tests.FatCat.Testing`) and a scratch console (`OneOff`).

Do not introduce endpoints, repositories, DI containers, logging frameworks, or other infrastructure —
none of it exists here.

## C# Rules

Apply these rules to all C# code.

@.claude/rules/csharp/naming-and-structure.md
@.claude/rules/csharp/types.md
@.claude/rules/csharp/toolchain.md
@.claude/rules/csharp/async.md
@.claude/rules/csharp/errors.md
@.claude/rules/csharp/testing.md
@.claude/rules/csharp/not-allowed.md

## PowerShell Rules

Apply these rules to any PowerShell script added for build or release automation.
Do not apply them to C#.

@.claude/rules/powershell/powershell.md
