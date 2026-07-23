namespace Tests.FatCat.Testing.Floats;

public class FloatBeGreaterThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => 2.0f.Should().BeGreaterThanOrEqualTo(3.0f), "2 should be greater than or equal to 3");
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 2.0f.Should().BeGreaterThanOrEqualTo(3.0f, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => 3.0f.Should().Not.BeGreaterThanOrEqualTo(3.0f), "3 should not be greater than or equal to 3");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => 5.0f.Should().Not.BeGreaterThanOrEqualTo(3.0f, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualTo()
	{
		3.0f.Should().BeGreaterThanOrEqualTo(3.0f);
		4.0f.Should().BeGreaterThanOrEqualTo(3.0f);
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualTo()
	{
		2.0f.Should().Not.BeGreaterThanOrEqualTo(3.0f);
	}
}
