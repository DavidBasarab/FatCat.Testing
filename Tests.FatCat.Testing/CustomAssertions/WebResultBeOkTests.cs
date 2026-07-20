namespace Tests.FatCat.Testing.CustomAssertions;

public class WebResultBeOkTests : BaseTest
{
	[Fact]
	public void BadBeOk()
	{
		RunCompareFailTest(() => new FakeWebResult(404).Should().BeOk());
	}

	[Fact]
	public void BadBeOkShowsCorrectMessage()
	{
		RunCompareFailTest(() => new FakeWebResult(404).Should().BeOk(), "Expected OK but web result was 404");
	}

	[Fact]
	public void BadBeOkWithBecause()
	{
		RunCompareFailTest(() => new FakeWebResult(404).Should().BeOk("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeOk()
	{
		new FakeWebResult(200).Should().BeOk();
	}
}
