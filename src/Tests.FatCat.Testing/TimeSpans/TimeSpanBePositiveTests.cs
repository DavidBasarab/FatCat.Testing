namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBePositiveTests : BaseTest
{
	[Fact]
	public void BadBePositive()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => span.Should().BePositive());
	}

	[Fact]
	public void BadBePositiveShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => span.Should().BePositive(), "-01:00:00 should be positive");
	}

	[Fact]
	public void BadBePositiveWithBecause()
	{
		var span = TimeSpan.FromHours(-1);

		RunCompareFailTest(() => span.Should().BePositive("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBePositive()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.BePositive());
	}

	[Fact]
	public void BadNotBePositiveShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.BePositive(), "01:00:00 should not be positive");
	}

	[Fact]
	public void BadNotBePositiveWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.BePositive("custom because"), "custom because");
	}

	[Fact]
	public void GoodBePositive()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().BePositive();
	}

	[Fact]
	public void GoodNotBePositive()
	{
		var span = TimeSpan.FromHours(-1);

		span.Should().Not.BePositive();
	}
}
