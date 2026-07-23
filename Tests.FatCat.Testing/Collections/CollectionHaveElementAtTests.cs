namespace Tests.FatCat.Testing.Collections;

public class CollectionHaveElementAtTests : BaseTest
{
	[Fact]
	public void GoodHaveElementAt() { new List<int> { 1, 2, 3 }.Should().HaveElementAt(1, 2); }

	[Fact]
	public void BadHaveElementAt() { RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveElementAt(1, 5)); }

	[Fact]
	public void BadHaveElementAtOutOfRange() { RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveElementAt(5, 2)); }

	[Fact]
	public void BadHaveElementAtShowsCorrectMessage()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveElementAt(1, 5), "[1, 2, 3] should have 5 at index 1");
	}

	[Fact]
	public void BadHaveElementAtWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().HaveElementAt(1, 5, "custom because"), "custom because");
	}

	[Fact]
	public void BadHaveElementAtOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().HaveElementAt(1, 5), "null should have 5 at index 1");
	}
}
