namespace Tests.FatCat.Testing.Booleans;

public class NullableBoolBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(() => ((bool?)true).Should().BeNull(), "True should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(() => ((bool?)true).Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadHaveValue()
	{
		RunCompareFailTest(() => ((bool?)null).Should().HaveValue(), "value should not be null");
	}

	[Fact]
	public void BadHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((bool?)null).Should().HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveValue()
	{
		RunCompareFailTest(() => ((bool?)true).Should().Not.HaveValue(), "True should not have a value");
	}

	[Fact]
	public void BadNotHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((bool?)true).Should().Not.HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNull()
	{
		((bool?)null).Should().BeNull();
	}

	[Fact]
	public void GoodHaveValue()
	{
		((bool?)true).Should().HaveValue();
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((bool?)null).Should().Not.HaveValue();
	}
}
