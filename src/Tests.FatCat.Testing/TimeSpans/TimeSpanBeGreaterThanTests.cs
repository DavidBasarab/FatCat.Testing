namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBeGreaterThanTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThan()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().BeGreaterThan(TimeSpan.FromHours(2)));
	}

	[Fact]
	public void BadBeGreaterThanShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() => span.Should().BeGreaterThan(TimeSpan.FromHours(2)),
			"01:00:00 should be greater than 02:00:00"
		);
	}

	[Fact]
	public void BadBeGreaterThanWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() => span.Should().BeGreaterThan(TimeSpan.FromHours(2), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBeGreaterThan()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(() => span.Should().Not.BeGreaterThan(TimeSpan.FromHours(1)));
	}

	[Fact]
	public void BadNotBeGreaterThanShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
			() => span.Should().Not.BeGreaterThan(TimeSpan.FromHours(1)),
			"02:00:00 should not be greater than 01:00:00"
		);
	}

	[Fact]
	public void BadNotBeGreaterThanWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
			() => span.Should().Not.BeGreaterThan(TimeSpan.FromHours(1), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBeGreaterThan()
	{
		var span = TimeSpan.FromHours(2);

		span.Should().BeGreaterThan(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBeGreaterThan()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().Not.BeGreaterThan(TimeSpan.FromHours(2));
	}
}
