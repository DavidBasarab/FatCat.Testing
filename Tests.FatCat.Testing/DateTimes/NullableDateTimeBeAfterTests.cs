namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeBeAfterTests : BaseTest
{
	[Fact]
	public void BadBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().BeAfter(laterDate),
							"2024-06-15 10:30:45 should be after 2024-06-16 00:00:00"
						);
	}

	[Fact]
	public void BadBeAfterNullValue()
	{
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
							() => ((DateTime?)null).Should().BeAfter(laterDate),
							"null should be after 2024-06-16 00:00:00"
						);
	}

	[Fact]
	public void BadBeAfterWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().BeAfter(laterDate, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().Not.BeAfter(earlierDate),
							"2024-06-15 10:30:45 should not be after 2024-06-14 00:00:00"
						);
	}

	[Fact]
	public void BadNotBeAfterWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().Not.BeAfter(earlierDate, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var earlierDate = new DateTime(2024, 6, 14);

		((DateTime?)date).Should().BeAfter(earlierDate);
	}

	[Fact]
	public void GoodNotBeAfter()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var laterDate = new DateTime(2024, 6, 16);

		((DateTime?)date).Should().Not.BeAfter(laterDate);
	}

	[Fact]
	public void GoodNotBeAfterWhenNull()
	{
		var laterDate = new DateTime(2024, 6, 16);

		((DateTime?)null).Should().Not.BeAfter(laterDate);
	}
}