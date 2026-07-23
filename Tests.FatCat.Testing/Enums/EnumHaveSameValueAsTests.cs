namespace Tests.FatCat.Testing.Enums;

public class EnumHaveSameValueAsTests : BaseTest
{
	[Fact]
	public void BadHaveSameValueAs() { RunCompareFailTest(() => TestStatus.Active.Should().HaveSameValueAs(OtherStatus.Disabled)); }

	[Fact]
	public void BadHaveSameValueAsShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => TestStatus.Active.Should().HaveSameValueAs(OtherStatus.Disabled),
							"Active should have the same value as Disabled"
						);
	}

	[Fact]
	public void BadHaveSameValueAsWithBecause()
	{
		RunCompareFailTest(
							() => TestStatus.Active.Should().HaveSameValueAs(OtherStatus.Disabled, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotHaveSameValueAs() { RunCompareFailTest(() => TestStatus.Inactive.Should().Not.HaveSameValueAs(OtherStatus.Disabled)); }

	[Fact]
	public void BadNotHaveSameValueAsShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => TestStatus.Inactive.Should().Not.HaveSameValueAs(OtherStatus.Disabled),
							"Inactive should not have the same value as Disabled"
						);
	}

	[Fact]
	public void BadNotHaveSameValueAsWithBecause()
	{
		RunCompareFailTest(
							() => TestStatus.Inactive.Should().Not.HaveSameValueAs(OtherStatus.Disabled, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodHaveSameValueAs() { TestStatus.Inactive.Should().HaveSameValueAs(OtherStatus.Disabled); }

	[Fact]
	public void GoodNotHaveSameValueAs() { TestStatus.Active.Should().Not.HaveSameValueAs(OtherStatus.Disabled); }
}