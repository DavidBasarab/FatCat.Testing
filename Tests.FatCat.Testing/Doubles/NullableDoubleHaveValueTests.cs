namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleHaveValueTests : BaseTest
{
	[Fact]
	public void BadHaveValue()
	{
		RunCompareFailTest(() => ((double?)null).Should().HaveValue());
	}

	[Fact]
	public void BadHaveValueShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)null).Should().HaveValue(), "value should not be null");
	}

	[Fact]
	public void BadHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((double?)null).Should().HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveValue()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.HaveValue());
	}

	[Fact]
	public void BadNotHaveValueShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.HaveValue(), "1.5 should not have a value");
	}

	[Fact]
	public void BadNotHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().Not.HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveValue()
	{
		((double?)1.5).Should().HaveValue();
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((double?)null).Should().Not.HaveValue();
	}
}
