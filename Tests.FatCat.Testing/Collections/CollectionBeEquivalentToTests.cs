using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionBeEquivalentToTests : BaseTest
{
	[Fact]
	public void GoodBeEquivalentToIgnoresOrder()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().BeEquivalentTo([3, 2, 1]);
	}

	[Fact]
	public void GoodBeEquivalentToWithStructurallyEqualElements()
	{
		Widget[] widgets = [new Widget { Name = "First" }, new Widget { Name = "Second" }];

		widgets.Should().BeEquivalentTo([new Widget { Name = "Second" }, new Widget { Name = "First" }]);
	}

	[Fact]
	public void BadBeEquivalentTo()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().BeEquivalentTo([1, 2, 4]));
	}

	[Fact]
	public void BadBeEquivalentToDifferentCount()
	{
		int[] numbers = [1, 2];

		RunCompareFailTest(() => numbers.Should().BeEquivalentTo([1, 2, 3]));
	}

	[Fact]
	public void BadBeEquivalentToShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().BeEquivalentTo([1, 2, 4]), "{ 1, 2, 3 } should be equivalent to { 1, 2, 4 }");
	}

	[Fact]
	public void BadBeEquivalentToWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().BeEquivalentTo([1, 2, 4], "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotBeEquivalentTo()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Not.BeEquivalentTo([1, 2, 4]);
	}

	[Fact]
	public void BadNotBeEquivalentTo()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Not.BeEquivalentTo([3, 2, 1]));
	}

	[Fact]
	public void BadNotBeEquivalentToShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(
			() => numbers.Should().Not.BeEquivalentTo([3, 2, 1]),
			"{ 1, 2, 3 } should not be equivalent to { 3, 2, 1 }"
		);
	}

	[Fact]
	public void BadNotBeEquivalentToWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Not.BeEquivalentTo([3, 2, 1], "custom because"), "custom because");
	}
}
