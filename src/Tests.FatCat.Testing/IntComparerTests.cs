namespace Tests.FatCat.Testing;

public class IntComparerTests : BaseTest
{
	[Fact]
	public void BadBeAround()
	{
		RunCompareFailTest(() => 5.Should().BeAround(2, 2), "5 should be around 2 within 2");
	}

	[Fact]
	public void Playing()
	{
		1.Should().Be(3);
	}

	[Fact]
	public void BadBeAroundWithBecause()
	{
		RunCompareFailTest(() => 5.Should().BeAround(2, 2, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeInRange()
	{
		RunCompareFailTest(() => 2.Should().BeInRange(3, 5), "2 should be between 3 and 5");
		RunCompareFailTest(() => 6.Should().BeInRange(3, 5), "6 should be between 3 and 5");
	}

	[Fact]
	public void BadBeInRangeWithBecause()
	{
		RunCompareFailTest(() => 2.Should().BeInRange(3, 5, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeNegative()
	{
		RunCompareFailTest(() => 1.Should().BeNegative(), "1 should be negative");
		RunCompareFailTest(() => 0.Should().BeNegative(), "0 should be negative");
	}

	[Fact]
	public void BadBeNegativeWithBecause()
	{
		RunCompareFailTest(() => 1.Should().BeNegative("custom because"), "custom because");
	}

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
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => 1.Should().Be(2, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeZero()
	{
		RunCompareFailTest(() => 5.Should().BeZero(), "5 should be zero");
	}

	[Fact]
	public void BadBeZeroWithBecause()
	{
		RunCompareFailTest(() => 5.Should().BeZero("custom because"), "custom because");
	}

	[Fact]
	public void BadGreaterThan()
	{
		RunCompareFailTest(() => 2.Should().BeGreaterThan(3), "2 should be greater than 3");
	}

	[Fact]
	public void BadGreaterThanWithBecause()
	{
		RunCompareFailTest(() => 2.Should().BeGreaterThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadLessThan()
	{
		RunCompareFailTest(() => 2.Should().BeLessThan(1), "2 should be less than 1");
	}

	[Fact]
	public void BadLessThanWithBecause()
	{
		RunCompareFailTest(() => 2.Should().BeLessThan(1, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThan()
	{
		RunCompareFailTest(() => 5.Should().Not.BeGreaterThan(3), "5 should not be greater than 3");
	}

	[Fact]
	public void BadNotBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => 5.Should().Not.BeGreaterThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeInRange()
	{
		RunCompareFailTest(() => 2.Should().Not.BeInRange(1, 7), "2 should not be between 1 and 7");
	}

	[Fact]
	public void BadNotBeInRangeWithBecause()
	{
		RunCompareFailTest(() => 2.Should().Not.BeInRange(1, 7, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThan()
	{
		RunCompareFailTest(() => 1.Should().Not.BeLessThan(3), "1 should not be less than 3");
	}

	[Fact]
	public void BadNotBeLessThanWithBecause()
	{
		RunCompareFailTest(() => 1.Should().Not.BeLessThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNegative()
	{
		RunCompareFailTest(() => (-1).Should().Not.BeNegative(), "-1 should not be negative");
	}

	[Fact]
	public void BadNotBeNegativeWithBecause()
	{
		RunCompareFailTest(() => (-1).Should().Not.BeNegative("custom because"), "custom because");
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
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeZero()
	{
		RunCompareFailTest(() => 0.Should().Not.BeZero(), "0 should not be zero");
	}

	[Fact]
	public void BadNotBeZeroWithBecause()
	{
		RunCompareFailTest(() => 0.Should().Not.BeZero("custom because"), "custom because");
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
	public void GoodBeAround()
	{
		3.Should().BeAround(2, 2);
		2.Should().BeAround(2, 2);
		4.Should().BeAround(2, 2);
	}

	[Fact]
	public void GoodBeInRange()
	{
		2.Should().BeInRange(1, 7);
	}

	[Fact]
	public void GoodBeNegative()
	{
		(-1).Should().BeNegative();
		(-100).Should().BeNegative();
	}

	[Fact]
	public void GoodBePositive()
	{
		1.Should().BePositive();
		100.Should().BePositive();
	}

	[Fact]
	public void GoodBeZero()
	{
		0.Should().BeZero();
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
	public void GoodLessThan()
	{
		1.Should().BeLessThan(2);
	}

	[Fact]
	public void GoodNotBeGreaterThan()
	{
		2.Should().Not.BeGreaterThan(3);
		3.Should().Not.BeGreaterThan(3);
	}

	[Fact]
	public void GoodNotBeInRange()
	{
		2.Should().Not.BeInRange(3, 5);
		6.Should().Not.BeInRange(3, 5);
	}

	[Fact]
	public void GoodNotBeLessThan()
	{
		3.Should().Not.BeLessThan(3);
		5.Should().Not.BeLessThan(3);
	}

	[Fact]
	public void GoodNotBeNegative()
	{
		0.Should().Not.BeNegative();
		1.Should().Not.BeNegative();
	}

	[Fact]
	public void GoodNotBePositive()
	{
		0.Should().Not.BePositive();
		(-1).Should().Not.BePositive();
	}

	[Fact]
	public void GoodNotBeZero()
	{
		1.Should().Not.BeZero();
		(-1).Should().Not.BeZero();
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
}
