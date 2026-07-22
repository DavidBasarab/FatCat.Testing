using FatCat.Testing.Formatting;

namespace Tests.FatCat.Testing.Formatting;

public class ValueFormatterTests : BaseTest
{
	[Fact]
	public void GoodFormatCharIsBareAtTopLevel() { ValueFormatter.Format('c').Should().Be("c"); }

	[Fact]
	public void GoodFormatCharIsQuotedInCollection() { ValueFormatter.Format(new[] { 'a', 'b' }).Should().Be("['a', 'b']"); }

	[Fact]
	public void GoodFormatCollectionCapsAtThirtyTwo()
	{
		var numbers = Enumerable.Range(1, 40).ToList();
		var expected = $"[{string.Join(", ", Enumerable.Range(1, 32))}, …and 8 more]";

		ValueFormatter.Format(numbers).Should().Be(expected);
	}

	[Fact]
	public void GoodFormatCyclicReference()
	{
		var first = new CyclicFirst();
		var second = new CyclicSecond();

		first.Second = second;
		second.First = first;

		ValueFormatter
			.Format(first)
			.Should()
			.Be("CyclicFirst { Second = CyclicSecond { First = { cyclic reference to CyclicFirst } } }");
	}

	[Fact]
	public void GoodFormatDateTimeUsesToString()
	{
		var moment = new DateTime(2026, 7, 22, 10, 30, 0);

		ValueFormatter.Format(moment).Should().Be(moment.ToString());
	}

	[Fact]
	public void GoodFormatDepthCapped()
	{
		var top = new DeepNode();
		var current = top;

		for (var level = 0; level < 6; level++)
		{
			current.Child = new DeepNode();
			current = current.Child;
		}

		ValueFormatter
			.Format(top)
			.Should()
			.Be(
				"DeepNode { Child = DeepNode { Child = DeepNode { Child = DeepNode { Child = DeepNode { Child = DeepNode { Child = { … } } } } } } }"
			);
	}

	[Fact]
	public void GoodFormatEmptyCollection() { ValueFormatter.Format(Array.Empty<int>()).Should().Be("[]"); }

	[Fact]
	public void GoodFormatEnumUsesToString() { ValueFormatter.Format(DateTimeKind.Utc).Should().Be("Utc"); }

	[Fact]
	public void GoodFormatGuidUsesToString()
	{
		var id = new Guid("12345678-1234-1234-1234-1234567890ab");

		ValueFormatter.Format(id).Should().Be(id.ToString());
	}

	[Fact]
	public void GoodFormatIntCollection() { ValueFormatter.Format(new[] { 1, 2, 3 }).Should().Be("[1, 2, 3]"); }

	[Fact]
	public void GoodFormatIntUsesToString() { ValueFormatter.Format(42).Should().Be("42"); }

	[Fact]
	public void GoodFormatNestedObject()
	{
		var nested = new NestedDto { Label = "outer", Inner = new Dto { Name = "Bob", Age = 42 } };

		ValueFormatter.Format(nested).Should().Be("NestedDto { Label = \"outer\", Inner = Dto { Name = \"Bob\", Age = 42 } }");
	}

	[Fact]
	public void GoodFormatNull() { ValueFormatter.Format(null).Should().Be("null"); }

	[Fact]
	public void GoodFormatObjectDumpsProperties()
	{
		var dto = new Dto { Name = "Bob", Age = 42 };

		ValueFormatter.Format(dto).Should().Be("Dto { Name = \"Bob\", Age = 42 }");
	}

	[Fact]
	public void GoodFormatObjectWithNoPropertiesRendersEmptyBraces() { ValueFormatter.Format(new NoProperties()).Should().Be("NoProperties { }"); }

	[Fact]
	public void GoodFormatStringCollectionQuotesElements() { ValueFormatter.Format(new[] { "a", "b" }).Should().Be("[\"a\", \"b\"]"); }

	[Fact]
	public void GoodFormatStringIsBare() { ValueFormatter.Format("hello").Should().Be("hello"); }

	[Fact]
	public void GoodFormatThrowingPropertyIsCaught()
	{
		ValueFormatter.Format(new ThrowingProperty()).Should().Be("ThrowingProperty { Value = <threw InvalidOperationException> }");
	}

	[Fact]
	public void GoodFormatTypeOverridingToStringUsesIt() { ValueFormatter.Format(new ToStringOverride()).Should().Be("custom-to-string"); }
}