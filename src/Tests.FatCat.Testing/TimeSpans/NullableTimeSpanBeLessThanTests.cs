namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeLessThanTests : BaseTest
{
	[Fact]
	public void BadBeLessThan()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().BeLessThan(TimeSpan.FromHours(1)),
							"02:00:00 should be less than 01:00:00"
						);
	}

	[Fact]
	public void BadBeLessThanNullValue()
	{
		RunCompareFailTest(
							() => ((TimeSpan?)null).Should().BeLessThan(TimeSpan.FromHours(1)),
							"null should be less than 01:00:00"
						);
	}

	[Fact]
	public void BadBeLessThanWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().BeLessThan(TimeSpan.FromHours(1), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeLessThan()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.BeLessThan(TimeSpan.FromHours(2)),
							"01:00:00 should not be less than 02:00:00"
						);
	}

	[Fact]
	public void BadNotBeLessThanWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.BeLessThan(TimeSpan.FromHours(2), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeLessThan()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().BeLessThan(TimeSpan.FromHours(2));
	}

	[Fact]
	public void GoodNotBeLessThan()
	{
		var span = TimeSpan.FromHours(2);

		((TimeSpan?)span).Should().Not.BeLessThan(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBeLessThanWhenNull() { ((TimeSpan?)null).Should().Not.BeLessThan(TimeSpan.FromHours(1)); }
}