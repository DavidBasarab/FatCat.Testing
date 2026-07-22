namespace Tests.FatCat.Testing.Collections;

public class CollectionMultipleEnumerationTests : BaseTest
{
	[Fact]
	public void GoodSnapshotEnumeratesSourceOnce()
	{
		var enumerationCount = 0;

		IEnumerable<int> CountingSource()
		{
			enumerationCount++;

			yield return 1;
			yield return 2;
			yield return 3;
		}

		var comparer = CountingSource().Should();

		comparer.Contain(1);
		comparer.HaveCount(3);
		comparer.Not.Contain(9);

		enumerationCount.Should().Be(1);
	}
}