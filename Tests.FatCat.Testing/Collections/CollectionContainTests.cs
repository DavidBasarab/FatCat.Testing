namespace Tests.FatCat.Testing.Collections;

public class CollectionContainTests : BaseTest
{
	[Fact]
	public void GoodContain()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.Contain(2);
	}

	[Fact]
	public void BadContain()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.Contain(5)
		);
	}

	[Fact]
	public void BadContainShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Contain(5),
			"[1, 2, 3] should contain 5"
		);
	}

	[Fact]
	public void BadContainWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Contain(5, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodNotContain()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.Not.Contain(5);
	}

	[Fact]
	public void BadNotContain()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.Not.Contain(2)
		);
	}

	[Fact]
	public void BadNotContainShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Not.Contain(2),
			"[1, 2, 3] should not contain 2"
		);
	}

	[Fact]
	public void BadNotContainWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Not.Contain(2, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContainOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().Contain(5), "null should contain 5");
	}
}
