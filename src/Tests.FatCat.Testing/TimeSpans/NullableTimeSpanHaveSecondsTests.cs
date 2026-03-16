namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanHaveSecondsTests : BaseTest
{
	[Fact]
	public void BadHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveSeconds(30), "00:00:45 should have seconds 30");
	}

	[Fact]
	public void BadHaveSecondsNullValue()
	{
		RunCompareFailTest(() => ((TimeSpan?)null).Should().HaveSeconds(30), "null should have seconds 30");
	}

	[Fact]
	public void BadHaveSecondsWithBecause()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().HaveSeconds(30, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(
			() => ((TimeSpan?)span).Should().Not.HaveSeconds(45),
			"00:00:45 should not have seconds 45"
		);
	}

	[Fact]
	public void BadNotHaveSecondsWithBecause()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(
			() => ((TimeSpan?)span).Should().Not.HaveSeconds(45, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		((TimeSpan?)span).Should().HaveSeconds(45);
	}

	[Fact]
	public void GoodNotHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		((TimeSpan?)span).Should().Not.HaveSeconds(30);
	}

	[Fact]
	public void GoodNotHaveSecondsWhenNull()
	{
		((TimeSpan?)null).Should().Not.HaveSeconds(45);
	}
}
