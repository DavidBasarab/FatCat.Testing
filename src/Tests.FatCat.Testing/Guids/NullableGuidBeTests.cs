namespace Tests.FatCat.Testing.Guids;

public class NullableGuidBeTests : BaseTest
{
	private static readonly Guid TestGuid = new("12345678-1234-1234-1234-123456789012");

	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(
			() => ((Guid?)TestGuid).Should().Be(Guid.Empty),
			"12345678-1234-1234-1234-123456789012 should be 00000000-0000-0000-0000-000000000000"
		);
	}

	[Fact]
	public void BadBeNullValue()
	{
		RunCompareFailTest(
			() => ((Guid?)null).Should().Be(Guid.Empty),
			"null should be 00000000-0000-0000-0000-000000000000"
		);
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => ((Guid?)TestGuid).Should().Be(Guid.Empty, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(
			() => ((Guid?)TestGuid).Should().Not.Be(TestGuid),
			"12345678-1234-1234-1234-123456789012 should not be 12345678-1234-1234-1234-123456789012"
		);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => ((Guid?)TestGuid).Should().Not.Be(TestGuid, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		((Guid?)TestGuid).Should().Be(TestGuid);
	}

	[Fact]
	public void GoodNotBe()
	{
		((Guid?)TestGuid).Should().Not.Be(Guid.Empty);
	}

	[Fact]
	public void GoodNotBeWhenNull()
	{
		((Guid?)null).Should().Not.Be(Guid.Empty);
	}
}
