namespace Tests.FatCat.Testing.Numbers;

public class IntBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => 1.Should().Be(2));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 1.Should().Be(2), "1 should be 2");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => 1.Should().Be(2, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1), "1 should not be 1");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		1.Should().Be(1);
	}

	[Fact]
	public void GoodNotBe()
	{
		1.Should().Not.Be(2);
	}
}
