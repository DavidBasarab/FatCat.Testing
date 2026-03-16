namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeHaveDayTests : BaseTest
{
	[Fact]
	public void BadHaveDay()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveDay(16));
	}

	[Fact]
	public void BadHaveDayShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveDay(16), "2024-06-15 10:30:45 should have day 16");
	}

	[Fact]
	public void BadHaveDayWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveDay(16, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveDay()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveDay(15));
	}

	[Fact]
	public void BadNotHaveDayShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveDay(15), "2024-06-15 10:30:45 should not have day 15");
	}

	[Fact]
	public void BadNotHaveDayWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveDay(15, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveDay()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().HaveDay(15);
	}

	[Fact]
	public void GoodNotHaveDay()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().Not.HaveDay(16);
	}
}