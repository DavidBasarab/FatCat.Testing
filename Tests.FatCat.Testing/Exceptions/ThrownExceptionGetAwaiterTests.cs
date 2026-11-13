namespace Tests.FatCat.Testing.Exceptions;

public class ThrownExceptionGetAwaiterTests : BaseTest
{
	[Fact]
	public async Task AwaitReturnsTheThrownExceptionComparer()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");
		var comparer = action.Should().ThrowAsync<InvalidOperationException>();

		var awaited = await comparer;

		awaited.Should().BeSameAs(comparer);
	}

	[Fact]
	public async Task BadAwaitThrowAsync()
	{
		Func<Task> action = () => Task.CompletedTask;

		await RunCompareFailTestAsync(
			async () => await action.Should().ThrowAsync<InvalidOperationException>(),
			"should throw InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public async Task BadAwaitThrowAsyncWithMessage()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await RunCompareFailTestAsync(
			async () => await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("bang"),
			"exception message boom should be bang"
		);
	}

	[Fact]
	public async Task BadAwaitThrowExactlyAsync()
	{
		Func<Task> action = () => throw new ArgumentNullException("name");

		await RunCompareFailTestAsync(
			async () => await action.Should().ThrowExactlyAsync<ArgumentException>(),
			"should throw exactly ArgumentException but threw ArgumentNullException"
		);
	}

	[Fact]
	public async Task GoodAwaitThrowAsync()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await action.Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task GoodAwaitThrowAsyncWithMessage()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("boom");
	}

	[Fact]
	public async Task GoodAwaitThrowExactlyAsync()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await action.Should().ThrowExactlyAsync<InvalidOperationException>();
	}
}
