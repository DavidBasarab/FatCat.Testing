using FatCat.Testing;
using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing;

public class NumberTests : BaseTest
{
	[Fact]
	public void GoodEqual()
	{
		new NumberComparer().Compare(1, 1);
	}

	[Fact]
	public void BasicFail()
	{
		RunCompareFailTest(() => new NumberComparer().Compare(1, 2));
	}
}

public abstract class BaseTest
{
	protected void RunCompareFailTest(Action testAction)
	{
		Assert.Throws<CompareException>(testAction);
	}
}
