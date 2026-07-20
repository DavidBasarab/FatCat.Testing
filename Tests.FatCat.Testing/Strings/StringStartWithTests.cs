using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringStartWithTests : BaseTest
{
	[Fact]
	public void BadNotStartWith()
	{
		RunCompareFailTest(
							() => "hello world".Should().Not.StartWith("hello"),
							"hello world should not start with hello"
						);
	}

	[Fact]
	public void BadNotStartWithEquivalentOf()
	{
		RunCompareFailTest(
							() => "hello world".Should().Not.StartWithEquivalentOf("HELLO", Options.IgnoreCase),
							"hello world should not start with equivalent of HELLO"
						);
	}

	[Fact]
	public void BadNotStartWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
							() => "hello world".Should().Not.StartWithEquivalentOf("HELLO", Options.IgnoreCase, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotStartWithIgnoreCase()
	{
		RunCompareFailTest(
							() => "hello world".Should().Not.StartWith("HELLO", Options.IgnoreCase),
							"hello world should not start with HELLO"
						);
	}

	[Fact]
	public void BadNotStartWithWithBecause()
	{
		RunCompareFailTest(
							() => "hello world".Should().Not.StartWith("hello", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadStartWith() { RunCompareFailTest(() => "hello world".Should().StartWith("world"), "hello world should start with world"); }

	[Fact]
	public void BadStartWithDifferentCase() { RunCompareFailTest(() => "hello world".Should().StartWith("HELLO"), "hello world should start with HELLO"); }

	[Fact]
	public void BadStartWithEquivalentOf()
	{
		RunCompareFailTest(
							() => "hello world".Should().StartWithEquivalentOf("world"),
							"hello world should start with equivalent of world"
						);
	}

	[Fact]
	public void BadStartWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
							() => "hello world".Should().StartWithEquivalentOf("world", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadStartWithWhenNull() { RunCompareFailTest(() => ((string)null).Should().StartWith("hello"), "null should start with hello"); }

	[Fact]
	public void BadStartWithWithBecause()
	{
		RunCompareFailTest(
							() => "hello world".Should().StartWith("world", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodNotStartWith() { "hello world".Should().Not.StartWith("world"); }

	[Fact]
	public void GoodNotStartWithEquivalentOf() { "hello world".Should().Not.StartWithEquivalentOf("WORLD"); }

	[Fact]
	public void GoodNotStartWithIgnoreCase() { "hello world".Should().Not.StartWith("WORLD", Options.IgnoreCase); }

	[Fact]
	public void GoodNotStartWithWhenNull() { ((string)null).Should().Not.StartWith("hello"); }

	[Fact]
	public void GoodStartWith() { "hello world".Should().StartWith("hello"); }

	[Fact]
	public void GoodStartWithEquivalentOf() { "hello world".Should().StartWithEquivalentOf("HELLO", Options.IgnoreCase); }

	[Fact]
	public void GoodStartWithIgnoreCase() { "hello world".Should().StartWith("HELLO", Options.IgnoreCase); }
}