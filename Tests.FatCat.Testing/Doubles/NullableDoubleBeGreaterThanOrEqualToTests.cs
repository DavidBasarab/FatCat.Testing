namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeGreaterThanOrEqualToTests : BaseTest
{
	[Fact]
	public void BadBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeGreaterThanOrEqualTo(2.5));
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToNullValue()
	{
		RunCompareFailTest(
			() => ((double?)null).Should().BeGreaterThanOrEqualTo(2.5),
			"null should be greater than or equal to 2.5"
		);
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => ((double?)1.5).Should().BeGreaterThanOrEqualTo(2.5),
			"1.5 should be greater than or equal to 2.5"
		);
	}

	[Fact]
	public void BadBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeGreaterThanOrEqualTo(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualTo()
	{
		RunCompareFailTest(() => ((double?)2.5).Should().Not.BeGreaterThanOrEqualTo(2.5));
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => ((double?)2.5).Should().Not.BeGreaterThanOrEqualTo(2.5),
			"2.5 should not be greater than or equal to 2.5"
		);
	}

	[Fact]
	public void BadNotBeGreaterThanOrEqualToWithBecause()
	{
		RunCompareFailTest(() => ((double?)2.5).Should().Not.BeGreaterThanOrEqualTo(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualTo()
	{
		((double?)3.5).Should().BeGreaterThanOrEqualTo(2.5);
	}

	[Fact]
	public void GoodBeGreaterThanOrEqualToWhenEqual()
	{
		((double?)2.5).Should().BeGreaterThanOrEqualTo(2.5);
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualTo()
	{
		((double?)1.5).Should().Not.BeGreaterThanOrEqualTo(2.5);
	}

	[Fact]
	public void GoodNotBeGreaterThanOrEqualToWhenNull()
	{
		((double?)null).Should().Not.BeGreaterThanOrEqualTo(2.5);
	}
}
