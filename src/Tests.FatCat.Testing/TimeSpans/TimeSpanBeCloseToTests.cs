namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBeCloseToTests : BaseTest
{
	[Fact]
	public void BadBeCloseTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30)));
	}

	[Fact]
	public void BadBeCloseToShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => span.Should().BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30)),
							"01:00:00 should be within 00:30:00 of 03:00:00"
						);
	}

	[Fact]
	public void BadBeCloseToWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => span.Should().BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeCloseTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() =>
								span.Should()
									.Not.BeCloseTo(TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(15)), TimeSpan.FromMinutes(30))
						);
	}

	[Fact]
	public void BadNotBeCloseToShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() =>
								span.Should()
									.Not.BeCloseTo(TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(15)), TimeSpan.FromMinutes(30)),
							"01:00:00 should not be within 00:30:00 of 01:15:00"
						);
	}

	[Fact]
	public void BadNotBeCloseToWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() =>
								span.Should()
									.Not.BeCloseTo(
													TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(15)),
													TimeSpan.FromMinutes(30),
													"custom because"
												),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeCloseTo()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().BeCloseTo(TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(15)), TimeSpan.FromMinutes(30));
	}

	[Fact]
	public void GoodNotBeCloseTo()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().Not.BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30));
	}
}