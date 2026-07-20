namespace Tests.FatCat.Testing.Strings;

public class StringBeNullOrWhiteSpaceTests : BaseTest
{
	[Fact]
	public void BadBeNullOrWhiteSpace() { RunCompareFailTest(() => "hello".Should().BeNullOrWhiteSpace(), "hello should be null or whitespace"); }

	[Fact]
	public void BadBeNullOrWhiteSpaceWithBecause() { RunCompareFailTest(() => "hello".Should().BeNullOrWhiteSpace("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWhenEmpty() { RunCompareFailTest(() => "".Should().Not.BeNullOrWhiteSpace(), "subject should not be null or whitespace"); }

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWhenNull()
	{
		RunCompareFailTest(
							() => ((string)null).Should().Not.BeNullOrWhiteSpace(),
							"null should not be null or whitespace"
						);
	}

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWhenWhiteSpace() { RunCompareFailTest(() => "   ".Should().Not.BeNullOrWhiteSpace(), "    should not be null or whitespace"); }

	[Fact]
	public void BadNotBeNullOrWhiteSpaceWithBecause() { RunCompareFailTest(() => "   ".Should().Not.BeNullOrWhiteSpace("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNullOrWhiteSpaceWhenEmpty() { "".Should().BeNullOrWhiteSpace(); }

	[Fact]
	public void GoodBeNullOrWhiteSpaceWhenNull() { ((string)null).Should().BeNullOrWhiteSpace(); }

	[Fact]
	public void GoodBeNullOrWhiteSpaceWhenWhiteSpace() { "   ".Should().BeNullOrWhiteSpace(); }

	[Fact]
	public void GoodNotBeNullOrWhiteSpace() { "hello".Should().Not.BeNullOrWhiteSpace(); }
}