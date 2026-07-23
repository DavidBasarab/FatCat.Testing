namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeBeCloseToTests : BaseTest
{
	[Fact]
	public void BadBeCloseTo()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var farDate = new DateTime(2024, 6, 15, 10, 30, 42);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().BeCloseTo(farDate, TimeSpan.FromSeconds(2)),
			"2024-06-15 10:30:45 should be within 00:00:02 of 2024-06-15 10:30:42"
		);
	}

	[Fact]
	public void BadBeCloseToNullValue()
	{
		var farDate = new DateTime(2024, 6, 15, 10, 30, 42);

		RunCompareFailTest(
			() => ((DateTime?)null).Should().BeCloseTo(farDate, TimeSpan.FromSeconds(2)),
			"null should be within 00:00:02 of 2024-06-15 10:30:42"
		);
	}

	[Fact]
	public void BadBeCloseToWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var farDate = new DateTime(2024, 6, 15, 10, 30, 42);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().BeCloseTo(farDate, TimeSpan.FromSeconds(2), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBeCloseTo()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var closeDate = new DateTime(2024, 6, 15, 10, 30, 44);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().Not.BeCloseTo(closeDate, TimeSpan.FromSeconds(2)),
			"2024-06-15 10:30:45 should not be within 00:00:02 of 2024-06-15 10:30:44"
		);
	}

	[Fact]
	public void BadNotBeCloseToWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var closeDate = new DateTime(2024, 6, 15, 10, 30, 44);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().Not.BeCloseTo(closeDate, TimeSpan.FromSeconds(2), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBeCloseTo()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var closeDate = new DateTime(2024, 6, 15, 10, 30, 44);

		((DateTime?)date).Should().BeCloseTo(closeDate, TimeSpan.FromSeconds(2));
	}

	[Fact]
	public void GoodNotBeCloseTo()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var farDate = new DateTime(2024, 6, 15, 10, 30, 42);

		((DateTime?)date).Should().Not.BeCloseTo(farDate, TimeSpan.FromSeconds(2));
	}

	[Fact]
	public void GoodNotBeCloseToWhenNull()
	{
		var farDate = new DateTime(2024, 6, 15, 10, 30, 42);

		((DateTime?)null).Should().Not.BeCloseTo(farDate, TimeSpan.FromSeconds(2));
	}
}
