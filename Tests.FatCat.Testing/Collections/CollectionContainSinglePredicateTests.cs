namespace Tests.FatCat.Testing.Collections;

public class CollectionContainSinglePredicateTests : BaseTest
{
	[Fact]
	public void GoodContainSingle()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.ContainSingle(value => value == 2);
	}

	[Fact]
	public void BadContainSingle()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.ContainSingle(value => value > 1)
		);
	}

	[Fact]
	public void BadContainSingleShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.ContainSingle(value => value > 1),
			"[1, 2, 3] should contain a single element matching the predicate but 2 matched"
		);
	}

	[Fact]
	public void BadContainSingleWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.ContainSingle(value => value > 1, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContainSingleOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(
			() => subject.Should().ContainSingle(value => value > 1),
			"null should contain a single element matching the predicate but 0 matched"
		);
	}
}
