namespace Tests.FatCat.Testing.Characters;

public class CharBeLetterOrDigitTests : BaseTest
{
	[Fact]
	public void BadBeLetterOrDigit()
	{
		RunCompareFailTest(() => ' '.Should().BeLetterOrDigit());
	}

	[Fact]
	public void BadBeLetterOrDigitShowsCorrectMessage()
	{
		RunCompareFailTest(() => ' '.Should().BeLetterOrDigit(), "  should be a letter or digit");
	}

	[Fact]
	public void BadBeLetterOrDigitWithBecause()
	{
		RunCompareFailTest(() => ' '.Should().BeLetterOrDigit("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLetterOrDigit()
	{
		RunCompareFailTest(() => 'a'.Should().Not.BeLetterOrDigit());
	}

	[Fact]
	public void BadNotBeLetterOrDigitShowsCorrectMessage()
	{
		RunCompareFailTest(() => 'a'.Should().Not.BeLetterOrDigit(), "a should not be a letter or digit");
	}

	[Fact]
	public void BadNotBeLetterOrDigitWithBecause()
	{
		RunCompareFailTest(() => 'a'.Should().Not.BeLetterOrDigit("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLetterOrDigit()
	{
		'a'.Should().BeLetterOrDigit();
	}

	[Fact]
	public void GoodNotBeLetterOrDigit()
	{
		' '.Should().Not.BeLetterOrDigit();
	}
}
