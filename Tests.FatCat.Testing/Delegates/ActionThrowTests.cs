namespace Tests.FatCat.Testing.Delegates;

public class ActionThrowTests : BaseTest
{
	[Fact]
	public void GoodThrow()
	{
		Action act = () => throw new InvalidOperationException("boom");

		act.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void GoodThrowDerived()
	{
		Action act = () => throw new ArgumentNullException("name");

		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void BadThrowNoException()
	{
		Action act = () => { };

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadThrowNoExceptionShowsCorrectMessage()
	{
		Action act = () => { };

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>(),
			"Expected InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public void BadThrowWrongType()
	{
		Action act = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadThrowWrongTypeShowsCorrectMessage()
	{
		Action act = () => throw new ArgumentException("boom");

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>(),
			"Expected InvalidOperationException but ArgumentException was thrown"
		);
	}

	[Fact]
	public void BadThrowWithBecause()
	{
		Action act = () => { };

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrow()
	{
		Action act = () => throw new ArgumentException("boom");

		act.Should().Not.Throw<InvalidOperationException>();
	}

	[Fact]
	public void BadNotThrow()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => act.Should().Not.Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadNotThrowShowsCorrectMessage()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(
			() => act.Should().Not.Throw<InvalidOperationException>(),
			"Expected no InvalidOperationException but InvalidOperationException was thrown"
		);
	}

	[Fact]
	public void BadNotThrowWithBecause()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => act.Should().Not.Throw<InvalidOperationException>("custom because"), "custom because");
	}
}
