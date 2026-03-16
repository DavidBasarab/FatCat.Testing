namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeTests : BaseTest
{
	[Fact]
	public void BadBe() { RunCompareFailTest(() => ((char?)'a').Should().Be('b'), "a should be b"); }

	[Fact]
	public void BadBeNullValue() { RunCompareFailTest(() => ((char?)null).Should().Be('b'), "null should be b"); }

	[Fact]
	public void BadBeWithBecause() { RunCompareFailTest(() => ((char?)'a').Should().Be('b', "custom because"), "custom because"); }

	[Fact]
	public void BadNotBe() { RunCompareFailTest(() => ((char?)'a').Should().Not.Be('a'), "a should not be a"); }

	[Fact]
	public void BadNotBeWithBecause() { RunCompareFailTest(() => ((char?)'a').Should().Not.Be('a', "custom because"), "custom because"); }

	[Fact]
	public void GoodBe() { ((char?)'a').Should().Be('a'); }

	[Fact]
	public void GoodNotBe() { ((char?)'a').Should().Not.Be('b'); }

	[Fact]
	public void GoodNotBeWhenNull() { ((char?)null).Should().Not.Be('a'); }
}