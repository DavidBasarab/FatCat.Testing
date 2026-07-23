namespace Tests.FatCat.Testing.Doubles;

public class DoubleBeGreaterThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => 2.0.Should().BeGreaterThanOrEqualTo(3.0), "2 should be greater than or equal to 3");
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 2.0.Should().BeGreaterThanOrEqualTo(3.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => 3.0.Should().Not.BeGreaterThanOrEqualTo(3.0), "3 should not be greater than or equal to 3");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 5.0.Should().Not.BeGreaterThanOrEqualTo(3.0, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualTo()
	{
		3.0.Should().BeGreaterThanOrEqualTo(3.0);
		4.0.Should().BeGreaterThanOrEqualTo(3.0);
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualTo()
	{
		2.0.Should().Not.BeGreaterThanOrEqualTo(3.0);
	}
}
