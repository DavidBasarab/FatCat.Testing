using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionContainSingleTests : BaseTest
{
	[Fact]
	public void GoodContainSingle()
	{
		int[] numbers = [42];

		numbers.Should().ContainSingle();
	}

	[Fact]
	public void BadContainSingle()
	{
		int[] numbers = [1, 2];

		RunCompareFailTest(() => numbers.Should().ContainSingle());
	}

	[Fact]
	public void BadContainSingleShowsCorrectMessage()
	{
		int[] numbers = [1, 2];

		RunCompareFailTest(() => numbers.Should().ContainSingle(), "{ 1, 2 } should contain a single element");
	}

	[Fact]
	public void BadContainSingleWithBecause()
	{
		int[] numbers = [1, 2];

		RunCompareFailTest(() => numbers.Should().ContainSingle("custom because"), "custom because");
	}
}
