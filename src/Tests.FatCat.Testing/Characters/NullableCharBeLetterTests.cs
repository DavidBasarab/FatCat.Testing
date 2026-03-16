namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeLetterTests : BaseTest
{
	[Fact]
	public void BadBeLetter()
	{
		RunCompareFailTest(() => ((char?)'1').Should().BeLetter(), "1 should be a letter");
	}

	[Fact]
	public void BadBeLetterNullValue()
	{
		RunCompareFailTest(() => ((char?)null).Should().BeLetter(), "null should be a letter");
	}

	[Fact]
	public void BadBeLetterWithBecause()
	{
		RunCompareFailTest(() => ((char?)'1').Should().BeLetter("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLetter()
	{
		RunCompareFailTest(() => ((char?)'a').Should().Not.BeLetter(), "a should not be a letter");
	}

	[Fact]
	public void BadNotBeLetterWithBecause()
	{
		RunCompareFailTest(() => ((char?)'a').Should().Not.BeLetter("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLetter()
	{
		((char?)'a').Should().BeLetter();
	}

	[Fact]
	public void GoodNotBeLetter()
	{
		((char?)'1').Should().Not.BeLetter();
	}

	[Fact]
	public void GoodNotBeLetterWhenNull()
	{
		((char?)null).Should().Not.BeLetter();
	}
}
