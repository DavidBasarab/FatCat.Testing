namespace Tests.FatCat.Testing.Characters;

public class CharBeWhiteSpaceTests : BaseTest
{
	[Fact]
	public void BadBeWhiteSpace() { RunCompareFailTest(() => 'a'.Should().BeWhiteSpace()); }

	[Fact]
	public void BadBeWhiteSpaceShowsCorrectMessage() { RunCompareFailTest(() => 'a'.Should().BeWhiteSpace(), "a should be white space"); }

	[Fact]
	public void BadBeWhiteSpaceWithBecause() { RunCompareFailTest(() => 'a'.Should().BeWhiteSpace("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeWhiteSpace() { RunCompareFailTest(() => ' '.Should().Not.BeWhiteSpace()); }

	[Fact]
	public void BadNotBeWhiteSpaceShowsCorrectMessage() { RunCompareFailTest(() => ' '.Should().Not.BeWhiteSpace(), "  should not be white space"); }

	[Fact]
	public void BadNotBeWhiteSpaceWithBecause() { RunCompareFailTest(() => ' '.Should().Not.BeWhiteSpace("custom because"), "custom because"); }

	[Fact]
	public void GoodBeWhiteSpace() { ' '.Should().BeWhiteSpace(); }

	[Fact]
	public void GoodNotBeWhiteSpace() { 'a'.Should().Not.BeWhiteSpace(); }
}