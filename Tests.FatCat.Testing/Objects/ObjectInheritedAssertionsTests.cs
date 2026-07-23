using FatCat.Testing.Objects;

namespace Tests.FatCat.Testing.Objects;

public class ObjectInheritedAssertionsTests : BaseTest
{
	[Fact]
	public void GoodBeAssignableTo() { new Thing().Should().BeAssignableTo<IThing>(); }

	[Fact]
	public void GoodBeOfType() { new Dto("Alice").Should().BeOfType<Dto>(); }

	[Fact]
	public void GoodBeOneOf() { new Dto("Alice").Should().BeOneOf(new Dto("Alice"), new Dto("Bob")); }
}
