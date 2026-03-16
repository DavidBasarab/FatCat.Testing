#nullable enable

using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class NullableStringEndWithTests : BaseTest
{
	[Fact]
	public void BadEndWith()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().EndWith("hello"),
			"hello world should end with hello"
		);
	}

	[Fact]
	public void BadEndWithEquivalentOf()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().EndWithEquivalentOf("hello"),
			"hello world should end with equivalent of hello"
		);
	}

	[Fact]
	public void BadEndWithWhenNull()
	{
		RunCompareFailTest(() => ((string?)null).Should().EndWith("world"), "null should end with world");
	}

	[Fact]
	public void BadEndWithWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().EndWith("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotEndWith()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.EndWith("world"),
			"hello world should not end with world"
		);
	}

	[Fact]
	public void BadNotEndWithEquivalentOf()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.EndWithEquivalentOf("world"),
			"hello world should not end with equivalent of world"
		);
	}

	[Fact]
	public void BadNotEndWithIgnoreCase()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.EndWith("WORLD", Options.IgnoreCase),
			"hello world should not end with WORLD"
		);
	}

	[Fact]
	public void BadNotEndWithWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.EndWith("world", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodEndWith()
	{
		((string?)"hello world").Should().EndWith("world");
	}

	[Fact]
	public void GoodEndWithEquivalentOf()
	{
		((string?)"hello world").Should().EndWithEquivalentOf("world");
	}

	[Fact]
	public void GoodEndWithIgnoreCase()
	{
		((string?)"hello world").Should().EndWith("WORLD", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotEndWith()
	{
		((string?)"hello world").Should().Not.EndWith("hello");
	}

	[Fact]
	public void GoodNotEndWithWhenNull()
	{
		((string?)null).Should().Not.EndWith("world");
	}
}
