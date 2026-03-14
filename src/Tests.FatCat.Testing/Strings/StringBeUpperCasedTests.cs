namespace Tests.FatCat.Testing.Strings;

public class StringBeUpperCasedTests : BaseTest
{
	[Fact]
	public void BadBeUpperCasedLowerCase()
	{
		RunCompareFailTest(() => "hello".Should().BeUpperCased(), "hello should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedMixedCase()
	{
		RunCompareFailTest(() => "Hello".Should().BeUpperCased(), "Hello should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedNoLetters()
	{
		RunCompareFailTest(() => "123".Should().BeUpperCased(), "123 should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedWhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().BeUpperCased(), "null should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeUpperCased()
	{
		RunCompareFailTest(() => "HELLO".Should().Not.BeUpperCased(), "HELLO should not be upper cased");
	}

	[Fact]
	public void BadNotBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => "HELLO".Should().Not.BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeUpperCased()
	{
		"HELLO".Should().BeUpperCased();
	}

	[Fact]
	public void GoodBeUpperCasedWithNonLetters()
	{
		"HELLO 123".Should().BeUpperCased();
	}

	[Fact]
	public void GoodNotBeUpperCasedLowerCase()
	{
		"hello".Should().Not.BeUpperCased();
	}

	[Fact]
	public void GoodNotBeUpperCasedWhenNull()
	{
		((string)null).Should().Not.BeUpperCased();
	}
}
