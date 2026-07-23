using FatCat.Testing.Objects;

namespace Tests.FatCat.Testing.Objects;

public class ObjectBeSameAsTests : BaseTest
{
	[Fact]
	public void BadBeSameAs()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().BeSameAs(new Dto("Bob")));
	}

	[Fact]
	public void BadBeSameAsShowsCorrectMessage()
	{
		RunCompareFailTest(
			() => new Dto("Alice").Should().BeSameAs(new Dto("Bob")),
			"Dto { Name = \"Alice\" } should be the same instance as Dto { Name = \"Bob\" }"
		);
	}

	[Fact]
	public void BadBeSameAsWhenEqualButNotSame()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().BeSameAs(new Dto("Alice")));
	}

	[Fact]
	public void BadBeSameAsWithBecause()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().BeSameAs(new Dto("Bob"), "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeSameAs()
	{
		var subject = new Dto("Alice");

		RunCompareFailTest(() => subject.Should().Not.BeSameAs(subject));
	}

	[Fact]
	public void BadNotBeSameAsShowsCorrectMessage()
	{
		var subject = new Dto("Alice");

		RunCompareFailTest(
			() => subject.Should().Not.BeSameAs(subject),
			"Dto { Name = \"Alice\" } should not be the same instance as Dto { Name = \"Alice\" }"
		);
	}

	[Fact]
	public void BadNotBeSameAsWithBecause()
	{
		var subject = new Dto("Alice");

		RunCompareFailTest(() => subject.Should().Not.BeSameAs(subject, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBeSameAs()
	{
		var subject = new Dto("Alice");

		subject.Should().BeSameAs(subject);
	}

	[Fact]
	public void GoodNotBeSameAs()
	{
		new Dto("Alice").Should().Not.BeSameAs(new Dto("Alice"));
	}
}
