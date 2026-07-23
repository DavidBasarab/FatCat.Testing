namespace Tests.FatCat.Testing.Collections;

public class CollectionContainInOrderTests : BaseTest
{
	[Fact]
	public void GoodContainInOrder() { new List<int> { 1, 2, 3 }.Should().ContainInOrder(1, 2, 3); }

	[Fact]
	public void GoodContainInOrderNonContiguous() { new List<int> { 1, 2, 3, 4, 5 }.Should().ContainInOrder(1, 3, 5); }

	[Fact]
	public void BadContainInOrderWhenOutOfOrder() { RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().ContainInOrder(3, 1)); }

	[Fact]
	public void BadContainInOrderShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 2, 3 }.Should().ContainInOrder(3, 1),
							"[1, 2, 3] should contain [3, 1] in order"
						);
	}

	[Fact]
	public void BadContainInOrderWhenMissing()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 2, 3 }.Should().ContainInOrder(1, 9),
							"[1, 2, 3] should contain [1, 9] in order"
						);
	}

	[Fact]
	public void BadContainInOrderWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().ContainInOrder(new List<int> { 3, 1 }, "custom because"), "custom because");
	}

	[Fact]
	public void BadContainInOrderOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().ContainInOrder(1, 2, 3), "null should contain [1, 2, 3] in order");
	}
}