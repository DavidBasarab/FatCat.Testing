namespace FatCat.Testing.Exceptions;

public class ThrownExceptionComparer(Exception thrownException)
{
	public ThrownExceptionComparer WithMessage(string expected, string because = null)
	{
		if (thrownException.Message != expected) { CompareException.New(because ?? $"exception message {thrownException.Message} should be {expected}"); }

		return this;
	}
}
