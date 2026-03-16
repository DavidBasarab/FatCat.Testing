namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Be(TimeSpan.FromHours(2)),
							"01:00:00 should be 02:00:00"
						);
	}

	[Fact]
	public void BadBeNullValue() { RunCompareFailTest(() => ((TimeSpan?)null).Should().Be(TimeSpan.FromHours(2)), "null should be 02:00:00"); }

	[Fact]
	public void BadBeWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Be(TimeSpan.FromHours(2), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBe()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.Be(TimeSpan.FromHours(1)),
							"01:00:00 should not be 01:00:00"
						);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.Be(TimeSpan.FromHours(1), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBe()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().Be(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBe()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().Not.Be(TimeSpan.FromHours(2));
	}

	[Fact]
	public void GoodNotBeWhenNull() { ((TimeSpan?)null).Should().Not.Be(TimeSpan.FromHours(1)); }
}