namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeCloseToTests : BaseTest
{
	[Fact]
	public void BadBeCloseTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() => ((TimeSpan?)span).Should().BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30)),
			"01:00:00 should be within 00:30:00 of 03:00:00"
		);
	}

	[Fact]
	public void BadBeCloseToNullValue()
	{
		RunCompareFailTest(
			() => ((TimeSpan?)null).Should().BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30)),
			"null should be within 00:30:00 of 03:00:00"
		);
	}

	[Fact]
	public void BadBeCloseToWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() =>
				((TimeSpan?)span)
					.Should()
					.BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBeCloseTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() =>
				((TimeSpan?)span)
					.Should()
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
				((TimeSpan?)span)
					.Should()
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

		((TimeSpan?)span)
			.Should()
			.BeCloseTo(TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(15)), TimeSpan.FromMinutes(30));
	}

	[Fact]
	public void GoodNotBeCloseTo()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().Not.BeCloseTo(TimeSpan.FromHours(3), TimeSpan.FromMinutes(30));
	}

	[Fact]
	public void GoodNotBeCloseToWhenNull()
	{
		((TimeSpan?)null).Should().Not.BeCloseTo(TimeSpan.FromHours(1), TimeSpan.FromMinutes(30));
	}
}
