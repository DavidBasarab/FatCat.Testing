using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing.Exceptions;

public class ExceptionOverloadResolutionTests : BaseTest
{
	[Fact]
	public void ActionBindsToActionComparer()
	{
		// A bare lambda has no type and cannot be an extension-method receiver, so the delegate variable
		// must be declared first before calling Should().
		Action action = () => { };

		var comparer = action.Should();

		Assert.IsType<ActionComparer>(comparer);
	}

	[Fact]
	public void FuncTaskBindsToAsyncActionComparer()
	{
		Func<Task> action = () => Task.CompletedTask;

		var comparer = action.Should();

		Assert.IsType<AsyncActionComparer>(comparer);
	}
}
