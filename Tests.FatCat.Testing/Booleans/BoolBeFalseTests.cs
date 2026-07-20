namespace Tests.FatCat.Testing.Booleans;

public class BoolBeFalseTests : BaseTest
{
	[Fact]
	public void BadBeFalse() { RunCompareFailTest(() => true.Should().BeFalse()); }

	[Fact]
	public void BadBeFalseShowsCorrectMessage() { RunCompareFailTest(() => true.Should().BeFalse(), "True should be False"); }

	[Fact]
	public void BadBeFalseWithBecause() { RunCompareFailTest(() => true.Should().BeFalse("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeFalse() { RunCompareFailTest(() => false.Should().Not.BeFalse()); }

	[Fact]
	public void BadNotBeFalseShowsCorrectMessage() { RunCompareFailTest(() => false.Should().Not.BeFalse(), "False should not be False"); }

	[Fact]
	public void BadNotBeFalseWithBecause() { RunCompareFailTest(() => false.Should().Not.BeFalse("custom because"), "custom because"); }

	[Fact]
	public void GoodBeFalse() { false.Should().BeFalse(); }

	[Fact]
	public void GoodNotBeFalse() { true.Should().Not.BeFalse(); }
}