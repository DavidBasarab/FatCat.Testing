using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionBeInDescendingOrderTests : BaseTest
{
	[Fact]
	public void GoodBeInDescendingOrder()
	{
		int[] numbers = [3, 2, 1];

		numbers.Should().BeInDescendingOrder();
	}

	[Fact]
	public void BadBeInDescendingOrder()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().BeInDescendingOrder());
	}

	[Fact]
	public void BadBeInDescendingOrderShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().BeInDescendingOrder(), "{ 1, 2, 3 } should be in descending order");
	}

	[Fact]
	public void BadBeInDescendingOrderWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().BeInDescendingOrder("custom because"), "custom because");
	}
}
