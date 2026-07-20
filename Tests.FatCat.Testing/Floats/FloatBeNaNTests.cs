namespace Tests.FatCat.Testing.Floats;

public class FloatBeNaNTests : BaseTest
{
	[Fact]
	public void BadBeNaN() { RunCompareFailTest(() => 1.5f.Should().BeNaN()); }

	[Fact]
	public void BadBeNaNShowsCorrectMessage() { RunCompareFailTest(() => 1.5f.Should().BeNaN(), "1.5 should be NaN"); }

	[Fact]
	public void BadBeNaNWithBecause() { RunCompareFailTest(() => 1.5f.Should().BeNaN("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeNaN() { RunCompareFailTest(() => float.NaN.Should().Not.BeNaN()); }

	[Fact]
	public void BadNotBeNaNShowsCorrectMessage() { RunCompareFailTest(() => float.NaN.Should().Not.BeNaN(), "NaN should not be NaN"); }

	[Fact]
	public void BadNotBeNaNWithBecause() { RunCompareFailTest(() => float.NaN.Should().Not.BeNaN("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNaN() { float.NaN.Should().BeNaN(); }

	[Fact]
	public void GoodNotBeNaN() { 1.5f.Should().Not.BeNaN(); }
}