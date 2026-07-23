namespace Tests.FatCat.Testing.Collections;

public class CollectionBeEquivalentToTests : BaseTest
{
	[Fact]
	public void GoodBeEquivalentToSameOrder()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.BeEquivalentTo(new List<int> { 1, 2, 3 });
	}

	[Fact]
	public void GoodBeEquivalentToDifferentOrder()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.BeEquivalentTo(new List<int> { 3, 2, 1 });
	}

	[Fact]
	public void GoodBeEquivalentToStructuralElements()
	{
		var subject = new List<Gadget>
		{
			new() { Name = "wrench", Weight = 2 },
			new() { Name = "hammer", Weight = 5 },
		};
		var expected = new List<Gadget>
		{
			new() { Name = "hammer", Weight = 5 },
			new() { Name = "wrench", Weight = 2 },
		};

		subject.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void BadBeEquivalentToCountMismatch()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.BeEquivalentTo(new List<int> { 1, 2 })
		);
	}

	[Fact]
	public void BadBeEquivalentToCountMismatchShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.BeEquivalentTo(new List<int> { 1, 2 }),
			"[1, 2, 3] should be equivalent to [1, 2] but they have different counts (3 and 2)"
		);
	}

	[Fact]
	public void BadBeEquivalentToElementMissing()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.BeEquivalentTo(new List<int> { 1, 2, 4 })
		);
	}

	[Fact]
	public void BadBeEquivalentToElementMissingShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.BeEquivalentTo(new List<int> { 1, 2, 4 }),
			"[1, 2, 3] should be equivalent to [1, 2, 4] but could not find a match for 3"
		);
	}

	[Fact]
	public void BadBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.BeEquivalentTo(new List<int> { 1, 2, 4 }, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodNotBeEquivalentTo()
	{
		new List<int> { 1, 2, 3 }
			.Should()
			.Not.BeEquivalentTo(new List<int> { 1, 2, 4 });
	}

	[Fact]
	public void BadNotBeEquivalentTo()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2, 3 }
				.Should()
				.Not.BeEquivalentTo(new List<int> { 3, 2, 1 })
		);
	}

	[Fact]
	public void BadNotBeEquivalentToShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Not.BeEquivalentTo(new List<int> { 3, 2, 1 }),
			"[1, 2, 3] should not be equivalent to [3, 2, 1]"
		);
	}

	[Fact]
	public void BadNotBeEquivalentToWithBecause()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2, 3 }
					.Should()
					.Not.BeEquivalentTo(new List<int> { 3, 2, 1 }, "custom because"),
			"custom because"
		);
	}

	[Fact]
	public void BadBeEquivalentToOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(
			() => subject.Should().BeEquivalentTo(new List<int> { 1, 2, 3 }),
			"null should be equivalent to [1, 2, 3]"
		);
	}
}
