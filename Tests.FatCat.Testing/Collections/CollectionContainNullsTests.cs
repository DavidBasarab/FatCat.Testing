namespace Tests.FatCat.Testing.Collections;

public class CollectionContainNullsTests : BaseTest
{
	[Fact]
	public void GoodContainNulls()
	{
		new List<string> { "a", null, "b" }
			.Should()
			.ContainNulls();
	}

	[Fact]
	public void BadContainNulls()
	{
		RunCompareFailTest(() =>
			new List<string> { "a", "b" }
				.Should()
				.ContainNulls()
		);
	}

	[Fact]
	public void BadContainNullsShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<string> { "a", "b" }
					.Should()
					.ContainNulls(),
			"[\"a\", \"b\"] should contain nulls"
		);
	}

	[Fact]
	public void BadContainNullsWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<string> { "a", "b" }
					.Should()
					.ContainNulls("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodNotContainNulls()
	{
		new List<string> { "a", "b" }
			.Should()
			.Not.ContainNulls();
	}

	[Fact]
	public void BadNotContainNulls()
	{
		RunCompareFailTest(() =>
			new List<string> { "a", null, "b" }
				.Should()
				.Not.ContainNulls()
		);
	}

	[Fact]
	public void BadNotContainNullsShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<string> { "a", null, "b" }
					.Should()
					.Not.ContainNulls(),
			"[\"a\", null, \"b\"] should not contain nulls"
		);
	}

	[Fact]
	public void BadNotContainNullsWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<string> { "a", null, "b" }
					.Should()
					.Not.ContainNulls("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContainNullsOnNull()
	{
		List<string> subject = null;

		RunCompareFailTest(() => subject.Should().ContainNulls(), "null should contain nulls");
	}
}
