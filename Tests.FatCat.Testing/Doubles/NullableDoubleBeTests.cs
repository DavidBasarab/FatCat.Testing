namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Be(2.5));
	}

	[Fact]
	public void BadBeNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().Be(2.5), "null should be 2.5");
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Be(2.5), "1.5 should be 2.5");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Be(2.5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.Be(1.5));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.Be(1.5), "1.5 should not be 1.5");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.Be(1.5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		((double?)1.5).Should().Be(1.5);
	}

	[Fact]
	public void GoodBeNaN()
	{
		((double?)double.NaN).Should().Be(double.NaN);
	}

	[Fact]
	public void GoodNotBe()
	{
		((double?)1.5).Should().Not.Be(2.5);
	}

	[Fact]
	public void GoodNotBeWhenNull()
	{
		((double?)null).Should().Not.Be(1.5);
	}
}
