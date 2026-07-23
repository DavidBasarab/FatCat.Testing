namespace Tests.FatCat.Testing.Collections;

public class CollectionBeInAscendingOrderTests : BaseTest
{
	[Fact]
	public void GoodBeInAscendingOrder()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.BeInAscendingOrder();
	}

	[Fact]
	public void BadBeInAscendingOrder()
	{
		RunCompareFailTest(() =>
			new List<int> { 3, 1, 2 }
				.Should()
				.BeInAscendingOrder()
		);
	}

	[Fact]
	public void BadBeInAscendingOrderShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 3, 1, 2 }
					.Should()
					.BeInAscendingOrder(),
			"[3, 1, 2] should be in ascending order"
		);
	}

	[Fact]
	public void BadBeInAscendingOrderWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 3, 1, 2 }
					.Should()
					.BeInAscendingOrder("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadBeInAscendingOrderOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().BeInAscendingOrder(), "null should be in ascending order");
	}
}
