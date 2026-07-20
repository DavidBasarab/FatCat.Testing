namespace Tests.FatCat.Testing.Enums;

public class NullableEnumBeDefinedTests : BaseTest
{
	[Fact]
	public void BadBeDefined() { RunCompareFailTest(() => ((TestStatus?)(TestStatus)99).Should().BeDefined(), "99 should be defined"); }

	[Fact]
	public void BadBeDefinedNullValue() { RunCompareFailTest(() => ((TestStatus?)null).Should().BeDefined(), "null should be defined"); }

	[Fact]
	public void BadBeDefinedWithBecause()
	{
		RunCompareFailTest(
							() => ((TestStatus?)(TestStatus)99).Should().BeDefined("custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeDefined()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().Not.BeDefined(),
							"Active should not be defined"
						);
	}

	[Fact]
	public void BadNotBeDefinedWithBecause()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().Not.BeDefined("custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBeDefined() { ((TestStatus?)TestStatus.Active).Should().BeDefined(); }

	[Fact]
	public void GoodNotBeDefined() { ((TestStatus?)(TestStatus)99).Should().Not.BeDefined(); }

	[Fact]
	public void GoodNotBeDefinedWhenNull() { ((TestStatus?)null).Should().Not.BeDefined(); }
}