namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeLowerCasedTests : BaseTest
{
	[Fact]
	public void BadBeLowerCased()
	{
		RunCompareFailTest(() => ((char?)'A').Should().BeLowerCased(), "A should be lower cased");
	}

	[Fact]
	public void BadBeLowerCasedNullValue()
	{
		RunCompareFailTest(() => ((char?)null).Should().BeLowerCased(), "null should be lower cased");
	}

	[Fact]
	public void BadBeLowerCasedWithBecause()
	{
		RunCompareFailTest(() => ((char?)'A').Should().BeLowerCased("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLowerCased()
	{
		RunCompareFailTest(() => ((char?)'a').Should().Not.BeLowerCased(), "a should not be lower cased");
	}

	[Fact]
	public void BadNotBeLowerCasedWithBecause()
	{
		RunCompareFailTest(() => ((char?)'a').Should().Not.BeLowerCased("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLowerCased()
	{
		((char?)'a').Should().BeLowerCased();
	}

	[Fact]
	public void GoodNotBeLowerCased()
	{
		((char?)'A').Should().Not.BeLowerCased();
	}

	[Fact]
	public void GoodNotBeLowerCasedWhenNull()
	{
		((char?)null).Should().Not.BeLowerCased();
	}
}
