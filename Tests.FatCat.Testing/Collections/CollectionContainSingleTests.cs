namespace Tests.FatCat.Testing.Collections;

public class CollectionContainSingleTests : BaseTest
{
	[Fact]
	public void GoodContainSingle()
	{
		new List<int> { 42 }
			.Should()
			.ContainSingle();
	}

	[Fact]
	public void BadContainSingle()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.ContainSingle()
		);
	}

	[Fact]
	public void BadContainSingleShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.ContainSingle(),
			"[1, 2, 3] should contain a single element but has 3"
		);
	}

	[Fact]
	public void BadContainSingleWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.ContainSingle("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContainSingleOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().ContainSingle(), "null should contain a single element but has 0");
	}
}
