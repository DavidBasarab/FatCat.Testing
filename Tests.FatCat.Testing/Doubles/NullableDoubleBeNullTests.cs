namespace Tests.FatCat.Testing.Doubles;

public class NullableDoubleBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeNull());
	}

	[Fact]
	public void BadBeNullShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeNull(), "1.5 should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(() => ((double?)1.5).Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNull()
	{
		((double?)null).Should().BeNull();
	}
}
