namespace Tests.FatCat.Testing.Doubles;

public class DoubleBeLessThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => 5.0.Should().BeLessThanOrEqualTo(3.0), "5 should be less than or equal to 3");
	}

	[Fact]
	public void BadBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 5.0.Should().BeLessThanOrEqualTo(3.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => 3.0.Should().Not.BeLessThanOrEqualTo(3.0), "3 should not be less than or equal to 3");
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 2.0.Should().Not.BeLessThanOrEqualTo(3.0, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLessThanOrEqualTo()
	{
		3.0.Should().BeLessThanOrEqualTo(3.0);
		2.0.Should().BeLessThanOrEqualTo(3.0);
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualTo()
	{
		5.0.Should().Not.BeLessThanOrEqualTo(3.0);
	}
}
