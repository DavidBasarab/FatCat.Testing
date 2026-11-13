namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeInRangeTests : BaseTest
{
	[Fact]
	public void BadBeInRange()
	{
		RunCompareFailTest(() => ((double?)5.5).Should().BeInRange(1.5, 2.5));
	}

	[Fact]
	public void BadBeInRangeNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().BeInRange(1.5, 2.5), "null should be between 1.5 and 2.5");
	}

	[Fact]
	public void BadBeInRangeShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)5.5).Should().BeInRange(1.5, 2.5), "5.5 should be between 1.5 and 2.5");
	}

	[Fact]
	public void BadBeInRangeWithBecause()
	{
		RunCompareFailTest(() => ((double?)5.5).Should().BeInRange(1.5, 2.5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeInRange()
	{
		RunCompareFailTest(() => ((double?)2.25).Should().Not.BeInRange(1.5, 2.5));
	}

	[Fact]
	public void BadNotBeInRangeShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)2.25).Should().Not.BeInRange(1.5, 2.5), "2.25 should not be between 1.5 and 2.5");
	}

	[Fact]
	public void BadNotBeInRangeWithBecause()
	{
		RunCompareFailTest(() => ((double?)2.25).Should().Not.BeInRange(1.5, 2.5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeInRange()
	{
		((double?)2.25).Should().BeInRange(1.5, 2.5);
	}

	[Fact]
	public void GoodNotBeInRange()
	{
		((double?)5.5).Should().Not.BeInRange(1.5, 2.5);
	}

	[Fact]
	public void GoodNotBeInRangeWhenNull()
	{
		((double?)null).Should().Not.BeInRange(1.5, 2.5);
	}
}
