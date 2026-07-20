namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanHaveMinutesTests : BaseTest
{
	[Fact]
	public void BadHaveMinutes()
	{
		var span = TimeSpan.FromMinutes(30);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveMinutes(45), "00:30:00 should have minutes 45");
	}

	[Fact]
	public void BadHaveMinutesNullValue() { RunCompareFailTest(() => ((TimeSpan?)null).Should().HaveMinutes(45), "null should have minutes 45"); }

	[Fact]
	public void BadHaveMinutesWithBecause()
	{
		var span = TimeSpan.FromMinutes(30);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveMinutes(45, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveMinutes()
	{
		var span = TimeSpan.FromMinutes(30);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.HaveMinutes(30),
							"00:30:00 should not have minutes 30"
						);
	}

	[Fact]
	public void BadNotHaveMinutesWithBecause()
	{
		var span = TimeSpan.FromMinutes(30);

		RunCompareFailTest(
							() => ((TimeSpan?)span).Should().Not.HaveMinutes(30, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodHaveMinutes()
	{
		var span = TimeSpan.FromMinutes(30);

		((TimeSpan?)span).Should().HaveMinutes(30);
	}

	[Fact]
	public void GoodNotHaveMinutes()
	{
		var span = TimeSpan.FromMinutes(30);

		((TimeSpan?)span).Should().Not.HaveMinutes(45);
	}

	[Fact]
	public void GoodNotHaveMinutesWhenNull() { ((TimeSpan?)null).Should().Not.HaveMinutes(30); }
}