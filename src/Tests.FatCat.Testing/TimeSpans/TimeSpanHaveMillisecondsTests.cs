namespace Tests.FatCat.Testing.TimeSpans;

public class TimeSpanHaveMillisecondsTests : BaseTest
{
	[Fact]
	public void BadHaveMilliseconds()
	{
		var span = TimeSpan.FromMilliseconds(500);

		RunCompareFailTest(() => span.Should().HaveMilliseconds(600));
	}

	[Fact]
	public void BadHaveMillisecondsShowsCorrectMessage()
	{
		var span = TimeSpan.FromMilliseconds(500);

		RunCompareFailTest(
							() => span.Should().HaveMilliseconds(600),
							"00:00:00.5000000 should have milliseconds 600"
						);
	}

	[Fact]
	public void BadHaveMillisecondsWithBecause()
	{
		var span = TimeSpan.FromMilliseconds(500);

		RunCompareFailTest(() => span.Should().HaveMilliseconds(600, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveMilliseconds()
	{
		var span = TimeSpan.FromMilliseconds(500);

		RunCompareFailTest(() => span.Should().Not.HaveMilliseconds(500));
	}

	[Fact]
	public void BadNotHaveMillisecondsShowsCorrectMessage()
	{
		var span = TimeSpan.FromMilliseconds(500);

		RunCompareFailTest(
							() => span.Should().Not.HaveMilliseconds(500),
							"00:00:00.5000000 should not have milliseconds 500"
						);
	}

	[Fact]
	public void BadNotHaveMillisecondsWithBecause()
	{
		var span = TimeSpan.FromMilliseconds(500);

		RunCompareFailTest(() => span.Should().Not.HaveMilliseconds(500, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveMilliseconds()
	{
		var span = TimeSpan.FromMilliseconds(500);

		span.Should().HaveMilliseconds(500);
	}

	[Fact]
	public void GoodNotHaveMilliseconds()
	{
		var span = TimeSpan.FromMilliseconds(500);

		span.Should().Not.HaveMilliseconds(600);
	}
}