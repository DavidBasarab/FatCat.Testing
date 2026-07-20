namespace Tests.FatCat.Testing.Objects;

public class ObjectSatisfyTests : BaseTest
{
	[Fact]
	public void GoodSatisfy()
	{
		object subject = 5;

		subject.Should().Satisfy(value => value.Should().Be(5));
	}

	[Fact]
	public void BadSatisfy()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Satisfy(value => value.Should().Be(6)));
	}

	[Fact]
	public void BadSatisfyShowsCorrectMessage()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().Satisfy(value => value.Should().Be(6)), "5 should be 6");
	}
}
