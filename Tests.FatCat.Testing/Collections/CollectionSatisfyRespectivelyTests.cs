namespace Tests.FatCat.Testing.Collections;

public class CollectionSatisfyRespectivelyTests : BaseTest
{
	[Fact]
	public void GoodSatisfyRespectively()
	{
		new List<int> { 1, 2 }
			.Should()
			.SatisfyRespectively(value => value.Should().Be(1), value => value.Should().Be(2));
	}

	[Fact]
	public void BadSatisfyRespectivelyCountMismatch()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2 }
				.Should()
				.SatisfyRespectively(value => value.Should().Be(1))
		);
	}

	[Fact]
	public void BadSatisfyRespectivelyCountMismatchShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.SatisfyRespectively(value => value.Should().Be(1)),
			"[1, 2] should satisfy 1 inspectors respectively but has 2 elements"
		);
	}

	[Fact]
	public void BadSatisfyRespectivelyInspectorFails()
	{
		RunCompareFailTest(() =>
			new List<int> { 1, 2 }
				.Should()
				.SatisfyRespectively(value => value.Should().Be(1), value => value.Should().Be(5))
		);
	}

	[Fact]
	public void BadSatisfyRespectivelyInspectorFailsShowsCorrectMessage()
	{
		RunCompareFailTest(
			() =>
				new List<int> { 1, 2 }
					.Should()
					.SatisfyRespectively(value => value.Should().Be(1), value => value.Should().Be(5)),
			"[1, 2] should satisfy 2 inspectors respectively but element at index 1 did not"
		);
	}

	[Fact]
	public void BadSatisfyRespectivelyOnNull()
	{
		List<int> subject = null;

		RunCompareFailTest(
			() => subject.Should().SatisfyRespectively(value => value.Should().Be(1)),
			"null should satisfy 1 inspectors respectively but has 0 elements"
		);
	}
}
