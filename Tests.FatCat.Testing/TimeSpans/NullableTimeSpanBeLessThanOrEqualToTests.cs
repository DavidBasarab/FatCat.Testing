namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeLessThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeLessThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().BeLessThanOrEqualTo(TimeSpan.FromHours(1)),
							"02:00:00 should be less than or equal to 01:00:00"
						);
	}

	[Fact]
	public void BadBeLessThanOrEqualToNullValue()
	{
		RunCompareFailTest(
							() => ((TimeSpan?)null).Should().BeLessThanOrEqualTo(TimeSpan.FromHours(1)),
							"null should be less than or equal to 01:00:00"
						);
	}

	[Fact]
	public void BadBeLessThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(2);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().BeLessThanOrEqualTo(TimeSpan.FromHours(1), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeLessThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(2)),
							"01:00:00 should not be less than or equal to 02:00:00"
						);
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(2), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeLessThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().BeLessThanOrEqualTo(TimeSpan.FromHours(2));
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualTo()
	{
		var span = TimeSpan.FromHours(2);

		((TimeSpan?)span).Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualToWhenNull() { ((TimeSpan?)null).Should().Not.BeLessThanOrEqualTo(TimeSpan.FromHours(1)); }
}