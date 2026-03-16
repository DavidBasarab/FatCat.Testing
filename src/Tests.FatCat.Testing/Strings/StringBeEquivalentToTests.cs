using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringBeEquivalentToTests : BaseTest
{
	[Fact]
	public void BadBeEquivalentTo() { RunCompareFailTest(() => "hello".Should().BeEquivalentTo("world"), "hello should be equivalent to world"); }

	[Fact]
	public void BadBeEquivalentToIgnoreCase()
	{
		RunCompareFailTest(
							() => "hello".Should().BeEquivalentTo("world", Options.IgnoreCase),
							"hello should be equivalent to world"
						);
	}

	[Fact]
	public void BadBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
							() => "hello".Should().BeEquivalentTo("world", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeEquivalentTo()
	{
		RunCompareFailTest(
							() => "hello".Should().Not.BeEquivalentTo("hello"),
							"hello should not be equivalent to hello"
						);
	}

	[Fact]
	public void BadNotBeEquivalentToIgnoreCase()
	{
		RunCompareFailTest(
							() => "hello".Should().Not.BeEquivalentTo("HELLO", Options.IgnoreCase),
							"hello should not be equivalent to HELLO"
						);
	}

	[Fact]
	public void BadNotBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
							() => "hello".Should().Not.BeEquivalentTo("hello", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeEquivalentTo() { "hello".Should().BeEquivalentTo("hello"); }

	[Fact]
	public void GoodBeEquivalentToIgnoreCase() { "hello".Should().BeEquivalentTo("HELLO", Options.IgnoreCase); }

	[Fact]
	public void GoodNotBeEquivalentTo() { "hello".Should().Not.BeEquivalentTo("world"); }

	[Fact]
	public void GoodNotBeEquivalentToIgnoreCase() { "hello".Should().Not.BeEquivalentTo("world", Options.IgnoreCase); }
}