namespace Tests.FatCat.Testing.Delegates;

public class AsyncActionThrowWithMessageTests : BaseTest
{
	[Fact]
	public void GoodWithMessage()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		act.Should().Throw<InvalidOperationException>().WithMessage("boom");
	}

	[Fact]
	public void BadWithMessage()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>().WithMessage("bang"));
	}

	[Fact]
	public void BadWithMessageShowsCorrectMessage()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>().WithMessage("bang"),
			"Expected exception message \"bang\" but found \"boom\""
		);
	}

	[Fact]
	public void BadWithMessageWithBecause()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>().WithMessage("bang", "custom because"),
			"custom because"
		);
	}
}
