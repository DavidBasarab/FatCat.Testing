using FatCat.Testing.Objects;
using Tests.FatCat.Testing.Equivalency;

namespace Tests.FatCat.Testing.Objects;

public class ObjectBeEquivalentToTests : BaseTest
{
	[Fact]
	public void BadBeEquivalentTo()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		RunCompareFailTest(() => subject.Should().BeEquivalentTo(expected));
	}

	[Fact]
	public void BadBeEquivalentToNestedShowsPath()
	{
		var subject = new Person
		{
			Name = "Sam",
			Age = 30,
			Address = new Address { City = "Leeds", Postcode = "LS1" },
		};
		var expected = new Person
		{
			Name = "Sam",
			Age = 30,
			Address = new Address { City = "York", Postcode = "LS1" },
		};

		RunCompareFailTest(
			() => subject.Should().BeEquivalentTo(expected),
			"Person { Name = \"Sam\", Age = 30, Address = Address { City = \"Leeds\", Postcode = \"LS1\" } } should be equivalent to Person { Name = \"Sam\", Age = 30, Address = Address { City = \"York\", Postcode = \"LS1\" } } but Address.City differs: expected \"York\" but found \"Leeds\""
		);
	}

	[Fact]
	public void BadBeEquivalentToShowsCorrectMessage()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		RunCompareFailTest(
			() => subject.Should().BeEquivalentTo(expected),
			"Person { Name = \"Alice\", Age = 30, Address = null } should be equivalent to Person { Name = \"Bob\", Age = 30, Address = null } but Name differs: expected \"Bob\" but found \"Alice\""
		);
	}

	[Fact]
	public void BadBeEquivalentToWithBecause()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		RunCompareFailTest(() => subject.Should().BeEquivalentTo(expected, "custom because"), "custom because");
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

	[Fact]
	public void GoodBeEquivalentTo()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Alice", Age = 30 };

		subject.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void GoodNotBeEquivalentTo()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Bob", Age = 30 };

		subject.Should().Not.BeEquivalentTo(expected);
	}
}
