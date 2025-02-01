namespace Tests.FatCat.Testing;

public class NumberTests : BaseTest
{
	[Fact]
	public void BadBeInRange()
	{
		RunCompareFailTest(() => 2.Should().BeInRange(3, 5), "2 should be between 3 and 5");
		RunCompareFailTest(() => 6.Should().BeInRange(3, 5), "6 should be between 3 and 5");
	}

	[Fact]
	public void BadGreaterThan()
	{
		RunCompareFailTest(() => 2.Should().BeGreaterThan(3), "2 should be greater than 3");
	}

	[Fact]
	public void BadNotBeInRange()
	{
		RunCompareFailTest(() => 2.Should().Not.BeInRange(1, 7), "2 should not be between 1 and 7");
	}

	[Fact]
	public void BadNotEqual()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1));
	}

	[Fact]
	public void BasicFail()
	{
		RunCompareFailTest(() => 1.Should().Be(2));
	}

	[Fact]
	public void CompareMessageTest()
	{
		RunCompareFailTest(() => 1.Should().Be(2), "1 should be 2");
	}

	[Fact]
	public void GoodBeInRange()
	{
		2.Should().BeInRange(1, 7);
	}

	[Fact]
	public void GoodEqual()
	{
		1.Should().Be(1);
	}

	[Fact]
	public void GoodGreaterThan()
	{
		2.Should().BeGreaterThan(1);
	}

	[Fact]
	public void GoodNotBeInRange()
	{
		2.Should().Not.BeInRange(3, 5);
		6.Should().Not.BeInRange(3, 5);
	}

	[Fact]
	public void GoodNotEqual()
	{
		1.Should().Not.Be(2);
	}

	[Fact]
	public void NotCompareMessageTest()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1), "1 should not be 1");
	}

	[Fact]
	public void GoodLessThan()
	{
		1.Should().BeLessThan(2);
	}

	[Fact]
	public void BadLessThan()
	{
		RunCompareFailTest(() => 2.Should().BeLessThan(1), "2 should be less than 1");
	}
}
