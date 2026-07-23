namespace Tests.FatCat.Testing.Collections;

public class CollectionHaveSameCountTests : BaseTest
{
	[Fact]
	public void GoodHaveSameCount() { new List<int> { 1, 2 }.Should().HaveSameCount(new List<int> { 3, 4 }); }

	[Fact]
	public void BadHaveSameCount() { RunCompareFailTest(() => new List<int> { 1, 2 }.Should().HaveSameCount(new List<int> { 3, 4, 5 })); }

	[Fact]
	public void BadHaveSameCountShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 2 }.Should().HaveSameCount(new List<int> { 3, 4, 5 }),
							"[1, 2] should have the same count as [3, 4, 5] (3) but has 2"
						);
	}

	[Fact]
	public void BadHaveSameCountWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2 }.Should().HaveSameCount(new List<int> { 3, 4, 5 }, "custom because"), "custom because");
	}

	[Fact]
	public void BadHaveSameCountOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(
							() => subject.Should().HaveSameCount(new List<int> { 3, 4, 5 }),
							"null should have the same count as [3, 4, 5] (3) but has 0"
						);
	}
}
