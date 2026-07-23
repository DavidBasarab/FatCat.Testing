using FatCat.Testing.DateTimes;

namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeLessThanBeforeTests : BaseTest
{
	[Fact]
	public void BadBeLessThanBefore()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 13, 0, 0);

		RunCompareFailTest(() => subject.Should().BeLessThan(2.Hours()).Before(other));
	}

	[Fact]
	public void BadBeLessThanBeforeShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 13, 0, 0);

		RunCompareFailTest(
							() => subject.Should().BeLessThan(2.Hours()).Before(other),
							"2026-01-01 10:00:00 should be less than 02:00:00 before 2026-01-01 13:00:00 but the difference is 03:00:00"
						);
	}

	[Fact]
	public void BadBeLessThanBeforeWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 13, 0, 0);

		RunCompareFailTest(() => subject.Should().BeLessThan(2.Hours()).Before(other, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeLessThanBeforeWrongDirection()
	{
		var subject = new DateTime(2026, 1, 1, 12, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(
							() => subject.Should().BeLessThan(2.Hours()).Before(other),
							"2026-01-01 12:00:00 should be less than 02:00:00 before 2026-01-01 10:00:00 but is not before it"
						);
	}

	[Fact]
	public void GoodBeLessThanBefore()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 11, 30, 0);

		subject.Should().BeLessThan(2.Hours()).Before(other);
	}
}