namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeSameDateAsTests : BaseTest
{
	[Fact]
	public void BadBeSameDateAs()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 2, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeSameDateAs(other));
	}

	[Fact]
	public void BadBeSameDateAsShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 2, 10, 0, 0);

		RunCompareFailTest(
			() => subject.Should().BeSameDateAs(other),
			"2026-01-01 10:00:00 should be on the same date as 2026-01-02"
		);
	}

	[Fact]
	public void BadBeSameDateAsWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 2, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeSameDateAs(other, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeSameDateAs()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 22, 45, 0);

		RunCompareFailTest(() => subject.Should().Not.BeSameDateAs(other));
	}

	[Fact]
	public void BadNotBeSameDateAsShowsCorrectMessage()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 22, 45, 0);

		RunCompareFailTest(
			() => subject.Should().Not.BeSameDateAs(other),
			"2026-01-01 10:00:00 should not be on the same date as 2026-01-01"
		);
	}

	[Fact]
	public void BadNotBeSameDateAsWithBecause()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 22, 45, 0);

		RunCompareFailTest(() => subject.Should().Not.BeSameDateAs(other, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeSameDateAs()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 1, 22, 45, 0);

		subject.Should().BeSameDateAs(other);
	}

	[Fact]
	public void GoodNotBeSameDateAs()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var other = new DateTime(2026, 1, 2, 10, 0, 0);

		subject.Should().Not.BeSameDateAs(other);
	}
}
