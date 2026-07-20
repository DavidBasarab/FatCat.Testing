using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionEqualTests : BaseTest
{
	[Fact]
	public void GoodEqual()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().Equal([1, 2, 3]);
	}

	[Fact]
	public void BadEqual()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Equal([3, 2, 1]));
	}

	[Fact]
	public void BadEqualShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Equal([3, 2, 1]), "{ 1, 2, 3 } should equal { 3, 2, 1 }");
	}

	[Fact]
	public void BadEqualWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().Equal([3, 2, 1], "custom because"), "custom because");
	}
}
