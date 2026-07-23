namespace Tests.FatCat.Testing.Numbers;

public class IntBeLessThanTests : BaseTest
{
	[Fact]
	public void BadBeLessThan()
	{
		RunCompareFailTest(() => 2.Should().BeLessThan(1), "2 should be less than 1");
	}

	[Fact]
	public void BadBeLessThanWithBecause()
	{
		RunCompareFailTest(() => 2.Should().BeLessThan(1, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThan()
	{
		RunCompareFailTest(() => 1.Should().Not.BeLessThan(3), "1 should not be less than 3");
	}

	[Fact]
	public void BadNotBeLessThanWithBecause()
	{
		RunCompareFailTest(() => 1.Should().Not.BeLessThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLessThan()
	{
		1.Should().BeLessThan(2);
	}

	[Fact]
	public void GoodNotBeLessThan()
	{
		3.Should().Not.BeLessThan(3);
		5.Should().Not.BeLessThan(3);
	}
}
