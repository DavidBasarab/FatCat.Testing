namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeDigitTests : BaseTest
{
	[Fact]
	public void BadBeDigit() { RunCompareFailTest(() => ((char?)'a').Should().BeDigit(), "a should be a digit"); }

	[Fact]
	public void BadBeDigitNullValue() { RunCompareFailTest(() => ((char?)null).Should().BeDigit(), "null should be a digit"); }

	[Fact]
	public void BadBeDigitWithBecause() { RunCompareFailTest(() => ((char?)'a').Should().BeDigit("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeDigit() { RunCompareFailTest(() => ((char?)'1').Should().Not.BeDigit(), "1 should not be a digit"); }

	[Fact]
	public void BadNotBeDigitWithBecause() { RunCompareFailTest(() => ((char?)'1').Should().Not.BeDigit("custom because"), "custom because"); }

	[Fact]
	public void GoodBeDigit() { ((char?)'1').Should().BeDigit(); }

	[Fact]
	public void GoodNotBeDigit() { ((char?)'a').Should().Not.BeDigit(); }

	[Fact]
	public void GoodNotBeDigitWhenNull() { ((char?)null).Should().Not.BeDigit(); }
}