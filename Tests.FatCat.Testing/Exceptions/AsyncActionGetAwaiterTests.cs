namespace Tests.FatCat.Testing.Exceptions;

public class AsyncActionGetAwaiterTests : BaseTest
{
	[Fact]
	public async Task AwaitReturnsTheAsyncActionComparer()
	{
		Func<Task> action = () => Task.CompletedTask;
		var comparer = action.Should().NotThrowAsync();

		var awaited = await comparer;

		awaited.Should().BeSameAs(comparer);
	}

	[Fact]
	public async Task BadAwaitNotThrowAsync()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await RunCompareFailTestAsync(
			async () => await action.Should().NotThrowAsync(),
			"should not throw but threw InvalidOperationException: boom"
		);
	}

	[Fact]
	public async Task BadAwaitNotThrowAsyncWithBecause()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await RunCompareFailTestAsync(async () => await action.Should().NotThrowAsync("custom because"), "custom because");
	}

	[Fact]
	public async Task GoodAwaitNotThrowAsync()
	{
		Func<Task> action = () => Task.CompletedTask;

		await action.Should().NotThrowAsync();
	}
}
