namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeGreaterThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() => ((TimeSpan?)span).Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(2)),
			"01:00:00 should be greater than or equal to 02:00:00"
		);
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToNullValue()
	{
		RunCompareFailTest(
			() => ((TimeSpan?)null).Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(2)),
			"null should be greater than or equal to 02:00:00"
		);
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
			() => ((TimeSpan?)span).Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(2), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
			() => ((TimeSpan?)span).Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(1)),
			"02:00:00 should not be greater than or equal to 01:00:00"
		);
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
			() => ((TimeSpan?)span).Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(1), "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(2);

		((TimeSpan?)span).Should().BeGreaterThanOrEqualTo(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(2));
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualToWhenNull()
	{
		((TimeSpan?)null).Should().Not.BeGreaterThanOrEqualTo(TimeSpan.FromHours(1));
	}
}
