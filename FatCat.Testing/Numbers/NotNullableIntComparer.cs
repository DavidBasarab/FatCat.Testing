using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NotNullableIntComparer(int? subject) : NotComparerBase<int?, NotNullableIntComparer>(subject)
{
	public NotNullableIntComparer Be(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value == expected) { CompareException.New(because ?? $"{expected} should not be {Subject.Value}"); }

		return this;
	}

	public NotNullableIntComparer BeGreaterThan(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value > expected) { CompareException.New(because ?? $"{Subject.Value} should not be greater than {expected}"); }

		return this;
	}

	public NotNullableIntComparer BeInRange(int lower, int upper, string because = null)
	{
		if (Subject.HasValue && Subject.Value >= lower && Subject.Value <= upper) { CompareException.New(because ?? $"{Subject.Value} should not be between {lower} and {upper}"); }

		return this;
	}

	public NotNullableIntComparer BeLessThan(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value < expected) { CompareException.New(because ?? $"{Subject.Value} should not be less than {expected}"); }

		return this;
	}

	public NotNullableIntComparer BeNegative(string because = null)
	{
		if (Subject.HasValue && Subject.Value < 0) { CompareException.New(because ?? $"{Subject.Value} should not be negative"); }

		return this;
	}

	public NotNullableIntComparer BePositive(string because = null)
	{
		if (Subject.HasValue && Subject.Value > 0) { CompareException.New(because ?? $"{Subject.Value} should not be positive"); }

		return this;
	}

	public NotNullableIntComparer BeZero(string because = null)
	{
		if (Subject.HasValue && Subject.Value == 0) { CompareException.New(because ?? $"{Subject.Value} should not be zero"); }

		return this;
	}

	public NotNullableIntComparer HaveValue(string because = null)
	{
		if (Subject.HasValue) { CompareException.New(because ?? $"{Subject.Value} should not have a value"); }

		return this;
	}

	public NotNullableIntComparer Match(Func<int, bool> predicate, string because = null)
	{
		if (Subject.HasValue && predicate(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not match the predicate"); }

		return this;
	}
}