#nullable enable

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeNullOrEmptyTests : BaseTest
{
	[Fact]
	public void BadBeNullOrEmpty() { RunCompareFailTest(() => ((string?)"hello").Should().BeNullOrEmpty(), "hello should be null or empty"); }

	[Fact]
	public void BadBeNullOrEmptyWithBecause() { RunCompareFailTest(() => ((string?)"hello").Should().BeNullOrEmpty("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeNullOrEmptyWhenEmpty()
	{
		RunCompareFailTest(
							() => ((string?)"").Should().Not.BeNullOrEmpty(),
							"subject should not be null or empty"
						);
	}

	[Fact]
	public void BadNotBeNullOrEmptyWhenNull() { RunCompareFailTest(() => ((string?)null).Should().Not.BeNullOrEmpty(), "null should not be null or empty"); }

	[Fact]
	public void BadNotBeNullOrEmptyWithBecause() { RunCompareFailTest(() => ((string?)null).Should().Not.BeNullOrEmpty("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNullOrEmptyWhenEmpty() { ((string?)"").Should().BeNullOrEmpty(); }

	[Fact]
	public void GoodBeNullOrEmptyWhenNull() { ((string?)null).Should().BeNullOrEmpty(); }

	[Fact]
	public void GoodNotBeNullOrEmpty() { ((string?)"hello").Should().Not.BeNullOrEmpty(); }
}