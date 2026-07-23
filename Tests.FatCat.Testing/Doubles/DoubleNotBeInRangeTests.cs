namespace Tests.FatCat.Testing.Doubles;

public class DoubleNotBeInRangeTests : BaseTest
{
	[Fact]
	public void BadNotBeInRange()
	{
		RunCompareFailTest(() => 3.0.Should().Not.BeInRange(1.0, 5.0));
	}

	[Fact]
	public void BadNotBeInRangeAtLowerBoundary()
	{
		RunCompareFailTest(() => 1.0.Should().Not.BeInRange(1.0, 5.0), "1 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeAtUpperBoundary()
	{
		RunCompareFailTest(() => 5.0.Should().Not.BeInRange(1.0, 5.0), "5 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 3.0.Should().Not.BeInRange(1.0, 5.0), "3 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeWithBecause()
	{
		RunCompareFailTest(() => 3.0.Should().Not.BeInRange(1.0, 5.0, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotBeInRange()
	{
		10.0.Should().Not.BeInRange(1.0, 5.0);
		0.0.Should().Not.BeInRange(1.0, 5.0);
	}
}
