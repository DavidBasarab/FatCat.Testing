namespace Tests.FatCat.Testing.Numbers;

public class NumericComparerTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => 3.0.Should().Be(5.0), "3 should be 5");
	}

	[Fact]
	public void BadBeAround()
	{
		RunCompareFailTest(() => 10.0.Should().BeAround(2.0, 2.0), "10 should be around 2 within 2");
	}

	[Fact]
	public void BadBeAroundWithBecause()
	{
		RunCompareFailTest(() => 10.0.Should().BeAround(2.0, 2.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeGreaterThan()
	{
		RunCompareFailTest(() => 1.0.Should().BeGreaterThan(5.0), "1 should be greater than 5");
	}

	[Fact]
	public void BadBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => 1.0.Should().BeGreaterThan(5.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeInRange()
	{
		RunCompareFailTest(() => 10.0.Should().BeInRange(1.0, 5.0), "10 should be between 1 and 5");
	}

	[Fact]
	public void BadBeInRangeWithBecause()
	{
		RunCompareFailTest(() => 10.0.Should().BeInRange(1.0, 5.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeLessThan()
	{
		RunCompareFailTest(() => 5.0.Should().BeLessThan(1.0), "5 should be less than 1");
	}

	[Fact]
	public void BadBeLessThanWithBecause()
	{
		RunCompareFailTest(() => 5.0.Should().BeLessThan(1.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeNegative()
	{
		RunCompareFailTest(() => 1.5.Should().BeNegative(), "1.5 should be negative");
	}

	[Fact]
	public void BadBeNegativeWithBecause()
	{
		RunCompareFailTest(() => 1.5.Should().BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void BadBePositive()
	{
		RunCompareFailTest(() => (-1.5).Should().BePositive(), "-1.5 should be positive");
	}

	[Fact]
	public void BadBePositiveWithBecause()
	{
		RunCompareFailTest(() => (-1.5).Should().BePositive("custom because"), "custom because");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => 3.0.Should().Be(5.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeZero()
	{
		RunCompareFailTest(() => 5.0.Should().BeZero(), "5 should be zero");
	}

	[Fact]
	public void BadBeZeroWithBecause()
	{
		RunCompareFailTest(() => 5.0.Should().BeZero("custom because"), "custom because");
	}

	[Fact]
	public void BadMatch()
	{
		RunCompareFailTest(() => 3.0.Should().Match(x => x > 5.0), "3 did not match the predicate");
	}

	[Fact]
	public void BadMatchWithBecause()
	{
		RunCompareFailTest(() => 3.0.Should().Match(x => x > 5.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => 3.0.Should().Not.Be(3.0), "3 should not be 3");
	}

	[Fact]
	public void BadNotBeGreaterThan()
	{
		RunCompareFailTest(() => 5.0.Should().Not.BeGreaterThan(3.0), "5 should not be greater than 3");
	}

	[Fact]
	public void BadNotBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => 5.0.Should().Not.BeGreaterThan(3.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeInRange()
	{
		RunCompareFailTest(() => 3.0.Should().Not.BeInRange(1.0, 5.0), "3 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeWithBecause()
	{
		RunCompareFailTest(() => 3.0.Should().Not.BeInRange(1.0, 5.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThan()
	{
		RunCompareFailTest(() => 1.0.Should().Not.BeLessThan(3.0), "1 should not be less than 3");
	}

	[Fact]
	public void BadNotBeLessThanWithBecause()
	{
		RunCompareFailTest(() => 1.0.Should().Not.BeLessThan(3.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNegative()
	{
		RunCompareFailTest(() => (-1.0).Should().Not.BeNegative(), "-1 should not be negative");
	}

	[Fact]
	public void BadNotBeNegativeWithBecause()
	{
		RunCompareFailTest(() => (-1.0).Should().Not.BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBePositive()
	{
		RunCompareFailTest(() => 1.0.Should().Not.BePositive(), "1 should not be positive");
	}

	[Fact]
	public void BadNotBePositiveWithBecause()
	{
		RunCompareFailTest(() => 1.0.Should().Not.BePositive("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => 3.0.Should().Not.Be(3.0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeZero()
	{
		RunCompareFailTest(() => 0.0.Should().Not.BeZero(), "0 should not be zero");
	}

	[Fact]
	public void BadNotBeZeroWithBecause()
	{
		RunCompareFailTest(() => 0.0.Should().Not.BeZero("custom because"), "custom because");
	}

	[Fact]
	public void BadNotMatch()
	{
		RunCompareFailTest(() => 4.0.Should().Not.Match(x => x > 3.0), "4 should not match the predicate");
	}

	[Fact]
	public void BadNotMatchWithBecause()
	{
		RunCompareFailTest(() => 4.0.Should().Not.Match(x => x > 3.0, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		3.0.Should().Be(3.0);
	}

	[Fact]
	public void GoodBeAround()
	{
		3.5.Should().BeAround(2.5, 2.0);
	}

	[Fact]
	public void GoodBeGreaterThan()
	{
		5.0.Should().BeGreaterThan(3.0);
	}

	[Fact]
	public void GoodBeInRange()
	{
		3.0.Should().BeInRange(1.0, 5.0);
	}

	[Fact]
	public void GoodBeLessThan()
	{
		1.0.Should().BeLessThan(5.0);
	}

	[Fact]
	public void GoodBeNegative()
	{
		(-1.5).Should().BeNegative();
	}

	[Fact]
	public void GoodBePositive()
	{
		1.5.Should().BePositive();
	}

	[Fact]
	public void GoodBeZero()
	{
		0.0.Should().BeZero();
	}

	[Fact]
	public void GoodMatch()
	{
		4.0.Should().Match(x => x > 3.0);
	}

	[Fact]
	public void GoodNotBe()
	{
		3.0.Should().Not.Be(5.0);
	}

	[Fact]
	public void GoodNotBeGreaterThan()
	{
		1.0.Should().Not.BeGreaterThan(5.0);
	}

	[Fact]
	public void GoodNotBeInRange()
	{
		10.0.Should().Not.BeInRange(1.0, 5.0);
	}

	[Fact]
	public void GoodNotBeLessThan()
	{
		5.0.Should().Not.BeLessThan(3.0);
	}

	[Fact]
	public void GoodNotBeNegative()
	{
		1.0.Should().Not.BeNegative();
	}

	[Fact]
	public void GoodNotBePositive()
	{
		(-1.0).Should().Not.BePositive();
	}

	[Fact]
	public void GoodNotBeZero()
	{
		1.0.Should().Not.BeZero();
	}

	[Fact]
	public void GoodNotMatch()
	{
		3.0.Should().Not.Match(x => x > 5.0);
	}
}
