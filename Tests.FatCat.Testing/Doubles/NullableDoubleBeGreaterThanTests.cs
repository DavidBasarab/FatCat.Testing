namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeGreaterThanTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThan()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeGreaterThan(2.5));
	}

	[Fact]
	public void BadBeGreaterThanNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().BeGreaterThan(2.5), "null should be greater than 2.5");
	}

	[Fact]
	public void BadBeGreaterThanShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeGreaterThan(2.5), "1.5 should be greater than 2.5");
	}

	[Fact]
	public void BadBeGreaterThanWhenEqual()
	{
		RunCompareFailTest(() => ((double?)2.5).Should().BeGreaterThan(2.5), "2.5 should be greater than 2.5");
	}

	[Fact]
	public void BadBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeGreaterThan(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThan()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().Not.BeGreaterThan(2.5));
	}

	[Fact]
	public void BadNotBeGreaterThanShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().Not.BeGreaterThan(2.5), "3.5 should not be greater than 2.5");
	}

	[Fact]
	public void BadNotBeGreaterThanWithBecause()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().Not.BeGreaterThan(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeGreaterThan()
	{
		((double?)3.5).Should().BeGreaterThan(2.5);
	}

	[Fact]
	public void GoodNotBeGreaterThan()
	{
		((double?)2.5).Should().Not.BeGreaterThan(2.5);
	}

	[Fact]
	public void GoodNotBeGreaterThanWhenNull()
	{
		((double?)null).Should().Not.BeGreaterThan(2.5);
	}
}
