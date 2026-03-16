namespace Tests.FatCat.Testing.Guids;

public class GuidBeEmptyTests : BaseTest
{
	private static readonly Guid TestGuid = new("12345678-1234-1234-1234-123456789012");

	[Fact]
	public void BadBeEmpty() { RunCompareFailTest(() => TestGuid.Should().BeEmpty()); }

	[Fact]
	public void BadBeEmptyShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => TestGuid.Should().BeEmpty(),
							"12345678-1234-1234-1234-123456789012 should be empty"
						);
	}

	[Fact]
	public void BadBeEmptyWithBecause() { RunCompareFailTest(() => TestGuid.Should().BeEmpty("custom because"), "custom because"); }

	[Fact]
	public void BadNotBeEmpty() { RunCompareFailTest(() => Guid.Empty.Should().Not.BeEmpty()); }

	[Fact]
	public void BadNotBeEmptyShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => Guid.Empty.Should().Not.BeEmpty(),
							"00000000-0000-0000-0000-000000000000 should not be empty"
						);
	}

	[Fact]
	public void BadNotBeEmptyWithBecause() { RunCompareFailTest(() => Guid.Empty.Should().Not.BeEmpty("custom because"), "custom because"); }

	[Fact]
	public void GoodBeEmpty() { Guid.Empty.Should().BeEmpty(); }

	[Fact]
	public void GoodNotBeEmpty() { TestGuid.Should().Not.BeEmpty(); }
}