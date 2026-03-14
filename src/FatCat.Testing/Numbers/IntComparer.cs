using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class IntComparer(int subject) : ComparerBase<int, IntComparer>(subject)
{
	public NotIntComparer Not { get; } = new(subject);

	public IntComparer Be(int expected, string because = null)
	{
		if (Subject != expected)
		{
			CompareException.New(because ?? $"{Subject} should be {expected}");
		}

		return this;
	}

	public IntComparer BeAround(int center, int tolerance, string because = null)
	{
		if (Math.Abs(Subject - center) > tolerance)
		{
			CompareException.New(because ?? $"{Subject} should be around {center} within {tolerance}");
		}

		return this;
	}

	public IntComparer BeGreaterThan(int expected, string because = null)
	{
		if (Subject < expected)
		{
			CompareException.New(because ?? $"{Subject} should be greater than {expected}");
		}

		return this;
	}

	public IntComparer BeInRange(int lower, int upper, string because = null)
	{
		if (Subject < lower || Subject > upper)
		{
			CompareException.New(because ?? $"{Subject} should be between {lower} and {upper}");
		}

		return this;
	}

	public IntComparer BeLessThan(int expected, string because = null)
	{
		if (Subject > expected)
		{
			CompareException.New(because ?? $"{Subject} should be less than {expected}");
		}

		return this;
	}

	public IntComparer BeNegative(string because = null)
	{
		if (Subject >= 0)
		{
			CompareException.New(because ?? $"{Subject} should be negative");
		}

		return this;
	}

	public IntComparer BePositive(string because = null)
	{
		if (Subject <= 0)
		{
			CompareException.New(because ?? $"{Subject} should be positive");
		}

		return this;
	}

	public IntComparer BeZero(string because = null)
	{
		if (Subject != 0)
		{
			CompareException.New(because ?? $"{Subject} should be zero");
		}

		return this;
	}

	public IntComparer Match(Func<int, bool> predicate, string because = null)
	{
		if (!predicate(Subject))
		{
			CompareException.New(because ?? $"{Subject} did not match the predicate");
		}

		return this;
	}
}
