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
	public void BadBeLowerCasedWithBecause()
	{
		RunCompareFailTest(() => "HELLO".Should().BeLowerCased("custom because"), "custom because");
	}

	[Fact]
	public void BadBeLowerCased_MixedCase()
	{
		RunCompareFailTest(() => "Hello".Should().BeLowerCased(), "Hello should be lower cased");
	}

	[Fact]
	public void BadBeLowerCased_NoLetters()
	{
		RunCompareFailTest(() => "123".Should().BeLowerCased(), "123 should be lower cased");
	}

	[Fact]
	public void BadBeLowerCased_UpperCase()
	{
		RunCompareFailTest(() => "HELLO".Should().BeLowerCased(), "HELLO should be lower cased");
	}

	[Fact]
	public void BadBeLowerCased_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().BeLowerCased(), "null should be lower cased");
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
	public void BadBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void BadBeUpperCased_LowerCase()
	{
		RunCompareFailTest(() => "hello".Should().BeUpperCased(), "hello should be upper cased");
	}

	[Fact]
	public void BadBeUpperCased_MixedCase()
	{
		RunCompareFailTest(() => "Hello".Should().BeUpperCased(), "Hello should be upper cased");
	}

	[Fact]
	public void BadBeUpperCased_NoLetters()
	{
		RunCompareFailTest(() => "123".Should().BeUpperCased(), "123 should be upper cased");
	}

	[Fact]
	public void BadBeUpperCased_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().BeUpperCased(), "null should be upper cased");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Be("world", because: "custom because"), "custom because");
	}

	[Fact]
	public void BadContain()
	{
		RunCompareFailTest(() => "hello world".Should().Contain("xyz"), "hello world should contain xyz");
	}

	[Fact]
	public void BadContainAllWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().ContainAll(["xyz"], because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContainAll_MissingOne()
	{
		RunCompareFailTest(
			() => "hello world".Should().ContainAll(["hello", "xyz"]),
			"hello world should contain all of [xyz]"
		);
	}

	[Fact]
	public void BadContainAll_WhenNull()
	{
		RunCompareFailTest(
			() => ((string)null).Should().ContainAll(["hello"]),
			"null should contain all of [hello]"
		);
	}

	[Fact]
	public void BadContainAnyWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().ContainAny(["xyz"], because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContainAny_ContainsNone()
	{
		RunCompareFailTest(
			() => "hello world".Should().ContainAny(["xyz", "abc"]),
			"hello world should contain at least one of [xyz, abc]"
		);
	}

	[Fact]
	public void BadContainAny_WhenNull()
	{
		RunCompareFailTest(
			() => ((string)null).Should().ContainAny(["hello"]),
			"null should contain at least one of [hello]"
		);
	}

	[Fact]
	public void BadContainWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Contain("xyz", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadContain_AtLeastTwice()
	{
		RunCompareFailTest(
			() => "hello world".Should().Contain("hello", AtLeast.Twice()),
			"hello world should contain hello at least 2 times but found 1"
		);
	}

	[Fact]
	public void BadContain_AtMostThrice()
	{
		RunCompareFailTest(
			() => "hello hello hello hello".Should().Contain("hello", AtMost.Thrice()),
			"hello hello hello hello should contain hello at most 3 times but found 4"
		);
	}

	[Fact]
	public void BadContain_DifferentCase()
	{
		RunCompareFailTest(() => "hello world".Should().Contain("HELLO"), "hello world should contain HELLO");
	}

	[Fact]
	public void BadContain_ExactlyOnce_TooFew()
	{
		RunCompareFailTest(
			() => "world".Should().Contain("hello", Exactly.Once()),
			"world should contain hello exactly 1 time but found 0"
		);
	}

	[Fact]
	public void BadContain_ExactlyOnce_TooMany()
	{
		RunCompareFailTest(
			() => "hello hello".Should().Contain("hello", Exactly.Once()),
			"hello hello should contain hello exactly 1 time but found 2"
		);
	}

	[Fact]
	public void BadContain_LessThanThrice()
	{
		RunCompareFailTest(
			() => "hello hello hello".Should().Contain("hello", LessThan.Thrice()),
			"hello hello hello should contain hello less than 3 times but found 3"
		);
	}

	[Fact]
	public void BadContain_MoreThanOnce()
	{
		RunCompareFailTest(
			() => "hello world".Should().Contain("hello", MoreThan.Once()),
			"hello world should contain hello more than 1 time but found 1"
		);
	}

	[Fact]
	public void BadContain_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().Contain("hello"), "null should contain hello");
	}

	// EndWith

	[Fact]
	public void BadEndWith()
	{
		RunCompareFailTest(() => "hello world".Should().EndWith("hello"), "hello world should end with hello");
	}

	// EndWithEquivalentOf

	[Fact]
	public void BadEndWithEquivalentOf()
	{
		RunCompareFailTest(
			() => "hello world".Should().EndWithEquivalentOf("hello"),
			"hello world should end with equivalent of hello"
		);
	}

	[Fact]
	public void BadEndWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().EndWithEquivalentOf("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadEndWithWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().EndWith("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadEndWith_DifferentCase()
	{
		RunCompareFailTest(() => "hello world".Should().EndWith("WORLD"), "hello world should end with WORLD");
	}

	[Fact]
	public void BadEndWith_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().EndWith("world"), "null should end with world");
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
	public void BadNotBeLowerCased()
	{
		RunCompareFailTest(() => "hello".Should().Not.BeLowerCased(), "hello should not be lower cased");
	}

	[Fact]
	public void BadNotBeLowerCasedWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.BeLowerCased("custom because"), "custom because");
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
	public void BadNotBeUpperCased()
	{
		RunCompareFailTest(() => "HELLO".Should().Not.BeUpperCased(), "HELLO should not be upper cased");
	}

	[Fact]
	public void BadNotBeUpperCasedWithBecause()
	{
		RunCompareFailTest(() => "HELLO".Should().Not.BeUpperCased("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.Be("hello", because: "custom because"), "custom because");
	}

	[Fact]
	public void BadNotContain()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.Contain("hello"),
			"hello world should not contain hello"
		);
	}

	[Fact]
	public void BadNotContainAllWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.ContainAll(["hello", "world"], because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotContainAll_ContainsAll()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.ContainAll(["hello", "world"]),
			"hello world should not contain all of [hello, world]"
		);
	}

	[Fact]
	public void BadNotContainAny()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.ContainAny(["hello", "xyz"]),
			"hello world should not contain any of [hello, xyz]"
		);
	}

	[Fact]
	public void BadNotContainAnyWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.ContainAny(["hello"], because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotContainWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.Contain("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotContain_IgnoreCase()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.Contain("HELLO", Options.IgnoreCase),
			"hello world should not contain HELLO"
		);
	}

	[Fact]
	public void BadNotEndWith()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWith("world"),
			"hello world should not end with world"
		);
	}

	[Fact]
	public void BadNotEndWithEquivalentOf()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWithEquivalentOf("WORLD", Options.IgnoreCase),
			"hello world should not end with equivalent of WORLD"
		);
	}

	[Fact]
	public void BadNotEndWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWithEquivalentOf("WORLD", Options.IgnoreCase, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotEndWithWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWith("world", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotEndWith_IgnoreCase()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.EndWith("WORLD", Options.IgnoreCase),
			"hello world should not end with WORLD"
		);
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
	public void BadNotStartWith()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.StartWith("hello"),
			"hello world should not start with hello"
		);
	}

	[Fact]
	public void BadNotStartWithEquivalentOf()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.StartWithEquivalentOf("HELLO", Options.IgnoreCase),
			"hello world should not start with equivalent of HELLO"
		);
	}

	[Fact]
	public void BadNotStartWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.StartWithEquivalentOf("HELLO", Options.IgnoreCase, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotStartWithWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.StartWith("hello", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotStartWith_IgnoreCase()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.StartWith("HELLO", Options.IgnoreCase),
			"hello world should not start with HELLO"
		);
	}

	// StartWith

	[Fact]
	public void BadStartWith()
	{
		RunCompareFailTest(() => "hello world".Should().StartWith("world"), "hello world should start with world");
	}

	// StartWithEquivalentOf

	[Fact]
	public void BadStartWithEquivalentOf()
	{
		RunCompareFailTest(
			() => "hello world".Should().StartWithEquivalentOf("world"),
			"hello world should start with equivalent of world"
		);
	}

	[Fact]
	public void BadStartWithEquivalentOfWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().StartWithEquivalentOf("world", because: "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadStartWithWithBecause()
	{
		RunCompareFailTest(
			() => "hello world".Should().StartWith("world", because: "custom because"),
			"custom because"
		);
	}

	// StartWith / EndWith with Options

	[Fact]
	public void BadStartWith_DifferentCase()
	{
		RunCompareFailTest(() => "hello world".Should().StartWith("HELLO"), "hello world should start with HELLO");
	}

	[Fact]
	public void BadStartWith_WhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().StartWith("hello"), "null should start with hello");
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

	// BeLowerCased

	[Fact]
	public void GoodBeLowerCased()
	{
		"hello".Should().BeLowerCased();
	}

	[Fact]
	public void GoodBeLowerCased_WithNonLetters()
	{
		"hello 123".Should().BeLowerCased();
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

	// BeUpperCased

	[Fact]
	public void GoodBeUpperCased()
	{
		"HELLO".Should().BeUpperCased();
	}

	[Fact]
	public void GoodBeUpperCased_WithNonLetters()
	{
		"HELLO 123".Should().BeUpperCased();
	}

	// Contain (simple)

	[Fact]
	public void GoodContain()
	{
		"hello world".Should().Contain("hello");
	}

	// ContainAll

	[Fact]
	public void GoodContainAll()
	{
		"hello world".Should().ContainAll(["hello", "world"]);
	}

	[Fact]
	public void GoodContainAll_IgnoreCase()
	{
		"hello world".Should().ContainAll(["HELLO", "WORLD"], Options.IgnoreCase);
	}

	// ContainAny

	[Fact]
	public void GoodContainAny()
	{
		"hello world".Should().ContainAny(["hello", "xyz"]);
	}

	[Fact]
	public void GoodContainAny_IgnoreCase()
	{
		"hello world".Should().ContainAny(["HELLO", "XYZ"], Options.IgnoreCase);
	}

	[Fact]
	public void GoodContain_AtLeastTimes()
	{
		"ab ab ab".Should().Contain("ab", AtLeast.Times(2));
	}

	[Fact]
	public void GoodContain_AtLeastTwice()
	{
		"hello hello hello".Should().Contain("hello", AtLeast.Twice());
	}

	[Fact]
	public void GoodContain_AtMostThrice()
	{
		"hello hello".Should().Contain("hello", AtMost.Thrice());
	}

	[Fact]
	public void GoodContain_AtMostTimes()
	{
		"ab ab".Should().Contain("ab", AtMost.Times(5));
	}

	// Contain with OccurrenceConstraint

	[Fact]
	public void GoodContain_ExactlyOnce()
	{
		"hello world".Should().Contain("hello", Exactly.Once());
	}

	[Fact]
	public void GoodContain_ExactlyTimes()
	{
		"ab ab ab".Should().Contain("ab", Exactly.Times(3));
	}

	[Fact]
	public void GoodContain_IgnoreCase()
	{
		"hello world".Should().Contain("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodContain_LessThanThrice()
	{
		"hello hello".Should().Contain("hello", LessThan.Thrice());
	}

	[Fact]
	public void GoodContain_LessThanTimes()
	{
		"ab ab".Should().Contain("ab", LessThan.Times(5));
	}

	[Fact]
	public void GoodContain_MoreThanOnce()
	{
		"hello hello".Should().Contain("hello", MoreThan.Once());
	}

	[Fact]
	public void GoodContain_MoreThanTimes()
	{
		"ab ab ab".Should().Contain("ab", MoreThan.Times(2));
	}

	[Fact]
	public void GoodEndWith()
	{
		"hello world".Should().EndWith("world");
	}

	[Fact]
	public void GoodEndWithEquivalentOf()
	{
		"hello world".Should().EndWithEquivalentOf("WORLD", Options.IgnoreCase);
	}

	[Fact]
	public void GoodEndWith_IgnoreCase()
	{
		"hello world".Should().EndWith("WORLD", Options.IgnoreCase);
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

	// Not.BeLowerCased

	[Fact]
	public void GoodNotBeLowerCased_UpperCase()
	{
		"HELLO".Should().Not.BeLowerCased();
	}

	[Fact]
	public void GoodNotBeLowerCased_WhenNull()
	{
		((string)null).Should().Not.BeLowerCased();
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

	// Not.BeUpperCased

	[Fact]
	public void GoodNotBeUpperCased_LowerCase()
	{
		"hello".Should().Not.BeUpperCased();
	}

	[Fact]
	public void GoodNotBeUpperCased_WhenNull()
	{
		((string)null).Should().Not.BeUpperCased();
	}

	// Not.Contain

	[Fact]
	public void GoodNotContain()
	{
		"hello world".Should().Not.Contain("xyz");
	}

	// Not.ContainAll

	[Fact]
	public void GoodNotContainAll_MissingSome()
	{
		"hello world".Should().Not.ContainAll(["hello", "xyz"]);
	}

	[Fact]
	public void GoodNotContainAll_WhenNull()
	{
		((string)null).Should().Not.ContainAll(["hello"]);
	}

	// Not.ContainAny

	[Fact]
	public void GoodNotContainAny()
	{
		"hello world".Should().Not.ContainAny(["xyz", "abc"]);
	}

	[Fact]
	public void GoodNotContainAny_WhenNull()
	{
		((string)null).Should().Not.ContainAny(["hello"]);
	}

	[Fact]
	public void GoodNotContain_IgnoreCase()
	{
		"hello world".Should().Not.Contain("XYZ", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotContain_WhenNull()
	{
		((string)null).Should().Not.Contain("hello");
	}

	[Fact]
	public void GoodNotEndWith()
	{
		"hello world".Should().Not.EndWith("hello");
	}

	[Fact]
	public void GoodNotEndWithEquivalentOf()
	{
		"hello world".Should().Not.EndWithEquivalentOf("HELLO");
	}

	[Fact]
	public void GoodNotEndWith_IgnoreCase()
	{
		"hello world".Should().Not.EndWith("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotEndWith_WhenNull()
	{
		((string)null).Should().Not.EndWith("world");
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

	[Fact]
	public void GoodNotStartWith()
	{
		"hello world".Should().Not.StartWith("world");
	}

	[Fact]
	public void GoodNotStartWithEquivalentOf()
	{
		"hello world".Should().Not.StartWithEquivalentOf("WORLD");
	}

	[Fact]
	public void GoodNotStartWith_IgnoreCase()
	{
		"hello world".Should().Not.StartWith("WORLD", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotStartWith_WhenNull()
	{
		((string)null).Should().Not.StartWith("hello");
	}

	[Fact]
	public void GoodStartWith()
	{
		"hello world".Should().StartWith("hello");
	}

	[Fact]
	public void GoodStartWithEquivalentOf()
	{
		"hello world".Should().StartWithEquivalentOf("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodStartWith_IgnoreCase()
	{
		"hello world".Should().StartWith("HELLO", Options.IgnoreCase);
	}
}
