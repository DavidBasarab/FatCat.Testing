namespace Tests.FatCat.Testing.Guids;

public class GuidBeStringTests : BaseTest
{
	private static readonly Guid testGuid = new("12345678-1234-1234-1234-123456789012");

	[Fact]
	public void BadBeString()
	{
		RunCompareFailTest(() => testGuid.Should().Be("00000000-0000-0000-0000-000000000000"));
	}

	[Fact]
	public void BadBeStringShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => testGuid.Should().Be("00000000-0000-0000-0000-000000000000"),
			"12345678-1234-1234-1234-123456789012 should be 00000000-0000-0000-0000-000000000000"
		);
	}

	[Fact]
	public void BadBeStringWithBecause()
	{
		RunCompareFailTest(
			() => testGuid.Should().Be("00000000-0000-0000-0000-000000000000", "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadBeStringWithInvalidGuidThrowsArgumentException()
	{
		Assert.Throws<ArgumentException>(() => testGuid.Should().Be("not-a-guid"));
	}

	[Fact]
	public void BadNotBeString()
	{
		RunCompareFailTest(() => testGuid.Should().Not.Be("12345678-1234-1234-1234-123456789012"));
	}

	[Fact]
	public void BadNotBeStringShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => testGuid.Should().Not.Be("12345678-1234-1234-1234-123456789012"),
			"12345678-1234-1234-1234-123456789012 should not be 12345678-1234-1234-1234-123456789012"
		);
	}

	[Fact]
	public void BadNotBeStringWithBecause()
	{
		RunCompareFailTest(
			() => testGuid.Should().Not.Be("12345678-1234-1234-1234-123456789012", "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadNotBeStringWithInvalidGuidThrowsArgumentException()
	{
		Assert.Throws<ArgumentException>(() => testGuid.Should().Not.Be("not-a-guid"));
	}

	[Fact]
	public void GoodBeString()
	{
		testGuid.Should().Be("12345678-1234-1234-1234-123456789012");
	}

	[Fact]
	public void GoodNotBeString()
	{
		testGuid.Should().Not.Be("00000000-0000-0000-0000-000000000000");
	}
}
