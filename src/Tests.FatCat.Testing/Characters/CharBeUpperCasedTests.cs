namespace Tests.FatCat.Testing.Characters;

public class CharBeUpperCasedTests : BaseTest
{
	[Fact]
	public void BadBeUpperCased() { RunCompareFailTest(() => 'a'.Should().BeUpperCased()); }

	[Fact]
	public void BadBeUpperCasedShowsCorrectMessage() { RunCompareFailTest(() => 'a'.Should().BeUpperCased(), "a should be upper cased"); }

	[Fact]
	public void BadBeUpperCasedWithBecause() { RunCompareFailTest(() => 'a'.Should().BeUpperCased("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeUpperCased() { RunCompareFailTest(() => 'A'.Should().Not.BeUpperCased()); }

	[Fact]
	public void BadNotBeUpperCasedShowsCorrectMessage() { RunCompareFailTest(() => 'A'.Should().Not.BeUpperCased(), "A should not be upper cased"); }

	[Fact]
	public void BadNotBeUpperCasedWithBecause() { RunCompareFailTest(() => 'A'.Should().Not.BeUpperCased("custom because"), "custom because"); }

	[Fact]
	public void GoodBeUpperCased() { 'A'.Should().BeUpperCased(); }

	[Fact]
	public void GoodNotBeUpperCased() { 'a'.Should().Not.BeUpperCased(); }
}