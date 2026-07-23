namespace Tests.FatCat.Testing.Collections;

public class CollectionOnlyContainTests : BaseTest
{
	[Fact]
	public void GoodOnlyContain() { new List<int> { 1, 2, 3 }.Should().OnlyContain(value => value > 0); }

	[Fact]
	public void BadOnlyContain() { RunCompareFailTest(() => new List<int> { 1, -2, 3 }.Should().OnlyContain(value => value > 0)); }

	[Fact]
	public void BadOnlyContainShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, -2, 3 }.Should().OnlyContain(value => value > 0),
							"[1, -2, 3] should only contain elements matching the predicate"
						);
	}

	[Fact]
	public void BadOnlyContainWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, -2, 3 }.Should().OnlyContain(value => value > 0, "custom because"), "custom because");
	}

	[Fact]
	public void BadOnlyContainOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().OnlyContain(value => value > 0), "null should only contain elements matching the predicate");
	}
}