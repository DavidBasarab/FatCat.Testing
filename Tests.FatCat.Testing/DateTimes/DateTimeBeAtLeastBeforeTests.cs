using FatCat.Testing.DateTimes;

namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeAtLeastBeforeTests : BaseTest
{
	[Fact]
	public void BadBeAtLeastBefore()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 11, 0, 0);

		RunCompareFailTest(() => subject.Should().BeAtLeast(2.Hours()).Before(other));
	}

	[Fact]
	public void BadBeAtLeastBeforeShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 11, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeAtLeast(2.Hours()).Before(other),
			"2026-01-01 10:00:00 should be at least 02:00:00 before 2026-01-01 11:00:00 but the difference is 01:00:00"
		);
	}

	[Fact]
	public void BadBeAtLeastBeforeWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 11, 0, 0);

		RunCompareFailTest(() => subject.Should().BeAtLeast(2.Hours()).Before(other, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeAtLeastBeforeWrongDirection()
	{
		var subject = new DateTime(2026, 1, 1, 12, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeAtLeast(2.Hours()).Before(other),
			"2026-01-01 12:00:00 should be at least 02:00:00 before 2026-01-01 10:00:00 but is not before it"
		);
	}

	[Fact]
	public void GoodBeAtLeastBefore()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 13, 0, 0);

		subject.Should().BeAtLeast(2.Hours()).Before(other);
	}
}
