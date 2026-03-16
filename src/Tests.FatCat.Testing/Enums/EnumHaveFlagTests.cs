namespace Tests.FatCat.Testing.Enums;

public class EnumHaveFlagTests : BaseTest
{
	[Fact]
	public void BadHaveFlag()
	{
		RunCompareFailTest(() => TestStatus.Active.Should().HaveFlag(TestStatus.Pending));
	}

	[Fact]
	public void BadHaveFlagShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().HaveFlag(TestStatus.Pending),
			"Active should have flag Pending"
		);
	}

	[Fact]
	public void BadHaveFlagWithBecause()
	{
		RunCompareFailTest(
			() => TestStatus.Active.Should().HaveFlag(TestStatus.Pending, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotHaveFlag()
	{
		RunCompareFailTest(() =>
			(TestStatus.Active | TestStatus.Pending).Should().Not.HaveFlag(TestStatus.Active)
		);
	}

	[Fact]
	public void BadNotHaveFlagShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => (TestStatus.Active | TestStatus.Pending).Should().Not.HaveFlag(TestStatus.Active),
			"Active, Pending should not have flag Active"
		);
	}

	[Fact]
	public void BadNotHaveFlagWithBecause()
	{
		RunCompareFailTest(
			() =>
				(TestStatus.Active | TestStatus.Pending)
					.Should()
					.Not.HaveFlag(TestStatus.Active, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodHaveFlag()
	{
		(TestStatus.Active | TestStatus.Pending).Should().HaveFlag(TestStatus.Active);
	}

	[Fact]
	public void GoodNotHaveFlag()
	{
		TestStatus.Active.Should().Not.HaveFlag(TestStatus.Pending);
	}
}
