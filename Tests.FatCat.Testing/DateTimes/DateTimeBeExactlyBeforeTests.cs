using FatCat.Testing.DateTimes;

namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeExactlyBeforeTests : BaseTest
{
	[Fact]
	public void BadBeExactlyBefore()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 13, 0, 0);

		RunCompareFailTest(() => subject.Should().BeExactly(2.Hours()).Before(other));
	}

	[Fact]
	public void BadBeExactlyBeforeShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 13, 0, 0);

		RunCompareFailTest(
							() => subject.Should().BeExactly(2.Hours()).Before(other),
							"2026-01-01 10:00:00 should be exactly 02:00:00 before 2026-01-01 13:00:00 but the difference is 03:00:00"
						);
	}

	[Fact]
	public void BadBeExactlyBeforeWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 13, 0, 0);

		RunCompareFailTest(() => subject.Should().BeExactly(2.Hours()).Before(other, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeExactlyBeforeWrongDirection()
	{
		var subject = new DateTime(2026, 1, 1, 12, 0, 0);
		var other = new DateTime(2026, 1, 1, 10, 0, 0);

		RunCompareFailTest(
							() => subject.Should().BeExactly(2.Hours()).Before(other),
							"2026-01-01 12:00:00 should be exactly 02:00:00 before 2026-01-01 10:00:00 but is not before it"
						);
	}

	[Fact]
	public void GoodBeExactlyBefore()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 12, 0, 0);

		subject.Should().BeExactly(2.Hours()).Before(other);
	}
}