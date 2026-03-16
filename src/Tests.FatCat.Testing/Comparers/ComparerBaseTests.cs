namespace Tests.FatCat.Testing.Comparers;

public class ComparerBaseTests : BaseTest
{
	[Fact]
	public void BadBeAssignableToGeneric() { RunCompareFailTest(() => 3.Should().BeAssignableTo<string>(), "3 should be assignable to String"); }

	[Fact]
	public void BadBeAssignableToGenericWithBecause() { RunCompareFailTest(() => 3.Should().BeAssignableTo<string>("custom because"), "custom because"); }

	[Fact]
	public void BadBeAssignableToWithType() { RunCompareFailTest(() => 3.Should().BeAssignableTo(typeof(string)), "3 should be assignable to String"); }

	[Fact]
	public void BadBeAssignableToWithTypeWithBecause() { RunCompareFailTest(() => 3.Should().BeAssignableTo(typeof(string), "custom because"), "custom because"); }

	[Fact]
	public void BadBeOfTypeGeneric() { RunCompareFailTest(() => 3.Should().BeOfType<string>(), "3 should be of type String"); }

	[Fact]
	public void BadBeOfTypeGenericWithBecause() { RunCompareFailTest(() => 3.Should().BeOfType<string>("custom because"), "custom because"); }

	[Fact]
	public void BadBeOfTypeWithType() { RunCompareFailTest(() => 3.Should().BeOfType(typeof(string)), "3 should be of type String"); }

	[Fact]
	public void BadBeOfTypeWithTypeWithBecause() { RunCompareFailTest(() => 3.Should().BeOfType(typeof(string), "custom because"), "custom because"); }

	[Fact]
	public void BadBeOneOfList()
	{
		RunCompareFailTest(() => 3.Should()
							.BeOneOf(new List<int>
									{
										1,
										2,
										4
									}), "3 should be one of [1, 2, 4]");
	}

	[Fact]
	public void BadBeOneOfParams() { RunCompareFailTest(() => 3.Should().BeOneOf(1, 2, 4, 5), "3 should be one of [1, 2, 4, 5]"); }

	[Fact]
	public void BadBeOneOfWithBecause()
	{
		RunCompareFailTest(
							() => 3.Should()
							.BeOneOf(new List<int>
									{
										1,
										2,
										4
									}, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadSatisfy() { RunCompareFailTest(() => 3.Should().Satisfy(x => x.Should().BeLessThan(1)), "3 should be less than 1"); }

	[Fact]
	public void GoodBeAssignableToGeneric() { 3.Should().BeAssignableTo<IComparable>(); }

	[Fact]
	public void GoodBeAssignableToWithType() { 3.Should().BeAssignableTo(typeof(IComparable)); }

	[Fact]
	public void GoodBeOfTypeGeneric() { 3.Should().BeOfType<int>(); }

	[Fact]
	public void GoodBeOfTypeWithType() { 3.Should().BeOfType(typeof(int)); }

	[Fact]
	public void GoodBeOneOfList()
	{
		3.Should()
		.BeOneOf(new List<int>
				{
					2,
					3,
					4
				});
	}

	[Fact]
	public void GoodBeOneOfParams() { 3.Should().BeOneOf(2, 3, 4, 5); }

	[Fact]
	public void GoodSatisfy() { 3.Should().Satisfy(x => x.Should().BePositive()); }
}