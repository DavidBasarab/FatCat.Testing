namespace Tests.FatCat.Testing.Characters;

public class CharBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => 'a'.Should().Be('b'));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 'a'.Should().Be('b'), "a should be b");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => 'a'.Should().Be('b', "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => 'a'.Should().Not.Be('a'));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => 'a'.Should().Not.Be('a'), "a should not be a");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => 'a'.Should().Not.Be('a', "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		'a'.Should().Be('a');
	}

	[Fact]
	public void GoodNotBe()
	{
		'a'.Should().Not.Be('b');
	}
}
