#nullable enable

using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class NullableStringMatchTests : BaseTest
{
	[Fact]
	public void BadMatch()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().Match("world*"), "hello should match world*");
	}

	[Fact]
	public void BadMatchWhenNull()
	{
		RunCompareFailTest(() => ((string?)null).Should().Match("hello*"), "null should match hello*");
	}

	[Fact]
	public void BadMatchWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().Match("world*", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotMatch()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.Match("hello*"),
			"hello world should not match hello*"
		);
	}

	[Fact]
	public void BadNotMatchWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.Match("hello*", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodMatch()
	{
		((string?)"hello world").Should().Match("hello*");
	}

	[Fact]
	public void GoodMatchIgnoreCase()
	{
		((string?)"hello world").Should().Match("HELLO*", Options.IgnoreCase);
	}

	[Fact]
	public void GoodMatchWildcardQuestion()
	{
		((string?)"hello").Should().Match("hell?");
	}

	[Fact]
	public void GoodNotMatch()
	{
		((string?)"hello").Should().Not.Match("world*");
	}

	[Fact]
	public void GoodNotMatchWhenNull()
	{
		((string?)null).Should().Not.Match("hello*");
	}
}
