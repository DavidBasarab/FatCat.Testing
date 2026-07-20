namespace Tests.FatCat.Testing.Characters;

public class CharBeLetterTests : BaseTest
{
	[Fact]
	public void BadBeLetter()
	{
		RunCompareFailTest(() => '1'.Should().BeLetter());
	}

	[Fact]
	public void BadBeLetterShowsCorrectMessage()
	{
		RunCompareFailTest(() => '1'.Should().BeLetter(), "1 should be a letter");
	}

	[Fact]
	public void BadBeLetterWithBecause()
	{
		RunCompareFailTest(() => '1'.Should().BeLetter("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLetter()
	{
		RunCompareFailTest(() => 'a'.Should().Not.BeLetter());
	}

	[Fact]
	public void BadNotBeLetterShowsCorrectMessage()
	{
		RunCompareFailTest(() => 'a'.Should().Not.BeLetter(), "a should not be a letter");
	}

	[Fact]
	public void BadNotBeLetterWithBecause()
	{
		RunCompareFailTest(() => 'a'.Should().Not.BeLetter("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLetter()
	{
		'a'.Should().BeLetter();
	}

	[Fact]
	public void GoodNotBeLetter()
	{
		'1'.Should().Not.BeLetter();
	}
}
