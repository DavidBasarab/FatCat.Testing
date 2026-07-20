namespace Tests.FatCat.Testing.Objects;

public class ObjectMigrationTests : BaseTest
{
	[Fact]
	public void NotBeNullRewrite()
	{
		var subject = new SampleObject();

		subject.Should().Not.BeNull();
	}

	[Fact]
	public void NotBeSameAsRewrite()
	{
		var subject = new object();
		var other = new object();

		subject.Should().Not.BeSameAs(other);
	}

	[Fact]
	public void BeRewrite()
	{
		object subject = 5;

		subject.Should().Be(5);
	}
}
