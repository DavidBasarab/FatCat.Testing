using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Delegates;

public class NotActionComparer(Action subject) : NotComparerBase<Action, NotActionComparer>(subject)
{
	public NotActionComparer Throw(string because = null)
	{
		var caught = Capture();

		if (caught is not null)
		{
			CompareException.New(because ?? $"Expected no exception but {caught.GetType().Name} was thrown");
		}

		return this;
	}

	public NotActionComparer Throw<TException>(string because = null)
		where TException : Exception
	{
		var caught = Capture();

		if (caught is TException)
		{
			CompareException.New(because ?? $"Expected no {typeof(TException).Name} but {caught.GetType().Name} was thrown");
		}

		return this;
	}

	private Exception Capture()
	{
		try
		{
			Subject();

			return null;
		}
		catch (Exception exception)
		{
			return exception;
		}
	}
}
