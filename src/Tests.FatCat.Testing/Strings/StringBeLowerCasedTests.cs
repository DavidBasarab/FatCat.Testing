namespace Tests.FatCat.Testing.Strings;

public class StringBeLowerCasedTests : BaseTest
{
	[Fact]
	public void BadBeLowerCasedMixedCase()
	{
		RunCompareFailTest(() => "Hello".Should().BeLowerCased(), "Hello should be lower cased");
	}

	[Fact]
	public void BadBeLowerCasedNoLetters()
	{
		RunCompareFailTest(() => "123".Should().BeLowerCased(), "123 should be lower cased");
	}

	[Fact]
	public void BadBeLowerCasedUpperCase()
	{
		RunCompareFailTest(() => "HELLO".Should().BeLowerCased(), "HELLO should be lower cased");
	}

	[Fact]
	public void BadBeLowerCasedWhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().BeLowerCased(), "null should be lower cased");
	}

	[Fact]
	public void BadBeLowerCasedWithBecause()
	{
		RunCompareFailTest(() => "HELLO".Should().BeLowerCased("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLowerCased()
	{
		RunCompareFailTest(() => "hello".Should().Not.BeLowerCased(), "hello should not be lower cased");
	}

	[Fact]
	public void BadNotBeLowerCasedWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.BeLowerCased("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLowerCased()
	{
		"hello".Should().BeLowerCased();
	}

	[Fact]
	public void GoodBeLowerCasedWithNonLetters()
	{
		"hello 123".Should().BeLowerCased();
	}

	[Fact]
	public void GoodNotBeLowerCasedUpperCase()
	{
		"HELLO".Should().Not.BeLowerCased();
	}

	[Fact]
	public void GoodNotBeLowerCasedWhenNull()
	{
		((string)null).Should().Not.BeLowerCased();
	}
}
