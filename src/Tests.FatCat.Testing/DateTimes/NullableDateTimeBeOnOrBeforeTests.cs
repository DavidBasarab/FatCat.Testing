namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeBeOnOrBeforeTests : BaseTest
{
	[Fact]
	public void BadBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().BeOnOrBefore(earlierDate),
							"2024-06-15 10:30:45 should be on or before 2024-06-14 00:00:00"
						);
	}

	[Fact]
	public void BadBeOnOrBeforeNullValue()
	{
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
							() => ((DateTime?)null).Should().BeOnOrBefore(earlierDate),
							"null should be on or before 2024-06-14 00:00:00"
						);
	}

	[Fact]
	public void BadBeOnOrBeforeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().BeOnOrBefore(earlierDate, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().Not.BeOnOrBefore(laterDate),
							"2024-06-15 10:30:45 should not be on or before 2024-06-16 00:00:00"
						);
	}

	[Fact]
	public void BadNotBeOnOrBeforeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().Not.BeOnOrBefore(laterDate, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().BeOnOrBefore(date);
	}

	[Fact]
	public void GoodNotBeOnOrBefore()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		((DateTime?)date).Should().Not.BeOnOrBefore(earlierDate);
	}

	[Fact]
	public void GoodNotBeOnOrBeforeWhenNull()
	{
		var earlierDate = new DateTime(2024, 6, 14);

		((DateTime?)null).Should().Not.BeOnOrBefore(earlierDate);
	}
}