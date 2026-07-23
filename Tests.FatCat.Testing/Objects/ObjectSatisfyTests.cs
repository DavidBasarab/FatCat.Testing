using FatCat.Testing.Objects;

namespace Tests.FatCat.Testing.Objects;

public class ObjectSatisfyTests : BaseTest
{
	[Fact]
	public void BadSatisfy() { RunCompareFailTest(() => new Dto("Alice").Should().Satisfy(dto => dto.Name.Should().Be("Bob")), "Alice should be Bob"); }

	[Fact]
	public void GoodSatisfy() { new Dto("Alice").Should().Satisfy(dto => dto.Name.Should().Be("Alice")); }
}
