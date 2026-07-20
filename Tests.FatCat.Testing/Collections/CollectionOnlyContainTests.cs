using FatCat.Testing.Collections;

namespace Tests.FatCat.Testing.Collections;

public class CollectionOnlyContainTests : BaseTest
{
	[Fact]
	public void GoodOnlyContain()
	{
		int[] numbers = [2, 4, 6];

		numbers.Should().OnlyContain(number => number % 2 == 0);
	}

	[Fact]
	public void BadOnlyContain()
	{
		int[] numbers = [2, 3, 4];

		RunCompareFailTest(() => numbers.Should().OnlyContain(number => number % 2 == 0));
	}

	[Fact]
	public void BadOnlyContainShowsCorrectMessage()
	{
		int[] numbers = [2, 3, 4];

		RunCompareFailTest(
			() => numbers.Should().OnlyContain(number => number % 2 == 0),
			"{ 2, 3, 4 } should only contain elements matching the predicate"
		);
	}

	[Fact]
	public void BadOnlyContainWithBecause()
	{
		int[] numbers = [2, 3, 4];

		RunCompareFailTest(() => numbers.Should().OnlyContain(number => number % 2 == 0, "custom because"), "custom because");
	}
}
