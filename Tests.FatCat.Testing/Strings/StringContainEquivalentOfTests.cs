using FatCat.Testing.Strings;

namespace Tests.FatCat.Testing.Strings;

public class StringContainEquivalentOfTests : BaseTest
{
	[Fact]
	public void BadContainEquivalentOf() { RunCompareFailTest(() => "hello world".Should().ContainEquivalentOf("xyz")); }

	[Fact]
	public void BadContainEquivalentOfShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => "hello world".Should().ContainEquivalentOf("xyz"),
							"hello world should contain equivalent of xyz"
						);
	}

	[Fact]
	public void BadContainEquivalentOfWhenNull()
	{
		RunCompareFailTest(
							() => ((string)null).Should().ContainEquivalentOf("hello"),
							"null should contain equivalent of hello"
						);
	}

	[Fact]
	public void BadContainEquivalentOfWithBecause()
	{
		RunCompareFailTest(
							() => "hello world".Should().ContainEquivalentOf("xyz", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotContainEquivalentOf()
	{
		RunCompareFailTest(
							() => "Hello World".Should().Not.ContainEquivalentOf("hello"),
							"Hello World should not contain equivalent of hello"
						);
	}

	[Fact]
	public void BadNotContainEquivalentOfWithBecause()
	{
		RunCompareFailTest(
							() => "Hello World".Should().Not.ContainEquivalentOf("hello", because: "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodContainEquivalentOf() { "Hello World".Should().ContainEquivalentOf("hello"); }

	[Fact]
	public void GoodNotContainEquivalentOf() { "Hello World".Should().Not.ContainEquivalentOf("xyz"); }

	[Fact]
	public void GoodNotContainEquivalentOfWhenNull() { ((string)null).Should().Not.ContainEquivalentOf("hello"); }
}
