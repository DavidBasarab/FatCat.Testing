namespace Tests.FatCat.Testing.Exceptions;

public class ThrownExceptionWithInnerExceptionExactlyTests : BaseTest
{
	[Fact]
	public void BadWithInnerExceptionExactly()
	{
		Action action = () => throw new InvalidOperationException("outer");

		RunCompareFailTest(() =>
			action.Should().Throw<InvalidOperationException>().WithInnerExceptionExactly<ArgumentException>()
		);
	}

	[Fact]
	public void BadWithInnerExceptionExactlyWhenDerivedTypeShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("outer", new ArgumentNullException("param"));

		RunCompareFailTest(
			() => action.Should().Throw<InvalidOperationException>().WithInnerExceptionExactly<ArgumentException>(),
			"thrown InvalidOperationException should have inner exception exactly ArgumentException but had ArgumentNullException"
		);
	}

	[Fact]
	public void BadWithInnerExceptionExactlyWhenNoInnerShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("outer");

		RunCompareFailTest(
			() => action.Should().Throw<InvalidOperationException>().WithInnerExceptionExactly<ArgumentException>(),
			"thrown InvalidOperationException should have inner exception exactly ArgumentException but had none"
		);
	}

	[Fact]
	public void BadWithInnerExceptionExactlyWithBecause()
	{
		Action action = () => throw new InvalidOperationException("outer");

		RunCompareFailTest(
			() =>
				action
					.Should()
					.Throw<InvalidOperationException>()
					.WithInnerExceptionExactly<ArgumentException>("custom because"),
			"custom because"
		);
	}

	[Fact]
	public void GoodWithInnerExceptionExactly()
	{
		Action action = () => throw new InvalidOperationException("outer", new ArgumentException("inner"));

		action.Should().Throw<InvalidOperationException>().WithInnerExceptionExactly<ArgumentException>();
	}

	[Fact]
	public void GoodWithInnerExceptionExactlyChainsWithMessage()
	{
		Action action = () => throw new InvalidOperationException("outer", new ArgumentException("inner"));

		action.Should().Throw<InvalidOperationException>().WithInnerExceptionExactly<ArgumentException>().WithMessage("inner");
	}
}
