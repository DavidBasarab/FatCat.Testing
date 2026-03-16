namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeGreaterThanTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThan()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().BeGreaterThan(TimeSpan.FromHours(2)),
							"01:00:00 should be greater than 02:00:00"
						);
	}

	[Fact]
	public void BadBeGreaterThanNullValue()
	{
		RunCompareFailTest(
							() => ((TimeSpan?)null).Should().BeGreaterThan(TimeSpan.FromHours(2)),
							"null should be greater than 02:00:00"
						);
	}

	[Fact]
	public void BadBeGreaterThanWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().BeGreaterThan(TimeSpan.FromHours(2), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeGreaterThan()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.BeGreaterThan(TimeSpan.FromHours(1)),
							"02:00:00 should not be greater than 01:00:00"
						);
	}

	[Fact]
	public void BadNotBeGreaterThanWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.BeGreaterThan(TimeSpan.FromHours(1), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeGreaterThan()
	{
		var span = TimeSpan.FromHours(2);

		((TimeSpan?)span).Should().BeGreaterThan(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBeGreaterThan()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().Not.BeGreaterThan(TimeSpan.FromHours(2));
	}

	[Fact]
	public void GoodNotBeGreaterThanWhenNull() { ((TimeSpan?)null).Should().Not.BeGreaterThan(TimeSpan.FromHours(1)); }
}