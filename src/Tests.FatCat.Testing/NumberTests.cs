namespace Tests.FatCat.Testing;

public class NumberTests : BaseTest
{
	[Fact]
	public void GoodEqual()
	{
		1.Should().Be(1);
	}

	[Fact]
	public void BasicFail()
	{
		RunCompareFailTest(() => 1.Should().Be(2));
	}

	[Fact]
	public void GoodNotEqual()
	{
		1.Should().Not.Be(2);
	}

	[Fact]
	public void BadNotEqual()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1));
	}

	[Fact]
	public void CompareMessageTest()
	{
		RunCompareFailTest(() => 1.Should().Be(2), "1 should be 2");
	}

	[Fact]
	public void NotCompareMessageTest()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1), "1 should not be 1");
	}
}
