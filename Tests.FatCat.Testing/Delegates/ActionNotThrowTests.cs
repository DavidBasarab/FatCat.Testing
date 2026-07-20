namespace Tests.FatCat.Testing.Delegates;

public class ActionNotThrowTests : BaseTest
{
	[Fact]
	public void GoodNotThrow()
	{
		Action act = () => { };

		act.Should().Not.Throw();
	}

	[Fact]
	public void BadNotThrow()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => act.Should().Not.Throw());
	}

	[Fact]
	public void BadNotThrowShowsCorrectMessage()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => act.Should().Not.Throw(), "Expected no exception but InvalidOperationException was thrown");
	}

	[Fact]
	public void BadNotThrowWithBecause()
	{
		Action act = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => act.Should().Not.Throw("custom because"), "custom because");
	}
}
