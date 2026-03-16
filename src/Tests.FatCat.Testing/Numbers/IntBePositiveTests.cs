namespace Tests.FatCat.Testing.Numbers;

public class IntBePositiveTests : BaseTest
{
	[Fact]
	public void BadBePositive()
	{
		RunCompareFailTest(() => (-1).Should().BePositive(), "-1 should be positive");
		RunCompareFailTest(() => 0.Should().BePositive(), "0 should be positive");
	}

	[Fact]
	public void BadBePositiveWithBecause()
	{
		RunCompareFailTest(() => (-1).Should().BePositive("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBePositive()
	{
		RunCompareFailTest(() => 1.Should().Not.BePositive(), "1 should not be positive");
	}

	[Fact]
	public void BadNotBePositiveWithBecause()
	{
		RunCompareFailTest(() => 1.Should().Not.BePositive("custom because"), "custom because");
	}

	[Fact]
	public void GoodBePositive()
	{
		1.Should().BePositive();
		100.Should().BePositive();
	}

	[Fact]
	public void GoodNotBePositive()
	{
		0.Should().Not.BePositive();
		(-1).Should().Not.BePositive();
	}
}
