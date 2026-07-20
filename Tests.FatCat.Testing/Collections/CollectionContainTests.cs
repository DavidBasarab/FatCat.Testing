using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionContainTests : BaseTest
{
	[Fact]
	public void GoodContain()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Contain(2);
	}

	[Fact]
	public void BadContain()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Contain(5));
	}

	[Fact]
	public void BadContainShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Contain(5), "{ 1, 2, 3 } should contain 5");
	}

	[Fact]
	public void BadContainWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Contain(5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotContain()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Not.Contain(5);
	}

	[Fact]
	public void BadNotContain()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Not.Contain(2));
	}

	[Fact]
	public void BadNotContainShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Not.Contain(2), "{ 1, 2, 3 } should not contain 2");
	}

	[Fact]
	public void BadNotContainWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Not.Contain(2, "custom because"), "custom because");
	}
}
