using FatCat.Testing.Collections;
using FatCat.Testing.Objects;
using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Collections;

public class CollectionShouldOverloadTests : BaseTest
{
	[Fact]
	public void StringSubjectStaysStringComparer()
	{
		Assert.IsType<NullableStringComparer>("text".Should());
	}

	[Fact]
	public void IntArraySubjectBindsToCollectionComparer()
	{
		int[] numbers = [1, 2, 3];

		Assert.IsType<CollectionComparer<int>>(numbers.Should());
	}

	[Fact]
	public void ListSubjectBindsToCollectionComparer()
	{
		List<string> names = [];

		Assert.IsType<CollectionComparer<string>>(names.Should());
	}

	[Fact]
	public void NonEnumerableDtoBindsToObjectComparer()
	{
		Assert.IsType<ObjectComparer>(new Widget().Should());
	}

	[Fact]
	public void EnumerableDtoBindsToCollectionComparer()
	{
		var sequence = new NumberSequence([1, 2, 3]);

		Assert.IsType<CollectionComparer<int>>(sequence.Should());
	}
}
