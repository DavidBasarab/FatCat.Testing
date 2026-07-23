using FatCat.Testing.DateTimes;

namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeWithinAfterTests : BaseTest
{
	[Fact]
	public void BadBeWithinAfter()
	{
		var subject = new DateTime(2026, 1, 1, 13, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeWithin(2.Hours()).After(other));
	}

	[Fact]
	public void BadBeWithinAfterShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 13, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeWithin(2.Hours()).After(other),
			"2026-01-01 13:00:00 should be within 02:00:00 after 2026-01-01 10:00:00 but the difference is 03:00:00"
		);
	}

	[Fact]
	public void BadBeWithinAfterWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 13, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeWithin(2.Hours()).After(other, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeWithinAfterWrongDirection()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 12, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeWithin(2.Hours()).After(other),
			"2026-01-01 10:00:00 should be within 02:00:00 after 2026-01-01 12:00:00 but is not after it"
		);
	}

	[Fact]
	public void GoodBeWithinAfter()
	{
		var subject = new DateTime(2026, 1, 1, 11, 30, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		subject.Should().BeWithin(2.Hours()).After(other);
	}
}
