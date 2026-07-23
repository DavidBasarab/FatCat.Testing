namespace Tests.FatCat.Testing.Booleans;

public class BoolImplyTests : BaseTest
{
	[Fact]
	public void BadImply()
	{
		RunCompareFailTest(() => true.Should().Imply(false));
	}

	[Fact]
	public void BadImplyShowsCorrectMessage()
	{
		RunCompareFailTest(() => true.Should().Imply(false), "True should imply False");
	}

	[Fact]
	public void BadImplyWithBecause()
	{
		RunCompareFailTest(() => true.Should().Imply(false, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotImply()
	{
		RunCompareFailTest(() => true.Should().Not.Imply(true));
	}

	[Fact]
	public void BadNotImplyShowsCorrectMessage()
	{
		RunCompareFailTest(() => true.Should().Not.Imply(true), "True should not imply True");
	}

	[Fact]
	public void BadNotImplyWithBecause()
	{
		RunCompareFailTest(() => true.Should().Not.Imply(true, "custom because"), "custom because");
	}

	[Fact]
	public void GoodImplyWhenAntecedentFalse()
	{
		false.Should().Imply(false);
	}

	[Fact]
	public void GoodImplyWhenBothTrue()
	{
		true.Should().Imply(true);
	}

	[Fact]
	public void GoodNotImply()
	{
		true.Should().Not.Imply(false);
	}
}
