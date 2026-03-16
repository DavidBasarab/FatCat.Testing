namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeHaveMillisecondTests : BaseTest
{
	[Fact]
	public void BadHaveMillisecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		RunCompareFailTest(() => date.Should().HaveMillisecond(600));
	}

	[Fact]
	public void BadHaveMillisecondShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		RunCompareFailTest(
							() => date.Should().HaveMillisecond(600),
							"2024-06-15 10:30:45 should have millisecond 600"
						);
	}

	[Fact]
	public void BadHaveMillisecondWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		RunCompareFailTest(() => date.Should().HaveMillisecond(600, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveMillisecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		RunCompareFailTest(() => date.Should().Not.HaveMillisecond(500));
	}

	[Fact]
	public void BadNotHaveMillisecondShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		RunCompareFailTest(
							() => date.Should().Not.HaveMillisecond(500),
							"2024-06-15 10:30:45 should not have millisecond 500"
						);
	}

	[Fact]
	public void BadNotHaveMillisecondWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		RunCompareFailTest(() => date.Should().Not.HaveMillisecond(500, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveMillisecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		date.Should().HaveMillisecond(500);
	}

	[Fact]
	public void GoodNotHaveMillisecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45, 500);

		date.Should().Not.HaveMillisecond(600);
	}
}