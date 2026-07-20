namespace Tests.FatCat.Testing.CustomAssertions;

public class ProvingComparerBeTheAnswerTests : BaseTest
{
	[Fact]
	public void BadBeTheAnswer()
	{
		RunCompareFailTest(() => new ProvingComparer(7).BeTheAnswer());
	}

	[Fact]
	public void BadBeTheAnswerShowsCorrectMessage()
	{
		RunCompareFailTest(() => new ProvingComparer(7).BeTheAnswer(), "7 should be the answer");
	}

	[Fact]
	public void BadBeTheAnswerWithBecause()
	{
		RunCompareFailTest(() => new ProvingComparer(7).BeTheAnswer("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeTheAnswer()
	{
		new ProvingComparer(42).BeTheAnswer();
	}
}
