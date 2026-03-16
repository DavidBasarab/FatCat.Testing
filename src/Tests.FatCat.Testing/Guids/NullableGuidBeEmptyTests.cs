namespace Tests.FatCat.Testing.Guids;

public class NullableGuidBeEmptyTests : BaseTest
{
	private static readonly Guid TestGuid = new("12345678-1234-1234-1234-123456789012");

	[Fact]
	public void BadBeEmpty()
	{
		RunCompareFailTest(
			() => ((Guid?)TestGuid).Should().BeEmpty(),
			"12345678-1234-1234-1234-123456789012 should be empty"
		);
	}

	[Fact]
	public void BadBeEmptyNullValue()
	{
		RunCompareFailTest(() => ((Guid?)null).Should().BeEmpty(), "null should be empty");
	}

	[Fact]
	public void BadBeEmptyWithBecause()
	{
		RunCompareFailTest(() => ((Guid?)TestGuid).Should().BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeEmpty()
	{
		RunCompareFailTest(
			() => ((Guid?)Guid.Empty).Should().Not.BeEmpty(),
			"00000000-0000-0000-0000-000000000000 should not be empty"
		);
	}

	[Fact]
	public void BadNotBeEmptyWithBecause()
	{
		RunCompareFailTest(() => ((Guid?)Guid.Empty).Should().Not.BeEmpty("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeEmpty()
	{
		((Guid?)Guid.Empty).Should().BeEmpty();
	}

	[Fact]
	public void GoodNotBeEmpty()
	{
		((Guid?)TestGuid).Should().Not.BeEmpty();
	}

	[Fact]
	public void GoodNotBeEmptyWhenNull()
	{
		((Guid?)null).Should().Not.BeEmpty();
	}
}
