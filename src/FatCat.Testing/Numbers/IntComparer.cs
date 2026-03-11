using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class IntComparer(int subject)
{
	public NotIntComparer Not { get; } = new(subject);

	public IntComparer Be(int expected, string because = null)
	{
		if (subject != expected)
		{
			CompareException.New(because ?? $"{subject} should be {expected}");
		}

		return this;
	}

	public IntComparer BeAround(int center, int tolerance, string because = null)
	{
		if (Math.Abs(subject - center) > tolerance)
		{
			CompareException.New(because ?? $"{subject} should be around {center} within {tolerance}");
		}

		return this;
	}

	public IntComparer BeGreaterThan(int expected, string because = null)
	{
		if (subject < expected)
		{
			CompareException.New(because ?? $"{subject} should be greater than {expected}");
		}

		return this;
	}

	public IntComparer BeInRange(int lower, int upper, string because = null)
	{
		if (subject < lower || subject > upper)
		{
			CompareException.New(because ?? $"{subject} should be between {lower} and {upper}");
		}

		return this;
	}

	public IntComparer BeLessThan(int expected, string because = null)
	{
		if (subject > expected)
		{
			CompareException.New(because ?? $"{subject} should be less than {expected}");
		}

		return this;
	}

	public IntComparer BeNegative(string because = null)
	{
		if (subject >= 0)
		{
			CompareException.New(because ?? $"{subject} should be negative");
		}

		return this;
	}

	public IntComparer BePositive(string because = null)
	{
		if (subject <= 0)
		{
			CompareException.New(because ?? $"{subject} should be positive");
		}

		return this;
	}

	public IntComparer BeZero(string because = null)
	{
		if (subject != 0)
		{
			CompareException.New(because ?? $"{subject} should be zero");
		}

		return this;
	}
}
