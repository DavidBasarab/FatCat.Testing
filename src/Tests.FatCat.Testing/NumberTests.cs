using FatCat.Testing;

namespace Tests.FatCat.Testing;

public class NumberTests : BaseTest
{
	[Fact]
	public void GoodEqual()
	{
		new NumberComparer().Compare(1, 1);
	}

	[Fact]
	public void BasicFail()
	{
		RunCompareFailTest(() => new NumberComparer().Compare(1, 2));
	}

	[Fact]
	public void GoodFluent()
	{
		1.Should().Be(1);
	}

	[Fact]
	public void BadFluent()
	{
		RunCompareFailTest(() => 1.Should().Be(2));
	}

	[Fact]
	public void GoodNotEqual()
	{
		1.Should().Not.Be(2);
	}

	[Fact]
	public void BadNotEqual()
	{
		RunCompareFailTest(() => 1.Should().Not.Be(1));
	}
}
