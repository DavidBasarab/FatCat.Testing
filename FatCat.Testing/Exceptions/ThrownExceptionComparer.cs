namespace FatCat.Testing.Exceptions;

public class ThrownExceptionComparer(Exception thrownException)
{
	public ThrownExceptionComparer Where(Func<Exception, bool> predicate, string because = null)
	{
		if (!predicate(thrownException))
		{
			CompareException.New(because ?? "thrown exception should match the predicate but did not");
		}

		return this;
	}

	public ThrownExceptionComparer WithInnerException<TInner>(string because = null)
		where TInner : Exception
	{
		var inner = thrownException.InnerException;

		if (inner == null)
		{
			CompareException.New(
				because
					?? $"thrown {thrownException.GetType().Name} should have inner exception {typeof(TInner).Name} but had none"
			);
		}

		if (inner is not TInner)
		{
			CompareException.New(
				because
					?? $"thrown {thrownException.GetType().Name} should have inner exception {typeof(TInner).Name} but had {inner.GetType().Name}"
			);
		}

		return new ThrownExceptionComparer(inner);
	}

	public ThrownExceptionComparer WithInnerExceptionExactly<TInner>(string because = null)
		where TInner : Exception
	{
		var inner = thrownException.InnerException;

		if (inner == null)
		{
			CompareException.New(
				because
					?? $"thrown {thrownException.GetType().Name} should have inner exception exactly {typeof(TInner).Name} but had none"
			);
		}

		if (inner.GetType() != typeof(TInner))
		{
			CompareException.New(
				because
					?? $"thrown {thrownException.GetType().Name} should have inner exception exactly {typeof(TInner).Name} but had {inner.GetType().Name}"
			);
		}

		return new ThrownExceptionComparer(inner);
	}

	public ThrownExceptionComparer WithMessage(string expected, string because = null)
	{
		if (thrownException.Message != expected)
		{
			CompareException.New(because ?? $"exception message {thrownException.Message} should be {expected}");
		}

		return this;
	}

	public ThrownExceptionComparer WithParameterName(string expected, string because = null)
	{
		if (thrownException is not ArgumentException argumentException)
		{
			CompareException.New(
				because
					?? $"thrown {thrownException.GetType().Name} should be an ArgumentException to read the parameter name but was not"
			);

			return this;
		}

		if (argumentException.ParamName != expected)
		{
			CompareException.New(
				because ?? $"thrown exception parameter name {argumentException.ParamName} should be {expected}"
			);
		}

		return this;
	}
}
