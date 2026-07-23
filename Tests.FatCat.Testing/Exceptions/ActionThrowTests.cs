namespace Tests.FatCat.Testing.Exceptions;

public class ActionThrowTests : BaseTest
{
	[Fact]
	public void BadNotThrow()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Not.Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadNotThrowShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(
			() => action.Should().Not.Throw<InvalidOperationException>(),
			"should not throw InvalidOperationException but did"
		);
	}

	[Fact]
	public void BadNotThrowWithBecause()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Not.Throw<InvalidOperationException>("custom because"), "custom because");
	}

	[Fact]
	public void BadThrow()
	{
		Action action = () => { };

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadThrowShowsCorrectMessage()
	{
		Action action = () => { };

		RunCompareFailTest(
			() => action.Should().Throw<InvalidOperationException>(),
			"should throw InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public void BadThrowWhenWrongTypeShowsCorrectMessage()
	{
		Action action = () => throw new ArgumentException("bad arg");

		RunCompareFailTest(
			() => action.Should().Throw<InvalidOperationException>(),
			"should throw InvalidOperationException but threw ArgumentException"
		);
	}

	[Fact]
	public void BadThrowWithBecause()
	{
		Action action = () => { };

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrow()
	{
		Action action = () => { };

		action.Should().Not.Throw<InvalidOperationException>();
	}

	[Fact]
	public void GoodThrow()
	{
		Action action = () => throw new InvalidOperationException("boom");

		action.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void GoodThrowMatchesDerivedType()
	{
		Action action = () => throw new ArgumentNullException("param");

		action.Should().Throw<ArgumentException>();
	}
}
