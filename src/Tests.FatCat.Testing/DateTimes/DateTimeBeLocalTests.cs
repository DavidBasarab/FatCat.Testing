namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeLocalTests : BaseTest
{
	[Fact]
	public void BadBeLocal()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().BeLocal());
	}

	[Fact]
	public void BadBeLocalShowsCorrectMessage()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().BeLocal(), "2024-06-15 10:30:45 should be local");
	}

	[Fact]
	public void BadBeLocalWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().BeLocal("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLocal()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => localDate.Should().Not.BeLocal());
	}

	[Fact]
	public void BadNotBeLocalShowsCorrectMessage()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => localDate.Should().Not.BeLocal(), "2024-06-15 10:30:45 should not be local");
	}

	[Fact]
	public void BadNotBeLocalWithBecause()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => localDate.Should().Not.BeLocal("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLocal()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		localDate.Should().BeLocal();
	}

	[Fact]
	public void GoodNotBeLocal()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		utcDate.Should().Not.BeLocal();
	}
}