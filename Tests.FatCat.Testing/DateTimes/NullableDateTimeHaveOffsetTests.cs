namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeHaveOffsetTests : BaseTest
{
	[Fact]
	public void BadHaveOffset()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => ((DateTime?)utcDate).Should().HaveOffset(TimeSpan.FromHours(5)),
			"2024-06-15 10:30:45 should have offset 05:00:00"
		);
	}

	[Fact]
	public void BadHaveOffsetNullValue()
	{
		RunCompareFailTest(
			() => ((DateTime?)null).Should().HaveOffset(TimeSpan.FromHours(5)),
			"null should have offset 05:00:00"
		);
	}

	[Fact]
	public void BadHaveOffsetWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => ((DateTime?)utcDate).Should().HaveOffset(TimeSpan.FromHours(5), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotHaveOffset()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => ((DateTime?)utcDate).Should().Not.HaveOffset(TimeSpan.Zero),
			"2024-06-15 10:30:45 should not have offset 00:00:00"
		);
	}

	[Fact]
	public void BadNotHaveOffsetWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => ((DateTime?)utcDate).Should().Not.HaveOffset(TimeSpan.Zero, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodHaveOffset()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		((DateTime?)utcDate).Should().HaveOffset(TimeSpan.Zero);
	}

	[Fact]
	public void GoodNotHaveOffset()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		((DateTime?)utcDate).Should().Not.HaveOffset(TimeSpan.FromHours(5));
	}

	[Fact]
	public void GoodNotHaveOffsetWhenNull()
	{
		((DateTime?)null).Should().Not.HaveOffset(TimeSpan.Zero);
	}
}
