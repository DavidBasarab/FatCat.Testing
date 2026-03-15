namespace Tests.FatCat.Testing.Numbers;

public class IntBeZeroTests : BaseTest
{
	[Fact]
	public void BadBeZero()
	{
		RunCompareFailTest(() => 5.Should().BeZero(), "5 should be zero");
	}

	[Fact]
	public void BadBeZeroWithBecause()
	{
		RunCompareFailTest(() => 5.Should().BeZero("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeZero()
	{
		RunCompareFailTest(() => 0.Should().Not.BeZero(), "0 should not be zero");
	}

	[Fact]
	public void BadNotBeZeroWithBecause()
	{
		RunCompareFailTest(() => 0.Should().Not.BeZero("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeZero()
	{
		0.Should().BeZero();
	}

	[Fact]
	public void GoodNotBeZero()
	{
		1.Should().Not.BeZero();
		(-1).Should().Not.BeZero();
	}
}
