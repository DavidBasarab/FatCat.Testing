namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeUpperCasedTests : BaseTest
{
	[Fact]
	public void BadBeUpperCased()
	{
		RunCompareFailTest(() => ((char?)'a').Should().BeUpperCased(), "a should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedNullValue()
	{
		RunCompareFailTest(() => ((char?)null).Should().BeUpperCased(), "null should be upper cased");
	}

	[Fact]
	public void BadBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => ((char?)'a').Should().BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeUpperCased()
	{
		RunCompareFailTest(() => ((char?)'A').Should().Not.BeUpperCased(), "A should not be upper cased");
	}

	[Fact]
	public void BadNotBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => ((char?)'A').Should().Not.BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeUpperCased()
	{
		((char?)'A').Should().BeUpperCased();
	}

	[Fact]
	public void GoodNotBeUpperCased()
	{
		((char?)'a').Should().Not.BeUpperCased();
	}

	[Fact]
	public void GoodNotBeUpperCasedWhenNull()
	{
		((char?)null).Should().Not.BeUpperCased();
	}
}
