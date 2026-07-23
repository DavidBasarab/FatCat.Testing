namespace Tests.FatCat.Testing.Enums;

public class EnumHaveSameNameAsTests : BaseTest
{
	[Fact]
	public void BadHaveSameNameAs()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().HaveSameNameAs(OtherStatus.Disabled));
	}

	[Fact]
	public void BadHaveSameNameAsShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().HaveSameNameAs(OtherStatus.Disabled),
			"Active should have the same name as Disabled"
		);
	}

	[Fact]
	public void BadHaveSameNameAsWithBecause()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().HaveSameNameAs(OtherStatus.Disabled, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotHaveSameNameAs()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().Not.HaveSameNameAs(OtherStatus.Active));
	}

	[Fact]
	public void BadNotHaveSameNameAsShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().Not.HaveSameNameAs(OtherStatus.Active),
			"Active should not have the same name as Active"
		);
	}

	[Fact]
	public void BadNotHaveSameNameAsWithBecause()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().Not.HaveSameNameAs(OtherStatus.Active, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodHaveSameNameAs()
	{
		TestStatus.Active.Should().HaveSameNameAs(OtherStatus.Active);
	}

	[Fact]
	public void GoodNotHaveSameNameAs()
	{
		TestStatus.Active.Should().Not.HaveSameNameAs(OtherStatus.Disabled);
	}
}
