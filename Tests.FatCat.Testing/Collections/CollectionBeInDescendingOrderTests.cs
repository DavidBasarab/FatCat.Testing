namespace Tests.FatCat.Testing.Collections;

public class CollectionBeInDescendingOrderTests : BaseTest
{
	[Fact]
	public void GoodBeInDescendingOrder() { new List<int> { 3, 2, 1 }.Should().BeInDescendingOrder(); }

	[Fact]
	public void BadBeInDescendingOrder() { RunCompareFailTest(() => new List<int> { 3, 1, 2 }.Should().BeInDescendingOrder()); }

	[Fact]
	public void BadBeInDescendingOrderShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 3, 1, 2 }.Should().BeInDescendingOrder(),
							"[3, 1, 2] should be in descending order"
						);
	}

	[Fact]
	public void BadBeInDescendingOrderWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 3, 1, 2 }.Should().BeInDescendingOrder("custom because"), "custom because");
	}

	[Fact]
	public void BadBeInDescendingOrderOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().BeInDescendingOrder(), "null should be in descending order");
	}
}