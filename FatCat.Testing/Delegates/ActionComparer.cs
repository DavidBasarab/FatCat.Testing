using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Delegates;

public class ActionComparer(Action subject) : ComparerBase<Action, ActionComparer>(subject)
{
	private Exception caughtException;

	public NotActionComparer Not { get; } = new(subject);

	public ActionComparer Throw<TException>(string because = null)
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

	public ActionComparer WithMessage(string expected, string because = null)
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
