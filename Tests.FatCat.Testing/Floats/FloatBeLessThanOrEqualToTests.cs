namespace Tests.FatCat.Testing.Floats;

public class FloatBeLessThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => 5.0f.Should().BeLessThanOrEqualTo(3.0f), "5 should be less than or equal to 3");
	}

	[Fact]
	public void BadBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 5.0f.Should().BeLessThanOrEqualTo(3.0f, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => 3.0f.Should().Not.BeLessThanOrEqualTo(3.0f), "3 should not be less than or equal to 3");
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 2.0f.Should().Not.BeLessThanOrEqualTo(3.0f, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLessThanOrEqualTo()
	{
		3.0f.Should().BeLessThanOrEqualTo(3.0f);
		2.0f.Should().BeLessThanOrEqualTo(3.0f);
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualTo()
	{
		5.0f.Should().Not.BeLessThanOrEqualTo(3.0f);
	}
}
