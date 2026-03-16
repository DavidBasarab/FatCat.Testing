namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeOnOrBeforeTests : BaseTest
{
	[Fact]
	public void BadBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(() => date.Should().BeOnOrBefore(earlierDate));
	}

	[Fact]
	public void BadBeOnOrBeforeShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
							() => date.Should().BeOnOrBefore(earlierDate),
							"2024-06-15 10:30:45 should be on or before 2024-06-14 00:00:00"
						);
	}

	[Fact]
	public void BadBeOnOrBeforeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(() => date.Should().BeOnOrBefore(earlierDate, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(() => date.Should().Not.BeOnOrBefore(laterDate));
	}

	[Fact]
	public void BadNotBeOnOrBeforeShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
							() => date.Should().Not.BeOnOrBefore(laterDate),
							"2024-06-15 10:30:45 should not be on or before 2024-06-16 00:00:00"
						);
	}

	[Fact]
	public void BadNotBeOnOrBeforeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(() => date.Should().Not.BeOnOrBefore(laterDate, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().BeOnOrBefore(date);
	}

	[Fact]
	public void GoodNotBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		date.Should().Not.BeOnOrBefore(earlierDate);
	}
}