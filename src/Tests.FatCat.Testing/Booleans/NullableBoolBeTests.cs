namespace Tests.FatCat.Testing.Booleans;

public class NullableBoolBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => ((bool?)false).Should().Be(true), "False should be True");
	}

	[Fact]
	public void BadBeNullValue()
	{
		RunCompareFailTest(() => ((bool?)null).Should().Be(true), "null should be True");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => ((bool?)false).Should().Be(true, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => ((bool?)true).Should().Not.Be(true), "True should not be True");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => ((bool?)true).Should().Not.Be(true, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		((bool?)true).Should().Be(true);
	}

	[Fact]
	public void GoodNotBe()
	{
		((bool?)false).Should().Not.Be(true);
	}

	[Fact]
	public void GoodNotBeWhenNull()
	{
		((bool?)null).Should().Not.Be(true);
	}
}
