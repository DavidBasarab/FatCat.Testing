using FatCat.Testing.Comparers;

namespace FatCat.Testing.Exceptions;

public class NotActionComparer(Action subject) : NotComparerBase<Action, NotActionComparer>(subject)
{
	public NotActionComparer Throw<TException>(string because = null)
		where TException : Exception
	{
		var exception = ActionComparer.CaptureException(Subject);

		if (exception is TException) { CompareException.New(because ?? $"should not throw {typeof(TException).Name} but did"); }

		return this;
	}
}
