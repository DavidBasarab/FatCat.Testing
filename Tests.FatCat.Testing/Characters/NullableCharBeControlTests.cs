namespace Tests.FatCat.Testing.Characters;

public class NullableCharBeControlTests : BaseTest
{
	[Fact]
	public void BadBeControl() { RunCompareFailTest(() => ((char?)'a').Should().BeControl(), "a should be a control character"); }

	[Fact]
	public void BadBeControlNullValue() { RunCompareFailTest(() => ((char?)null).Should().BeControl(), "null should be a control character"); }

	[Fact]
	public void BadBeControlWithBecause() { RunCompareFailTest(() => ((char?)'a').Should().BeControl("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeControl() { RunCompareFailTest(() => ((char?)'\t').Should().Not.BeControl(), "\t should not be a control character"); }

	[Fact]
	public void BadNotBeControlWithBecause() { RunCompareFailTest(() => ((char?)'\t').Should().Not.BeControl("custom because"), "custom because"); }

	[Fact]
	public void GoodBeControl() { ((char?)'\t').Should().BeControl(); }

	[Fact]
	public void GoodNotBeControl() { ((char?)'a').Should().Not.BeControl(); }

	[Fact]
	public void GoodNotBeControlWhenNull() { ((char?)null).Should().Not.BeControl(); }
}