namespace Tests.FatCat.Testing.Strings;

public class StringBeEmptyTests : BaseTest
{
	[Fact]
	public void BadBeEmptyWhenNotEmpty()
	{
		RunCompareFailTest(() => "hello".Should().BeEmpty(), "hello should be empty");
	}

	[Fact]
	public void BadBeEmptyWhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().BeEmpty(), "null should be empty");
	}

	[Fact]
	public void BadBeEmptyWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeEmpty()
	{
		RunCompareFailTest(() => "".Should().Not.BeEmpty(), "subject should not be empty");
	}

	[Fact]
	public void BadNotBeEmptyWithBecause()
	{
		RunCompareFailTest(() => "".Should().Not.BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeEmpty()
	{
		"".Should().BeEmpty();
	}

	[Fact]
	public void GoodNotBeEmpty()
	{
		"hello".Should().Not.BeEmpty();
	}

	[Fact]
	public void GoodNotBeEmptyWhenNull()
	{
		((string)null).Should().Not.BeEmpty();
	}
}
