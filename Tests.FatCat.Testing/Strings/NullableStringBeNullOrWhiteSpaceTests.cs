#nullable enable

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeNullOrWhiteSpaceTests : BaseTest
{
	[Fact]
	public void BadBeNullOrWhiteSpace()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().BeNullOrWhiteSpace(),
							"hello should be null or whitespace"
						);
	}

	[Fact]
	public void BadBeNullOrWhiteSpaceWithBecause()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().BeNullOrWhiteSpace("custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWhenNull()
	{
		RunCompareFailTest(
							() => ((string?)null).Should().Not.BeNullOrWhiteSpace(),
							"null should not be null or whitespace"
						);
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWhenWhiteSpace()
	{
		RunCompareFailTest(
							() => ((string?)"   ").Should().Not.BeNullOrWhiteSpace(),
							"    should not be null or whitespace"
						);
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWithBecause()
	{
		RunCompareFailTest(
							() => ((string?)null).Should().Not.BeNullOrWhiteSpace("custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeNullOrWhiteSpaceWhenEmpty() { ((string?)"").Should().BeNullOrWhiteSpace(); }

	[Fact]
	public void GoodBeNullOrWhiteSpaceWhenNull() { ((string?)null).Should().BeNullOrWhiteSpace(); }

	[Fact]
	public void GoodBeNullOrWhiteSpaceWhenWhiteSpace() { ((string?)"   ").Should().BeNullOrWhiteSpace(); }

	[Fact]
	public void GoodNotBeNullOrWhiteSpace() { ((string?)"hello").Should().Not.BeNullOrWhiteSpace(); }
}