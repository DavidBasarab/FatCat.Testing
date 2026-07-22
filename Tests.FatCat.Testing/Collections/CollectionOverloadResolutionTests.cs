using FatCat.Testing.Collections;
using FatCat.Testing.Numbers;
using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Collections;

public class CollectionOverloadResolutionTests : BaseTest
{
	[Fact]
	public void ListBindsToCollectionComparer()
	{
		var subject = new List<string> { "a", "b" };

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void ArrayBindsToCollectionComparer()
	{
		var subject = new[] { "a", "b" };

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void EnumerableBindsToCollectionComparer()
	{
		IEnumerable<string> subject = new[] { "a", "b" }.Where(value => value == "a");

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void StringStillBindsToStringComparer()
	{
		var comparer = "hello".Should();

		Assert.IsType<NullableStringComparer>(comparer);
	}

	[Fact]
	public void IntStillBindsToNumericComparer()
	{
		var comparer = 42.Should();

		Assert.IsType<NumericComparer<int>>(comparer);
	}
}