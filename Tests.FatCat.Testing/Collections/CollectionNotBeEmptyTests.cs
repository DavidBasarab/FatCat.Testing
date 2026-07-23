namespace Tests.FatCat.Testing.Collections;

public class CollectionNotBeEmptyTests : BaseTest
{
	[Fact]
	public void GoodNotBeEmpty()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.Not.BeEmpty();
	}

	[Fact]
	public void BadNotBeEmpty()
	{
		RunCompareFailTest(() => new List<int>().Should().Not.BeEmpty());
	}

	[Fact]
	public void BadNotBeEmptyShowsCorrectMessage()
	{
		RunCompareFailTest(() => new List<int>().Should().Not.BeEmpty(), "[] should not be empty");
	}

	[Fact]
	public void BadNotBeEmptyWithBecause()
	{
		RunCompareFailTest(() => new List<int>().Should().Not.BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeEmptyOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().Not.BeEmpty(), "null should not be empty");
	}
}
