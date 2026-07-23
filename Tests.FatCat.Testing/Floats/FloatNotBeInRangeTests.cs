namespace Tests.FatCat.Testing.Floats;

public class FloatNotBeInRangeTests : BaseTest
{
	[Fact]
	public void BadNotBeInRange()
	{
		RunCompareFailTest(() => 3.0f.Should().Not.BeInRange(1.0f, 5.0f));
	}

	[Fact]
	public void BadNotBeInRangeAtLowerBoundary()
	{
		RunCompareFailTest(() => 1.0f.Should().Not.BeInRange(1.0f, 5.0f), "1 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeAtUpperBoundary()
	{
		RunCompareFailTest(() => 5.0f.Should().Not.BeInRange(1.0f, 5.0f), "5 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 3.0f.Should().Not.BeInRange(1.0f, 5.0f), "3 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeWithBecause()
	{
		RunCompareFailTest(() => 3.0f.Should().Not.BeInRange(1.0f, 5.0f, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotBeInRange()
	{
		10.0f.Should().Not.BeInRange(1.0f, 5.0f);
		0.0f.Should().Not.BeInRange(1.0f, 5.0f);
	}
}
