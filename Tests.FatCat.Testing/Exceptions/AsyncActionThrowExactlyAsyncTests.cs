namespace Tests.FatCat.Testing.Exceptions;

public class AsyncActionThrowExactlyAsyncTests : BaseTest
{
	[Fact]
	public void BadNotThrowExactlyAsync()
	{
		Func<Task> action = () => Task.FromException(new ArgumentException("boom"));

		RunCompareFailTest(() => action.Should().Not.ThrowExactlyAsync<ArgumentException>());
	}

	[Fact]
	public void BadNotThrowExactlyAsyncShowsCorrectMessage()
	{
		Func<Task> action = () => Task.FromException(new ArgumentException("boom"));

		RunCompareFailTest(() => action.Should().Not.ThrowExactlyAsync<ArgumentException>(), "should not throw exactly ArgumentException but did");
	}

	[Fact]
	public void BadNotThrowExactlyAsyncWithBecause()
	{
		Func<Task> action = () => Task.FromException(new ArgumentException("boom"));

		RunCompareFailTest(() => action.Should().Not.ThrowExactlyAsync<ArgumentException>("custom because"), "custom because");
	}

	[Fact]
	public void BadThrowExactlyAsync()
	{
		Func<Task> action = () => Task.CompletedTask;

		RunCompareFailTest(() => action.Should().ThrowExactlyAsync<ArgumentException>());
	}

	[Fact]
	public void BadThrowExactlyAsyncShowsCorrectMessage()
	{
		Func<Task> action = () => Task.CompletedTask;

		RunCompareFailTest(() => action.Should().ThrowExactlyAsync<ArgumentException>(), "should throw exactly ArgumentException but no exception was thrown");
	}

	[Fact]
	public void BadThrowExactlyAsyncWhenDerivedType()
	{
		Func<Task> action = () => Task.FromException(new ArgumentNullException("param"));

		RunCompareFailTest(() => action.Should().ThrowExactlyAsync<ArgumentException>());
	}

	[Fact]
	public void BadThrowExactlyAsyncWhenDerivedTypeShowsCorrectMessage()
	{
		Func<Task> action = () => Task.FromException(new ArgumentNullException("param"));

		RunCompareFailTest(() => action.Should().ThrowExactlyAsync<ArgumentException>(), "should throw exactly ArgumentException but threw ArgumentNullException");
	}

	[Fact]
	public void BadThrowExactlyAsyncWithBecause()
	{
		Func<Task> action = () => Task.CompletedTask;

		RunCompareFailTest(() => action.Should().ThrowExactlyAsync<ArgumentException>("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrowExactlyAsync()
	{
		Func<Task> action = () => Task.FromException(new ArgumentNullException("param"));

		action.Should().Not.ThrowExactlyAsync<ArgumentException>();
	}

	[Fact]
	public void GoodThrowExactlyAsync()
	{
		Func<Task> action = () => Task.FromException(new ArgumentException("boom"));

		action.Should().ThrowExactlyAsync<ArgumentException>();
	}

	[Fact]
	public void GoodThrowExactlyAsyncChainsWithMessage()
	{
		Func<Task> action = () => Task.FromException(new ArgumentException("boom"));

		action.Should().ThrowExactlyAsync<ArgumentException>().WithMessage("boom");
	}
}
