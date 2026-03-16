#nullable enable

using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class NullableStringStartWithTests : BaseTest
{
	[Fact]
	public void BadNotStartWith()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.StartWith("hello"),
			"hello world should not start with hello"
		);
	}

	[Fact]
	public void BadNotStartWithEquivalentOf()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.StartWithEquivalentOf("hello"),
			"hello world should not start with equivalent of hello"
		);
	}

	[Fact]
	public void BadNotStartWithIgnoreCase()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.StartWith("HELLO", Options.IgnoreCase),
			"hello world should not start with HELLO"
		);
	}

	[Fact]
	public void BadNotStartWithWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().Not.StartWith("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadStartWith()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().StartWith("world"),
			"hello world should start with world"
		);
	}

	[Fact]
	public void BadStartWithEquivalentOf()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().StartWithEquivalentOf("world"),
			"hello world should start with equivalent of world"
		);
	}

	[Fact]
	public void BadStartWithWhenNull()
	{
		RunCompareFailTest(() => ((string?)null).Should().StartWith("hello"), "null should start with hello");
	}

	[Fact]
	public void BadStartWithWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello world").Should().StartWith("world", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodNotStartWith()
	{
		((string?)"hello world").Should().Not.StartWith("world");
	}

	[Fact]
	public void GoodNotStartWithWhenNull()
	{
		((string?)null).Should().Not.StartWith("hello");
	}

	[Fact]
	public void GoodStartWith()
	{
		((string?)"hello world").Should().StartWith("hello");
	}

	[Fact]
	public void GoodStartWithEquivalentOf()
	{
		((string?)"hello world").Should().StartWithEquivalentOf("hello");
	}

	[Fact]
	public void GoodStartWithIgnoreCase()
	{
		((string?)"hello world").Should().StartWith("HELLO", Options.IgnoreCase);
	}
}
