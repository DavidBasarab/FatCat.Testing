using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Doubles;

public class DoubleComparer(double subject) : ComparerBase<double, DoubleComparer>(subject)
{
	public NotDoubleComparer Not { get; } = new(subject);

	public DoubleComparer BeApproximately(double expected, double tolerance, string because = null)
	{
		if (Math.Abs(Subject - expected) > tolerance) { CompareException.New(because ?? $"{Subject} should be approximately {expected} within {tolerance}"); }

		return this;
	}

	public DoubleComparer BeGreaterThan(double expected, string because = null)
	{
		if (Subject <= expected) { CompareException.New(because ?? $"{Subject} should be greater than {expected}"); }

		return this;
	}

	public DoubleComparer BeInRange(double lower, double upper, string because = null)
	{
		if (Subject < lower || Subject > upper) { CompareException.New(because ?? $"{Subject} should be between {lower} and {upper}"); }

		return this;
	}

	public DoubleComparer BeLessThan(double expected, string because = null)
	{
		if (Subject >= expected) { CompareException.New(because ?? $"{Subject} should be less than {expected}"); }

		return this;
	}

	public DoubleComparer BeNaN(string because = null)
	{
		if (!double.IsNaN(Subject)) { CompareException.New(because ?? $"{Subject} should be NaN"); }

		return this;
	}

	public DoubleComparer BeNegative(string because = null)
	{
		if (Subject >= 0) { CompareException.New(because ?? $"{Subject} should be negative"); }

		return this;
	}

	public DoubleComparer BePositive(string because = null)
	{
		if (Subject <= 0) { CompareException.New(because ?? $"{Subject} should be positive"); }

		return this;
	}

	public DoubleComparer BeZero(string because = null)
	{
		if (Subject != 0) { CompareException.New(because ?? $"{Subject} should be zero"); }

		return this;
	}

	public DoubleComparer Match(Func<double, bool> predicate, string because = null)
	{
		if (!predicate(Subject)) { CompareException.New(because ?? $"{Subject} did not match the predicate"); }

		return this;
	}
}