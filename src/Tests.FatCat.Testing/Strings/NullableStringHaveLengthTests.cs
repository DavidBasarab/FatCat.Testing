#nullable enable

namespace Tests.FatCat.Testing.Strings;

public class NullableStringHaveLengthTests : BaseTest
{
	[Fact]
	public void BadHaveLength() { RunCompareFailTest(() => ((string?)"hello").Should().HaveLength(3), "hello should have length 3"); }

	[Fact]
	public void BadHaveLengthWhenNull() { RunCompareFailTest(() => ((string?)null).Should().HaveLength(3), "null should have length 3"); }

	[Fact]
	public void BadHaveLengthWithBecause() { RunCompareFailTest(() => ((string?)"hello").Should().HaveLength(3, "custom because"), "custom because"); }

	[Fact]
	public void BadNotHaveLength() { RunCompareFailTest(() => ((string?)"hello").Should().Not.HaveLength(5), "hello should not have length 5"); }

	[Fact]
	public void BadNotHaveLengthWithBecause()
	{
		RunCompareFailTest(
							() => ((string?)"hello").Should().Not.HaveLength(5, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodHaveLength() { ((string?)"hello").Should().HaveLength(5); }

	[Fact]
	public void GoodNotHaveLength() { ((string?)"hello").Should().Not.HaveLength(3); }

	[Fact]
	public void GoodNotHaveLengthWhenNull() { ((string?)null).Should().Not.HaveLength(5); }
}