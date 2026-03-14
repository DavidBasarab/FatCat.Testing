namespace Tests.FatCat.Testing;

public class NullableIntComparerTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => ((int?)3).Should().Be(5), "3 should be 5");
	}

	[Fact]
	public void BadBeAround()
	{
		RunCompareFailTest(() => ((int?)10).Should().BeAround(2, 2), "10 should be around 2 within 2");
	}

	[Fact]
	public void BadBeAroundWithBecause()
	{
		RunCompareFailTest(() => ((int?)10).Should().BeAround(2, 2, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeGreaterThan()
	{
		RunCompareFailTest(() => ((int?)1).Should().BeGreaterThan(5), "1 should be greater than 5");
	}

	[Fact]
	public void BadBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => ((int?)1).Should().BeGreaterThan(5, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeInRange()
	{
		RunCompareFailTest(() => ((int?)10).Should().BeInRange(1, 5), "10 should be between 1 and 5");
	}

	[Fact]
	public void BadBeInRangeWithBecause()
	{
		RunCompareFailTest(() => ((int?)10).Should().BeInRange(1, 5, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeLessThan()
	{
		RunCompareFailTest(() => ((int?)5).Should().BeLessThan(1), "5 should be less than 1");
	}

	[Fact]
	public void BadBeLessThanWithBecause()
	{
		RunCompareFailTest(() => ((int?)5).Should().BeLessThan(1, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeNegative()
	{
		RunCompareFailTest(() => ((int?)1).Should().BeNegative(), "1 should be negative");
	}

	[Fact]
	public void BadBeNegativeNullValue()
	{
		RunCompareFailTest(() => ((int?)null).Should().BeNegative(), "null should be negative");
	}

	[Fact]
	public void BadBeNegativeWithBecause()
	{
		RunCompareFailTest(() => ((int?)1).Should().BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(() => ((int?)3).Should().BeNull(), "3 should be null");
	}

	[Fact]
	public void BadBeNullValue()
	{
		RunCompareFailTest(() => ((int?)null).Should().Be(5), "null should be 5");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(() => ((int?)3).Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadBePositive()
	{
		RunCompareFailTest(() => ((int?)-1).Should().BePositive(), "-1 should be positive");
	}

	[Fact]
	public void BadBePositiveNullValue()
	{
		RunCompareFailTest(() => ((int?)null).Should().BePositive(), "null should be positive");
	}

	[Fact]
	public void BadBePositiveWithBecause()
	{
		RunCompareFailTest(() => ((int?)-1).Should().BePositive("custom because"), "custom because");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => ((int?)3).Should().Be(5, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeZero()
	{
		RunCompareFailTest(() => ((int?)5).Should().BeZero(), "5 should be zero");
	}

	[Fact]
	public void BadBeZeroNullValue()
	{
		RunCompareFailTest(() => ((int?)null).Should().BeZero(), "null should be zero");
	}

	[Fact]
	public void BadBeZeroWithBecause()
	{
		RunCompareFailTest(() => ((int?)5).Should().BeZero("custom because"), "custom because");
	}

	[Fact]
	public void BadHaveValue()
	{
		RunCompareFailTest(() => ((int?)null).Should().HaveValue(), "value should not be null");
	}

	[Fact]
	public void BadHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((int?)null).Should().HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void BadMatch()
	{
		RunCompareFailTest(() => ((int?)3).Should().Match(x => x % 2 == 0), "3 did not match the predicate");
	}

	[Fact]
	public void BadMatchWithBecause()
	{
		RunCompareFailTest(() => ((int?)3).Should().Match(x => x % 2 == 0, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => ((int?)3).Should().Not.Be(3), "3 should not be 3");
	}

	[Fact]
	public void BadNotBeGreaterThan()
	{
		RunCompareFailTest(() => ((int?)5).Should().Not.BeGreaterThan(3), "5 should not be greater than 3");
	}

	[Fact]
	public void BadNotBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => ((int?)5).Should().Not.BeGreaterThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeInRange()
	{
		RunCompareFailTest(() => ((int?)3).Should().Not.BeInRange(1, 5), "3 should not be between 1 and 5");
	}

	[Fact]
	public void BadNotBeInRangeWithBecause()
	{
		RunCompareFailTest(() => ((int?)3).Should().Not.BeInRange(1, 5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThan()
	{
		RunCompareFailTest(() => ((int?)1).Should().Not.BeLessThan(3), "1 should not be less than 3");
	}

	[Fact]
	public void BadNotBeLessThanWithBecause()
	{
		RunCompareFailTest(() => ((int?)1).Should().Not.BeLessThan(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNegative()
	{
		RunCompareFailTest(() => ((int?)-1).Should().Not.BeNegative(), "-1 should not be negative");
	}

	[Fact]
	public void BadNotBeNegativeWithBecause()
	{
		RunCompareFailTest(() => ((int?)-1).Should().Not.BeNegative("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBePositive()
	{
		RunCompareFailTest(() => ((int?)1).Should().Not.BePositive(), "1 should not be positive");
	}

	[Fact]
	public void BadNotBePositiveWithBecause()
	{
		RunCompareFailTest(() => ((int?)1).Should().Not.BePositive("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => ((int?)3).Should().Not.Be(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeZero()
	{
		RunCompareFailTest(() => ((int?)0).Should().Not.BeZero(), "0 should not be zero");
	}

	[Fact]
	public void BadNotBeZeroWithBecause()
	{
		RunCompareFailTest(() => ((int?)0).Should().Not.BeZero("custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveValue()
	{
		RunCompareFailTest(() => ((int?)3).Should().Not.HaveValue(), "3 should not have a value");
	}

	[Fact]
	public void BadNotHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((int?)3).Should().Not.HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void BadNotMatch()
	{
		RunCompareFailTest(
			() => ((int?)4).Should().Not.Match(x => x % 2 == 0),
			"4 should not match the predicate"
		);
	}

	[Fact]
	public void BadNotMatchWithBecause()
	{
		RunCompareFailTest(
			() => ((int?)4).Should().Not.Match(x => x % 2 == 0, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBe()
	{
		((int?)3).Should().Be(3);
	}

	[Fact]
	public void GoodBeAround()
	{
		((int?)3).Should().BeAround(2, 2);
	}

	[Fact]
	public void GoodBeGreaterThan()
	{
		((int?)5).Should().BeGreaterThan(3);
	}

	[Fact]
	public void GoodBeInRange()
	{
		((int?)3).Should().BeInRange(1, 5);
	}

	[Fact]
	public void GoodBeLessThan()
	{
		((int?)1).Should().BeLessThan(5);
	}

	[Fact]
	public void GoodBeNegative()
	{
		((int?)-1).Should().BeNegative();
	}

	[Fact]
	public void GoodBeNull()
	{
		((int?)null).Should().BeNull();
	}

	[Fact]
	public void GoodBePositive()
	{
		((int?)3).Should().BePositive();
	}

	[Fact]
	public void GoodBeZero()
	{
		((int?)0).Should().BeZero();
	}

	[Fact]
	public void GoodHaveValue()
	{
		((int?)3).Should().HaveValue();
	}

	[Fact]
	public void GoodMatch()
	{
		((int?)4).Should().Match(x => x % 2 == 0);
	}

	[Fact]
	public void GoodNotBe()
	{
		((int?)3).Should().Not.Be(5);
	}

	[Fact]
	public void GoodNotBeGreaterThan()
	{
		((int?)1).Should().Not.BeGreaterThan(5);
	}

	[Fact]
	public void GoodNotBeInRange()
	{
		((int?)10).Should().Not.BeInRange(1, 5);
	}

	[Fact]
	public void GoodNotBeLessThan()
	{
		((int?)5).Should().Not.BeLessThan(3);
	}

	[Fact]
	public void GoodNotBeNegative()
	{
		((int?)1).Should().Not.BeNegative();
	}

	[Fact]
	public void GoodNotBePositive()
	{
		((int?)-1).Should().Not.BePositive();
	}

	[Fact]
	public void GoodNotBeZero()
	{
		((int?)1).Should().Not.BeZero();
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((int?)null).Should().Not.HaveValue();
	}

	[Fact]
	public void GoodNotMatch()
	{
		((int?)3).Should().Not.Match(x => x % 2 == 0);
	}
}
