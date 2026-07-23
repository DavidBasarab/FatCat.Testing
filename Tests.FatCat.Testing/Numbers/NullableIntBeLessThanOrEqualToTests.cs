namespace Tests.FatCat.Testing.Numbers;

public class NullableIntBeLessThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => ((int?)5).Should().BeLessThanOrEqualTo(3), "5 should be less than or equal to 3");
	}

	[Fact]
	public void BadBeLessThanOrEqualToNullValue()
	{
		RunCompareFailTest(() => ((int?)null).Should().BeLessThanOrEqualTo(3), "null should be less than or equal to 3");
	}

	[Fact]
	public void BadBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((int?)5).Should().BeLessThanOrEqualTo(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => ((int?)3).Should().Not.BeLessThanOrEqualTo(3), "3 should not be less than or equal to 3");
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((int?)2).Should().Not.BeLessThanOrEqualTo(3, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLessThanOrEqualTo()
	{
		((int?)3).Should().BeLessThanOrEqualTo(3);
		((int?)2).Should().BeLessThanOrEqualTo(3);
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualTo()
	{
		((int?)5).Should().Not.BeLessThanOrEqualTo(3);
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualToWhenNull()
	{
		((int?)null).Should().Not.BeLessThanOrEqualTo(3);
	}
}
