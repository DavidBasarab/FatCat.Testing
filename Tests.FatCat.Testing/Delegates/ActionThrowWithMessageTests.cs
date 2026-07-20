namespace Tests.FatCat.Testing.Delegates;

public class ActionThrowWithMessageTests : BaseTest
{
	[Fact]
	public void GoodWithMessage()
	{
		Action act = () => throw new InvalidOperationException("boom");

		act.Should().Throw<InvalidOperationException>().WithMessage("boom");
	}

	[Fact]
	public void BadWithMessage()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => act.Should().Throw<InvalidOperationException>().WithMessage("bang"));
	}

	[Fact]
	public void BadWithMessageShowsCorrectMessage()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>().WithMessage("bang"),
			"Expected exception message \"bang\" but found \"boom\""
		);
	}

	[Fact]
	public void BadWithMessageWithBecause()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(
			() => act.Should().Throw<InvalidOperationException>().WithMessage("bang", "custom because"),
			"custom because"
		);
	}
}
