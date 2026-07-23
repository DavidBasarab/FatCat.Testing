namespace Tests.FatCat.Testing.Guids;

public class NullableGuidBeStringTests : BaseTest
{
	private static readonly Guid testGuid = new("12345678-1234-1234-1234-123456789012");

	[Fact]
	public void BadBeString()
	{
		RunCompareFailTest(
							() => ((Guid?)testGuid).Should().Be("00000000-0000-0000-0000-000000000000"),
							"12345678-1234-1234-1234-123456789012 should be 00000000-0000-0000-0000-000000000000"
						);
	}

	[Fact]
	public void BadBeStringNullValue()
	{
		RunCompareFailTest(
							() => ((Guid?)null).Should().Be("00000000-0000-0000-0000-000000000000"),
							"null should be 00000000-0000-0000-0000-000000000000"
						);
	}

	[Fact]
	public void BadBeStringWithBecause()
	{
		RunCompareFailTest(
							() => ((Guid?)testGuid).Should().Be("00000000-0000-0000-0000-000000000000", "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadBeStringWithInvalidGuidThrowsArgumentException()
	{
		Assert.Throws<ArgumentException>(() => ((Guid?)testGuid).Should().Be("not-a-guid"));
	}

	[Fact]
	public void BadNotBeString()
	{
		RunCompareFailTest(
							() => ((Guid?)testGuid).Should().Not.Be("12345678-1234-1234-1234-123456789012"),
							"12345678-1234-1234-1234-123456789012 should not be 12345678-1234-1234-1234-123456789012"
						);
	}

	[Fact]
	public void BadNotBeStringWithBecause()
	{
		RunCompareFailTest(
							() => ((Guid?)testGuid).Should().Not.Be("12345678-1234-1234-1234-123456789012", "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void BadNotBeStringWithInvalidGuidThrowsArgumentException()
	{
		Assert.Throws<ArgumentException>(() => ((Guid?)testGuid).Should().Not.Be("not-a-guid"));
	}

	[Fact]
	public void GoodBeString() { ((Guid?)testGuid).Should().Be("12345678-1234-1234-1234-123456789012"); }

	[Fact]
	public void GoodNotBeString() { ((Guid?)testGuid).Should().Not.Be("00000000-0000-0000-0000-000000000000"); }

	[Fact]
	public void GoodNotBeStringWhenNull() { ((Guid?)null).Should().Not.Be("00000000-0000-0000-0000-000000000000"); }
}