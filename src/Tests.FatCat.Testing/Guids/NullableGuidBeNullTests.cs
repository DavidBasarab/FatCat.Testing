namespace Tests.FatCat.Testing.Guids;

public class NullableGuidBeNullTests : BaseTest
{
	private static readonly Guid TestGuid = new("12345678-1234-1234-1234-123456789012");

	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(
			() => ((Guid?)TestGuid).Should().BeNull(),
			"12345678-1234-1234-1234-123456789012 should be null"
		);
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(() => ((Guid?)TestGuid).Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadHaveValue()
	{
		RunCompareFailTest(() => ((Guid?)null).Should().HaveValue(), "value should not be null");
	}

	[Fact]
	public void BadHaveValueWithBecause()
	{
		RunCompareFailTest(() => ((Guid?)null).Should().HaveValue("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNull()
	{
		((Guid?)null).Should().BeNull();
	}

	[Fact]
	public void GoodHaveValue()
	{
		((Guid?)TestGuid).Should().HaveValue();
	}
}
