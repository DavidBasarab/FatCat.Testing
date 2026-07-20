namespace Tests.FatCat.Testing.Objects;

public class ObjectBeTests : BaseTest
{
	[Fact]
	public void GoodBe()
	{
		object subject = 5;
		object expected = 5;

		subject.Should().Be(expected);
	}

	[Fact]
	public void BadBe()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Be(6));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Be(6), "5 should be 6");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Be(6, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotBe()
	{
		object subject = 5;

		subject.Should().Not.Be(6);
	}

	[Fact]
	public void BadNotBe()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Not.Be(5));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Not.Be(5), "5 should not be 5");
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Not.Be(5, "custom because"), "custom because");
	}
}
