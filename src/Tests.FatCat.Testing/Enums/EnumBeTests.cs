namespace Tests.FatCat.Testing.Enums;

public class EnumBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().Be(TestStatus.Inactive));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().Be(TestStatus.Inactive), "Active should be Inactive");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().Be(TestStatus.Inactive, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().Not.Be(TestStatus.Active));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().Not.Be(TestStatus.Active),
			"Active should not be Active"
		);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().Not.Be(TestStatus.Active, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBe()
	{
		TestStatus.Active.Should().Be(TestStatus.Active);
	}

	[Fact]
	public void GoodNotBe()
	{
		TestStatus.Active.Should().Not.Be(TestStatus.Inactive);
	}
}
