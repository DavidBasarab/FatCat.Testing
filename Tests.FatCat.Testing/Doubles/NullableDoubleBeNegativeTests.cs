namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeNegativeTests : BaseTest
{
	[Fact]
	public void BadBeNegative()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeNegative());
	}

	[Fact]
	public void BadBeNegativeNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().BeNegative(), "null should be negative");
	}

	[Fact]
	public void BadBeNegativeShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeNegative(), "1.5 should be negative");
	}

	[Fact]
	public void BadBeNegativeWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNegative()
	{
		RunCompareFailTest(() => ((double?)-1.5).Should().Not.BeNegative());
	}

	[Fact]
	public void BadNotBeNegativeShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)-1.5).Should().Not.BeNegative(), "-1.5 should not be negative");
	}

	[Fact]
	public void BadNotBeNegativeWithBecause()
	{
		RunCompareFailTest(() => ((double?)-1.5).Should().Not.BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNegative()
	{
		((double?)-1.5).Should().BeNegative();
	}

	[Fact]
	public void GoodNotBeNegative()
	{
		((double?)1.5).Should().Not.BeNegative();
	}

	[Fact]
	public void GoodNotBeNegativeWhenNull()
	{
		((double?)null).Should().Not.BeNegative();
	}
}
