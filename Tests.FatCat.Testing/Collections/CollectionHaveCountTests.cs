using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionHaveCountTests : BaseTest
{
	[Fact]
	public void GoodHaveCount()
	{
		int[] numbers = [1, 2, 3];

		numbers.Should().HaveCount(3);
	}

	[Fact]
	public void BadHaveCount()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().HaveCount(5));
	}

	[Fact]
	public void BadHaveCountShowsCorrectMessage()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().HaveCount(5), "{ 1, 2, 3 } should have count 5");
	}

	[Fact]
	public void BadHaveCountWithBecause()
	{
		int[] numbers = [1, 2, 3];

		RunCompareFailTest(() => numbers.Should().HaveCount(5, "custom because"), "custom because");
	}
}
