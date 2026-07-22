namespace Tests.FatCat.Testing.Exceptions;

public class ActionThrowWithMessageTests : BaseTest
{
	[Fact]
	public void BadThrowWithMessage()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithMessage("bang"));
	}

	[Fact]
	public void BadThrowWithMessageShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithMessage("bang"), "exception message boom should be bang");
	}

	[Fact]
	public void BadThrowWithMessageWithBecause()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithMessage("bang", "custom because"), "custom because");
	}

	[Fact]
	public void GoodThrowWithMessage()
	{
		Action action = () => throw new InvalidOperationException("boom");

		action.Should().Throw<InvalidOperationException>().WithMessage("boom");
	}
}
