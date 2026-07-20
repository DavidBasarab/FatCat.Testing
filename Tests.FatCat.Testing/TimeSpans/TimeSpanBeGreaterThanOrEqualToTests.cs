namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBeGreaterThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(2)));
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => span.Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(2)),
							"01:00:00 should be greater than or equal to 02:00:00"
						);
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => span.Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(2), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(() => span.Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(1)));
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => span.Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(1)),
							"02:00:00 should not be greater than or equal to 01:00:00"
						);
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => span.Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(1), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualToWhenEqual()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualToWhenGreater()
	{
		var span = TimeSpan.FromHours(2);

		span.Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(2));
	}
}