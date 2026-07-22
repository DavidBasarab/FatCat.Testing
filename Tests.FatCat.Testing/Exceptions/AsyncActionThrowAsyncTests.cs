namespace Tests.FatCat.Testing.Exceptions;

public class AsyncActionThrowAsyncTests : BaseTest
{
	[Fact]
	public void BadNotThrowAsync()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Not.ThrowAsync<InvalidOperationException>());
	}

	[Fact]
	public void BadNotThrowAsyncShowsCorrectMessage()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Not.ThrowAsync<InvalidOperationException>(), "should not throw InvalidOperationException but did");
	}

	[Fact]
	public void BadNotThrowAsyncWithBecause()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Not.ThrowAsync<InvalidOperationException>("custom because"), "custom because");
	}

	[Fact]
	public void BadThrowAsync()
	{
		Func<Task> action = () => Task.CompletedTask;

		RunCompareFailTest(() => action.Should().ThrowAsync<InvalidOperationException>());
	}

	[Fact]
	public void BadThrowAsyncShowsCorrectMessage()
	{
		Func<Task> action = () => Task.CompletedTask;

		RunCompareFailTest(() => action.Should().ThrowAsync<InvalidOperationException>(), "should throw InvalidOperationException but no exception was thrown");
	}

	[Fact]
	public void BadThrowAsyncWhenWrongTypeShowsCorrectMessage()
	{
		Func<Task> action = () => Task.FromException(new ArgumentException("bad arg"));

		RunCompareFailTest(() => action.Should().ThrowAsync<InvalidOperationException>(), "should throw InvalidOperationException but threw ArgumentException");
	}

	[Fact]
	public void BadThrowAsyncWithBecause()
	{
		Func<Task> action = () => Task.CompletedTask;

		RunCompareFailTest(() => action.Should().ThrowAsync<InvalidOperationException>("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrowAsync()
	{
		Func<Task> action = () => Task.CompletedTask;

		action.Should().Not.ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public void GoodThrowAsync()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		action.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public void GoodThrowAsyncFromAsyncMethod()
	{
		Func<Task> action = async () =>
		{
			await Task.Yield();

			throw new InvalidOperationException("boom");
		};

		action.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public void GoodThrowAsyncUnwrapsAggregateException()
	{
		Func<Task> action = () => Task.FromException(new InvalidOperationException("boom"));

		action.Should().ThrowAsync<InvalidOperationException>();
	}
}
