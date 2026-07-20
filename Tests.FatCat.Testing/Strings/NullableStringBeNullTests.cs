#nullable enable

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull() { RunCompareFailTest(() => ((string?)"hello").Should().BeNull(), "hello should be null"); }

	[Fact]
	public void BadBeNullWithBecause() { RunCompareFailTest(() => ((string?)"hello").Should().BeNull("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeNull() { RunCompareFailTest(() => ((string?)null).Should().Not.BeNull(), "subject should not be null"); }

	[Fact]
	public void BadNotBeNullWithBecause() { RunCompareFailTest(() => ((string?)null).Should().Not.BeNull("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNull() { ((string?)null).Should().BeNull(); }

	[Fact]
	public void GoodNotBeNull() { ((string?)"hello").Should().Not.BeNull(); }
}