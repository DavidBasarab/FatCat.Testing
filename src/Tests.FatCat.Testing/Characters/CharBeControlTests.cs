namespace Tests.FatCat.Testing.Characters;

public class CharBeControlTests : BaseTest
{
	[Fact]
	public void BadBeControl()
	{
		RunCompareFailTest(() => 'a'.Should().BeControl());
	}

	[Fact]
	public void BadBeControlShowsCorrectMessage()
	{
		RunCompareFailTest(() => 'a'.Should().BeControl(), "a should be a control character");
	}

	[Fact]
	public void BadBeControlWithBecause()
	{
		RunCompareFailTest(() => 'a'.Should().BeControl("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeControl()
	{
		RunCompareFailTest(() => '\t'.Should().Not.BeControl());
	}

	[Fact]
	public void BadNotBeControlShowsCorrectMessage()
	{
		RunCompareFailTest(() => '\t'.Should().Not.BeControl(), "\t should not be a control character");
	}

	[Fact]
	public void BadNotBeControlWithBecause()
	{
		RunCompareFailTest(() => '\t'.Should().Not.BeControl("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeControl()
	{
		'\t'.Should().BeControl();
	}

	[Fact]
	public void GoodNotBeControl()
	{
		'a'.Should().Not.BeControl();
	}
}
