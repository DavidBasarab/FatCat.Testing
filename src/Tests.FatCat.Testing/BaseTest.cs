using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing;

public abstract class BaseTest
{
	protected void RunCompareFailTest(Action testAction)
	{
		Assert.Throws<CompareException>(testAction);
	}
}
