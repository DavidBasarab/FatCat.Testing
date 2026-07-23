using FatCat.Testing.Equivalency;

namespace Tests.FatCat.Testing.Equivalency;

public class EquivalencyComparerTests : BaseTest
{
	[Fact]
	public void BadCompareCollectionMemberReportsDifference()
	{
		var subject = new TagHolder { Tags = ["alpha", "beta"] };
		var expected = new TagHolder { Tags = ["alpha", "gamma"] };

		var result = EquivalencyComparer.Compare(subject, expected);

		result.AreEquivalent.Should().BeFalse();
		result.Path.Should().Be("Tags");
		result.Difference.Should().Be("could not find match for \"gamma\"");
	}

	[Fact]
	public void BadCompareExpectedNull()
	{
		var result = EquivalencyComparer.Compare("subject", null);

		result.AreEquivalent.Should().BeFalse();
		result.Difference.Should().Be("expected null but found \"subject\"");
	}

	[Fact]
	public void BadCompareReportsDifferingProperty()
	{
		var subject = new Person { Name = "Bob", Age = 30 };
		var expected = new Person { Name = "Alice", Age = 30 };

		var result = EquivalencyComparer.Compare(subject, expected);

		result.AreEquivalent.Should().BeFalse();
		result.Path.Should().Be("Name");
		result.Difference.Should().Be("expected \"Alice\" but found \"Bob\"");
	}

	[Fact]
	public void BadCompareReportsNestedPath()
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

		var result = EquivalencyComparer.Compare(subject, expected);

		result.AreEquivalent.Should().BeFalse();
		result.Path.Should().Be("Address.City");
		result.Difference.Should().Be("expected \"York\" but found \"Leeds\"");
	}

	[Fact]
	public void BadCompareSubjectNull()
	{
		var result = EquivalencyComparer.Compare(null, "expected");

		result.AreEquivalent.Should().BeFalse();
		result.Difference.Should().Be("expected \"expected\" but found null");
	}

	[Fact]
	public void BadCompareThrowingPropertyIsReported()
	{
		var result = EquivalencyComparer.Compare(new ThrowingMember(), new ThrowingMember());

		result.AreEquivalent.Should().BeFalse();
		result.Path.Should().Be("Value");
		result.Difference.Should().Be("<threw InvalidOperationException>");
	}

	[Fact]
	public void GoodCompareBothNull()
	{
		EquivalencyComparer.Compare(null, null).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareCollectionMemberOrderInsensitive()
	{
		var subject = new TagHolder { Tags = ["alpha", "beta"] };
		var expected = new TagHolder { Tags = ["beta", "alpha"] };

		EquivalencyComparer.Compare(subject, expected).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareCollectionMemberRespectsCap()
	{
		var subject = new TagHolder();
		var expected = new TagHolder();

		for (var index = 0; index < 40; index++)
		{
			subject.Tags.Add($"item{index}");
			expected.Tags.Add($"item{index}");
		}

		expected.Tags[35] = "different";

		EquivalencyComparer.Compare(subject, expected).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareCyclicGraph()
	{
		var subjectLeft = new Ring { Name = "left" };
		var subjectRight = new Ring { Name = "right" };
		subjectLeft.Partner = subjectRight;
		subjectRight.Partner = subjectLeft;

		var expectedLeft = new Ring { Name = "left" };
		var expectedRight = new Ring { Name = "right" };
		expectedLeft.Partner = expectedRight;
		expectedRight.Partner = expectedLeft;

		EquivalencyComparer.Compare(subjectLeft, expectedLeft).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareEqualScalars()
	{
		EquivalencyComparer.Compare(5, 5).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareEqualStrings()
	{
		EquivalencyComparer.Compare("hello", "hello").AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareIgnoresFields()
	{
		var subject = new FieldHolder { Id = "same", Tag = "one" };
		var expected = new FieldHolder { Id = "same", Tag = "two" };

		EquivalencyComparer.Compare(subject, expected).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareRespectsDepthCap()
	{
		var subject = new DeepChain();
		var expected = new DeepChain();
		var subjectTail = subject;
		var expectedTail = expected;

		for (var level = 0; level < 15; level++)
		{
			subjectTail.Next = new DeepChain();
			expectedTail.Next = new DeepChain();
			subjectTail = subjectTail.Next;
			expectedTail = expectedTail.Next;
		}

		subjectTail.Value = 1;
		expectedTail.Value = 2;

		EquivalencyComparer.Compare(subject, expected).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareStructurallyEqualDtos()
	{
		var subject = new Person { Name = "Alice", Age = 30 };
		var expected = new Person { Name = "Alice", Age = 30 };

		EquivalencyComparer.Compare(subject, expected).AreEquivalent.Should().BeTrue();
	}

	[Fact]
	public void GoodCompareUsesEqualsForTypesThatOverrideIt()
	{
		var subject = new Money(10m, "USD");
		var expected = new Money(10m, "GBP");

		EquivalencyComparer.Compare(subject, expected).AreEquivalent.Should().BeTrue();
	}
}
