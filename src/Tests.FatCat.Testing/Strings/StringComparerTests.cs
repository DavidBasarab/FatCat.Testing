using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringComparerTests : BaseTest
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
	public void BadBeEquivalentTo()
	{
		RunCompareFailTest(() => "hello".Should().BeEquivalentTo("world"), "hello should be equivalent to world");
	}

	[Fact]
	public void BadBeEquivalentToIgnoreCase()
	{
		RunCompareFailTest(
			() => "hello".Should().BeEquivalentTo("world", Options.IgnoreCase),
			"hello should be equivalent to world"
		);
	}

	[Fact]
	public void BadBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
			() => "hello".Should().BeEquivalentTo("world", because: "custom because"),
			"custom because"
		);
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
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Be("world", because: "custom because"), "custom because");
	}

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
	public void BadNotBe()
	{
		RunCompareFailTest(() => "hello".Should().Not.Be("hello"), "hello should not be hello");
	}

	[Fact]
	public void BadNotBeEquivalentTo()
	{
		RunCompareFailTest(
			() => "hello".Should().Not.BeEquivalentTo("hello"),
			"hello should not be equivalent to hello"
		);
	}

	[Fact]
	public void BadNotBeEquivalentToIgnoreCase()
	{
		RunCompareFailTest(
			() => "hello".Should().Not.BeEquivalentTo("HELLO", Options.IgnoreCase),
			"hello should not be equivalent to HELLO"
		);
	}

	[Fact]
	public void BadNotBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
			() => "hello".Should().Not.BeEquivalentTo("hello", because: "custom because"),
			"custom because"
		);
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
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.Be("hello", because: "custom because"), "custom because");
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
	public void GoodBeEquivalentTo()
	{
		"hello".Should().BeEquivalentTo("hello");
	}

	[Fact]
	public void GoodBeEquivalentToIgnoreCase()
	{
		"hello".Should().BeEquivalentTo("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodBeIgnoreCase()
	{
		"hello".Should().Be("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodBeNull()
	{
		((string)null).Should().BeNull();
	}

	[Fact]
	public void GoodHaveValue()
	{
		"hello".Should().HaveValue();
	}

	[Fact]
	public void GoodNotBe()
	{
		"hello".Should().Not.Be("world");
	}

	[Fact]
	public void GoodNotBeEquivalentTo()
	{
		"hello".Should().Not.BeEquivalentTo("world");
	}

	[Fact]
	public void GoodNotBeEquivalentToIgnoreCase()
	{
		"hello".Should().Not.BeEquivalentTo("world", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotBeIgnoreCase()
	{
		"hello".Should().Not.Be("world", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotBeNull()
	{
		"hello".Should().Not.BeNull();
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((string)null).Should().Not.HaveValue();
	}
}
