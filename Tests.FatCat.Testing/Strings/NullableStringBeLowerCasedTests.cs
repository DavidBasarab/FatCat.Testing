#nullable enable

namespace Tests.FatCat.Testing.Strings;

public class NullableStringBeLowerCasedTests : BaseTest
{
	[Fact]
	public void BadBeLowerCased() { RunCompareFailTest(() => ((string?)"HELLO").Should().BeLowerCased(), "HELLO should be lower cased"); }

	[Fact]
	public void BadBeLowerCasedWhenNull() { RunCompareFailTest(() => ((string?)null).Should().BeLowerCased(), "null should be lower cased"); }

	[Fact]
	public void BadBeLowerCasedWithBecause() { RunCompareFailTest(() => ((string?)"HELLO").Should().BeLowerCased("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeLowerCased()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().Not.BeLowerCased(),
							"hello should not be lower cased"
						);
	}

	[Fact]
	public void BadNotBeLowerCasedWithBecause() { RunCompareFailTest(() => ((string?)"hello").Should().Not.BeLowerCased("custom because"), "custom because"); }

	[Fact]
	public void GoodBeLowerCased() { ((string?)"hello").Should().BeLowerCased(); }

	[Fact]
	public void GoodNotBeLowerCased() { ((string?)"HELLO").Should().Not.BeLowerCased(); }

	[Fact]
	public void GoodNotBeLowerCasedWhenNull() { ((string?)null).Should().Not.BeLowerCased(); }
}