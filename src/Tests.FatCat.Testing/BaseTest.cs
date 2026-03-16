using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing;

public abstract class BaseTest
{
	protected void RunCompareFailTest(Action testAction) { Assert.Throws<CompareException>(testAction); }

	protected void RunCompareFailTest(Action testAction, string message)
	{
		var exception = Assert.Throws<CompareException>(testAction);

		Assert.Equal(message, exception.Message);
	}
}