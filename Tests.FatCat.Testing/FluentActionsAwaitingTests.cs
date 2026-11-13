using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing;

public class FluentActionsAwaitingTests : BaseTest
{
	[Fact]
	public void AwaitingBindsToAsyncActionComparer()
	{
		var comparer = FluentActions.Awaiting(() => Task.CompletedTask).Should();

		Assert.IsType<AsyncActionComparer>(comparer);
	}

	[Fact]
	public void AwaitingDoesNotRunTheFunction()
	{
		var wallet = new Wallet(100);

		FluentActions.Awaiting(() => wallet.WithdrawAsync(10));

		wallet.Balance.Should().Be(100);
	}

	[Fact]
	public void BadAwaitingThrowAsync()
	{
		var wallet = new Wallet(100);

		RunCompareFailTest(
			() => FluentActions.Awaiting(() => wallet.WithdrawAsync(10)).Should().ThrowAsync<InvalidOperationException>(),
			"should throw InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public void BadSubjectAwaitingThrowAsync()
	{
		var wallet = new Wallet(100);

		RunCompareFailTest(
			() => wallet.Awaiting(subject => subject.WithdrawAsync(10)).Should().ThrowAsync<InvalidOperationException>(),
			"should throw InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public async Task GoodAwaitedAwaitingThrowAsync()
	{
		var wallet = new Wallet(100);

		await FluentActions.Awaiting(() => wallet.WithdrawAsync(500)).Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task GoodAwaitedSubjectAwaitingThrowAsync()
	{
		var wallet = new Wallet(100);

		await wallet.Awaiting(subject => subject.WithdrawAsync(500)).Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public void GoodAwaitingNotThrowAsync()
	{
		var wallet = new Wallet(100);

		FluentActions.Awaiting(() => wallet.WithdrawAsync(10)).Should().NotThrowAsync();
	}

	[Fact]
	public void GoodAwaitingThrowAsync()
	{
		var wallet = new Wallet(100);

		FluentActions.Awaiting(() => wallet.WithdrawAsync(500)).Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public void GoodSubjectAwaitingNotThrowAsync()
	{
		var wallet = new Wallet(100);

		wallet.Awaiting(subject => subject.WithdrawAsync(10)).Should().NotThrowAsync();
	}

	[Fact]
	public void GoodSubjectAwaitingThrowAsync()
	{
		var wallet = new Wallet(100);

		wallet.Awaiting(subject => subject.WithdrawAsync(500)).Should().ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public void SubjectAwaitingBindsToAsyncActionComparer()
	{
		var comparer = new Wallet(100).Awaiting(subject => subject.WithdrawAsync(10)).Should();

		Assert.IsType<AsyncActionComparer>(comparer);
	}

	[Fact]
	public void SubjectAwaitingDoesNotRunTheFunction()
	{
		var wallet = new Wallet(100);

		wallet.Awaiting(subject => subject.WithdrawAsync(10));

		wallet.Balance.Should().Be(100);
	}

	[Fact]
	public async Task SubjectAwaitingRunsTheFunctionAgainstTheSubject()
	{
		var wallet = new Wallet(100);
		var function = wallet.Awaiting(subject => subject.WithdrawAsync(10));

		await function();

		wallet.Balance.Should().Be(90);
	}
}
