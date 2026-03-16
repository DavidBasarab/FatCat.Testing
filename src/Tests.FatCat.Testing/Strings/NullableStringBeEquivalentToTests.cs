#nullable enable

using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeEquivalentToTests : BaseTest
{
	[Fact]
	public void BadBeEquivalentTo()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().BeEquivalentTo("world"),
							"hello should be equivalent to world"
						);
	}

	[Fact]
	public void BadBeEquivalentToWhenNull()
	{
		RunCompareFailTest(
							() => ((string?)null).Should().BeEquivalentTo("world"),
							"null should be equivalent to world"
						);
	}

	[Fact]
	public void BadBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().BeEquivalentTo("world", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeEquivalentTo()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().Not.BeEquivalentTo("hello"),
							"hello should not be equivalent to hello"
						);
	}

	[Fact]
	public void BadNotBeEquivalentToIgnoreCase()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().Not.BeEquivalentTo("HELLO", Options.IgnoreCase),
							"hello should not be equivalent to HELLO"
						);
	}

	[Fact]
	public void GoodBeEquivalentTo() { ((string?)"hello").Should().BeEquivalentTo("hello"); }

	[Fact]
	public void GoodBeEquivalentToIgnoreCase() { ((string?)"hello").Should().BeEquivalentTo("HELLO", Options.IgnoreCase); }

	[Fact]
	public void GoodNotBeEquivalentTo() { ((string?)"hello").Should().Not.BeEquivalentTo("world"); }
}