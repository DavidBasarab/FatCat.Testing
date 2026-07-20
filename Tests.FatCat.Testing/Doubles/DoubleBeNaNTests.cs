namespace Tests.FatCat.Testing.Doubles;

public class DoubleBeNaNTests : BaseTest
{
	[Fact]
	public void BadBeNaN() { RunCompareFailTest(() => 1.5.Should().BeNaN()); }

	[Fact]
	public void BadBeNaNShowsCorrectMessage() { RunCompareFailTest(() => 1.5.Should().BeNaN(), "1.5 should be NaN"); }

	[Fact]
	public void BadBeNaNWithBecause() { RunCompareFailTest(() => 1.5.Should().BeNaN("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeNaN() { RunCompareFailTest(() => double.NaN.Should().Not.BeNaN()); }

	[Fact]
	public void BadNotBeNaNShowsCorrectMessage() { RunCompareFailTest(() => double.NaN.Should().Not.BeNaN(), "NaN should not be NaN"); }

	[Fact]
	public void BadNotBeNaNWithBecause() { RunCompareFailTest(() => double.NaN.Should().Not.BeNaN("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNaN() { double.NaN.Should().BeNaN(); }

	[Fact]
	public void GoodNotBeNaN() { 1.5.Should().Not.BeNaN(); }
}