namespace Tests.FatCat.Testing.Guids;

public class GuidBeTests : BaseTest
{
	private static readonly Guid TestGuid = new("12345678-1234-1234-1234-123456789012");

	[Fact]
	public void BadBe() { RunCompareFailTest(() => TestGuid.Should().Be(Guid.Empty)); }

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => TestGuid.Should().Be(Guid.Empty),
							"12345678-1234-1234-1234-123456789012 should be 00000000-0000-0000-0000-000000000000"
						);
	}

	[Fact]
	public void BadBeWithBecause() { RunCompareFailTest(() => TestGuid.Should().Be(Guid.Empty, "custom because"), "custom because"); }

	[Fact]
	public void BadNotBe() { RunCompareFailTest(() => TestGuid.Should().Not.Be(TestGuid)); }

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => TestGuid.Should().Not.Be(TestGuid),
							"12345678-1234-1234-1234-123456789012 should not be 12345678-1234-1234-1234-123456789012"
						);
	}

	[Fact]
	public void BadNotBeWithBecause() { RunCompareFailTest(() => TestGuid.Should().Not.Be(TestGuid, "custom because"), "custom because"); }

	[Fact]
	public void GoodBe() { TestGuid.Should().Be(TestGuid); }

	[Fact]
	public void GoodNotBe() { TestGuid.Should().Not.Be(Guid.Empty); }
}