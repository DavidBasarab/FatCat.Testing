# Migrating from FluentAssertions to FatCat.Testing

FatCat.Testing is deliberately **not** a source-compatible clone of FluentAssertions. The negation shape
differs by design: FatCat expresses negation through the `Not` property (`x.Should().Not.Be(y)`) rather than
`NotXxx` methods. This guide is the migration path — anybody coming from FluentAssertions can follow it, not
just the FatCat repos.

This document grows one section per gap as gaps close. Each gap appends its rows to the mapping table below.

## Mapping Table

The rule is uniform: **`NotXxx(` → `Not.Xxx(`**. FluentAssertions negations are always the literal `Not`
followed by a PascalCase method name, which makes the rewrite mechanical. Positive assertions that FatCat now
supports for a new subject type map one-to-one.

| FluentAssertions | FatCat.Testing | Sites | Gap |
|---|---|---|---|
| `.Should().NotBeNull()` | `.Should().Not.BeNull()` | 216 | G1 |
| `.Should().NotBeSameAs(x)` | `.Should().Not.BeSameAs(x)` | 6 | G1 |
| `.Should().NotBe(x)` | `.Should().Not.Be(x)` | 1 | G1 |
| `.Should().Be(x)` (object) | `.Should().Be(x)` | 712 | G1 |
| `.Should().BeNull()` (object) | `.Should().BeNull()` | 111 | G1 |

**Each gap task appends its rows here as it lands.** The table above covers only what the current call-site
inventory proves is in use; it is not the complete negation surface.
