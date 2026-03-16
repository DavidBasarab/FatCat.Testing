namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeHaveMinuteTests : BaseTest
{
	[Fact]
	public void BadHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveMinute(31));
	}

	[Fact]
	public void BadHaveMinuteShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveMinute(31), "2024-06-15 10:30:45 should have minute 31");
	}

	[Fact]
	public void BadHaveMinuteWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveMinute(31, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveMinute(30));
	}

	[Fact]
	public void BadNotHaveMinuteShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => date.Should().Not.HaveMinute(30),
							"2024-06-15 10:30:45 should not have minute 30"
						);
	}

	[Fact]
	public void BadNotHaveMinuteWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveMinute(30, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().HaveMinute(30);
	}

	[Fact]
	public void GoodNotHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().Not.HaveMinute(31);
	}
}