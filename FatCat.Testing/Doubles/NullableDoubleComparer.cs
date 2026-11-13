using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Doubles;

public class NullableDoubleComparer(double? subject) : ComparerBase<double?, NullableDoubleComparer>(subject)
{
	public NotNullableDoubleComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get { return Subject.HasValue ? $"{Subject.Value}" : "null"; }
	}

	public NullableDoubleComparer Be(double expected, string because = null)
	{
		if (!Subject.HasValue || !Subject.Value.Equals(expected))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be {expected}");
		}

		return this;
	}

	public NullableDoubleComparer BeApproximately(double expected, double tolerance, string because = null)
	{
		if (!Subject.HasValue || Math.Abs(Subject.Value - expected) > tolerance)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be approximately {expected} within {tolerance}");
		}

		return this;
	}

	public NullableDoubleComparer BeGreaterThan(double expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value <= expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be greater than {expected}");
		}

		return this;
	}

	public NullableDoubleComparer BeGreaterThanOrEqualTo(double expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value < expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be greater than or equal to {expected}");
		}

		return this;
	}

	public NullableDoubleComparer BeInRange(double lower, double upper, string because = null)
	{
		if (!Subject.HasValue || Subject.Value < lower || Subject.Value > upper)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be between {lower} and {upper}");
		}

		return this;
	}

	public NullableDoubleComparer BeLessThan(double expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value >= expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be less than {expected}");
		}

		return this;
	}

	public NullableDoubleComparer BeLessThanOrEqualTo(double expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value > expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be less than or equal to {expected}");
		}

		return this;
	}

	public NullableDoubleComparer BeNegative(string because = null)
	{
		if (!Subject.HasValue || Subject.Value >= 0)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be negative");
		}

		return this;
	}

	public NullableDoubleComparer BeNull(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value} should be null");
		}

		return this;
	}

	public NullableDoubleComparer BePositive(string because = null)
	{
		if (!Subject.HasValue || Subject.Value <= 0)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be positive");
		}

		return this;
	}

	public NullableDoubleComparer BeZero(string because = null)
	{
		if (!Subject.HasValue || Subject.Value != 0)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be zero");
		}

		return this;
	}

	public NullableDoubleComparer HaveValue(string because = null)
	{
		if (!Subject.HasValue)
		{
			CompareException.New(because ?? "value should not be null");
		}

		return this;
	}

	public NullableDoubleComparer Match(Func<double, bool> predicate, string because = null)
	{
		if (!Subject.HasValue || !predicate(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} did not match the predicate");
		}

		return this;
	}
}
