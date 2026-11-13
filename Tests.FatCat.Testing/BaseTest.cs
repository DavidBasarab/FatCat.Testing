using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing;

public abstract class BaseTest
{
	protected void RunCompareFailTest(Action testAction)
	{
		Assert.Throws<CompareException>(testAction);
	}

	protected void RunCompareFailTest(Action testAction, string message)
	{
		var exception = Assert.Throws<CompareException>(testAction);

		Assert.Equal(message, exception.Message);
	}

	protected async Task RunCompareFailTestAsync(Func<Task> testAction, string message)
	{
		var exception = await Assert.ThrowsAsync<CompareException>(testAction);

		Assert.Equal(message, exception.Message);
	}
}
