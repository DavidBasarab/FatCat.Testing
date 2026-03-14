using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NotNullableIntComparer(int? subject)
{
	public NotNullableIntComparer Be(int expected, string because = null)
	{
		if (subject.HasValue && subject.Value == expected)
		{
			CompareException.New(because ?? $"{expected} should not be {subject.Value}");
		}

		return this;
	}

	public NotNullableIntComparer BeGreaterThan(int expected, string because = null)
	{
		if (subject.HasValue && subject.Value > expected)
		{
			CompareException.New(because ?? $"{subject.Value} should not be greater than {expected}");
		}

		return this;
	}

	public NotNullableIntComparer BeInRange(int lower, int upper, string because = null)
	{
		if (subject.HasValue && subject.Value >= lower && subject.Value <= upper)
		{
			CompareException.New(because ?? $"{subject.Value} should not be between {lower} and {upper}");
		}

		return this;
	}

	public NotNullableIntComparer BeLessThan(int expected, string because = null)
	{
		if (subject.HasValue && subject.Value < expected)
		{
			CompareException.New(because ?? $"{subject.Value} should not be less than {expected}");
		}

		return this;
	}

	public NotNullableIntComparer BeNegative(string because = null)
	{
		if (subject.HasValue && subject.Value < 0)
		{
			CompareException.New(because ?? $"{subject.Value} should not be negative");
		}

		return this;
	}

	public NotNullableIntComparer BePositive(string because = null)
	{
		if (subject.HasValue && subject.Value > 0)
		{
			CompareException.New(because ?? $"{subject.Value} should not be positive");
		}

		return this;
	}

	public NotNullableIntComparer BeZero(string because = null)
	{
		if (subject.HasValue && subject.Value == 0)
		{
			CompareException.New(because ?? $"{subject.Value} should not be zero");
		}

		return this;
	}

	public NotNullableIntComparer HaveValue(string because = null)
	{
		if (subject.HasValue)
		{
			CompareException.New(because ?? $"{subject.Value} should not have a value");
		}

		return this;
	}

	public NotNullableIntComparer Match(Func<int, bool> predicate, string because = null)
	{
		if (subject.HasValue && predicate(subject.Value))
		{
			CompareException.New(because ?? $"{subject.Value} should not match the predicate");
		}

		return this;
	}
}
