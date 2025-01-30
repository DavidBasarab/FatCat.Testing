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

	[Fact]
	public void GoodBeInRange()
	{
		2.Should().BeInRange(1, 7);
	}

	[Fact]
	public void BadBeInRange()
	{
		RunCompareFailTest(() => 2.Should().BeInRange(3, 5), "2 should be between 3 and 5");
		RunCompareFailTest(() => 6.Should().BeInRange(3, 5), "6 should be between 3 and 5");
	}
}
