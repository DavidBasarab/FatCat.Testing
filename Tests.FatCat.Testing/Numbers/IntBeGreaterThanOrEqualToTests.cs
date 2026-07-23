namespace Tests.FatCat.Testing.Numbers;

public class IntBeGreaterThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => 2.Should().BeGreaterThanOrEqualTo(3), "2 should be greater than or equal to 3");
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 2.Should().BeGreaterThanOrEqualTo(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => 3.Should().Not.BeGreaterThanOrEqualTo(3), "3 should not be greater than or equal to 3");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 5.Should().Not.BeGreaterThanOrEqualTo(3, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualTo()
	{
		3.Should().BeGreaterThanOrEqualTo(3);
		4.Should().BeGreaterThanOrEqualTo(3);
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualTo()
	{
		2.Should().Not.BeGreaterThanOrEqualTo(3);
	}
}
