namespace Tests.FatCat.Testing.Collections;

public class CollectionAllBeOfTypeTests : BaseTest
{
	[Fact]
	public void GoodAllBeOfType()
	{
		new List<object> { 1, 2, 3 }
			.Should()
			.AllBeOfType<int>();
	}

	[Fact]
	public void GoodAllBeOfTypeWithTypeArgument()
	{
		new List<object> { 1, 2, 3 }
			.Should()
			.AllBeOfType(typeof(int));
	}

	[Fact]
	public void BadAllBeOfType()
	{
		RunCompareFailTest(() =>
			new List<object> { 1, "two", 3 }
				.Should()
				.AllBeOfType<int>()
		);
	}

	[Fact]
	public void BadAllBeOfTypeShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<object> { 1, "two", 3 }
					.Should()
					.AllBeOfType<int>(),
			"[1, \"two\", 3] should have all elements of type System.Int32"
		);
	}

	[Fact]
	public void BadAllBeOfTypeWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<object> { 1, "two", 3 }
					.Should()
					.AllBeOfType<int>("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadAllBeOfTypeOnNull()
	{
		List<object> subject = null;

		RunCompareFailTest(() => subject.Should().AllBeOfType<int>(), "null should have all elements of type System.Int32");
	}
}
