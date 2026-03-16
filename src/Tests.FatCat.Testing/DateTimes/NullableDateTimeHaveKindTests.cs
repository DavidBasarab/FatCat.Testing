namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeHaveKindTests : BaseTest
{
	[Fact]
	public void BadHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
							() => ((DateTime?)utcDate).Should().HaveKind(DateTimeKind.Local),
							"2024-06-15 10:30:45 should have kind Local"
						);
	}

	[Fact]
	public void BadHaveKindNullValue()
	{
		RunCompareFailTest(
							() => ((DateTime?)null).Should().HaveKind(DateTimeKind.Local),
							"null should have kind Local"
						);
	}

	[Fact]
	public void BadHaveKindWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
							() => ((DateTime?)utcDate).Should().HaveKind(DateTimeKind.Local, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
							() => ((DateTime?)utcDate).Should().Not.HaveKind(DateTimeKind.Utc),
							"2024-06-15 10:30:45 should not have kind Utc"
						);
	}

	[Fact]
	public void BadNotHaveKindWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
							() => ((DateTime?)utcDate).Should().Not.HaveKind(DateTimeKind.Utc, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		((DateTime?)utcDate).Should().HaveKind(DateTimeKind.Utc);
	}

	[Fact]
	public void GoodNotHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		((DateTime?)utcDate).Should().Not.HaveKind(DateTimeKind.Local);
	}

	[Fact]
	public void GoodNotHaveKindWhenNull() { ((DateTime?)null).Should().Not.HaveKind(DateTimeKind.Utc); }
}