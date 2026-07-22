using FatCat.Testing.Comparers;

namespace FatCat.Testing.Exceptions;

public class AsyncActionComparer(Func<Task> subject) : ComparerBase<Func<Task>, AsyncActionComparer>(subject)
{
	public NotAsyncActionComparer Not { get; } = new(subject);

	public AsyncActionComparer NotThrowAsync(string because = null)
	{
		var exception = RunAndCaptureException(Subject);

		if (exception != null) { CompareException.New(because ?? $"should not throw but threw {exception.GetType().Name}: {exception.Message}"); }

		return this;
	}

	public ThrownExceptionComparer ThrowAsync<TException>(string because = null)
		where TException : Exception
	{
		var exception = RunAndCaptureException(Subject);

		if (exception == null) { CompareException.New(because ?? $"should throw {typeof(TException).Name} but no exception was thrown"); }

		if (exception is not TException) { CompareException.New(because ?? $"should throw {typeof(TException).Name} but threw {exception.GetType().Name}"); }

		return new ThrownExceptionComparer(exception);
	}

	// The fluent assertion surface is synchronous by design (async.md). Observing the task here is the
	// single, deliberate blocking call in the library; no other file may block. GetAwaiter().GetResult()
	// unwraps rather than wrapping in AggregateException, so the caught type is the type the author expects.
	internal static Exception RunAndCaptureException(Func<Task> subject)
	{
		try
		{
			subject().GetAwaiter().GetResult();

			return null;
		}
		catch (Exception exception)
		{
			return exception;
		}
	}
}
