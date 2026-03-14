namespace Tests.FatCat.Testing.Numbers;

public class IntBeGreaterThanTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThan()
	{
		RunCompareFailTest(() => 2.Should().BeGreaterThan(3), "2 should be greater than 3");
	}

	[Fact]
	public void BadBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => 2.Should().BeGreaterThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThan()
	{
		RunCompareFailTest(() => 5.Should().Not.BeGreaterThan(3), "5 should not be greater than 3");
	}

	[Fact]
	public void BadNotBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => 5.Should().Not.BeGreaterThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeGreaterThan()
	{
		2.Should().BeGreaterThan(1);
	}

	[Fact]
	public void GoodNotBeGreaterThan()
	{
		2.Should().Not.BeGreaterThan(3);
		3.Should().Not.BeGreaterThan(3);
	}
}
