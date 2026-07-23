namespace Tests.FatCat.Testing.Collections;

public class CollectionHaveCountGreaterThanTests : BaseTest
{
	[Fact]
	public void GoodHaveCountGreaterThan()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.HaveCountGreaterThan(2);
	}

	[Fact]
	public void BadHaveCountGreaterThan()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2 }
				.Should()
				.HaveCountGreaterThan(2)
		);
	}

	[Fact]
	public void BadHaveCountGreaterThanShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.HaveCountGreaterThan(2),
			"[1, 2] should have count greater than 2 but has 2"
		);
	}

	[Fact]
	public void BadHaveCountGreaterThanWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.HaveCountGreaterThan(2, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadHaveCountGreaterThanOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().HaveCountGreaterThan(2), "null should have count greater than 2 but has 0");
	}
}
