namespace Tests.FatCat.Testing.Collections;

public class CollectionBeEmptyTests : BaseTest
{
	[Fact]
	public void GoodBeEmpty()
	{
		new List<int>().Should().BeEmpty();
	}

	[Fact]
	public void BadBeEmpty()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.BeEmpty()
		);
	}

	[Fact]
	public void BadBeEmptyShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.BeEmpty(),
			"[1, 2, 3] should be empty"
		);
	}

	[Fact]
	public void BadBeEmptyWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.BeEmpty("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadBeEmptyOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().BeEmpty(), "null should be empty");
	}
}
