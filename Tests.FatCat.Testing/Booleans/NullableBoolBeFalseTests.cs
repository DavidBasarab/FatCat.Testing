namespace Tests.FatCat.Testing.Booleans;

public class NullableBoolBeFalseTests : BaseTest
{
	[Fact]
	public void BadBeFalse() { RunCompareFailTest(() => ((bool?)true).Should().BeFalse(), "True should be False"); }

	[Fact]
	public void BadBeFalseNullValue() { RunCompareFailTest(() => ((bool?)null).Should().BeFalse(), "null should be False"); }

	[Fact]
	public void BadBeFalseWithBecause() { RunCompareFailTest(() => ((bool?)true).Should().BeFalse("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeFalse() { RunCompareFailTest(() => ((bool?)false).Should().Not.BeFalse(), "False should not be False"); }

	[Fact]
	public void BadNotBeFalseWithBecause() { RunCompareFailTest(() => ((bool?)false).Should().Not.BeFalse("custom because"), "custom because"); }

	[Fact]
	public void GoodBeFalse() { ((bool?)false).Should().BeFalse(); }

	[Fact]
	public void GoodNotBeFalse() { ((bool?)true).Should().Not.BeFalse(); }

	[Fact]
	public void GoodNotBeFalseWhenNull() { ((bool?)null).Should().Not.BeFalse(); }
}