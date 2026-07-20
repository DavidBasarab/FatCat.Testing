namespace Tests.FatCat.Testing.Collections;

public class CollectionMigrationTests : BaseTest
{
	[Fact]
	public void NotContainRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Not.Contain(5);
	}

	[Fact]
	public void NotBeEmptyRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Not.BeEmpty();
	}

	[Fact]
	public void NotContainEquivalentOfRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Not.ContainEquivalentOf(9);
	}

	[Fact]
	public void ContainRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Contain(2);
	}

	[Fact]
	public void BeEmptyRewrite()
	{
		int[] numbers = [];

		numbers.Should().BeEmpty();
	}

	[Fact]
	public void HaveCountRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().HaveCount(3);
	}

	[Fact]
	public void ContainSingleRewrite()
	{
		int[] numbers = [42];

		numbers.Should().ContainSingle();
	}

	[Fact]
	public void ContainEquivalentOfRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().ContainEquivalentOf(2);
	}

	[Fact]
	public void OnlyContainRewrite()
	{
		int[] numbers = [2, 4, 6];

		numbers.Should().OnlyContain(number => number % 2 == 0);
	}

	[Fact]
	public void EqualRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Equal([1, 2, 3]);
	}

	[Fact]
	public void BeEquivalentToRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().BeEquivalentTo([3, 2, 1]);
	}

	[Fact]
	public void OnlyHaveUniqueItemsRewrite()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().OnlyHaveUniqueItems();
	}

	[Fact]
	public void BeInDescendingOrderRewrite()
	{
		int[] numbers = [3, 2, 1];

		numbers.Should().BeInDescendingOrder();
	}
}
