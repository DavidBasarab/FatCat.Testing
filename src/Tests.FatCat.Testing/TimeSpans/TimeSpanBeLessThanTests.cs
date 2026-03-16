namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBeLessThanTests : BaseTest
{
	[Fact]
	public void BadBeLessThan()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(() => span.Should().BeLessThan(TimeSpan.FromHours(1)));
	}

	[Fact]
	public void BadBeLessThanShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
			() => span.Should().BeLessThan(TimeSpan.FromHours(1)),
			"02:00:00 should be less than 01:00:00"
		);
	}

	[Fact]
	public void BadBeLessThanWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
			() => span.Should().BeLessThan(TimeSpan.FromHours(1), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBeLessThan()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.BeLessThan(TimeSpan.FromHours(2)));
	}

	[Fact]
	public void BadNotBeLessThanShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() => span.Should().Not.BeLessThan(TimeSpan.FromHours(2)),
			"01:00:00 should not be less than 02:00:00"
		);
	}

	[Fact]
	public void BadNotBeLessThanWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() => span.Should().Not.BeLessThan(TimeSpan.FromHours(2), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBeLessThan()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().BeLessThan(TimeSpan.FromHours(2));
	}

	[Fact]
	public void GoodNotBeLessThan()
	{
		var span = TimeSpan.FromHours(2);

		span.Should().Not.BeLessThan(TimeSpan.FromHours(1));
	}
}
