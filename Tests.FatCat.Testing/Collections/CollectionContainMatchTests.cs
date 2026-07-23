namespace Tests.FatCat.Testing.Collections;

public class CollectionContainMatchTests : BaseTest
{
	[Fact]
	public void GoodContainMatch() { new List<string> { "hello", "world" }.Should().ContainMatch("h*"); }

	[Fact]
	public void BadContainMatch() { RunCompareFailTest(() => new List<string> { "hello", "world" }.Should().ContainMatch("z*")); }

	[Fact]
	public void BadContainMatchShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new List<string> { "hello", "world" }.Should().ContainMatch("z*"),
							"[\"hello\", \"world\"] should contain a match for z*"
						);
	}

	[Fact]
	public void BadContainMatchWithBecause()
	{
		RunCompareFailTest(() => new List<string> { "hello", "world" }.Should().ContainMatch("z*", "custom because"), "custom because");
	}

	[Fact]
	public void BadContainMatchOnNull()
	{
		List<string> subject = null;

		RunCompareFailTest(() => subject.Should().ContainMatch("z*"), "null should contain a match for z*");
	}
}
