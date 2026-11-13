namespace Tests.FatCat.Testing.Exceptions;

public class NotAsyncActionGetAwaiterTests : BaseTest
{
	[Fact]
	public async Task AwaitReturnsTheNotAsyncActionComparer()
	{
		Func<Task> action = () => Task.CompletedTask;
		var comparer = action.Should().Not.ThrowAsync<InvalidOperationException>();

		var awaited = await comparer;

		awaited.Should().BeSameAs(comparer);
	}

	[Fact]
	public async Task BadAwaitNotThrowAsync()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await RunCompareFailTestAsync(
			async () => await action.Should().Not.ThrowAsync<InvalidOperationException>(),
			"should not throw InvalidOperationException but did"
		);
	}

	[Fact]
	public async Task BadAwaitNotThrowExactlyAsync()
	{
		Func<Task> action = () => throw new InvalidOperationException("boom");

		await RunCompareFailTestAsync(
			async () => await action.Should().Not.ThrowExactlyAsync<InvalidOperationException>(),
			"should not throw exactly InvalidOperationException but did"
		);
	}

	[Fact]
	public async Task GoodAwaitNotThrowAsync()
	{
		Func<Task> action = () => Task.CompletedTask;

		await action.Should().Not.ThrowAsync<InvalidOperationException>();
	}
}
