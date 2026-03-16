using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringMatchTests : BaseTest
{
	[Fact]
	public void BadMatch() { RunCompareFailTest(() => "hello".Should().Match("world*"), "hello should match world*"); }

	[Fact]
	public void BadMatchCaseSensitive() { RunCompareFailTest(() => "hello".Should().Match("HELLO"), "hello should match HELLO"); }

	[Fact]
	public void BadMatchWhenNull() { RunCompareFailTest(() => ((string)null).Should().Match("hello*"), "null should match hello*"); }

	[Fact]
	public void BadMatchWithBecause() { RunCompareFailTest(() => "hello".Should().Match("world*", because: "custom because"), "custom because"); }

	[Fact]
	public void BadNotMatch()
	{
		RunCompareFailTest(
							() => "hello world".Should().Not.Match("hello*"),
							"hello world should not match hello*"
						);
	}

	[Fact]
	public void BadNotMatchIgnoreCase()
	{
		RunCompareFailTest(
							() => "HELLO".Should().Not.Match("hello*", Options.IgnoreCase),
							"HELLO should not match hello*"
						);
	}

	[Fact]
	public void BadNotMatchWithBecause()
	{
		RunCompareFailTest(
							() => "hello world".Should().Not.Match("hello*", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodMatch() { "hello world".Should().Match("hello*"); }

	[Fact]
	public void GoodMatchAllWildcard() { "anything".Should().Match("*"); }

	[Fact]
	public void GoodMatchExact() { "hello".Should().Match("hello"); }

	[Fact]
	public void GoodMatchIgnoreCase() { "HELLO".Should().Match("hello*", Options.IgnoreCase); }

	[Fact]
	public void GoodMatchMiddleWildcard() { "hello world".Should().Match("hello*world"); }

	[Fact]
	public void GoodMatchQuestionMark() { "hello".Should().Match("hel?o"); }

	[Fact]
	public void GoodNotMatch() { "hello".Should().Not.Match("world*"); }

	[Fact]
	public void GoodNotMatchWhenNull() { ((string)null).Should().Not.Match("hello*"); }
}