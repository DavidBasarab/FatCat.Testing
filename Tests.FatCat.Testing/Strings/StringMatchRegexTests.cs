using System.Text.RegularExpressions;

namespace Tests.FatCat.Testing.Strings;

public class StringMatchRegexTests : BaseTest
{
	[Fact]
	public void BadMatchRegexObjectWhenNoMatch() { RunCompareFailTest(() => "hello".Should().MatchRegex(new Regex("\\d+")), "hello should match regex \\d+"); }

	[Fact]
	public void BadMatchRegexObjectWhenNull()
	{
		RunCompareFailTest(
							() => ((string)null).Should().MatchRegex(new Regex("\\d+")),
							"null should match regex \\d+"
						);
	}

	[Fact]
	public void BadMatchRegexObjectWithBecause()
	{
		RunCompareFailTest(
							() => "hello".Should().MatchRegex(new Regex("\\d+"), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadMatchRegexStringWhenNoMatch() { RunCompareFailTest(() => "hello".Should().MatchRegex("\\d+"), "hello should match regex \\d+"); }

	[Fact]
	public void BadMatchRegexStringWhenNull() { RunCompareFailTest(() => ((string)null).Should().MatchRegex("\\d+"), "null should match regex \\d+"); }

	[Fact]
	public void BadMatchRegexStringWithBecause() { RunCompareFailTest(() => "hello".Should().MatchRegex("\\d+", "custom because"), "custom because"); }

	[Fact]
	public void BadNotMatchRegexObjectWhenMatch()
	{
		RunCompareFailTest(
							() => "hello123".Should().Not.MatchRegex(new Regex("\\d+")),
							"hello123 should not match regex \\d+"
						);
	}

	[Fact]
	public void BadNotMatchRegexObjectWithBecause()
	{
		RunCompareFailTest(
							() => "hello123".Should().Not.MatchRegex(new Regex("\\d+"), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotMatchRegexStringWhenMatch()
	{
		RunCompareFailTest(
							() => "hello123".Should().Not.MatchRegex("\\d+"),
							"hello123 should not match regex \\d+"
						);
	}

	[Fact]
	public void BadNotMatchRegexStringWithBecause() { RunCompareFailTest(() => "hello123".Should().Not.MatchRegex("\\d+", "custom because"), "custom because"); }

	[Fact]
	public void GoodMatchRegexIgnoreCase() { "HELLO".Should().MatchRegex(new Regex("hello", RegexOptions.IgnoreCase)); }

	[Fact]
	public void GoodMatchRegexObject() { "hello123".Should().MatchRegex(new Regex("\\d+")); }

	[Fact]
	public void GoodMatchRegexString() { "hello123".Should().MatchRegex("\\d+"); }

	[Fact]
	public void GoodNotMatchRegexObject() { "hello".Should().Not.MatchRegex(new Regex("\\d+")); }

	[Fact]
	public void GoodNotMatchRegexObjectWhenNull() { ((string)null).Should().Not.MatchRegex(new Regex("\\d+")); }

	[Fact]
	public void GoodNotMatchRegexString() { "hello".Should().Not.MatchRegex("\\d+"); }

	[Fact]
	public void GoodNotMatchRegexStringWhenNull() { ((string)null).Should().Not.MatchRegex("\\d+"); }
}