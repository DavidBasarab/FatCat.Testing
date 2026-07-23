namespace Tests.FatCat.Testing.Strings;

public class StringHaveValueTests : BaseTest
{
	[Fact]
	public void BadHaveValue()
	{
		RunCompareFailTest(() => ((string)null).Should().HaveValue(), "subject should have a value");
	}

	[Fact]
	public void BadHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((string)null).Should().HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveValue()
	{
		RunCompareFailTest(() => "hello".Should().Not.HaveValue(), "hello should not have a value");
	}

	[Fact]
	public void BadNotHaveValueWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveValue()
	{
		"hello".Should().HaveValue();
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((string)null).Should().Not.HaveValue();
	}
}
