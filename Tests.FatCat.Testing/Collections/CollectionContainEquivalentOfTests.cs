using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionContainEquivalentOfTests : BaseTest
{
	[Fact]
	public void GoodContainEquivalentOf()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().ContainEquivalentOf(2);
	}

	[Fact]
	public void GoodContainEquivalentOfWithStructurallyEqualElement()
	{
		Widget[] widgets = [new Widget { Name = "First" }, new Widget { Name = "Second" }];

		widgets.Should().ContainEquivalentOf(new Widget { Name = "Second" });
	}

	[Fact]
	public void BadContainEquivalentOf()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().ContainEquivalentOf(9));
	}

	[Fact]
	public void BadContainEquivalentOfShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().ContainEquivalentOf(9), "{ 1, 2, 3 } should contain an equivalent of 9");
	}

	[Fact]
	public void BadContainEquivalentOfWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().ContainEquivalentOf(9, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotContainEquivalentOf()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Not.ContainEquivalentOf(9);
	}

	[Fact]
	public void BadNotContainEquivalentOf()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Not.ContainEquivalentOf(2));
	}

	[Fact]
	public void BadNotContainEquivalentOfShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(
			() => numbers.Should().Not.ContainEquivalentOf(2),
			"{ 1, 2, 3 } should not contain an equivalent of 2"
		);
	}

	[Fact]
	public void BadNotContainEquivalentOfWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Not.ContainEquivalentOf(2, "custom because"), "custom because");
	}
}
