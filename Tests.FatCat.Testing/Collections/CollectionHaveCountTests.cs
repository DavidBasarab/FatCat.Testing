namespace Tests.FatCat.Testing.Collections;

public class CollectionHaveCountTests : BaseTest
{
	[Fact]
	public void GoodHaveCount() { new List<int> { 1, 2, 3 }.Should().HaveCount(3); }

	[Fact]
	public void BadHaveCount() { RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveCount(5)); }

	[Fact]
	public void BadHaveCountShowsCorrectMessage()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveCount(5), "[1, 2, 3] should have count 5 but has 3");
	}

	[Fact]
	public void BadHaveCountWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveCount(5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotHaveCount() { new List<int> { 1, 2, 3 }.Should().Not.HaveCount(5); }

	[Fact]
	public void BadNotHaveCount() { RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().Not.HaveCount(3)); }

	[Fact]
	public void BadNotHaveCountShowsCorrectMessage()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().Not.HaveCount(3), "[1, 2, 3] should not have count 3");
	}

	[Fact]
	public void BadNotHaveCountWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().Not.HaveCount(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadHaveCountOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().HaveCount(3), "null should have count 3 but has 0");
	}
}