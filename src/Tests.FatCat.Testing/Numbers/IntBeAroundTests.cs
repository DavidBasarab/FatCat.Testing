namespace Tests.FatCat.Testing.Numbers;

public class IntBeAroundTests : BaseTest
{
	[Fact]
	public void BadBeAround()
	{
		RunCompareFailTest(() => 5.Should().BeAround(2, 2), "5 should be around 2 within 2");
	}

	[Fact]
	public void BadBeAroundWithBecause()
	{
		RunCompareFailTest(() => 5.Should().BeAround(2, 2, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeAround()
	{
		3.Should().BeAround(2, 2);
		2.Should().BeAround(2, 2);
		4.Should().BeAround(2, 2);
	}
}
