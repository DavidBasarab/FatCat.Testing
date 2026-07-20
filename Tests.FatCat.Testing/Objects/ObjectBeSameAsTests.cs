namespace Tests.FatCat.Testing.Objects;

public class ObjectBeSameAsTests : BaseTest
{
	[Fact]
	public void GoodBeSameAs()
	{
		var instance = new object();

		instance.Should().BeSameAs(instance);
	}

	[Fact]
	public void BadBeSameAs()
	{
		object subject = 5;
		object other = 5;

		RunCompareFailTest(() => subject.Should().BeSameAs(other));
	}

	[Fact]
	public void BadBeSameAsShowsCorrectMessage()
	{
		object subject = 5;
		object other = 5;

		RunCompareFailTest(() => subject.Should().BeSameAs(other), "5 should be the same instance as 5");
	}

	[Fact]
	public void BadBeSameAsWithBecause()
	{
		object subject = 5;
		object other = 5;

		RunCompareFailTest(() => subject.Should().BeSameAs(other, "custom because"), "custom because");
	}

	[Fact]
	public void GoodNotBeSameAs()
	{
		object subject = 5;
		object other = 5;

		subject.Should().Not.BeSameAs(other);
	}

	[Fact]
	public void BadNotBeSameAs()
	{
		var instance = new object();

		RunCompareFailTest(() => instance.Should().Not.BeSameAs(instance));
	}

	[Fact]
	public void BadNotBeSameAsShowsCorrectMessage()
	{
		var instance = new object();

		RunCompareFailTest(() => instance.Should().Not.BeSameAs(instance), "Object should not be the same instance as Object");
	}

	[Fact]
	public void BadNotBeSameAsWithBecause()
	{
		var instance = new object();

		RunCompareFailTest(() => instance.Should().Not.BeSameAs(instance, "custom because"), "custom because");
	}
}
