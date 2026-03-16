namespace Tests.FatCat.Testing.Enums;

public class NullableEnumHaveFlagTests : BaseTest
{
	[Fact]
	public void BadHaveFlag()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().HaveFlag(TestStatus.Pending),
							"Active should have flag Pending"
						);
	}

	[Fact]
	public void BadHaveFlagNullValue()
	{
		RunCompareFailTest(
							() => ((TestStatus?)null).Should().HaveFlag(TestStatus.Active),
							"null should have flag Active"
						);
	}

	[Fact]
	public void BadHaveFlagWithBecause()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().HaveFlag(TestStatus.Pending, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotHaveFlag()
	{
		RunCompareFailTest(
							() => ((TestStatus?)(TestStatus.Active | TestStatus.Pending)).Should().Not.HaveFlag(TestStatus.Active),
							"Active, Pending should not have flag Active"
						);
	}

	[Fact]
	public void BadNotHaveFlagWithBecause()
	{
		RunCompareFailTest(
							() =>
								((TestStatus?)(TestStatus.Active | TestStatus.Pending))
								.Should()
								.Not.HaveFlag(TestStatus.Active, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodHaveFlag() { ((TestStatus?)(TestStatus.Active | TestStatus.Pending)).Should().HaveFlag(TestStatus.Active); }

	[Fact]
	public void GoodNotHaveFlag() { ((TestStatus?)TestStatus.Active).Should().Not.HaveFlag(TestStatus.Pending); }

	[Fact]
	public void GoodNotHaveFlagWhenNull() { ((TestStatus?)null).Should().Not.HaveFlag(TestStatus.Active); }
}