using FatCat.Testing.Comparers;

namespace FatCat.Testing.Exceptions;

public class ActionComparer(Action subject) : ComparerBase<Action, ActionComparer>(subject)
{
	public NotActionComparer Not { get; } = new(subject);

	public ActionComparer NotThrow(string because = null)
	{
		var exception = CaptureException(Subject);

		if (exception != null) { CompareException.New(because ?? $"should not throw but threw {exception.GetType().Name}: {exception.Message}"); }

		return this;
	}

	public ThrownExceptionComparer Throw<TException>(string because = null)
		where TException : Exception
	{
		var exception = CaptureException(Subject);

		if (exception == null) { CompareException.New(because ?? $"should throw {typeof(TException).Name} but no exception was thrown"); }

		if (exception is not TException) { CompareException.New(because ?? $"should throw {typeof(TException).Name} but threw {exception.GetType().Name}"); }

		return new ThrownExceptionComparer(exception);
	}

	internal static Exception CaptureException(Action subject)
	{
		try
		{
			subject();

			return null;
		}
		catch (Exception exception)
		{
			return exception;
		}
	}
}
