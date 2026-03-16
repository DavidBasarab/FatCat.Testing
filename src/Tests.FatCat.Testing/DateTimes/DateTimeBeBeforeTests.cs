namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeBeforeTests : BaseTest
{
	[Fact]
	public void BadBeBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(() => date.Should().BeBefore(earlierDate));
	}

	[Fact]
	public void BadBeBeforeShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
			() => date.Should().BeBefore(earlierDate),
			"2024-06-15 10:30:45 should be before 2024-06-14 00:00:00"
		);
	}

	[Fact]
	public void BadBeBeforeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(() => date.Should().BeBefore(earlierDate, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(() => date.Should().Not.BeBefore(laterDate));
	}

	[Fact]
	public void BadNotBeBeforeShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
			() => date.Should().Not.BeBefore(laterDate),
			"2024-06-15 10:30:45 should not be before 2024-06-16 00:00:00"
		);
	}

	[Fact]
	public void BadNotBeBeforeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(() => date.Should().Not.BeBefore(laterDate, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		date.Should().BeBefore(laterDate);
	}

	[Fact]
	public void GoodNotBeBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		date.Should().Not.BeBefore(earlierDate);
	}
}
