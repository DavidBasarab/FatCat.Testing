namespace Tests.FatCat.Testing.Objects;

public class ObjectBeNullTests : BaseTest
{
	[Fact]
	public void GoodBeNull()
	{
		object subject = null;

		subject.Should().BeNull();
	}

	[Fact]
	public void BadBeNull()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().BeNull());
	}

	[Fact]
	public void BadBeNullShowsCorrectMessage()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().BeNull(), "5 should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotBeNull()
	{
		object subject = 5;

		subject.Should().Not.BeNull();
	}

	[Fact]
	public void BadNotBeNull()
	{
		object subject = null;

		RunCompareFailTest(() => subject.Should().Not.BeNull());
	}

	[Fact]
	public void BadNotBeNullShowsCorrectMessage()
	{
		object subject = null;

		RunCompareFailTest(() => subject.Should().Not.BeNull(), "null should not be null");
	}

	[Fact]
	public void BadNotBeNullWithBecause()
	{
		object subject = null;

		RunCompareFailTest(() => subject.Should().Not.BeNull("custom because"), "custom because");
	}
}
