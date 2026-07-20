using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Delegates;

public class AsyncActionComparer(Func<Task> subject) : ComparerBase<Func<Task>, AsyncActionComparer>(subject)
{
	private Exception caughtException;

	public NotAsyncActionComparer Not { get; } = new(subject);

	public AsyncActionComparer Throw<TException>(string because = null)
		where TException : Exception
	{
		caughtException = Capture();

		if (caughtException is not TException)
		{
			var failure = caughtException is null
				? $"Expected {typeof(TException).Name} but no exception was thrown"
				: $"Expected {typeof(TException).Name} but {caughtException.GetType().Name} was thrown";

			CompareException.New(because ?? failure);
		}

		return this;
	}

	public AsyncActionComparer WithMessage(string expected, string because = null)
	{
		if (caughtException?.Message != expected)
		{
			CompareException.New(
				because
					?? $"Expected exception message {ValueFormatter.Format(expected)} but found {ValueFormatter.Format(caughtException?.Message)}"
			);
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
