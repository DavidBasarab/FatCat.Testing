#nullable enable

using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class NullableStringContainTests : BaseTest
{
	[Fact]
	public void BadContain()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().Contain("xyz"),
							"hello world should contain xyz"
						);
	}

	[Fact]
	public void BadContainAllMissingOne()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().ContainAll(["hello", "xyz"]),
							"hello world should contain all of [xyz]"
						);
	}

	[Fact]
	public void BadContainAllWhenNull()
	{
		RunCompareFailTest(
							() => ((string?)null).Should().ContainAll(["hello"]),
							"null should contain all of [hello]"
						);
	}

	[Fact]
	public void BadContainAllWithBecause()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().ContainAll(["xyz"], because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadContainAnyContainsNone()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().ContainAny(["xyz", "abc"]),
							"hello world should contain at least one of [xyz, abc]"
						);
	}

	[Fact]
	public void BadContainAnyWhenNull()
	{
		RunCompareFailTest(
							() => ((string?)null).Should().ContainAny(["hello"]),
							"null should contain at least one of [hello]"
						);
	}

	[Fact]
	public void BadContainAtLeastTwice()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().Contain("hello", AtLeast.Twice()),
							"hello world should contain hello at least 2 times but found 1"
						);
	}

	[Fact]
	public void BadContainWhenNull() { RunCompareFailTest(() => ((string?)null).Should().Contain("hello"), "null should contain hello"); }

	[Fact]
	public void BadContainWithBecause()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().Contain("xyz", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotContain()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().Not.Contain("hello"),
							"hello world should not contain hello"
						);
	}

	[Fact]
	public void BadNotContainAllContainsAll()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().Not.ContainAll(["hello", "world"]),
							"hello world should not contain all of [hello, world]"
						);
	}

	[Fact]
	public void BadNotContainAny()
	{
		RunCompareFailTest(
							() => ((string?)"hello world").Should().Not.ContainAny(["hello", "xyz"]),
							"hello world should not contain any of [hello, xyz]"
						);
	}

	[Fact]
	public void GoodContain() { ((string?)"hello world").Should().Contain("hello"); }

	[Fact]
	public void GoodContainAll() { ((string?)"hello world").Should().ContainAll(["hello", "world"]); }

	[Fact]
	public void GoodContainAny() { ((string?)"hello world").Should().ContainAny(["hello", "xyz"]); }

	[Fact]
	public void GoodNotContain() { ((string?)"hello world").Should().Not.Contain("xyz"); }

	[Fact]
	public void GoodNotContainAllMissingSome() { ((string?)"hello world").Should().Not.ContainAll(["hello", "xyz"]); }

	[Fact]
	public void GoodNotContainAllWhenNull() { ((string?)null).Should().Not.ContainAll(["hello"]); }

	[Fact]
	public void GoodNotContainAny() { ((string?)"hello world").Should().Not.ContainAny(["xyz", "abc"]); }

	[Fact]
	public void GoodNotContainAnyWhenNull() { ((string?)null).Should().Not.ContainAny(["hello"]); }

	[Fact]
	public void GoodNotContainWhenNull() { ((string?)null).Should().Not.Contain("hello"); }
}