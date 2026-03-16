namespace Tests.FatCat.Testing.Characters;

public class CharBeLowerCasedTests : BaseTest
{
	[Fact]
	public void BadBeLowerCased() { RunCompareFailTest(() => 'A'.Should().BeLowerCased()); }

	[Fact]
	public void BadBeLowerCasedShowsCorrectMessage() { RunCompareFailTest(() => 'A'.Should().BeLowerCased(), "A should be lower cased"); }

	[Fact]
	public void BadBeLowerCasedWithBecause() { RunCompareFailTest(() => 'A'.Should().BeLowerCased("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeLowerCased() { RunCompareFailTest(() => 'a'.Should().Not.BeLowerCased()); }

	[Fact]
	public void BadNotBeLowerCasedShowsCorrectMessage() { RunCompareFailTest(() => 'a'.Should().Not.BeLowerCased(), "a should not be lower cased"); }

	[Fact]
	public void BadNotBeLowerCasedWithBecause() { RunCompareFailTest(() => 'a'.Should().Not.BeLowerCased("custom because"), "custom because"); }

	[Fact]
	public void GoodBeLowerCased() { 'a'.Should().BeLowerCased(); }

	[Fact]
	public void GoodNotBeLowerCased() { 'A'.Should().Not.BeLowerCased(); }
}