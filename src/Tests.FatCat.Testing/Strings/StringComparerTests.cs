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
	public void BadBeEmptyWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().BeEmpty("custom because"), "custom because");
	}

	// BeEmpty

	[Fact]
	public void BadBeEmpty_WhenNotEmpty()
	{
		RunCompareFailTest(() => "hello".Should().BeEmpty(), "hello should be empty");
	}

	[Fact]
	public void BadBeEmpty_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().BeEmpty(), "null should be empty");
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

	// BeNullOrEmpty

	[Fact]
	public void BadBeNullOrEmpty()
	{
		RunCompareFailTest(() => "hello".Should().BeNullOrEmpty(), "hello should be null or empty");
	}

	[Fact]
	public void BadBeNullOrEmptyWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().BeNullOrEmpty("custom because"), "custom because");
	}

	// BeNullOrWhiteSpace

	[Fact]
	public void BadBeNullOrWhiteSpace()
	{
		RunCompareFailTest(() => "hello".Should().BeNullOrWhiteSpace(), "hello should be null or whitespace");
	}

	[Fact]
	public void BadBeNullOrWhiteSpaceWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().BeNullOrWhiteSpace("custom because"), "custom because");
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
	public void BadHaveLengthWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().HaveLength(3, "custom because"), "custom because");
	}

	// HaveLength

	[Fact]
	public void BadHaveLength_WhenDifferentLength()
	{
		RunCompareFailTest(() => "hello".Should().HaveLength(3), "hello should have length 3");
	}

	[Fact]
	public void BadHaveLength_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().HaveLength(3), "null should have length 3");
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
	public void BadNotBeNullOrEmptyWithBecause()
	{
		RunCompareFailTest(() => ((string)null).Should().Not.BeNullOrEmpty("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNullOrEmpty_WhenEmpty()
	{
		RunCompareFailTest(() => "".Should().Not.BeNullOrEmpty(), "subject should not be null or empty");
	}

	[Fact]
	public void BadNotBeNullOrEmpty_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().Not.BeNullOrEmpty(), "null should not be null or empty");
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWithBecause()
	{
		RunCompareFailTest(() => "   ".Should().Not.BeNullOrWhiteSpace("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpace_WhenEmpty()
	{
		RunCompareFailTest(() => "".Should().Not.BeNullOrWhiteSpace(), "subject should not be null or whitespace");
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpace_WhenNull()
	{
		RunCompareFailTest(
			() => ((string)null).Should().Not.BeNullOrWhiteSpace(),
			"null should not be null or whitespace"
		);
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpace_WhenWhiteSpace()
	{
		RunCompareFailTest(() => "   ".Should().Not.BeNullOrWhiteSpace(), "    should not be null or whitespace");
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
	public void BadNotHaveLengthWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.HaveLength(5, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveLength_WhenMatchingLength()
	{
		RunCompareFailTest(() => "hello".Should().Not.HaveLength(5), "hello should not have length 5");
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
	public void GoodBeEmpty()
	{
		"".Should().BeEmpty();
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
	public void GoodBeNullOrEmpty_WhenEmpty()
	{
		"".Should().BeNullOrEmpty();
	}

	[Fact]
	public void GoodBeNullOrEmpty_WhenNull()
	{
		((string)null).Should().BeNullOrEmpty();
	}

	[Fact]
	public void GoodBeNullOrWhiteSpace_WhenEmpty()
	{
		"".Should().BeNullOrWhiteSpace();
	}

	[Fact]
	public void GoodBeNullOrWhiteSpace_WhenNull()
	{
		((string)null).Should().BeNullOrWhiteSpace();
	}

	[Fact]
	public void GoodBeNullOrWhiteSpace_WhenWhiteSpace()
	{
		"   ".Should().BeNullOrWhiteSpace();
	}

	[Fact]
	public void GoodHaveLength()
	{
		"hello".Should().HaveLength(5);
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
	public void GoodNotBeEmpty()
	{
		"hello".Should().Not.BeEmpty();
	}

	[Fact]
	public void GoodNotBeEmpty_WhenNull()
	{
		((string)null).Should().Not.BeEmpty();
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
	public void GoodNotBeNullOrEmpty()
	{
		"hello".Should().Not.BeNullOrEmpty();
	}

	[Fact]
	public void GoodNotBeNullOrWhiteSpace()
	{
		"hello".Should().Not.BeNullOrWhiteSpace();
	}

	[Fact]
	public void GoodNotHaveLength()
	{
		"hello".Should().Not.HaveLength(3);
	}

	[Fact]
	public void GoodNotHaveLength_WhenNull()
	{
		((string)null).Should().Not.HaveLength(5);
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((string)null).Should().Not.HaveValue();
	}
}
