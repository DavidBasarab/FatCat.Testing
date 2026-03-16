namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeUtcTests : BaseTest
{
	[Fact]
	public void BadBeUtc()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => localDate.Should().BeUtc());
	}

	[Fact]
	public void BadBeUtcShowsCorrectMessage()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => localDate.Should().BeUtc(), "2024-06-15 10:30:45 should be UTC");
	}

	[Fact]
	public void BadBeUtcWithBecause()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => localDate.Should().BeUtc("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeUtc()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().Not.BeUtc());
	}

	[Fact]
	public void BadNotBeUtcShowsCorrectMessage()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().Not.BeUtc(), "2024-06-15 10:30:45 should not be UTC");
	}

	[Fact]
	public void BadNotBeUtcWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => utcDate.Should().Not.BeUtc("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeUtc()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		utcDate.Should().BeUtc();
	}

	[Fact]
	public void GoodNotBeUtc()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		localDate.Should().Not.BeUtc();
	}
}