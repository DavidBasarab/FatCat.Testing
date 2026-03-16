namespace Tests.FatCat.Testing.Enums;

public class NullableEnumBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(() => ((TestStatus?)TestStatus.Active).Should().BeNull(), "Active should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(
			() => ((TestStatus?)TestStatus.Active).Should().BeNull("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadHaveValue()
	{
		RunCompareFailTest(() => ((TestStatus?)null).Should().HaveValue(), "value should not be null");
	}

	[Fact]
	public void BadHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((TestStatus?)null).Should().HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveValue()
	{
		RunCompareFailTest(
			() => ((TestStatus?)TestStatus.Active).Should().Not.HaveValue(),
			"Active should not have a value"
		);
	}

	[Fact]
	public void BadNotHaveValueWithBecause()
	{
		RunCompareFailTest(
			() => ((TestStatus?)TestStatus.Active).Should().Not.HaveValue("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodBeNull()
	{
		((TestStatus?)null).Should().BeNull();
	}

	[Fact]
	public void GoodHaveValue()
	{
		((TestStatus?)TestStatus.Active).Should().HaveValue();
	}

	[Fact]
	public void GoodNotHaveValue()
	{
		((TestStatus?)null).Should().Not.HaveValue();
	}
}
