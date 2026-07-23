namespace Tests.FatCat.Testing.Collections;

public class CollectionContainEquivalentOfTests : BaseTest
{
	[Fact]
	public void GoodContainEquivalentOf() { new List<int> { 1, 2, 3 }.Should().ContainEquivalentOf(2); }

	[Fact]
	public void GoodContainEquivalentOfStructuralElement()
	{
		var subject = new List<Gadget> { new() { Name = "wrench", Weight = 2 }, new() { Name = "hammer", Weight = 5 } };

		subject.Should().ContainEquivalentOf(new Gadget { Name = "hammer", Weight = 5 });
	}

	[Fact]
	public void BadContainEquivalentOf() { RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().ContainEquivalentOf(5)); }

	[Fact]
	public void BadContainEquivalentOfShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 2, 3 }.Should().ContainEquivalentOf(5),
							"[1, 2, 3] should contain an element equivalent to 5"
						);
	}

	[Fact]
	public void BadContainEquivalentOfWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().ContainEquivalentOf(5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotContainEquivalentOf() { new List<int> { 1, 2, 3 }.Should().Not.ContainEquivalentOf(5); }

	[Fact]
	public void BadNotContainEquivalentOf()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().Not.ContainEquivalentOf(2));
	}

	[Fact]
	public void BadNotContainEquivalentOfShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<int> { 1, 2, 3 }.Should().Not.ContainEquivalentOf(2),
							"[1, 2, 3] should not contain an element equivalent to 2"
						);
	}

	[Fact]
	public void BadNotContainEquivalentOfWithBecause()
	{
		RunCompareFailTest(() => new List<int> { 1, 2, 3 }.Should().Not.ContainEquivalentOf(2, "custom because"), "custom because");
	}

	[Fact]
	public void BadContainEquivalentOfOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().ContainEquivalentOf(5), "null should contain an element equivalent to 5");
	}
}