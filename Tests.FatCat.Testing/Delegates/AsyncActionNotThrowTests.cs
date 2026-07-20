namespace Tests.FatCat.Testing.Delegates;

public class AsyncActionNotThrowTests : BaseTest
{
	[Fact]
	public void GoodNotThrow()
	{
		Func<Task> act = () => Task.CompletedTask;

		act.Should().Not.Throw();
	}

	[Fact]
	public void BadNotThrow()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(() => act.Should().Not.Throw());
	}

	[Fact]
	public void BadNotThrowShowsCorrectMessage()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(() => act.Should().Not.Throw(), "Expected no exception but InvalidOperationException was thrown");
	}

	[Fact]
	public void BadNotThrowWithBecause()
	{
		Func<Task> act = () => Task.FromException(new InvalidOperationException("boom"));

		RunCompareFailTest(() => act.Should().Not.Throw("custom because"), "custom because");
	}
}
