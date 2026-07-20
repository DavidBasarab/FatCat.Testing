namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Be(TimeSpan.FromHours(2)));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Be(TimeSpan.FromHours(2)), "01:00:00 should be 02:00:00");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Be(TimeSpan.FromHours(2), "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.Be(TimeSpan.FromHours(1)));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.Be(TimeSpan.FromHours(1)), "01:00:00 should not be 01:00:00");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => span.Should().Not.Be(TimeSpan.FromHours(1), "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().Be(TimeSpan.FromHours(1));
	}

	[Fact]
	public void GoodNotBe()
	{
		var span = TimeSpan.FromHours(1);

		span.Should().Not.Be(TimeSpan.FromHours(2));
	}
}
