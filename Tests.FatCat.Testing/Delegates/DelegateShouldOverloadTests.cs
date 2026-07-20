using FatCat.Testing.Delegates;

namespace Tests.FatCat.Testing.Delegates;

public class DelegateShouldOverloadTests : BaseTest
{
	[Fact]
	public void ActionBindsToActionComparer()
	{
		Action act = () => { };

		var comparer = act.Should();

		Assert.IsType<ActionComparer>(comparer);
	}

	[Fact]
	public void FuncOfTaskBindsToAsyncActionComparer()
	{
		Func<Task> act = () => Task.CompletedTask;

		var comparer = act.Should();

		Assert.IsType<AsyncActionComparer>(comparer);
	}
}
