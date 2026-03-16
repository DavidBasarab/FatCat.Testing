namespace Tests.FatCat.Testing.Numbers;

public class IntBeInRangeTests : BaseTest
{
	[Fact]
	public void BadBeInRange()
	{
		RunCompareFailTest(() => 2.Should().BeInRange(3, 5), "2 should be between 3 and 5");
		RunCompareFailTest(() => 6.Should().BeInRange(3, 5), "6 should be between 3 and 5");
	}

	[Fact]
	public void BadBeInRangeWithBecause() { RunCompareFailTest(() => 2.Should().BeInRange(3, 5, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeInRange() { RunCompareFailTest(() => 2.Should().Not.BeInRange(1, 7), "2 should not be between 1 and 7"); }

	[Fact]
	public void BadNotBeInRangeWithBecause() { RunCompareFailTest(() => 2.Should().Not.BeInRange(1, 7, "custom because"), "custom because"); }

	[Fact]
	public void GoodBeInRange() { 2.Should().BeInRange(1, 7); }

	[Fact]
	public void GoodNotBeInRange()
	{
		2.Should().Not.BeInRange(3, 5);
		6.Should().Not.BeInRange(3, 5);
	}
}