namespace Tests.FatCat.Testing.Collections;

public class CollectionAllBeEquivalentToTests : BaseTest
{
	[Fact]
	public void GoodAllBeEquivalentTo()
	{
		new List<int> { 2, 2, 2 }
			.Should()
			.AllBeEquivalentTo(2);
	}

	[Fact]
	public void GoodAllBeEquivalentToStructuralElement()
	{
		var subject = new List<Gadget>
		{
			new() { Name = "hammer", Weight = 5 },
			new() { Name = "hammer", Weight = 5 },
		};

		subject.Should().AllBeEquivalentTo(new Gadget { Name = "hammer", Weight = 5 });
	}

	[Fact]
	public void BadAllBeEquivalentTo()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.AllBeEquivalentTo(2)
		);
	}

	[Fact]
	public void BadAllBeEquivalentToShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.AllBeEquivalentTo(2),
			"[1, 2, 3] should have all elements equivalent to 2"
		);
	}

	[Fact]
	public void BadAllBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.AllBeEquivalentTo(2, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadAllBeEquivalentToOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(() => subject.Should().AllBeEquivalentTo(2), "null should have all elements equivalent to 2");
	}
}
