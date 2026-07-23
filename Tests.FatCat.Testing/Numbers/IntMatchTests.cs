namespace Tests.FatCat.Testing.Numbers;

public class IntMatchTests : BaseTest
{
	[Fact]
	public void BadMatch()
	{
		RunCompareFailTest(() => 3.Should().Match(x => x % 2 == 0), "3 did not match the predicate");
	}

	[Fact]
	public void BadMatchWithBecause()
	{
		RunCompareFailTest(() => 3.Should().Match(x => x % 2 == 0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotMatch()
	{
		RunCompareFailTest(() => 4.Should().Not.Match(x => x % 2 == 0), "4 should not match the predicate");
	}

	[Fact]
	public void BadNotMatchWithBecause()
	{
		RunCompareFailTest(() => 4.Should().Not.Match(x => x % 2 == 0, "custom because"), "custom because");
	}

	[Fact]
	public void GoodMatch()
	{
		4.Should().Match(x => x % 2 == 0);
	}

	[Fact]
	public void GoodNotMatch()
	{
		3.Should().Not.Match(x => x % 2 == 0);
	}
}
