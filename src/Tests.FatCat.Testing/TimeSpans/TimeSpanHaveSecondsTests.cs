namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanHaveSecondsTests : BaseTest
{
	[Fact]
	public void BadHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => span.Should().HaveSeconds(30));
	}

	[Fact]
	public void BadHaveSecondsShowsCorrectMessage()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => span.Should().HaveSeconds(30), "00:00:45 should have seconds 30");
	}

	[Fact]
	public void BadHaveSecondsWithBecause()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => span.Should().HaveSeconds(30, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => span.Should().Not.HaveSeconds(45));
	}

	[Fact]
	public void BadNotHaveSecondsShowsCorrectMessage()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => span.Should().Not.HaveSeconds(45), "00:00:45 should not have seconds 45");
	}

	[Fact]
	public void BadNotHaveSecondsWithBecause()
	{
		var span = TimeSpan.FromSeconds(45);

		RunCompareFailTest(() => span.Should().Not.HaveSeconds(45, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		span.Should().HaveSeconds(45);
	}

	[Fact]
	public void GoodNotHaveSeconds()
	{
		var span = TimeSpan.FromSeconds(45);

		span.Should().Not.HaveSeconds(30);
	}
}