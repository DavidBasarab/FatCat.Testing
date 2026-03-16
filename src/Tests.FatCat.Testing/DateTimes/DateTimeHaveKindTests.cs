namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeHaveKindTests : BaseTest
{
	[Fact]
	public void BadHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().HaveKind(DateTimeKind.Local));
	}

	[Fact]
	public void BadHaveKindShowsCorrectMessage()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => utcDate.Should().HaveKind(DateTimeKind.Local),
			"2024-06-15 10:30:45 should have kind Local"
		);
	}

	[Fact]
	public void BadHaveKindWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => utcDate.Should().HaveKind(DateTimeKind.Local, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().Not.HaveKind(DateTimeKind.Utc));
	}

	[Fact]
	public void BadNotHaveKindShowsCorrectMessage()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => utcDate.Should().Not.HaveKind(DateTimeKind.Utc),
			"2024-06-15 10:30:45 should not have kind Utc"
		);
	}

	[Fact]
	public void BadNotHaveKindWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
			() => utcDate.Should().Not.HaveKind(DateTimeKind.Utc, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		utcDate.Should().HaveKind(DateTimeKind.Utc);
	}

	[Fact]
	public void GoodNotHaveKind()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		utcDate.Should().Not.HaveKind(DateTimeKind.Local);
	}
}
