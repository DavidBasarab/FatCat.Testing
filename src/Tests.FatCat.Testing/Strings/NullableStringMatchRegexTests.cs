#nullable enable

using System.Text.RegularExpressions;

namespace Tests.FatCat.Testing.Strings;

public class NullableStringMatchRegexTests : BaseTest
{
	[Fact]
	public void BadMatchRegexObjectWhenNoMatch()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().MatchRegex(new Regex("\\d+")),
			"hello should match regex \\d+"
		);
	}

	[Fact]
	public void BadMatchRegexObjectWhenNull()
	{
		RunCompareFailTest(
			() => ((string?)null).Should().MatchRegex(new Regex("\\d+")),
			"null should match regex \\d+"
		);
	}

	[Fact]
	public void BadMatchRegexStringWhenNoMatch()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().MatchRegex("\\d+"), "hello should match regex \\d+");
	}

	[Fact]
	public void BadMatchRegexStringWhenNull()
	{
		RunCompareFailTest(() => ((string?)null).Should().MatchRegex("\\d+"), "null should match regex \\d+");
	}

	[Fact]
	public void BadMatchRegexWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().MatchRegex("\\d+", "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotMatchRegexObjectWhenMatch()
	{
		RunCompareFailTest(
			() => ((string?)"hello123").Should().Not.MatchRegex(new Regex("\\d+")),
			"hello123 should not match regex \\d+"
		);
	}

	[Fact]
	public void BadNotMatchRegexStringWhenMatch()
	{
		RunCompareFailTest(
			() => ((string?)"hello123").Should().Not.MatchRegex("\\d+"),
			"hello123 should not match regex \\d+"
		);
	}

	[Fact]
	public void BadNotMatchRegexWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello123").Should().Not.MatchRegex("\\d+", "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodMatchRegexIgnoreCase()
	{
		((string?)"HELLO").Should().MatchRegex(new Regex("hello", RegexOptions.IgnoreCase));
	}

	[Fact]
	public void GoodMatchRegexObject()
	{
		((string?)"hello123").Should().MatchRegex(new Regex("\\d+"));
	}

	[Fact]
	public void GoodMatchRegexString()
	{
		((string?)"hello123").Should().MatchRegex("\\d+");
	}

	[Fact]
	public void GoodNotMatchRegexObject()
	{
		((string?)"hello").Should().Not.MatchRegex(new Regex("\\d+"));
	}

	[Fact]
	public void GoodNotMatchRegexObjectWhenNull()
	{
		((string?)null).Should().Not.MatchRegex(new Regex("\\d+"));
	}

	[Fact]
	public void GoodNotMatchRegexString()
	{
		((string?)"hello").Should().Not.MatchRegex("\\d+");
	}

	[Fact]
	public void GoodNotMatchRegexStringWhenNull()
	{
		((string?)null).Should().Not.MatchRegex("\\d+");
	}
}
