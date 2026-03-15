using System.Numerics;
using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NotNumericComparer<T>(T subject) : NotComparerBase<T, NotNumericComparer<T>>(subject)
	where T : INumber<T>
{
	public NotNumericComparer<T> Be(T expected, string because = null)
	{
		if (Subject == expected)
		{
			CompareException.New(because ?? $"{expected} should not be {Subject}");
		}

		return this;
	}

	public NotNumericComparer<T> BeGreaterThan(T expected, string because = null)
	{
		if (Subject > expected)
		{
			CompareException.New(because ?? $"{Subject} should not be greater than {expected}");
		}

		return this;
	}

	public NotNumericComparer<T> BeInRange(T lower, T upper, string because = null)
	{
		if (Subject >= lower && Subject <= upper)
		{
			CompareException.New(because ?? $"{Subject} should not be between {lower} and {upper}");
		}

		return this;
	}

	public NotNumericComparer<T> BeLessThan(T expected, string because = null)
	{
		if (Subject < expected)
		{
			CompareException.New(because ?? $"{Subject} should not be less than {expected}");
		}

		return this;
	}

	public NotNumericComparer<T> BeNegative(string because = null)
	{
		if (Subject < T.Zero)
		{
			CompareException.New(because ?? $"{Subject} should not be negative");
		}

		return this;
	}

	public NotNumericComparer<T> BePositive(string because = null)
	{
		if (Subject > T.Zero)
		{
			CompareException.New(because ?? $"{Subject} should not be positive");
		}

		return this;
	}

	public NotNumericComparer<T> BeZero(string because = null)
	{
		if (Subject == T.Zero)
		{
			CompareException.New(because ?? $"{Subject} should not be zero");
		}

		return this;
	}

	public NotNumericComparer<T> Match(Func<T, bool> predicate, string because = null)
	{
		if (predicate(Subject))
		{
			CompareException.New(because ?? $"{Subject} should not match the predicate");
		}

		return this;
	}
}
