namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleMatchTests : BaseTest
{
	[Fact]
	public void BadMatch()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Match(value => value > 2));
	}

	[Fact]
	public void BadMatchNullValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().Match(value => value > 2), "null did not match the predicate");
	}

	[Fact]
	public void BadMatchShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Match(value => value > 2), "1.5 did not match the predicate");
	}

	[Fact]
	public void BadMatchWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Match(value => value > 2, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotMatch()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.Match(value => value > 1));
	}

	[Fact]
	public void BadNotMatchShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.Match(value => value > 1), "1.5 should not match the predicate");
	}

	[Fact]
	public void BadNotMatchWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.Match(value => value > 1, "custom because"), "custom because");
	}

	[Fact]
	public void GoodMatch()
	{
		((double?)1.5).Should().Match(value => value > 1);
	}

	[Fact]
	public void GoodNotMatch()
	{
		((double?)1.5).Should().Not.Match(value => value > 2);
	}

	[Fact]
	public void GoodNotMatchWhenNull()
	{
		((double?)null).Should().Not.Match(value => value > 2);
	}
}
