namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeHaveHourTests : BaseTest
{
	[Fact]
	public void BadHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveHour(11));
	}

	[Fact]
	public void BadHaveHourShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveHour(11), "2024-06-15 10:30:45 should have hour 11");
	}

	[Fact]
	public void BadHaveHourWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveHour(11, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveHour(10));
	}

	[Fact]
	public void BadNotHaveHourShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveHour(10), "2024-06-15 10:30:45 should not have hour 10");
	}

	[Fact]
	public void BadNotHaveHourWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveHour(10, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().HaveHour(10);
	}

	[Fact]
	public void GoodNotHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().Not.HaveHour(11);
	}
}
