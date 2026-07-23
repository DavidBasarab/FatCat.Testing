using FatCat.Testing.Objects;

namespace Tests.FatCat.Testing.Objects;

public class ObjectBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().Be(new Dto("Bob")));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => new Dto("Alice").Should().Be(new Dto("Bob")),
			"Dto { Name = \"Alice\" } should be Dto { Name = \"Bob\" }"
		);
	}

	[Fact]
	public void BadBeWithBecause()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().Be(new Dto("Bob"), "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().Not.Be(new Dto("Alice")));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => new Dto("Alice").Should().Not.Be(new Dto("Alice")),
			"Dto { Name = \"Alice\" } should not be Dto { Name = \"Alice\" }"
		);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().Not.Be(new Dto("Alice"), "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		new Dto("Alice").Should().Be(new Dto("Alice"));
	}

	[Fact]
	public void GoodBeUsesEqualsNotReference()
	{
		var subject = new Dto("Alice");
		var expected = new Dto("Alice");

		subject.Should().Be(expected);
	}

	[Fact]
	public void GoodNotBe()
	{
		new Dto("Alice").Should().Not.Be(new Dto("Bob"));
	}
}
