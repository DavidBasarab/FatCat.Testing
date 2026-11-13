namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBePositiveTests : BaseTest
{
	[Fact]
	public void BadBePositive()
	{
		RunCompareFailTest(() => ((double?)-1.5).Should().BePositive());
	}

	[Fact]
	public void BadBePositiveNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().BePositive(), "null should be positive");
	}

	[Fact]
	public void BadBePositiveShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)-1.5).Should().BePositive(), "-1.5 should be positive");
	}

	[Fact]
	public void BadBePositiveWithBecause()
	{
		RunCompareFailTest(() => ((double?)-1.5).Should().BePositive("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBePositive()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BePositive());
	}

	[Fact]
	public void BadNotBePositiveShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BePositive(), "1.5 should not be positive");
	}

	[Fact]
	public void BadNotBePositiveWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.BePositive("custom because"), "custom because");
	}

	[Fact]
	public void GoodBePositive()
	{
		((double?)1.5).Should().BePositive();
	}

	[Fact]
	public void GoodNotBePositive()
	{
		((double?)-1.5).Should().Not.BePositive();
	}

	[Fact]
	public void GoodNotBePositiveWhenNull()
	{
		((double?)null).Should().Not.BePositive();
	}
}
