using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NullableIntComparer(int? subject) : ComparerBase<int?, NullableIntComparer>(subject)
{
	public NotNullableIntComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject.HasValue ? $"{Subject.Value}" : "null";
	}

	public NullableIntComparer Be(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value != expected) { CompareException.New(because ?? $"{SubjectDisplay} should be {expected}"); }

		return this;
	}

	public NullableIntComparer BeAround(int center, int tolerance, string because = null)
	{
		if (!Subject.HasValue || Math.Abs(Subject.Value - center) > tolerance) { CompareException.New(because ?? $"{SubjectDisplay} should be around {center} within {tolerance}"); }

		return this;
	}

	public NullableIntComparer BeGreaterThan(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value < expected) { CompareException.New(because ?? $"{SubjectDisplay} should be greater than {expected}"); }

		return this;
	}

	public NullableIntComparer BeInRange(int lower, int upper, string because = null)
	{
		if (!Subject.HasValue || Subject.Value < lower || Subject.Value > upper) { CompareException.New(because ?? $"{SubjectDisplay} should be between {lower} and {upper}"); }

		return this;
	}

	public NullableIntComparer BeLessThan(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value > expected) { CompareException.New(because ?? $"{SubjectDisplay} should be less than {expected}"); }

		return this;
	}

	public NullableIntComparer BeNegative(string because = null)
	{
		if (!Subject.HasValue || Subject.Value >= 0) { CompareException.New(because ?? $"{SubjectDisplay} should be negative"); }

		return this;
	}

	public NullableIntComparer BeNull(string because = null)
	{
		if (Subject.HasValue) { CompareException.New(because ?? $"{Subject.Value} should be null"); }

		return this;
	}

	public NullableIntComparer BePositive(string because = null)
	{
		if (!Subject.HasValue || Subject.Value <= 0) { CompareException.New(because ?? $"{SubjectDisplay} should be positive"); }

		return this;
	}

	public NullableIntComparer BeZero(string because = null)
	{
		if (!Subject.HasValue || Subject.Value != 0) { CompareException.New(because ?? $"{SubjectDisplay} should be zero"); }

		return this;
	}

	public NullableIntComparer HaveValue(string because = null)
	{
		if (!Subject.HasValue) { CompareException.New(because ?? "value should not be null"); }

		return this;
	}

	public NullableIntComparer Match(Func<int, bool> predicate, string because = null)
	{
		if (!Subject.HasValue || !predicate(Subject.Value)) { CompareException.New(because ?? $"{SubjectDisplay} did not match the predicate"); }

		return this;
	}
}