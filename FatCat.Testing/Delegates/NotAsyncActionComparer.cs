using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Delegates;

public class NotAsyncActionComparer(Func<Task> subject) : NotComparerBase<Func<Task>, NotAsyncActionComparer>(subject)
{
	public NotAsyncActionComparer Throw(string because = null)
	{
		var caught = Capture();

		if (caught is not null)
		{
			CompareException.New(because ?? $"Expected no exception but {caught.GetType().Name} was thrown");
		}

		return this;
	}

	public NotAsyncActionComparer Throw<TException>(string because = null)
		where TException : Exception
	{
		var caught = Capture();

		if (caught is TException)
		{
			CompareException.New(because ?? $"Expected no {typeof(TException).Name} but {caught.GetType().Name} was thrown");
		}

		return this;
	}

	// The fluent Should() surface is synchronous by design (async.md): the comparer observes the
	// task, it does not make the assertion API async. Blocking on the returned task with
	// GetAwaiter().GetResult() is the one permitted blocking call, isolated to this single method,
	// because the top-level sync entry point cannot become async without changing the public surface.
	private Exception Capture()
	{
		try
		{
			Subject().GetAwaiter().GetResult();

			return null;
		}
		catch (Exception exception)
		{
			return exception;
		}
	}
}
