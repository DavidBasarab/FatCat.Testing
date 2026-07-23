namespace Tests.FatCat.Testing.Exceptions;

public class AsyncActionNotThrowAsyncTests : BaseTest
{
	[Fact]
	public void BadNotThrowAsync()
	{
		Func<Task> action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().NotThrowAsync());
	}

	[Fact]
	public void BadNotThrowAsyncShowsCorrectMessage()
	{
		Func<Task> action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().NotThrowAsync(), "should not throw but threw ArgumentException: boom");
	}

	[Fact]
	public void BadNotThrowAsyncWithBecause()
	{
		Func<Task> action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().NotThrowAsync("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrowAsync()
	{
		Func<Task> action = () => Task.CompletedTask;

		action.Should().NotThrowAsync();
	}
}
