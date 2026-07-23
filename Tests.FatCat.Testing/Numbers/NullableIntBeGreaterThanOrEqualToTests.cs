namespace Tests.FatCat.Testing.Numbers;

public class NullableIntBeGreaterThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => ((int?)2).Should().BeGreaterThanOrEqualTo(3), "2 should be greater than or equal to 3");
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToNullValue()
	{
		RunCompareFailTest(() => ((int?)null).Should().BeGreaterThanOrEqualTo(3), "null should be greater than or equal to 3");
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((int?)2).Should().BeGreaterThanOrEqualTo(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(
			() => ((int?)3).Should().Not.BeGreaterThanOrEqualTo(3),
			"3 should not be greater than or equal to 3"
		);
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((int?)5).Should().Not.BeGreaterThanOrEqualTo(3, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualTo()
	{
		((int?)3).Should().BeGreaterThanOrEqualTo(3);
		((int?)4).Should().BeGreaterThanOrEqualTo(3);
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualTo()
	{
		((int?)2).Should().Not.BeGreaterThanOrEqualTo(3);
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualToWhenNull()
	{
		((int?)null).Should().Not.BeGreaterThanOrEqualTo(3);
	}
}
