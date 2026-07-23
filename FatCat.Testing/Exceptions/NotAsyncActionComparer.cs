using FatCat.Testing.Comparers;

namespace FatCat.Testing.Exceptions;

public class NotAsyncActionComparer(Func<Task> subject) : NotComparerBase<Func<Task>, NotAsyncActionComparer>(subject)
{
	public NotAsyncActionComparer ThrowAsync<TException>(string because = null)
		where TException : Exception
	{
		var exception = AsyncActionComparer.RunAndCaptureException(Subject);

		if (exception is TException) { CompareException.New(because ?? $"should not throw {typeof(TException).Name} but did"); }

		return this;
	}

	public NotAsyncActionComparer ThrowExactlyAsync<TException>(string because = null)
		where TException : Exception
	{
		var exception = AsyncActionComparer.RunAndCaptureException(Subject);

		if (exception?.GetType() == typeof(TException)) { CompareException.New(because ?? $"should not throw exactly {typeof(TException).Name} but did"); }

		return this;
	}
}
