namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(() => ((char?)'a').Should().BeNull(), "a should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(() => ((char?)'a').Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadHaveValue()
	{
		RunCompareFailTest(() => ((char?)null).Should().HaveValue(), "value should not be null");
	}

	[Fact]
	public void BadHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((char?)null).Should().HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveValue()
	{
		RunCompareFailTest(() => ((char?)'a').Should().Not.HaveValue(), "a should not have a value");
	}

	[Fact]
	public void BadNotHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((char?)'a').Should().Not.HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNull()
	{
		((char?)null).Should().BeNull();
	}

	[Fact]
	public void GoodHaveValue()
	{
		((char?)'a').Should().HaveValue();
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((char?)null).Should().Not.HaveValue();
	}
}
