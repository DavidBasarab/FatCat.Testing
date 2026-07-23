using FatCat.Testing.Objects;

namespace Tests.FatCat.Testing.Objects;

public class ObjectBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().BeNull());
	}

	[Fact]
	public void BadBeNullShowsCorrectMessage()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().BeNull(), "Dto { Name = \"Alice\" } should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		RunCompareFailTest(() => new Dto("Alice").Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeNull()
	{
		Dto subject = null;

		RunCompareFailTest(() => subject.Should().Not.BeNull());
	}

	[Fact]
	public void BadNotBeNullShowsCorrectMessage()
	{
		Dto subject = null;

		RunCompareFailTest(() => subject.Should().Not.BeNull(), "null should not be null");
	}

	[Fact]
	public void BadNotBeNullWithBecause()
	{
		Dto subject = null;

		RunCompareFailTest(() => subject.Should().Not.BeNull("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeNull()
	{
		Dto subject = null;

		subject.Should().BeNull();
	}

	[Fact]
	public void GoodNotBeNull()
	{
		new Dto("Alice").Should().Not.BeNull();
	}
}
