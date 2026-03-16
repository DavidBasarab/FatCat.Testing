namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBeLessThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeLessThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(() => span.Should().BeLessThanOrEqualTo(TimeSpan.FromHours(1)));
	}

	[Fact]
	public void BadBeLessThanOrEqualToShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => span.Should().BeLessThanOrEqualTo(TimeSpan.FromHours(1)),
							"02:00:00 should be less than or equal to 01:00:00"
						);
	}

	[Fact]
	public void BadBeLessThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => span.Should().BeLessThanOrEqualTo(TimeSpan.FromHours(1), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeLessThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(2)));
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => span.Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(2)),
							"01:00:00 should not be less than or equal to 02:00:00"
						);
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => span.Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(2), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeLessThanOrEqualToWhenEqual()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().BeLessThanOrEqualTo(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodBeLessThanOrEqualToWhenLess()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().BeLessThanOrEqualTo(TimeSpan.FromHours(2));
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(2);

		span.Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(1));
	}
}