namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeBeOnOrAfterTests : BaseTest
{
	[Fact]
	public void BadBeOnOrAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().BeOnOrAfter(laterDate),
			"2024-06-15 10:30:45 should be on or after 2024-06-16 00:00:00"
		);
	}

	[Fact]
	public void BadBeOnOrAfterNullValue()
	{
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
			() => ((DateTime?)null).Should().BeOnOrAfter(laterDate),
			"null should be on or after 2024-06-16 00:00:00"
		);
	}

	[Fact]
	public void BadBeOnOrAfterWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().BeOnOrAfter(laterDate, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBeOnOrAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().Not.BeOnOrAfter(earlierDate),
			"2024-06-15 10:30:45 should not be on or after 2024-06-14 00:00:00"
		);
	}

	[Fact]
	public void BadNotBeOnOrAfterWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().Not.BeOnOrAfter(earlierDate, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBeOnOrAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().BeOnOrAfter(date);
	}

	[Fact]
	public void GoodNotBeOnOrAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		((DateTime?)date).Should().Not.BeOnOrAfter(laterDate);
	}

	[Fact]
	public void GoodNotBeOnOrAfterWhenNull()
	{
		var laterDate = new DateTime(2024, 6, 16);

		((DateTime?)null).Should().Not.BeOnOrAfter(laterDate);
	}
}
