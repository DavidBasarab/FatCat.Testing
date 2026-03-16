#nullable enable

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeEmptyTests : BaseTest
{
	[Fact]
	public void BadBeEmpty()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().BeEmpty(), "hello should be empty");
	}

	[Fact]
	public void BadBeEmptyWhenNull()
	{
		RunCompareFailTest(() => ((string?)null).Should().BeEmpty(), "null should be empty");
	}

	[Fact]
	public void BadBeEmptyWithBecause()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeEmpty()
	{
		RunCompareFailTest(() => ((string?)"").Should().Not.BeEmpty(), "subject should not be empty");
	}

	[Fact]
	public void BadNotBeEmptyWithBecause()
	{
		RunCompareFailTest(() => ((string?)"").Should().Not.BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeEmpty()
	{
		((string?)"").Should().BeEmpty();
	}

	[Fact]
	public void GoodNotBeEmpty()
	{
		((string?)"hello").Should().Not.BeEmpty();
	}

	[Fact]
	public void GoodNotBeEmptyWhenNull()
	{
		((string?)null).Should().Not.BeEmpty();
	}
}
