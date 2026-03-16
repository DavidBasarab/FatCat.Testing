namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBeNegativeTests : BaseTest
{
	[Fact]
	public void BadBeNegative()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().BeNegative());
	}

	[Fact]
	public void BadBeNegativeShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().BeNegative(), "01:00:00 should be negative");
	}

	[Fact]
	public void BadBeNegativeWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNegative()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => span.Should().Not.BeNegative());
	}

	[Fact]
	public void BadNotBeNegativeShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => span.Should().Not.BeNegative(), "-01:00:00 should not be negative");
	}

	[Fact]
	public void BadNotBeNegativeWithBecause()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => span.Should().Not.BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNegative()
	{
		var span = TimeSpan.FromHours(-1);

		span.Should().BeNegative();
	}

	[Fact]
	public void GoodNotBeNegative()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().Not.BeNegative();
	}
}
