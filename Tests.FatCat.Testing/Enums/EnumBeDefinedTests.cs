namespace Tests.FatCat.Testing.Enums;

public class EnumBeDefinedTests : BaseTest
{
	[Fact]
	public void BadBeDefined()
	{
		RunCompareFailTest(() => ((TestStatus)99).Should().BeDefined());
	}

	[Fact]
	public void BadBeDefinedShowsCorrectMessage()
	{
		RunCompareFailTest(() => ((TestStatus)99).Should().BeDefined(), "99 should be defined");
	}

	[Fact]
	public void BadBeDefinedWithBecause()
	{
		RunCompareFailTest(() => ((TestStatus)99).Should().BeDefined("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeDefined()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().Not.BeDefined());
	}

	[Fact]
	public void BadNotBeDefinedShowsCorrectMessage()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().Not.BeDefined(), "Active should not be defined");
	}

	[Fact]
	public void BadNotBeDefinedWithBecause()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().Not.BeDefined("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeDefined()
	{
		TestStatus.Active.Should().BeDefined();
	}

	[Fact]
	public void GoodNotBeDefined()
	{
		((TestStatus)99).Should().Not.BeDefined();
	}
}
