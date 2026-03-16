namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeWhiteSpaceTests : BaseTest
{
	[Fact]
	public void BadBeWhiteSpace()
	{
		RunCompareFailTest(() => ((char?)'a').Should().BeWhiteSpace(), "a should be white space");
	}

	[Fact]
	public void BadBeWhiteSpaceNullValue()
	{
		RunCompareFailTest(() => ((char?)null).Should().BeWhiteSpace(), "null should be white space");
	}

	[Fact]
	public void BadBeWhiteSpaceWithBecause()
	{
		RunCompareFailTest(() => ((char?)'a').Should().BeWhiteSpace("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeWhiteSpace()
	{
		RunCompareFailTest(() => ((char?)' ').Should().Not.BeWhiteSpace(), "  should not be white space");
	}

	[Fact]
	public void BadNotBeWhiteSpaceWithBecause()
	{
		RunCompareFailTest(() => ((char?)' ').Should().Not.BeWhiteSpace("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeWhiteSpace()
	{
		((char?)' ').Should().BeWhiteSpace();
	}

	[Fact]
	public void GoodNotBeWhiteSpace()
	{
		((char?)'a').Should().Not.BeWhiteSpace();
	}

	[Fact]
	public void GoodNotBeWhiteSpaceWhenNull()
	{
		((char?)null).Should().Not.BeWhiteSpace();
	}
}
