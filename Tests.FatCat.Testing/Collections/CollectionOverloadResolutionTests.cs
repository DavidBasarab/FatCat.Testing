using System.Collections.Immutable;
using System.Collections.ObjectModel;
using FatCat.Testing.Collections;
using FatCat.Testing.Enums;
using FatCat.Testing.Numbers;
using FatCat.Testing.Objects;
using FatCat.Testing.Strings;
using Tests.FatCat.Testing.Enums;
using Tests.FatCat.Testing.Objects;

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

	[Fact]
	public void EnumStillBindsToEnumComparer()
	{
		var comparer = TestStatus.Active.Should();

		Assert.IsType<EnumComparer<TestStatus>>(comparer);
	}

	[Fact]
	public void DtoBindsToObjectComparer()
	{
		var subject = new Dto("Alice");

		var comparer = subject.Should();

		Assert.IsType<ObjectComparer<Dto>>(comparer);
	}

	[Fact]
	public void InterfaceTypedReferenceBindsToObjectComparer()
	{
		IThing subject = new Thing();

		var comparer = subject.Should();

		Assert.IsType<ObjectComparer<IThing>>(comparer);
	}

	[Fact]
	public void ReadOnlyListInterfaceBindsToCollectionComparer()
	{
		IReadOnlyList<string> subject = ["a", "b"];

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void ReadOnlyCollectionInterfaceBindsToCollectionComparer()
	{
		IReadOnlyCollection<string> subject = ["a", "b"];

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void ListInterfaceBindsToCollectionComparer()
	{
		IList<string> subject = ["a", "b"];

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void CollectionInterfaceBindsToCollectionComparer()
	{
		ICollection<string> subject = ["a", "b"];

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void ReadOnlyCollectionClassStillBindsToObjectComparer()
	{
		var subject = new ReadOnlyCollection<string>(["a", "b"]);

		var comparer = subject.Should();

		Assert.IsType<ObjectComparer<ReadOnlyCollection<string>>>(comparer);
	}

	[Fact]
	public void HashSetStillBindsToObjectComparer()
	{
		var subject = new HashSet<string> { "a", "b" };

		var comparer = subject.Should();

		Assert.IsType<ObjectComparer<HashSet<string>>>(comparer);
	}

	[Fact]
	public void CustomCollectionInterfaceStillBindsToObjectComparer()
	{
		IImmutableList<string> subject = ImmutableList.Create("a", "b");

		var comparer = subject.Should();

		Assert.IsType<ObjectComparer<IImmutableList<string>>>(comparer);
	}

	[Fact]
	public void AsEnumerableBindsCustomCollectionInterfaceToCollectionComparer()
	{
		IImmutableList<string> subject = ImmutableList.Create("a", "b");

		var comparer = subject.AsEnumerable().Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void ImmutableArrayStillBindsToCollectionComparer()
	{
		var subject = ImmutableArray.Create("a", "b");

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void ArraySegmentStillBindsToCollectionComparer()
	{
		var subject = new ArraySegment<string>(["a", "b"]);

		var comparer = subject.Should();

		Assert.IsType<CollectionComparer<string>>(comparer);
	}

	[Fact]
	public void DictionaryInterfaceStillBindsToObjectComparer()
	{
		IDictionary<string, int> subject = new Dictionary<string, int> { ["a"] = 1 };

		var comparer = subject.Should();

		Assert.IsType<ObjectComparer<IDictionary<string, int>>>(comparer);
	}

	[Fact]
	public void ReadOnlyDictionaryInterfaceStillBindsToObjectComparer()
	{
		IReadOnlyDictionary<string, int> subject = new Dictionary<string, int> { ["a"] = 1 };

		var comparer = subject.Should();

		Assert.IsType<ObjectComparer<IReadOnlyDictionary<string, int>>>(comparer);
	}
}
