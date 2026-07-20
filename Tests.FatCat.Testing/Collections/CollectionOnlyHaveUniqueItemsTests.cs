using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionOnlyHaveUniqueItemsTests : BaseTest
{
	[Fact]
	public void GoodOnlyHaveUniqueItems()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().OnlyHaveUniqueItems();
	}

	[Fact]
	public void BadOnlyHaveUniqueItems()
	{
		int[] numbers = [1, 2, 2];

		RunCompareFailTest(() => numbers.Should().OnlyHaveUniqueItems());
	}

	[Fact]
	public void BadOnlyHaveUniqueItemsShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 2];

		RunCompareFailTest(() => numbers.Should().OnlyHaveUniqueItems(), "{ 1, 2, 2 } should only have unique items");
	}

	[Fact]
	public void BadOnlyHaveUniqueItemsWithBecause()
	{
		int[] numbers = [1, 2, 2];

		RunCompareFailTest(() => numbers.Should().OnlyHaveUniqueItems("custom because"), "custom because");
	}
}
