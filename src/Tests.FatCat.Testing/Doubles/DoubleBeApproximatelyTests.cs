namespace Tests.FatCat.Testing.Doubles;

public class DoubleBeApproximatelyTests : BaseTest
{
	[Fact]
	public void BadBeApproximately() { RunCompareFailTest(() => 1.5.Should().BeApproximately(2.0, 0.1)); }

	[Fact]
	public void BadBeApproximatelyShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => 1.5.Should().BeApproximately(2.0, 0.1),
							"1.5 should be approximately 2 within 0.1"
						);
	}

	[Fact]
	public void BadBeApproximatelyWithBecause() { RunCompareFailTest(() => 1.5.Should().BeApproximately(2.0, 0.1, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeApproximately() { RunCompareFailTest(() => 1.5.Should().Not.BeApproximately(1.5, 0.1)); }

	[Fact]
	public void BadNotBeApproximatelyShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => 1.5.Should().Not.BeApproximately(1.5, 0.1),
							"1.5 should not be approximately 1.5 within 0.1"
						);
	}

	[Fact]
	public void BadNotBeApproximatelyWithBecause() { RunCompareFailTest(() => 1.5.Should().Not.BeApproximately(1.5, 0.1, "custom because"), "custom because"); }

	[Fact]
	public void GoodBeApproximately() { 1.5.Should().BeApproximately(1.55, 0.1); }

	[Fact]
	public void GoodNotBeApproximately() { 1.5.Should().Not.BeApproximately(2.0, 0.1); }
}