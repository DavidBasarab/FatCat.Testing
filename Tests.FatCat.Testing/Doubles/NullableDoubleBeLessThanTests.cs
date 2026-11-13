namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeLessThanTests : BaseTest
{
	[Fact]
	public void BadBeLessThan()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().BeLessThan(2.5));
	}

	[Fact]
	public void BadBeLessThanNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().BeLessThan(2.5), "null should be less than 2.5");
	}

	[Fact]
	public void BadBeLessThanShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().BeLessThan(2.5), "3.5 should be less than 2.5");
	}

	[Fact]
	public void BadBeLessThanWhenEqual()
	{
		RunCompareFailTest(() => ((double?)2.5).Should().BeLessThan(2.5), "2.5 should be less than 2.5");
	}

	[Fact]
	public void BadBeLessThanWithBecause()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().BeLessThan(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThan()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BeLessThan(2.5));
	}

	[Fact]
	public void BadNotBeLessThanShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BeLessThan(2.5), "1.5 should not be less than 2.5");
	}

	[Fact]
	public void BadNotBeLessThanWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BeLessThan(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLessThan()
	{
		((double?)1.5).Should().BeLessThan(2.5);
	}

	[Fact]
	public void GoodNotBeLessThan()
	{
		((double?)2.5).Should().Not.BeLessThan(2.5);
	}

	[Fact]
	public void GoodNotBeLessThanWhenNull()
	{
		((double?)null).Should().Not.BeLessThan(2.5);
	}
}
