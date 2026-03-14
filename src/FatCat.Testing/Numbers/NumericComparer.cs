using System.Numerics;
using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NumericComparer<T>(T subject) : ComparerBase<T, NumericComparer<T>>(subject)
	where T : INumber<T>
{
	public NotNumericComparer<T> Not { get; } = new(subject);

	public NumericComparer<T> Be(T expected, string because = null)
	{
		if (Subject != expected)
			CompareException.New(because ?? $"{Subject} should be {expected}");

		return this;
	}

	public NumericComparer<T> BeAround(T center, T tolerance, string because = null)
	{
		if (T.Abs(Subject - center) > tolerance)
			CompareException.New(because ?? $"{Subject} should be around {center} within {tolerance}");

		return this;
	}

	public NumericComparer<T> BeGreaterThan(T expected, string because = null)
	{
		if (Subject < expected)
			CompareException.New(because ?? $"{Subject} should be greater than {expected}");

		return this;
	}

	public NumericComparer<T> BeInRange(T lower, T upper, string because = null)
	{
		if (Subject < lower || Subject > upper)
			CompareException.New(because ?? $"{Subject} should be between {lower} and {upper}");

		return this;
	}

	public NumericComparer<T> BeLessThan(T expected, string because = null)
	{
		if (Subject > expected)
			CompareException.New(because ?? $"{Subject} should be less than {expected}");

		return this;
	}

	public NumericComparer<T> BeNegative(string because = null)
	{
		if (Subject >= T.Zero)
			CompareException.New(because ?? $"{Subject} should be negative");

		return this;
	}

	public NumericComparer<T> BePositive(string because = null)
	{
		if (Subject <= T.Zero)
			CompareException.New(because ?? $"{Subject} should be positive");

		return this;
	}

	public NumericComparer<T> BeZero(string because = null)
	{
		if (Subject != T.Zero)
			CompareException.New(because ?? $"{Subject} should be zero");

		return this;
	}

	public NumericComparer<T> Match(Func<T, bool> predicate, string because = null)
	{
		if (!predicate(Subject))
			CompareException.New(because ?? $"{Subject} did not match the predicate");

		return this;
	}
}
