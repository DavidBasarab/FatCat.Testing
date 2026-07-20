namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanHaveDaysTests : BaseTest
{
	[Fact]
	public void BadHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveDays(5), "3.00:00:00 should have days 5");
	}

	[Fact]
	public void BadHaveDaysNullValue() { RunCompareFailTest(() => ((TimeSpan?)null).Should().HaveDays(5), "null should have days 5"); }

	[Fact]
	public void BadHaveDaysWithBecause()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveDays(5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().Not.HaveDays(3), "3.00:00:00 should not have days 3");
	}

	[Fact]
	public void BadNotHaveDaysWithBecause()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().Not.HaveDays(3, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		((TimeSpan?)span).Should().HaveDays(3);
	}

	[Fact]
	public void GoodNotHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		((TimeSpan?)span).Should().Not.HaveDays(5);
	}

	[Fact]
	public void GoodNotHaveDaysWhenNull() { ((TimeSpan?)null).Should().Not.HaveDays(3); }
}