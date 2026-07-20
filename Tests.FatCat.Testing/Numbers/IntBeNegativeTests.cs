namespace Tests.FatCat.Testing.Numbers;

public class IntBeNegativeTests : BaseTest
{
	[Fact]
	public void BadBeNegative()
	{
		RunCompareFailTest(() => 1.Should().BeNegative(), "1 should be negative");
		RunCompareFailTest(() => 0.Should().BeNegative(), "0 should be negative");
	}

	[Fact]
	public void BadBeNegativeWithBecause() { RunCompareFailTest(() => 1.Should().BeNegative("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeNegative() { RunCompareFailTest(() => (-1).Should().Not.BeNegative(), "-1 should not be negative"); }

	[Fact]
	public void BadNotBeNegativeWithBecause() { RunCompareFailTest(() => (-1).Should().Not.BeNegative("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNegative()
	{
		(-1).Should().BeNegative();
		(-100).Should().BeNegative();
	}

	[Fact]
	public void GoodNotBeNegative()
	{
		0.Should().Not.BeNegative();
		1.Should().Not.BeNegative();
	}
}