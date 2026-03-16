using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringContainTests : BaseTest
{
	[Fact]
	public void BadContain()
	{
		RunCompareFailTest(() => "hello world".Should().Contain("xyz"), "hello world should contain xyz");
	}

	[Fact]
	public void BadContainAllMissingOne()
	{
		RunCompareFailTest(
			() => "hello world".Should().ContainAll(["hello", "xyz"]),
			"hello world should contain all of [xyz]"
		);
	}

	[Fact]
	public void BadContainAllWhenNull()
	{
		RunCompareFailTest(
			() => ((string)null).Should().ContainAll(["hello"]),
			"null should contain all of [hello]"
		);
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
	public void BadContainAnyContainsNone()
	{
		RunCompareFailTest(
			() => "hello world".Should().ContainAny(["xyz", "abc"]),
			"hello world should contain at least one of [xyz, abc]"
		);
	}

	[Fact]
	public void BadContainAnyWhenNull()
	{
		RunCompareFailTest(
			() => ((string)null).Should().ContainAny(["hello"]),
			"null should contain at least one of [hello]"
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
	public void BadContainAtLeastTwice()
	{
		RunCompareFailTest(
			() => "hello world".Should().Contain("hello", AtLeast.Twice()),
			"hello world should contain hello at least 2 times but found 1"
		);
	}

	[Fact]
	public void BadContainAtMostThrice()
	{
		RunCompareFailTest(
			() => "hello hello hello hello".Should().Contain("hello", AtMost.Thrice()),
			"hello hello hello hello should contain hello at most 3 times but found 4"
		);
	}

	[Fact]
	public void BadContainDifferentCase()
	{
		RunCompareFailTest(() => "hello world".Should().Contain("HELLO"), "hello world should contain HELLO");
	}

	[Fact]
	public void BadContainExactlyOnceTooFew()
	{
		RunCompareFailTest(
			() => "world".Should().Contain("hello", Exactly.Once()),
			"world should contain hello exactly 1 time but found 0"
		);
	}

	[Fact]
	public void BadContainExactlyOnceTooMany()
	{
		RunCompareFailTest(
			() => "hello hello".Should().Contain("hello", Exactly.Once()),
			"hello hello should contain hello exactly 1 time but found 2"
		);
	}

	[Fact]
	public void BadContainLessThanThrice()
	{
		RunCompareFailTest(
			() => "hello hello hello".Should().Contain("hello", LessThan.Thrice()),
			"hello hello hello should contain hello less than 3 times but found 3"
		);
	}

	[Fact]
	public void BadContainMoreThanOnce()
	{
		RunCompareFailTest(
			() => "hello world".Should().Contain("hello", MoreThan.Once()),
			"hello world should contain hello more than 1 time but found 1"
		);
	}

	[Fact]
	public void BadContainWhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().Contain("hello"), "null should contain hello");
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
	public void BadNotContain()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.Contain("hello"),
			"hello world should not contain hello"
		);
	}

	[Fact]
	public void BadNotContainAllContainsAll()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.ContainAll(["hello", "world"]),
			"hello world should not contain all of [hello, world]"
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
	public void BadNotContainIgnoreCase()
	{
		RunCompareFailTest(
			() => "hello world".Should().Not.Contain("HELLO", Options.IgnoreCase),
			"hello world should not contain HELLO"
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
	public void GoodContain()
	{
		"hello world".Should().Contain("hello");
	}

	[Fact]
	public void GoodContainAll()
	{
		"hello world".Should().ContainAll(["hello", "world"]);
	}

	[Fact]
	public void GoodContainAllIgnoreCase()
	{
		"hello world".Should().ContainAll(["HELLO", "WORLD"], Options.IgnoreCase);
	}

	[Fact]
	public void GoodContainAny()
	{
		"hello world".Should().ContainAny(["hello", "xyz"]);
	}

	[Fact]
	public void GoodContainAnyIgnoreCase()
	{
		"hello world".Should().ContainAny(["HELLO", "XYZ"], Options.IgnoreCase);
	}

	[Fact]
	public void GoodContainAtLeastTimes()
	{
		"ab ab ab".Should().Contain("ab", AtLeast.Times(2));
	}

	[Fact]
	public void GoodContainAtLeastTwice()
	{
		"hello hello hello".Should().Contain("hello", AtLeast.Twice());
	}

	[Fact]
	public void GoodContainAtMostThrice()
	{
		"hello hello".Should().Contain("hello", AtMost.Thrice());
	}

	[Fact]
	public void GoodContainAtMostTimes()
	{
		"ab ab".Should().Contain("ab", AtMost.Times(5));
	}

	[Fact]
	public void GoodContainExactlyOnce()
	{
		"hello world".Should().Contain("hello", Exactly.Once());
	}

	[Fact]
	public void GoodContainExactlyTimes()
	{
		"ab ab ab".Should().Contain("ab", Exactly.Times(3));
	}

	[Fact]
	public void GoodContainIgnoreCase()
	{
		"hello world".Should().Contain("HELLO", Options.IgnoreCase);
	}

	[Fact]
	public void GoodContainLessThanThrice()
	{
		"hello hello".Should().Contain("hello", LessThan.Thrice());
	}

	[Fact]
	public void GoodContainLessThanTimes()
	{
		"ab ab".Should().Contain("ab", LessThan.Times(5));
	}

	[Fact]
	public void GoodContainMoreThanOnce()
	{
		"hello hello".Should().Contain("hello", MoreThan.Once());
	}

	[Fact]
	public void GoodContainMoreThanTimes()
	{
		"ab ab ab".Should().Contain("ab", MoreThan.Times(2));
	}

	[Fact]
	public void GoodNotContain()
	{
		"hello world".Should().Not.Contain("xyz");
	}

	[Fact]
	public void GoodNotContainAllMissingSome()
	{
		"hello world".Should().Not.ContainAll(["hello", "xyz"]);
	}

	[Fact]
	public void GoodNotContainAllWhenNull()
	{
		((string)null).Should().Not.ContainAll(["hello"]);
	}

	[Fact]
	public void GoodNotContainAny()
	{
		"hello world".Should().Not.ContainAny(["xyz", "abc"]);
	}

	[Fact]
	public void GoodNotContainAnyWhenNull()
	{
		((string)null).Should().Not.ContainAny(["hello"]);
	}

	[Fact]
	public void GoodNotContainIgnoreCase()
	{
		"hello world".Should().Not.Contain("XYZ", Options.IgnoreCase);
	}

	[Fact]
	public void GoodNotContainWhenNull()
	{
		((string)null).Should().Not.Contain("hello");
	}
}
