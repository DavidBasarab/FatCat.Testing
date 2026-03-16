namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanHaveHoursTests : BaseTest
{
	[Fact]
	public void BadHaveHours()
	{
		var span = TimeSpan.FromHours(5);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveHours(3), "05:00:00 should have hours 3");
	}

	[Fact]
	public void BadHaveHoursNullValue()
	{
		RunCompareFailTest(() => ((TimeSpan?)null).Should().HaveHours(3), "null should have hours 3");
	}

	[Fact]
	public void BadHaveHoursWithBecause()
	{
		var span = TimeSpan.FromHours(5);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveHours(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveHours()
	{
		var span = TimeSpan.FromHours(5);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().Not.HaveHours(5), "05:00:00 should not have hours 5");
	}

	[Fact]
	public void BadNotHaveHoursWithBecause()
	{
		var span = TimeSpan.FromHours(5);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().Not.HaveHours(5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveHours()
	{
		var span = TimeSpan.FromHours(5);

		((TimeSpan?)span).Should().HaveHours(5);
	}

	[Fact]
	public void GoodNotHaveHours()
	{
		var span = TimeSpan.FromHours(5);

		((TimeSpan?)span).Should().Not.HaveHours(3);
	}

	[Fact]
	public void GoodNotHaveHoursWhenNull()
	{
		((TimeSpan?)null).Should().Not.HaveHours(5);
	}
}
