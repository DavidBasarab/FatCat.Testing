using FatCat.Testing.DateTimes;

namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeMoreThanAfterTests : BaseTest
{
	[Fact]
	public void BadBeMoreThanAfter()
	{
		var subject = new DateTime(2026, 1, 1, 11, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeMoreThan(2.Hours()).After(other));
	}

	[Fact]
	public void BadBeMoreThanAfterShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 11, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeMoreThan(2.Hours()).After(other),
			"2026-01-01 11:00:00 should be more than 02:00:00 after 2026-01-01 10:00:00 but the difference is 01:00:00"
		);
	}

	[Fact]
	public void BadBeMoreThanAfterWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 11, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeMoreThan(2.Hours()).After(other, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeMoreThanAfterWrongDirection()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 12, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeMoreThan(2.Hours()).After(other),
			"2026-01-01 10:00:00 should be more than 02:00:00 after 2026-01-01 12:00:00 but is not after it"
		);
	}

	[Fact]
	public void GoodBeMoreThanAfter()
	{
		var subject = new DateTime(2026, 1, 1, 13, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		subject.Should().BeMoreThan(2.Hours()).After(other);
	}
}
