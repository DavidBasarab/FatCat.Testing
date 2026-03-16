namespace Tests.FatCat.Testing.Strings;

public class StringHaveLengthTests : BaseTest
{
	[Fact]
	public void BadHaveLengthWhenDifferentLength()
	{
		RunCompareFailTest(() => "hello".Should().HaveLength(3), "hello should have length 3");
	}

	[Fact]
	public void BadHaveLengthWhenNull()
	{
		RunCompareFailTest(() => ((string)null).Should().HaveLength(3), "null should have length 3");
	}

	[Fact]
	public void BadHaveLengthWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().HaveLength(3, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveLengthWhenMatchingLength()
	{
		RunCompareFailTest(() => "hello".Should().Not.HaveLength(5), "hello should not have length 5");
	}

	[Fact]
	public void BadNotHaveLengthWithBecause()
	{
		RunCompareFailTest(() => "hello".Should().Not.HaveLength(5, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveLength()
	{
		"hello".Should().HaveLength(5);
	}

	[Fact]
	public void GoodNotHaveLength()
	{
		"hello".Should().Not.HaveLength(3);
	}

	[Fact]
	public void GoodNotHaveLengthWhenNull()
	{
		((string)null).Should().Not.HaveLength(5);
	}
}
