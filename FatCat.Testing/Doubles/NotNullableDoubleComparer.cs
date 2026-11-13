using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Doubles;

public class NotNullableDoubleComparer(double? subject) : NotComparerBase<double?, NotNullableDoubleComparer>(subject)
{
	public NotNullableDoubleComparer Be(double expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Equals(expected))
		{
			CompareException.New(because ?? $"{Subject.Value} should not be {expected}");
		}

		return this;
	}

	public NotNullableDoubleComparer BeApproximately(double expected, double tolerance, string because = null)
	{
		if (Subject.HasValue && Math.Abs(Subject.Value - expected) <= tolerance)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be approximately {expected} within {tolerance}");
		}

		return this;
	}

	public NotNullableDoubleComparer BeGreaterThan(double expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value > expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be greater than {expected}");
		}

		return this;
	}

	public NotNullableDoubleComparer BeGreaterThanOrEqualTo(double expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value >= expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be greater than or equal to {expected}");
		}

		return this;
	}

	public NotNullableDoubleComparer BeInRange(double lower, double upper, string because = null)
	{
		if (Subject.HasValue && Subject.Value >= lower && Subject.Value <= upper)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be between {lower} and {upper}");
		}

		return this;
	}

	public NotNullableDoubleComparer BeLessThan(double expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value < expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be less than {expected}");
		}

		return this;
	}

	public NotNullableDoubleComparer BeLessThanOrEqualTo(double expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value <= expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be less than or equal to {expected}");
		}

		return this;
	}

	public NotNullableDoubleComparer BeNegative(string because = null)
	{
		if (Subject.HasValue && Subject.Value < 0)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be negative");
		}

		return this;
	}

	public NotNullableDoubleComparer BePositive(string because = null)
	{
		if (Subject.HasValue && Subject.Value > 0)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be positive");
		}

		return this;
	}

	public NotNullableDoubleComparer BeZero(string because = null)
	{
		if (Subject.HasValue && Subject.Value == 0)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be zero");
		}

		return this;
	}

	public NotNullableDoubleComparer HaveValue(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have a value");
		}

		return this;
	}

	public NotNullableDoubleComparer Match(Func<double, bool> predicate, string because = null)
	{
		if (Subject.HasValue && predicate(Subject.Value))
		{
			CompareException.New(because ?? $"{Subject.Value} should not match the predicate");
		}

		return this;
	}
}
