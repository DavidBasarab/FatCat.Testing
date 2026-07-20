namespace Tests.FatCat.Testing.Comparers;

public class NotComparerBaseTests : BaseTest
{
	[Fact]
	public void BadNotBeAssignableToGeneric()
	{
		RunCompareFailTest(
							() => 3.Should().Not.BeAssignableTo<IComparable>(),
							"3 should not be assignable to IComparable"
						);
	}

	[Fact]
	public void BadNotBeAssignableToGenericWithBecause() { RunCompareFailTest(() => 3.Should().Not.BeAssignableTo<IComparable>("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeAssignableToWithType()
	{
		RunCompareFailTest(
							() => 3.Should().Not.BeAssignableTo(typeof(IComparable)),
							"3 should not be assignable to IComparable"
						);
	}

	[Fact]
	public void BadNotBeAssignableToWithTypeWithBecause()
	{
		RunCompareFailTest(
							() => 3.Should().Not.BeAssignableTo(typeof(IComparable), "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeOfTypeGeneric() { RunCompareFailTest(() => 3.Should().Not.BeOfType<int>(), "3 should not be of type Int32"); }

	[Fact]
	public void BadNotBeOfTypeGenericWithBecause() { RunCompareFailTest(() => 3.Should().Not.BeOfType<int>("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeOfTypeWithType() { RunCompareFailTest(() => 3.Should().Not.BeOfType(typeof(int)), "3 should not be of type Int32"); }

	[Fact]
	public void BadNotBeOfTypeWithTypeWithBecause() { RunCompareFailTest(() => 3.Should().Not.BeOfType(typeof(int), "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeOneOfList()
	{
		RunCompareFailTest(
							() => 3.Should()
							.Not.BeOneOf(new List<int>
										{
											2,
											3,
											4
										}),
							"3 should not be one of [2, 3, 4]"
						);
	}

	[Fact]
	public void BadNotBeOneOfParams() { RunCompareFailTest(() => 3.Should().Not.BeOneOf(2, 3, 4, 5), "3 should not be one of [2, 3, 4, 5]"); }

	[Fact]
	public void BadNotBeOneOfWithBecause()
	{
		RunCompareFailTest(
							() => 3.Should()
							.Not.BeOneOf(new List<int>
										{
											2,
											3,
											4
										}, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodNotBeAssignableToGeneric() { 3.Should().Not.BeAssignableTo<string>(); }

	[Fact]
	public void GoodNotBeAssignableToWithType() { 3.Should().Not.BeAssignableTo(typeof(string)); }

	[Fact]
	public void GoodNotBeOfTypeGeneric() { 3.Should().Not.BeOfType<string>(); }

	[Fact]
	public void GoodNotBeOfTypeWithType() { 3.Should().Not.BeOfType(typeof(string)); }

	[Fact]
	public void GoodNotBeOneOfList()
	{
		3.Should()
		.Not.BeOneOf(new List<int>
					{
						1,
						2,
						4
					});
	}

	[Fact]
	public void GoodNotBeOneOfParams() { 3.Should().Not.BeOneOf(1, 2, 4, 5); }
}