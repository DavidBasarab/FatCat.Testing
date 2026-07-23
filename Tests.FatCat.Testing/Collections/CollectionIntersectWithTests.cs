namespace Tests.FatCat.Testing.Collections;

public class CollectionIntersectWithTests : BaseTest
{
	[Fact]
	public void GoodIntersectWith()
	{
		new List<int> { 1, 2 }
			.Should()
			.IntersectWith(new List<int> { 2, 3 });
	}

	[Fact]
	public void BadIntersectWith()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2 }
				.Should()
				.IntersectWith(new List<int> { 3, 4 })
		);
	}

	[Fact]
	public void BadIntersectWithShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.IntersectWith(new List<int> { 3, 4 }),
			"[1, 2] should intersect with [3, 4]"
		);
	}

	[Fact]
	public void BadIntersectWithWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.IntersectWith(new List<int> { 3, 4 }, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodNotIntersectWith()
	{
		new List<int> { 1, 2 }
			.Should()
			.Not.IntersectWith(new List<int> { 3, 4 });
	}

	[Fact]
	public void BadNotIntersectWith()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2 }
				.Should()
				.Not.IntersectWith(new List<int> { 2, 3 })
		);
	}

	[Fact]
	public void BadNotIntersectWithShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.Not.IntersectWith(new List<int> { 2, 3 }),
			"[1, 2] should not intersect with [2, 3]"
		);
	}

	[Fact]
	public void BadNotIntersectWithWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.Not.IntersectWith(new List<int> { 2, 3 }, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadIntersectWithOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().IntersectWith(new List<int> { 1, 2 }), "null should intersect with [1, 2]");
	}
}
