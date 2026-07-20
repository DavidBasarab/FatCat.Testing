namespace Tests.FatCat.Testing.CustomAssertions;

public class WebResultBeNotFoundTests : BaseTest
{
	[Fact]
	public void BadBeNotFound()
	{
		RunCompareFailTest(() => new FakeWebResult(200).Should().BeNotFound());
	}

	[Fact]
	public void BadBeNotFoundShowsCorrectMessage()
	{
		RunCompareFailTest(() => new FakeWebResult(200).Should().BeNotFound(), "Expected Not Found but web result was 200");
	}

	[Fact]
	public void BadBeNotFoundWithBecause()
	{
		RunCompareFailTest(() => new FakeWebResult(200).Should().BeNotFound("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNotFound()
	{
		new FakeWebResult(404).Should().BeNotFound();
	}
}
