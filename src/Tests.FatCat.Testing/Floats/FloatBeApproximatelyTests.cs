namespace Tests.FatCat.Testing.Floats;

public class FloatBeApproximatelyTests : BaseTest
{
	[Fact]
	public void BadBeApproximately() { RunCompareFailTest(() => 1.5f.Should().BeApproximately(2.0f, 0.1f)); }

	[Fact]
	public void BadBeApproximatelyShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => 1.5f.Should().BeApproximately(2.0f, 0.1f),
							"1.5 should be approximately 2 within 0.1"
						);
	}

	[Fact]
	public void BadBeApproximatelyWithBecause() { RunCompareFailTest(() => 1.5f.Should().BeApproximately(2.0f, 0.1f, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBeApproximately() { RunCompareFailTest(() => 1.5f.Should().Not.BeApproximately(1.5f, 0.1f)); }

	[Fact]
	public void BadNotBeApproximatelyShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => 1.5f.Should().Not.BeApproximately(1.5f, 0.1f),
							"1.5 should not be approximately 1.5 within 0.1"
						);
	}

	[Fact]
	public void BadNotBeApproximatelyWithBecause()
	{
		RunCompareFailTest(
							() => 1.5f.Should().Not.BeApproximately(1.5f, 0.1f, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeApproximately() { 1.5f.Should().BeApproximately(1.55f, 0.1f); }

	[Fact]
	public void GoodNotBeApproximately() { 1.5f.Should().Not.BeApproximately(2.0f, 0.1f); }
}