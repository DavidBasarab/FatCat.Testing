namespace Tests.FatCat.Testing.Booleans;

public class BoolBeTrueTests : BaseTest
{
	[Fact]
	public void BadBeTrue() { RunCompareFailTest(() => false.Should().BeTrue()); }

	[Fact]
	public void BadBeTrueShowsCorrectMessage() { RunCompareFailTest(() => false.Should().BeTrue(), "False should be True"); }

	[Fact]
	public void BadBeTrueWithBecause() { RunCompareFailTest(() => false.Should().BeTrue("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeTrue() { RunCompareFailTest(() => true.Should().Not.BeTrue()); }

	[Fact]
	public void BadNotBeTrueShowsCorrectMessage() { RunCompareFailTest(() => true.Should().Not.BeTrue(), "True should not be True"); }

	[Fact]
	public void BadNotBeTrueWithBecause() { RunCompareFailTest(() => true.Should().Not.BeTrue("custom because"), "custom because"); }

	[Fact]
	public void GoodBeTrue() { true.Should().BeTrue(); }

	[Fact]
	public void GoodNotBeTrue() { false.Should().Not.BeTrue(); }
}