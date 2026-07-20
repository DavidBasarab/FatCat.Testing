using FatCat.Testing.Formatting;

namespace Tests.FatCat.Testing.Formatting;

public class ValueFormatterTests : BaseTest
{
	[Fact]
	public void FormatsNullAsNull()
	{
		ValueFormatter.Format(null).Should().Be("null");
	}

	[Fact]
	public void FormatsStringQuoted()
	{
		ValueFormatter.Format("hello").Should().Be("\"hello\"");
	}

	[Fact]
	public void FormatsCharQuoted()
	{
		ValueFormatter.Format('a').Should().Be("'a'");
	}

	[Fact]
	public void FormatsIntUnchanged()
	{
		ValueFormatter.Format(3).Should().Be("3");
	}

	[Fact]
	public void FormatsBoolUnchanged()
	{
		ValueFormatter.Format(false).Should().Be("False");
	}

	[Fact]
	public void FormatsGuidUnchanged()
	{
		var id = new Guid("11111111-1111-1111-1111-111111111111");

		ValueFormatter.Format(id).Should().Be("11111111-1111-1111-1111-111111111111");
	}

	[Fact]
	public void FormatsEmptyEnumerable()
	{
		List<int> empty = [];

		ValueFormatter.Format(empty).Should().Be("{ }");
	}

	[Fact]
	public void FormatsEnumerableElementwise()
	{
		List<string> words = ["a", "b"];

		ValueFormatter.Format(words).Should().Be("{ \"a\", \"b\" }");
	}

	[Fact]
	public void CapsLongEnumerable()
	{
		List<int> many = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

		ValueFormatter.Format(many).Should().Be("{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, … }");
	}

	[Fact]
	public void FormatsObjectWithMembers()
	{
		var sample = new FormatterSample { Name = "Bob", Age = 42 };

		ValueFormatter.Format(sample).Should().Be("FormatterSample { Name = \"Bob\", Age = 42 }");
	}

	[Fact]
	public void BoundsObjectDepth()
	{
		var deep = new DepthNode { Child = new DepthNode { Child = new DepthNode { Child = new DepthNode() } } };

		ValueFormatter.Format(deep).Should().Be("DepthNode { Child = DepthNode { Child = DepthNode { Child = … } } }");
	}

	[Fact]
	public void HandlesCycleWithoutStackOverflow()
	{
		var node = new DepthNode();
		node.Child = node;

		ValueFormatter.Format(node).Should().Be("DepthNode { Child = … }");
	}

	[Fact]
	public void ExistingBeMessageUnchanged()
	{
		RunCompareFailTest(() => false.Should().Be(true), "False should be True");
	}

	[Fact]
	public void ExistingBeOfTypeMessageUnchanged()
	{
		RunCompareFailTest(() => 3.Should().BeOfType<string>(), "3 should be of type String");
	}
}
