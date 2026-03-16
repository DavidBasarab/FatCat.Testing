using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => "hello".Should().Be("world"), "hello should be world");
	}

	[Fact]
	public void BadBeCaseSensitiveWithDifferentCase()
	{
		RunCompareFailTest(() => "hello".Should().Be("HELLO"), "hello should be HELLO");
	}

	[Fact]
	public void BadBeIgnoreCase()
	{
		RunCompareFailTest(() => "hello".Should().Be("world", Options.IgnoreCase), "hello should be world");
	}

	[Fact]
	public void BadBeIgnoreCaseWithBecause()
	{
		RunCompareFailTest(
			() => "hello".Should().Be("world", Options.IgnoreCase, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Be("world", because: "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => "hello".Should().Not.Be("hello"), "hello should not be hello");
	}

	[Fact]
	public void BadNotBeIgnoreCase()
	{
		RunCompareFailTest(
			() => "hello".Should().Not.Be("HELLO", Options.IgnoreCase),
			"hello should not be HELLO"
		);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.Be("hello", because: "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		"hello".Should().Be("hello");
	}

	[Fact]
	public void GoodBeCaseSensitiveSame()
	{
		"hello".Should().Be("hello");
	}

	[Fact]
	public void GoodBeIgnoreCase()
	{
		"hello".Should().Be("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotBe()
	{
		"hello".Should().Not.Be("world");
	}

	[Fact]
	public void GoodNotBeIgnoreCase()
	{
		"hello".Should().Not.Be("world", Options.IgnoreCase);
	}
}
