namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeNegativeTests : BaseTest
{
	[Fact]
	public void BadBeNegative()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().BeNegative(), "01:00:00 should be negative");
	}

	[Fact]
	public void BadBeNegativeNullValue()
	{
		RunCompareFailTest(() => ((TimeSpan?)null).Should().BeNegative(), "null should be negative");
	}

	[Fact]
	public void BadBeNegativeWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNegative()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().Not.BeNegative(), "-01:00:00 should not be negative");
	}

	[Fact]
	public void BadNotBeNegativeWithBecause()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().Not.BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNegative()
	{
		var span = TimeSpan.FromHours(-1);

		((TimeSpan?)span).Should().BeNegative();
	}

	[Fact]
	public void GoodNotBeNegative()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().Not.BeNegative();
	}

	[Fact]
	public void GoodNotBeNegativeWhenNull()
	{
		((TimeSpan?)null).Should().Not.BeNegative();
	}
}
