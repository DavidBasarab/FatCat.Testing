namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeAfterTests : BaseTest
{
	[Fact]
	public void BadBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(() => date.Should().BeAfter(laterDate));
	}

	[Fact]
	public void BadBeAfterShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
			() => date.Should().BeAfter(laterDate),
			"2024-06-15 10:30:45 should be after 2024-06-16 00:00:00"
		);
	}

	[Fact]
	public void BadBeAfterWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(() => date.Should().BeAfter(laterDate, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(() => date.Should().Not.BeAfter(earlierDate));
	}

	[Fact]
	public void BadNotBeAfterShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
			() => date.Should().Not.BeAfter(earlierDate),
			"2024-06-15 10:30:45 should not be after 2024-06-14 00:00:00"
		);
	}

	[Fact]
	public void BadNotBeAfterWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(() => date.Should().Not.BeAfter(earlierDate, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		date.Should().BeAfter(earlierDate);
	}

	[Fact]
	public void GoodNotBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		date.Should().Not.BeAfter(laterDate);
	}
}
