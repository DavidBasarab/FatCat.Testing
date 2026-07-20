namespace Tests.FatCat.Testing.Objects;

public class ObjectBeEquivalentToTests : BaseTest
{
	[Fact]
	public void GoodBeEquivalentTo()
	{
		var subject = new Person
		{
			Name = "Alice",
			Age = 30,
			Address = new Address { City = "Austin", Street = "Main" },
		};
		var expected = new Person
		{
			Name = "Alice",
			Age = 30,
			Address = new Address { City = "Austin", Street = "Main" },
		};

		subject.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void BadBeEquivalentTo()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		RunCompareFailTest(() => subject.Should().BeEquivalentTo(expected));
	}

	[Fact]
	public void BadBeEquivalentToShowsCorrectMessage()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		RunCompareFailTest(() => subject.Should().BeEquivalentTo(expected), "Expected Name to be \"Bob\" but found \"Alice\"");
	}

	[Fact]
	public void BadBeEquivalentToWithBecause()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		RunCompareFailTest(() => subject.Should().BeEquivalentTo(expected, "custom because"), "custom because");
	}

	[Fact]
	public void BadBeEquivalentToNestedShowsFullPath()
	{
		var subject = new Person
		{
			Name = "Alice",
			Age = 30,
			Address = new Address { City = "Austin", Street = "Main" },
		};
		var expected = new Person
		{
			Name = "Alice",
			Age = 30,
			Address = new Address { City = "Boston", Street = "Main" },
		};

		RunCompareFailTest(
			() => subject.Should().BeEquivalentTo(expected),
			"Expected Address.City to be \"Boston\" but found \"Austin\""
		);
	}

	[Fact]
	public void GoodBeEquivalentToRootScalar()
	{
		object subject = 5;

		subject.Should().BeEquivalentTo(5);
	}

	[Fact]
	public void BadBeEquivalentToRootScalarShowsCorrectMessage()
	{
		object subject = 5;

		RunCompareFailTest(() => subject.Should().BeEquivalentTo(6), "Expected 6 but found 5");
	}

	[Fact]
	public void GoodBeEquivalentToWithNulls()
	{
		object subject = null;

		subject.Should().BeEquivalentTo(null);
	}

	[Fact]
	public void BadBeEquivalentToNullMemberShowsCorrectMessage()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person
		{
			Name = "Alice",
			Age = 30,
			Address = new Address { City = "Austin" },
		};

		RunCompareFailTest(
			() => subject.Should().BeEquivalentTo(expected),
			"Expected Address to be Address { City = \"Austin\", Street = null } but found null"
		);
	}

	[Fact]
	public void GoodBeEquivalentToWithCycleDoesNotStackOverflow()
	{
		var subject = new Node { Name = "root" };
		subject.Link = subject;
		var expected = new Node { Name = "root" };
		expected.Link = expected;

		subject.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void GoodNotBeEquivalentTo()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		subject.Should().Not.BeEquivalentTo(expected);
	}

	[Fact]
	public void BadNotBeEquivalentTo()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Alice", Age = 30 };

		RunCompareFailTest(() => subject.Should().Not.BeEquivalentTo(expected));
	}

	[Fact]
	public void BadNotBeEquivalentToShowsCorrectMessage()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Alice", Age = 30 };

		RunCompareFailTest(
			() => subject.Should().Not.BeEquivalentTo(expected),
			"Person { Name = \"Alice\", Age = 30, Address = null } should not be equivalent to Person { Name = \"Alice\", Age = 30, Address = null }"
		);
	}

	[Fact]
	public void BadNotBeEquivalentToWithBecause()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Alice", Age = 30 };

		RunCompareFailTest(() => subject.Should().Not.BeEquivalentTo(expected, "custom because"), "custom because");
	}
}
