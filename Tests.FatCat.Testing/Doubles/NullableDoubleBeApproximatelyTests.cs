namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeApproximatelyTests : BaseTest
{
	[Fact]
	public void BadBeApproximately()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeApproximately(2.0, 0.1));
	}

	[Fact]
	public void BadBeApproximatelyNullValue()
	{
		RunCompareFailTest(
			() => ((double?)null).Should().BeApproximately(2.0, 0.1),
			"null should be approximately 2 within 0.1"
		);
	}

	[Fact]
	public void BadBeApproximatelyShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeApproximately(2.0, 0.1), "1.5 should be approximately 2 within 0.1");
	}

	[Fact]
	public void BadBeApproximatelyWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeApproximately(2.0, 0.1, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeApproximately()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BeApproximately(1.5, 0.1));
	}

	[Fact]
	public void BadNotBeApproximatelyShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => ((double?)1.5).Should().Not.BeApproximately(1.5, 0.1),
			"1.5 should not be approximately 1.5 within 0.1"
		);
	}

	[Fact]
	public void BadNotBeApproximatelyWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BeApproximately(1.5, 0.1, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeApproximately()
	{
		((double?)1.5).Should().BeApproximately(1.55, 0.1);
	}

	[Fact]
	public void GoodNotBeApproximately()
	{
		((double?)1.5).Should().Not.BeApproximately(2.0, 0.1);
	}

	[Fact]
	public void GoodNotBeApproximatelyWhenNull()
	{
		((double?)null).Should().Not.BeApproximately(2.0, 0.1);
	}
}
