namespace Tests.FatCat.Testing.Characters;

public class CharBeDigitTests : BaseTest
{
	[Fact]
	public void BadBeDigit() { RunCompareFailTest(() => 'a'.Should().BeDigit()); }

	[Fact]
	public void BadBeDigitShowsCorrectMessage() { RunCompareFailTest(() => 'a'.Should().BeDigit(), "a should be a digit"); }

	[Fact]
	public void BadBeDigitWithBecause() { RunCompareFailTest(() => 'a'.Should().BeDigit("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeDigit() { RunCompareFailTest(() => '1'.Should().Not.BeDigit()); }

	[Fact]
	public void BadNotBeDigitShowsCorrectMessage() { RunCompareFailTest(() => '1'.Should().Not.BeDigit(), "1 should not be a digit"); }

	[Fact]
	public void BadNotBeDigitWithBecause() { RunCompareFailTest(() => '1'.Should().Not.BeDigit("custom because"), "custom because"); }

	[Fact]
	public void GoodBeDigit() { '1'.Should().BeDigit(); }

	[Fact]
	public void GoodNotBeDigit() { 'a'.Should().Not.BeDigit(); }
}