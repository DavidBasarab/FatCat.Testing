#nullable enable

using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().Be("world"), "hello should be world");
	}

	[Fact]
	public void BadBeCaseSensitiveWithDifferentCase()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().Be("HELLO"), "hello should be HELLO");
	}

	[Fact]
	public void BadBeIgnoreCase()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().Be("world", Options.IgnoreCase),
			"hello should be world"
		);
	}

	[Fact]
	public void BadBeIgnoreCaseWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().Be("world", Options.IgnoreCase, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadBeWhenNull()
	{
		RunCompareFailTest(() => ((string?)null).Should().Be("world"), "null should be world");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().Be("world", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => ((string?)"hello").Should().Not.Be("hello"), "hello should not be hello");
	}

	[Fact]
	public void BadNotBeIgnoreCase()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().Not.Be("HELLO", Options.IgnoreCase),
			"hello should not be HELLO"
		);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(
			() => ((string?)"hello").Should().Not.Be("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBe()
	{
		((string?)"hello").Should().Be("hello");
	}

	[Fact]
	public void GoodBeIgnoreCase()
	{
		((string?)"hello").Should().Be("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotBe()
	{
		((string?)"hello").Should().Not.Be("world");
	}

	[Fact]
	public void GoodNotBeWhenNull()
	{
		((string?)null).Should().Not.Be("world");
	}
}
