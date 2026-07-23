namespace Tests.FatCat.Testing.Collections;

public class CollectionEqualTests : BaseTest
{
	[Fact]
	public void GoodEqual()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.Equal(new List<int> { 1, 2, 3 });
	}

	[Fact]
	public void BadEqual()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.Equal(new List<int> { 1, 2, 4 })
		);
	}

	[Fact]
	public void BadEqualShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Equal(new List<int> { 1, 2, 4 }),
			"[1, 2, 3] should equal [1, 2, 4]"
		);
	}

	[Fact]
	public void BadEqualWhenReorderedShowsItIsOrderSensitive()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Equal(new List<int> { 3, 2, 1 }),
			"[1, 2, 3] should equal [3, 2, 1]"
		);
	}

	[Fact]
	public void BadEqualWhenLengthsDiffer()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Equal(new List<int> { 1, 2 }),
			"[1, 2, 3] should equal [1, 2]"
		);
	}

	[Fact]
	public void BadEqualWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Equal(new List<int> { 1, 2, 4 }, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadEqualOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().Equal(new List<int> { 1, 2, 3 }), "null should equal [1, 2, 3]");
	}
}
