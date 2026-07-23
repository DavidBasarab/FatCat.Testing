namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeInTests : BaseTest
{
	[Fact]
	public void BadBeIn()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		RunCompareFailTest(() => subject.Should().BeIn(DateTimeKind.Local));
	}

	[Fact]
	public void BadBeInShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		RunCompareFailTest(() => subject.Should().BeIn(DateTimeKind.Local), "2026-01-01 10:00:00 should be in Local");
	}

	[Fact]
	public void BadBeInWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		RunCompareFailTest(() => subject.Should().BeIn(DateTimeKind.Local, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeIn()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		RunCompareFailTest(() => subject.Should().Not.BeIn(DateTimeKind.Utc));
	}

	[Fact]
	public void BadNotBeInShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		RunCompareFailTest(() => subject.Should().Not.BeIn(DateTimeKind.Utc), "2026-01-01 10:00:00 should not be in Utc");
	}

	[Fact]
	public void BadNotBeInWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		RunCompareFailTest(() => subject.Should().Not.BeIn(DateTimeKind.Utc, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeIn()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		subject.Should().BeIn(DateTimeKind.Utc);
	}

	[Fact]
	public void GoodNotBeIn()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

		subject.Should().Not.BeIn(DateTimeKind.Local);
	}
}