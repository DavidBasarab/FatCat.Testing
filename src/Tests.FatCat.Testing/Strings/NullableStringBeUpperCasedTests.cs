#nullable enable

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeUpperCasedTests : BaseTest
{
	[Fact]
	public void BadBeUpperCased()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().BeUpperCased(), "hello should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedWhenNull()
	{
		RunCompareFailTest(() => ((string?)null).Should().BeUpperCased(), "null should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeUpperCased()
	{
		RunCompareFailTest(
			() => ((string?)"HELLO").Should().Not.BeUpperCased(),
			"HELLO should not be upper cased"
		);
	}

	[Fact]
	public void BadNotBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => ((string?)"HELLO").Should().Not.BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeUpperCased()
	{
		((string?)"HELLO").Should().BeUpperCased();
	}

	[Fact]
	public void GoodNotBeUpperCased()
	{
		((string?)"hello").Should().Not.BeUpperCased();
	}

	[Fact]
	public void GoodNotBeUpperCasedWhenNull()
	{
		((string?)null).Should().Not.BeUpperCased();
	}
}
