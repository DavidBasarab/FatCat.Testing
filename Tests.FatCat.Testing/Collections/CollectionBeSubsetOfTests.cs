namespace Tests.FatCat.Testing.Collections;

public class CollectionBeSubsetOfTests : BaseTest
{
	[Fact]
	public void GoodBeSubsetOf() { new List<int> { 1, 2 }.Should().BeSubsetOf(new List<int> { 1, 2, 3 }); }

	[Fact]
	public void BadBeSubsetOf() { RunCompareFailTest(() => new List<int> { 1, 5 }.Should().BeSubsetOf(new List<int> { 1, 2, 3 })); }

	[Fact]
	public void BadBeSubsetOfShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 5 }.Should().BeSubsetOf(new List<int> { 1, 2, 3 }),
							"[1, 5] should be a subset of [1, 2, 3]"
						);
	}

	[Fact]
	public void BadBeSubsetOfWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 5 }.Should().BeSubsetOf(new List<int> { 1, 2, 3 }, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeSubsetOfOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().BeSubsetOf(new List<int> { 1, 2, 3 }), "null should be a subset of [1, 2, 3]");
	}
}
