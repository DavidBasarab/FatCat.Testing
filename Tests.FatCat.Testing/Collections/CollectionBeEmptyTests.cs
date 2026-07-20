using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionBeEmptyTests : BaseTest
{
	[Fact]
	public void GoodBeEmpty()
	{
		int[] numbers = [];

		numbers.Should().BeEmpty();
	}

	[Fact]
	public void BadBeEmpty()
	{
		int[] numbers = [1, 2];

		RunCompareFailTest(() => numbers.Should().BeEmpty());
	}

	[Fact]
	public void BadBeEmptyShowsCorrectMessage()
	{
		int[] numbers = [1, 2];

		RunCompareFailTest(() => numbers.Should().BeEmpty(), "{ 1, 2 } should be empty");
	}

	[Fact]
	public void BadBeEmptyWithBecause()
	{
		int[] numbers = [1, 2];

		RunCompareFailTest(() => numbers.Should().BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotBeEmpty()
	{
		int[] numbers = [1, 2];

		numbers.Should().Not.BeEmpty();
	}

	[Fact]
	public void BadNotBeEmpty()
	{
		int[] numbers = [];

		RunCompareFailTest(() => numbers.Should().Not.BeEmpty());
	}

	[Fact]
	public void BadNotBeEmptyShowsCorrectMessage()
	{
		int[] numbers = [];

		RunCompareFailTest(() => numbers.Should().Not.BeEmpty(), "{ } should not be empty");
	}

	[Fact]
	public void BadNotBeEmptyWithBecause()
	{
		int[] numbers = [];

		RunCompareFailTest(() => numbers.Should().Not.BeEmpty("custom because"), "custom because");
	}
}
