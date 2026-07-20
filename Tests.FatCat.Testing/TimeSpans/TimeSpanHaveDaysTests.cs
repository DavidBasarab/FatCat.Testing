namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanHaveDaysTests : BaseTest
{
	[Fact]
	public void BadHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => span.Should().HaveDays(5));
	}

	[Fact]
	public void BadHaveDaysShowsCorrectMessage()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => span.Should().HaveDays(5), "3.00:00:00 should have days 5");
	}

	[Fact]
	public void BadHaveDaysWithBecause()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => span.Should().HaveDays(5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => span.Should().Not.HaveDays(3));
	}

	[Fact]
	public void BadNotHaveDaysShowsCorrectMessage()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => span.Should().Not.HaveDays(3), "3.00:00:00 should not have days 3");
	}

	[Fact]
	public void BadNotHaveDaysWithBecause()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		RunCompareFailTest(() => span.Should().Not.HaveDays(3, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		span.Should().HaveDays(3);
	}

	[Fact]
	public void GoodNotHaveDays()
	{
		var span = new TimeSpan(3, 0, 0, 0);

		span.Should().Not.HaveDays(5);
	}
}