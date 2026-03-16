using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Doubles;

public class NotDoubleComparer(double subject) : NotComparerBase<double, NotDoubleComparer>(subject)
{
	public NotDoubleComparer BeApproximately(double expected, double tolerance, string because = null)
	{
		if (Math.Abs(Subject - expected) <= tolerance)
		{
			CompareException.New(
								because ?? $"{Subject} should not be approximately {expected} within {tolerance}"
								);
		}

		return this;
	}

	public NotDoubleComparer BeGreaterThan(double expected, string because = null)
	{
		if (Subject > expected) { CompareException.New(because ?? $"{Subject} should not be greater than {expected}"); }

		return this;
	}

	public NotDoubleComparer BeInRange(double lower, double upper, string because = null)
	{
		if (Subject >= lower && Subject <= upper) { CompareException.New(because ?? $"{Subject} should not be between {lower} and {upper}"); }

		return this;
	}

	public NotDoubleComparer BeLessThan(double expected, string because = null)
	{
		if (Subject < expected) { CompareException.New(because ?? $"{Subject} should not be less than {expected}"); }

		return this;
	}

	public NotDoubleComparer BeNaN(string because = null)
	{
		if (double.IsNaN(Subject)) { CompareException.New(because ?? $"{Subject} should not be NaN"); }

		return this;
	}

	public NotDoubleComparer BeNegative(string because = null)
	{
		if (Subject < 0) { CompareException.New(because ?? $"{Subject} should not be negative"); }

		return this;
	}

	public NotDoubleComparer BePositive(string because = null)
	{
		if (Subject > 0) { CompareException.New(because ?? $"{Subject} should not be positive"); }

		return this;
	}

	public NotDoubleComparer BeZero(string because = null)
	{
		if (Subject == 0) { CompareException.New(because ?? $"{Subject} should not be zero"); }

		return this;
	}

	public NotDoubleComparer Match(Func<double, bool> predicate, string because = null)
	{
		if (predicate(Subject)) { CompareException.New(because ?? $"{Subject} should not match the predicate"); }

		return this;
	}
}