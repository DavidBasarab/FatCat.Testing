namespace Tests.FatCat.Testing.Collections;

public class CollectionContainPredicateTests : BaseTest
{
	[Fact]
	public void GoodContain()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.Contain(value => value > 2);
	}

	[Fact]
	public void BadContain()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.Contain(value => value > 5)
		);
	}

	[Fact]
	public void BadContainShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Contain(value => value > 5),
			"[1, 2, 3] should contain an element matching the predicate"
		);
	}

	[Fact]
	public void BadContainWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Contain(value => value > 5, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodNotContain()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.Not.Contain(value => value > 5);
	}

	[Fact]
	public void BadNotContain()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.Not.Contain(value => value > 2)
		);
	}

	[Fact]
	public void BadNotContainShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Not.Contain(value => value > 2),
			"[1, 2, 3] should not contain an element matching the predicate"
		);
	}

	[Fact]
	public void BadNotContainWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Not.Contain(value => value > 2, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContainOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(
			() => subject.Should().Contain(value => value > 2),
			"null should contain an element matching the predicate"
		);
	}

	[Fact]
	public void BadNotContainOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(
			() => subject.Should().Not.Contain(value => value > 2),
			"null should not contain an element matching the predicate"
		);
	}

	[Fact]
	public void GoodContainPredicateOnObjectCollection()
	{
		new List<object> { 1, "two" }
			.Should()
			.Contain(element => element is string);
	}

	[Fact]
	public void GoodContainValueOnObjectCollection()
	{
		new List<object> { 1, "two" }
			.Should()
			.Contain("two");
	}

	[Fact]
	public void BadContainPredicateOnObjectCollectionShowsPredicateMessage()
	{
		RunCompareFailTest(
			() =>
				new List<object> { 1 }
					.Should()
					.Contain(element => element is string),
			"[1] should contain an element matching the predicate"
		);
	}

	[Fact]
	public void BadContainValueOnObjectCollectionShowsValueMessage()
	{
		RunCompareFailTest(
			() =>
				new List<object> { 1 }
					.Should()
					.Contain("two"),
			"[1] should contain two"
		);
	}

	[Fact]
	public void GoodContainExplicitlyTypedPredicateOnObjectCollection()
	{
		new List<object> { 1, "two" }
			.Should()
			.Contain((object element) => element is string);
	}

	[Fact]
	public void GoodContainPredicateOnStringCollection()
	{
		new List<string> { "alpha", "beta" }
			.Should()
			.Contain(text => text.StartsWith('b'));
	}

	[Fact]
	public void GoodContainValueOnStringCollection()
	{
		new List<string> { "alpha", "beta" }
			.Should()
			.Contain("beta");
	}

	[Fact]
	public void GoodContainTypedNullOnStringCollection()
	{
		new List<string> { "alpha", null }
			.Should()
			.Contain((string)null);
	}
}
