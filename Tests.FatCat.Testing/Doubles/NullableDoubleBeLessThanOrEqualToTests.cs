namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeLessThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().BeLessThanOrEqualTo(2.5));
	}

	[Fact]
	public void BadBeLessThanOrEqualToNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().BeLessThanOrEqualTo(2.5), "null should be less than or equal to 2.5");
	}

	[Fact]
	public void BadBeLessThanOrEqualToShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().BeLessThanOrEqualTo(2.5), "3.5 should be less than or equal to 2.5");
	}

	[Fact]
	public void BadBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((double?)3.5).Should().BeLessThanOrEqualTo(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLessThanOrEqualTo()
	{
		RunCompareFailTest(() => ((double?)2.5).Should().Not.BeLessThanOrEqualTo(2.5));
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => ((double?)2.5).Should().Not.BeLessThanOrEqualTo(2.5),
			"2.5 should not be less than or equal to 2.5"
		);
	}

	[Fact]
	public void BadNotBeLessThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((double?)2.5).Should().Not.BeLessThanOrEqualTo(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLessThanOrEqualTo()
	{
		((double?)1.5).Should().BeLessThanOrEqualTo(2.5);
	}

	[Fact]
	public void GoodBeLessThanOrEqualToWhenEqual()
	{
		((double?)2.5).Should().BeLessThanOrEqualTo(2.5);
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualTo()
	{
		((double?)3.5).Should().Not.BeLessThanOrEqualTo(2.5);
	}

	[Fact]
	public void GoodNotBeLessThanOrEqualToWhenNull()
	{
		((double?)null).Should().Not.BeLessThanOrEqualTo(2.5);
	}
}
