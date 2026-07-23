namespace Tests.FatCat.Testing.Collections;

public class CollectionOnlyHaveUniqueItemsTests : BaseTest
{
	[Fact]
	public void GoodOnlyHaveUniqueItems() { new List<int> { 1, 2, 3 }.Should().OnlyHaveUniqueItems(); }

	[Fact]
	public void BadOnlyHaveUniqueItems() { RunCompareFailTest(() => new List<int> { 1, 2, 2 }.Should().OnlyHaveUniqueItems()); }

	[Fact]
	public void BadOnlyHaveUniqueItemsShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 2, 2 }.Should().OnlyHaveUniqueItems(),
							"[1, 2, 2] should only have unique items"
						);
	}

	[Fact]
	public void BadOnlyHaveUniqueItemsWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 2 }.Should().OnlyHaveUniqueItems("custom because"), "custom because");
	}

	[Fact]
	public void BadOnlyHaveUniqueItemsOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().OnlyHaveUniqueItems(), "null should only have unique items");
	}
}