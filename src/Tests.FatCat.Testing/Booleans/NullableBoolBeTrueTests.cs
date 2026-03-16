namespace Tests.FatCat.Testing.Booleans;

public class NullableBoolBeTrueTests : BaseTest
{
	[Fact]
	public void BadBeTrue() { RunCompareFailTest(() => ((bool?)false).Should().BeTrue(), "False should be True"); }

	[Fact]
	public void BadBeTrueNullValue() { RunCompareFailTest(() => ((bool?)null).Should().BeTrue(), "null should be True"); }

	[Fact]
	public void BadBeTrueWithBecause() { RunCompareFailTest(() => ((bool?)false).Should().BeTrue("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeTrue() { RunCompareFailTest(() => ((bool?)true).Should().Not.BeTrue(), "True should not be True"); }

	[Fact]
	public void BadNotBeTrueWithBecause() { RunCompareFailTest(() => ((bool?)true).Should().Not.BeTrue("custom because"), "custom because"); }

	[Fact]
	public void GoodBeTrue() { ((bool?)true).Should().BeTrue(); }

	[Fact]
	public void GoodNotBeTrue() { ((bool?)false).Should().Not.BeTrue(); }

	[Fact]
	public void GoodNotBeTrueWhenNull() { ((bool?)null).Should().Not.BeTrue(); }
}