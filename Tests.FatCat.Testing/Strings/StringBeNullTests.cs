namespace Tests.FatCat.Testing.Strings;

public class StringBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(() => "hello".Should().BeNull(), "hello should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNull()
	{
		RunCompareFailTest(() => ((string)null).Should().Not.BeNull(), "subject should not be null");
	}

	[Fact]
	public void BadNotBeNullWithBecause()
	{
		RunCompareFailTest(() => ((string)null).Should().Not.BeNull("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNull()
	{
		((string)null).Should().BeNull();
	}

	[Fact]
	public void GoodNotBeNull()
	{
		"hello".Should().Not.BeNull();
	}
}
