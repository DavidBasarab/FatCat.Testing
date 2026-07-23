using FatCat.Testing.DateTimes;

namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeAtLeastAfterTests : BaseTest
{
	[Fact]
	public void BadBeAtLeastAfter()
	{
		var subject = new DateTime(2026, 1, 1, 11, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeAtLeast(2.Hours()).After(other));
	}

	[Fact]
	public void BadBeAtLeastAfterShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 11, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeAtLeast(2.Hours()).After(other),
			"2026-01-01 11:00:00 should be at least 02:00:00 after 2026-01-01 10:00:00 but the difference is 01:00:00"
		);
	}

	[Fact]
	public void BadBeAtLeastAfterWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 11, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeAtLeast(2.Hours()).After(other, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeAtLeastAfterWrongDirection()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 12, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeAtLeast(2.Hours()).After(other),
			"2026-01-01 10:00:00 should be at least 02:00:00 after 2026-01-01 12:00:00 but is not after it"
		);
	}

	[Fact]
	public void GoodBeAtLeastAfter()
	{
		var subject = new DateTime(2026, 1, 1, 13, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		subject.Should().BeAtLeast(2.Hours()).After(other);
	}
}
