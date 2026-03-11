using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NotIntComparer(int subject)
{
	public NotIntComparer Be(int expected, string because = null)
	{
		if (subject == expected)
		{
			CompareException.New(because ?? $"{expected} should not be {subject}");
		}

		return this;
	}

	public NotIntComparer BeGreaterThan(int expected, string because = null)
	{
		if (subject > expected)
		{
			CompareException.New(because ?? $"{subject} should not be greater than {expected}");
		}

		return this;
	}

	public NotIntComparer BeInRange(int lower, int upper, string because = null)
	{
		var upperValue = upper;
		var lowerValue = lower;

		if (subject >= lowerValue && subject <= upperValue)
		{
			CompareException.New(because ?? $"{subject} should not be between {lowerValue} and {upperValue}");
		}

		return this;
	}

	public NotIntComparer BeLessThan(int expected, string because = null)
	{
		if (subject < expected)
		{
			CompareException.New(because ?? $"{subject} should not be less than {expected}");
		}

		return this;
	}

	public NotIntComparer BeNegative(string because = null)
	{
		if (subject < 0)
		{
			CompareException.New(because ?? $"{subject} should not be negative");
		}

		return this;
	}

	public NotIntComparer BePositive(string because = null)
	{
		if (subject > 0)
		{
			CompareException.New(because ?? $"{subject} should not be positive");
		}

		return this;
	}

	public NotIntComparer BeZero(string because = null)
	{
		if (subject == 0)
		{
			CompareException.New(because ?? $"{subject} should not be zero");
		}

		return this;
	}
}
