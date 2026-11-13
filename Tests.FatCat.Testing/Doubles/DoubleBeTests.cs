namespace Tests.FatCat.Testing.Doubles;

public class DoubleBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => 1.5.Should().Be(2.5));
	}

	[Fact]
	public void BadBeNaNAgainstNumber()
	{
		RunCompareFailTest(() => double.NaN.Should().Be(1.5), "NaN should be 1.5");
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 1.5.Should().Be(2.5), "1.5 should be 2.5");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => 1.5.Should().Be(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => 1.5.Should().Not.Be(1.5));
	}

	[Fact]
	public void BadNotBeNaN()
	{
		RunCompareFailTest(() => double.NaN.Should().Not.Be(double.NaN), "NaN should not be NaN");
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 1.5.Should().Not.Be(1.5), "1.5 should not be 1.5");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => 1.5.Should().Not.Be(1.5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		1.5.Should().Be(1.5);
	}

	[Fact]
	public void GoodBeNaN()
	{
		double.NaN.Should().Be(double.NaN);
	}

	[Fact]
	public void GoodNotBe()
	{
		1.5.Should().Not.Be(2.5);
	}
}
