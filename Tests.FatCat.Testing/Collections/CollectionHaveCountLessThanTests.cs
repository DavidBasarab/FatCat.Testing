namespace Tests.FatCat.Testing.Collections;

public class CollectionHaveCountLessThanTests : BaseTest
{
	[Fact]
	public void GoodHaveCountLessThan() { new List<int> { 1, 2 }.Should().HaveCountLessThan(3); }

	[Fact]
	public void BadHaveCountLessThan() { RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveCountLessThan(3)); }

	[Fact]
	public void BadHaveCountLessThanShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 2, 3 }.Should().HaveCountLessThan(3),
							"[1, 2, 3] should have count less than 3 but has 3"
						);
	}

	[Fact]
	public void BadHaveCountLessThanWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveCountLessThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadHaveCountLessThanOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().HaveCountLessThan(3), "null should have count less than 3 but has 0");
	}
}
