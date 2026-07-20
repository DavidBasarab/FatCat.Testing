using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Floats;

public class NotFloatComparer(float subject) : NotComparerBase<float, NotFloatComparer>(subject)
{
	public NotFloatComparer BeApproximately(float expected, float tolerance, string because = null)
	{
		if (MathF.Abs(Subject - expected) <= tolerance)
		{
			CompareException.New(
								because ?? $"{Subject} should not be approximately {expected} within {tolerance}"
								);
		}

		return this;
	}

	public NotFloatComparer BeGreaterThan(float expected, string because = null)
	{
		if (Subject > expected) { CompareException.New(because ?? $"{Subject} should not be greater than {expected}"); }

		return this;
	}

	public NotFloatComparer BeInRange(float lower, float upper, string because = null)
	{
		if (Subject >= lower && Subject <= upper) { CompareException.New(because ?? $"{Subject} should not be between {lower} and {upper}"); }

		return this;
	}

	public NotFloatComparer BeLessThan(float expected, string because = null)
	{
		if (Subject < expected) { CompareException.New(because ?? $"{Subject} should not be less than {expected}"); }

		return this;
	}

	public NotFloatComparer BeNaN(string because = null)
	{
		if (float.IsNaN(Subject)) { CompareException.New(because ?? $"{Subject} should not be NaN"); }

		return this;
	}

	public NotFloatComparer BeNegative(string because = null)
	{
		if (Subject < 0) { CompareException.New(because ?? $"{Subject} should not be negative"); }

		return this;
	}

	public NotFloatComparer BePositive(string because = null)
	{
		if (Subject > 0) { CompareException.New(because ?? $"{Subject} should not be positive"); }

		return this;
	}

	public NotFloatComparer BeZero(string because = null)
	{
		if (Subject == 0) { CompareException.New(because ?? $"{Subject} should not be zero"); }

		return this;
	}

	public NotFloatComparer Match(Func<float, bool> predicate, string because = null)
	{
		if (predicate(Subject)) { CompareException.New(because ?? $"{Subject} should not match the predicate"); }

		return this;
	}
}