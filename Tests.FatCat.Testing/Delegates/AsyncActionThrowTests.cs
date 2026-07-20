namespace Tests.FatCat.Testing.Delegates;

public class AsyncActionThrowTests : BaseTest
{
	[Fact]
	public void GoodThrow()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		act.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void GoodThrowDerived()
	{
		Func<Task> act = () => Task.FromException(new ArgumentNullException("name"));

		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void BadThrowNoException()
	{
		Func<Task> act = () => Task.CompletedTask;

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadThrowNoExceptionShowsCorrectMessage()
	{
		Func<Task> act = () => Task.CompletedTask;

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>(),
			"Expected InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public void BadThrowWrongType()
	{
		Func<Task> act = () => Task.FromException(new ArgumentException("boom"));

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadThrowWrongTypeShowsCorrectMessage()
	{
		Func<Task> act = () => Task.FromException(new ArgumentException("boom"));

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>(),
			"Expected InvalidOperationException but ArgumentException was thrown"
		);
	}

	[Fact]
	public void BadThrowWithBecause()
	{
		Func<Task> act = () => Task.CompletedTask;

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrow()
	{
		Func<Task> act = () => Task.FromException(new ArgumentException("boom"));

		act.Should().Not.Throw<InvalidOperationException>();
	}

	[Fact]
	public void BadNotThrow()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(() => act.Should().Not.Throw<InvalidOperationException>());
	}

	[Fact]
	public void BadNotThrowShowsCorrectMessage()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(
			() => act.Should().Not.Throw<InvalidOperationException>(),
			"Expected no InvalidOperationException but InvalidOperationException was thrown"
		);
	}

	[Fact]
	public void BadNotThrowWithBecause()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(() => act.Should().Not.Throw<InvalidOperationException>("custom because"), "custom because");
	}
}
