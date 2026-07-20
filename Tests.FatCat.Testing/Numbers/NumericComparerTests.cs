namespace Tests.FatCat.Testing.Numbers;

public class NumericComparerTests : BaseTest
{
	[Fact]
	public void BadBe() { RunCompareFailTest(() => 3m.Should().Be(5m), "3 should be 5"); }

	[Fact]
	public void BadBeAround() { RunCompareFailTest(() => 10m.Should().BeAround(2m, 2m), "10 should be around 2 within 2"); }

	[Fact]
	public void BadBeAroundWithBecause() { RunCompareFailTest(() => 10m.Should().BeAround(2m, 2m, "custom because"), "custom because"); }

	[Fact]
	public void BadBeGreaterThan() { RunCompareFailTest(() => 1m.Should().BeGreaterThan(5m), "1 should be greater than 5"); }

	[Fact]
	public void BadBeGreaterThanWithBecause() { RunCompareFailTest(() => 1m.Should().BeGreaterThan(5m, "custom because"), "custom because"); }

	[Fact]
	public void BadBeInRange() { RunCompareFailTest(() => 10m.Should().BeInRange(1m, 5m), "10 should be between 1 and 5"); }

	[Fact]
	public void BadBeInRangeWithBecause() { RunCompareFailTest(() => 10m.Should().BeInRange(1m, 5m, "custom because"), "custom because"); }

	[Fact]
	public void BadBeLessThan() { RunCompareFailTest(() => 5m.Should().BeLessThan(1m), "5 should be less than 1"); }

	[Fact]
	public void BadBeLessThanWithBecause() { RunCompareFailTest(() => 5m.Should().BeLessThan(1m, "custom because"), "custom because"); }

	[Fact]
	public void BadBeNegative() { RunCompareFailTest(() => 1.5m.Should().BeNegative(), "1.5 should be negative"); }

	[Fact]
	public void BadBeNegativeWithBecause() { RunCompareFailTest(() => 1.5m.Should().BeNegative("custom because"), "custom because"); }

	[Fact]
	public void BadBePositive() { RunCompareFailTest(() => (-1.5m).Should().BePositive(), "-1.5 should be positive"); }

	[Fact]
	public void BadBePositiveWithBecause() { RunCompareFailTest(() => (-1.5m).Should().BePositive("custom because"), "custom because"); }

	[Fact]
	public void BadBeWithBecause() { RunCompareFailTest(() => 3m.Should().Be(5m, "custom because"), "custom because"); }

	[Fact]
	public void BadBeZero() { RunCompareFailTest(() => 5m.Should().BeZero(), "5 should be zero"); }

	[Fact]
	public void BadBeZeroWithBecause() { RunCompareFailTest(() => 5m.Should().BeZero("custom because"), "custom because"); }

	[Fact]
	public void BadMatch() { RunCompareFailTest(() => 3m.Should().Match(x => x > 5m), "3 did not match the predicate"); }

	[Fact]
	public void BadMatchWithBecause() { RunCompareFailTest(() => 3m.Should().Match(x => x > 5m, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBe() { RunCompareFailTest(() => 3m.Should().Not.Be(3m), "3 should not be 3"); }

	[Fact]
	public void BadNotBeGreaterThan() { RunCompareFailTest(() => 5m.Should().Not.BeGreaterThan(3m), "5 should not be greater than 3"); }

	[Fact]
	public void BadNotBeGreaterThanWithBecause() { RunCompareFailTest(() => 5m.Should().Not.BeGreaterThan(3m, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeInRange() { RunCompareFailTest(() => 3m.Should().Not.BeInRange(1m, 5m), "3 should not be between 1 and 5"); }

	[Fact]
	public void BadNotBeInRangeWithBecause() { RunCompareFailTest(() => 3m.Should().Not.BeInRange(1m, 5m, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeLessThan() { RunCompareFailTest(() => 1m.Should().Not.BeLessThan(3m), "1 should not be less than 3"); }

	[Fact]
	public void BadNotBeLessThanWithBecause() { RunCompareFailTest(() => 1m.Should().Not.BeLessThan(3m, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeNegative() { RunCompareFailTest(() => (-1m).Should().Not.BeNegative(), "-1 should not be negative"); }

	[Fact]
	public void BadNotBeNegativeWithBecause() { RunCompareFailTest(() => (-1m).Should().Not.BeNegative("custom because"), "custom because"); }

	[Fact]
	public void BadNotBePositive() { RunCompareFailTest(() => 1m.Should().Not.BePositive(), "1 should not be positive"); }

	[Fact]
	public void BadNotBePositiveWithBecause() { RunCompareFailTest(() => 1m.Should().Not.BePositive("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeWithBecause() { RunCompareFailTest(() => 3m.Should().Not.Be(3m, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeZero() { RunCompareFailTest(() => 0m.Should().Not.BeZero(), "0 should not be zero"); }

	[Fact]
	public void BadNotBeZeroWithBecause() { RunCompareFailTest(() => 0m.Should().Not.BeZero("custom because"), "custom because"); }

	[Fact]
	public void BadNotMatch() { RunCompareFailTest(() => 4m.Should().Not.Match(x => x > 3m), "4 should not match the predicate"); }

	[Fact]
	public void BadNotMatchWithBecause() { RunCompareFailTest(() => 4m.Should().Not.Match(x => x > 3m, "custom because"), "custom because"); }

	[Fact]
	public void GoodBe() { 3m.Should().Be(3m); }

	[Fact]
	public void GoodBeAround() { 3.5m.Should().BeAround(2.5m, 2m); }

	[Fact]
	public void GoodBeGreaterThan() { 5m.Should().BeGreaterThan(3m); }

	[Fact]
	public void GoodBeInRange() { 3m.Should().BeInRange(1m, 5m); }

	[Fact]
	public void GoodBeLessThan() { 1m.Should().BeLessThan(5m); }

	[Fact]
	public void GoodBeNegative() { (-1.5m).Should().BeNegative(); }

	[Fact]
	public void GoodBePositive() { 1.5m.Should().BePositive(); }

	[Fact]
	public void GoodBeZero() { 0m.Should().BeZero(); }

	[Fact]
	public void GoodMatch() { 4m.Should().Match(x => x > 3m); }

	[Fact]
	public void GoodNotBe() { 3m.Should().Not.Be(5m); }

	[Fact]
	public void GoodNotBeGreaterThan() { 1m.Should().Not.BeGreaterThan(5m); }

	[Fact]
	public void GoodNotBeInRange() { 10m.Should().Not.BeInRange(1m, 5m); }

	[Fact]
	public void GoodNotBeLessThan() { 5m.Should().Not.BeLessThan(3m); }

	[Fact]
	public void GoodNotBeNegative() { 1m.Should().Not.BeNegative(); }

	[Fact]
	public void GoodNotBePositive() { (-1m).Should().Not.BePositive(); }

	[Fact]
	public void GoodNotBeZero() { 1m.Should().Not.BeZero(); }

	[Fact]
	public void GoodNotMatch() { 3m.Should().Not.Match(x => x > 5m); }
}