namespace Tests.FatCat.Testing.Booleans;

public class BoolBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => false.Should().Be(true));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => false.Should().Be(true), "False should be True");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => false.Should().Be(true, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => true.Should().Not.Be(true));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => true.Should().Not.Be(true), "True should not be True");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => true.Should().Not.Be(true, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		true.Should().Be(true);
	}

	[Fact]
	public void GoodNotBe()
	{
		false.Should().Not.Be(true);
	}
}
