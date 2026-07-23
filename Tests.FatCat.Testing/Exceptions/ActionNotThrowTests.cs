namespace Tests.FatCat.Testing.Exceptions;

public class ActionNotThrowTests : BaseTest
{
	[Fact]
	public void BadNotThrow()
	{
		Action action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().NotThrow());
	}

	[Fact]
	public void BadNotThrowShowsCorrectMessage()
	{
		Action action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().NotThrow(), "should not throw but threw ArgumentException: boom");
	}

	[Fact]
	public void BadNotThrowWithBecause()
	{
		Action action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().NotThrow("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrow()
	{
		Action action = () => { };

		action.Should().NotThrow();
	}
}
