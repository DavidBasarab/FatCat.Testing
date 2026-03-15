using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringEndWithTests : BaseTest
{
	[Fact]
	public void BadEndWith()
	{
		RunCompareFailTest(() => "hello world".Should().EndWith("hello"), "hello world should end with hello");
	}

	[Fact]
	public void BadEndWithDifferentCase()
	{
		RunCompareFailTest(() => "hello world".Should().EndWith("WORLD"), "hello world should end with WORLD");
	}

	[Fact]
	public void BadEndWithEquivalentOf()
	{
		RunCompareFailTest(
			() => "hello world".Should().EndWithEquivalentOf("hello"),
			"hello world should end with equivalent of hello"
		);
	}

	[Fact]
	public void BadEndWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().EndWithEquivalentOf("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadEndWithWhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().EndWith("world"), "null should end with world");
	}

	[Fact]
	public void BadEndWithWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().EndWith("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotEndWith()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWith("world"),
			"hello world should not end with world"
		);
	}

	[Fact]
	public void BadNotEndWithEquivalentOf()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWithEquivalentOf("WORLD", Options.IgnoreCase),
			"hello world should not end with equivalent of WORLD"
		);
	}

	[Fact]
	public void BadNotEndWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWithEquivalentOf("WORLD", Options.IgnoreCase, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotEndWithIgnoreCase()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWith("WORLD", Options.IgnoreCase),
			"hello world should not end with WORLD"
		);
	}

	[Fact]
	public void BadNotEndWithWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWith("world", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodEndWith()
	{
		"hello world".Should().EndWith("world");
	}

	[Fact]
	public void GoodEndWithEquivalentOf()
	{
		"hello world".Should().EndWithEquivalentOf("WORLD", Options.IgnoreCase);
	}

	[Fact]
	public void GoodEndWithIgnoreCase()
	{
		"hello world".Should().EndWith("WORLD", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotEndWith()
	{
		"hello world".Should().Not.EndWith("hello");
	}

	[Fact]
	public void GoodNotEndWithEquivalentOf()
	{
		"hello world".Should().Not.EndWithEquivalentOf("HELLO");
	}

	[Fact]
	public void GoodNotEndWithIgnoreCase()
	{
		"hello world".Should().Not.EndWith("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotEndWithWhenNull()
	{
		((string)null).Should().Not.EndWith("world");
	}
}
