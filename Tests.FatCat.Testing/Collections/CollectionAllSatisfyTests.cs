namespace Tests.FatCat.Testing.Collections;

public class CollectionAllSatisfyTests : BaseTest
{
	[Fact]
	public void GoodAllSatisfy() { new List<int> { 1, 2, 3 }.Should().AllSatisfy(value => value.Should().BeGreaterThan(0)); }

	[Fact]
	public void BadAllSatisfy()
	{
		RunCompareFailTest(() => new List<int> { 1, -2, 3 }.Should().AllSatisfy(value => value.Should().BeGreaterThan(0)));
	}

	[Fact]
	public void BadAllSatisfyShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, -2, 3 }.Should().AllSatisfy(value => value.Should().BeGreaterThan(0)),
							"[1, -2, 3] should have every element satisfy the inspector but element at index 1 did not"
						);
	}

	[Fact]
	public void BadAllSatisfyWithBecause()
	{
		RunCompareFailTest(
							() => new List<int> { 1, -2, 3 }.Should().AllSatisfy(value => value.Should().BeGreaterThan(0), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadAllSatisfyOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(
							() => subject.Should().AllSatisfy(value => value.Should().BeGreaterThan(0)),
							"null should have every element satisfy the inspector"
						);
	}
}
