namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeLetterOrDigitTests : BaseTest
{
	[Fact]
	public void BadBeLetterOrDigit() { RunCompareFailTest(() => ((char?)' ').Should().BeLetterOrDigit(), "  should be a letter or digit"); }

	[Fact]
	public void BadBeLetterOrDigitNullValue() { RunCompareFailTest(() => ((char?)null).Should().BeLetterOrDigit(), "null should be a letter or digit"); }

	[Fact]
	public void BadBeLetterOrDigitWithBecause() { RunCompareFailTest(() => ((char?)' ').Should().BeLetterOrDigit("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeLetterOrDigit() { RunCompareFailTest(() => ((char?)'a').Should().Not.BeLetterOrDigit(), "a should not be a letter or digit"); }

	[Fact]
	public void BadNotBeLetterOrDigitWithBecause() { RunCompareFailTest(() => ((char?)'a').Should().Not.BeLetterOrDigit("custom because"), "custom because"); }

	[Fact]
	public void GoodBeLetterOrDigit() { ((char?)'a').Should().BeLetterOrDigit(); }

	[Fact]
	public void GoodNotBeLetterOrDigit() { ((char?)' ').Should().Not.BeLetterOrDigit(); }

	[Fact]
	public void GoodNotBeLetterOrDigitWhenNull() { ((char?)null).Should().Not.BeLetterOrDigit(); }
}