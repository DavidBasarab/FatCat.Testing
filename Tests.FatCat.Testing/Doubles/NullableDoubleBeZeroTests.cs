namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeZeroTests : BaseTest
{
	[Fact]
	public void BadBeZero()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeZero());
	}

	[Fact]
	public void BadBeZeroNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().BeZero(), "null should be zero");
	}

	[Fact]
	public void BadBeZeroShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeZero(), "1.5 should be zero");
	}

	[Fact]
	public void BadBeZeroWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeZero("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeZero()
	{
		RunCompareFailTest(() => ((double?)0.0).Should().Not.BeZero());
	}

	[Fact]
	public void BadNotBeZeroShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)0.0).Should().Not.BeZero(), "0 should not be zero");
	}

	[Fact]
	public void BadNotBeZeroWithBecause()
	{
		RunCompareFailTest(() => ((double?)0.0).Should().Not.BeZero("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeZero()
	{
		((double?)0.0).Should().BeZero();
	}

	[Fact]
	public void GoodNotBeZero()
	{
		((double?)1.5).Should().Not.BeZero();
	}

	[Fact]
	public void GoodNotBeZeroWhenNull()
	{
		((double?)null).Should().Not.BeZero();
	}
}
