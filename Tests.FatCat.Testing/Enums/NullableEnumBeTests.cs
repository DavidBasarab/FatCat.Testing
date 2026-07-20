namespace Tests.FatCat.Testing.Enums;

public class NullableEnumBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().Be(TestStatus.Inactive),
							"Active should be Inactive"
						);
	}

	[Fact]
	public void BadBeNullValue() { RunCompareFailTest(() => ((TestStatus?)null).Should().Be(TestStatus.Active), "null should be Active"); }

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().Be(TestStatus.Inactive, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().Not.Be(TestStatus.Active),
							"Active should not be Active"
						);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(
							() => ((TestStatus?)TestStatus.Active).Should().Not.Be(TestStatus.Active, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodBe() { ((TestStatus?)TestStatus.Active).Should().Be(TestStatus.Active); }

	[Fact]
	public void GoodNotBe() { ((TestStatus?)TestStatus.Active).Should().Not.Be(TestStatus.Inactive); }

	[Fact]
	public void GoodNotBeWhenNull() { ((TestStatus?)null).Should().Not.Be(TestStatus.Active); }
}