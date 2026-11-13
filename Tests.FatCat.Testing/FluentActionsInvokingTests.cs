using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing;

public class FluentActionsInvokingTests : BaseTest
{
	[Fact]
	public void BadInvokingThrow()
	{
		var wallet = new Wallet(100);

		RunCompareFailTest(
			() => FluentActions.Invoking(() => wallet.Withdraw(10)).Should().Throw<InvalidOperationException>(),
			"should throw InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public void BadSubjectInvokingThrow()
	{
		var wallet = new Wallet(100);

		RunCompareFailTest(
			() => wallet.Invoking(subject => subject.Withdraw(10)).Should().Throw<InvalidOperationException>(),
			"should throw InvalidOperationException but no exception was thrown"
		);
	}

	[Fact]
	public void GoodInvokingNotThrow()
	{
		var wallet = new Wallet(100);

		FluentActions.Invoking(() => wallet.Withdraw(10)).Should().NotThrow();
	}

	[Fact]
	public void GoodInvokingThrow()
	{
		var wallet = new Wallet(100);

		FluentActions.Invoking(() => wallet.Withdraw(500)).Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void GoodInvokingThrowWithMessage()
	{
		var wallet = new Wallet(100);

		FluentActions
			.Invoking(() => wallet.Withdraw(500))
			.Should()
			.Throw<InvalidOperationException>()
			.WithMessage("insufficient funds");
	}

	[Fact]
	public void GoodSubjectInvokingNotThrow()
	{
		var wallet = new Wallet(100);

		wallet.Invoking(subject => subject.Withdraw(10)).Should().NotThrow();
	}

	[Fact]
	public void GoodSubjectInvokingThrow()
	{
		var wallet = new Wallet(100);

		wallet.Invoking(subject => subject.Withdraw(500)).Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void InvokingBindsToActionComparer()
	{
		var comparer = FluentActions.Invoking(() => { }).Should();

		Assert.IsType<ActionComparer>(comparer);
	}

	[Fact]
	public void InvokingDoesNotRunTheAction()
	{
		var wallet = new Wallet(100);

		FluentActions.Invoking(() => wallet.Withdraw(10));

		wallet.Balance.Should().Be(100);
	}

	[Fact]
	public void SubjectInvokingBindsToActionComparer()
	{
		var comparer = new Wallet(100).Invoking(subject => subject.Withdraw(10)).Should();

		Assert.IsType<ActionComparer>(comparer);
	}

	[Fact]
	public void SubjectInvokingDoesNotRunTheAction()
	{
		var wallet = new Wallet(100);

		wallet.Invoking(subject => subject.Withdraw(10));

		wallet.Balance.Should().Be(100);
	}

	[Fact]
	public void SubjectInvokingRunsTheActionAgainstTheSubject()
	{
		var wallet = new Wallet(100);
		var action = wallet.Invoking(subject => subject.Withdraw(10));

		action();

		wallet.Balance.Should().Be(90);
	}
}
