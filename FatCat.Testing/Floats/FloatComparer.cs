using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Floats;

public class FloatComparer(float subject) : ComparerBase<float, FloatComparer>(subject)
{
	public NotFloatComparer Not { get; } = new(subject);

	public FloatComparer BeApproximately(float expected, float tolerance, string because = null)
	{
		if (MathF.Abs(Subject - expected) > tolerance) { CompareException.New(because ?? $"{Subject} should be approximately {expected} within {tolerance}"); }

		return this;
	}

	public FloatComparer BeGreaterThan(float expected, string because = null)
	{
		if (Subject <= expected) { CompareException.New(because ?? $"{Subject} should be greater than {expected}"); }

		return this;
	}

	public FloatComparer BeInRange(float lower, float upper, string because = null)
	{
		if (Subject < lower || Subject > upper) { CompareException.New(because ?? $"{Subject} should be between {lower} and {upper}"); }

		return this;
	}

	public FloatComparer BeLessThan(float expected, string because = null)
	{
		if (Subject >= expected) { CompareException.New(because ?? $"{Subject} should be less than {expected}"); }

		return this;
	}

	public FloatComparer BeNaN(string because = null)
	{
		if (!float.IsNaN(Subject)) { CompareException.New(because ?? $"{Subject} should be NaN"); }

		return this;
	}

	public FloatComparer BeNegative(string because = null)
	{
		if (Subject >= 0) { CompareException.New(because ?? $"{Subject} should be negative"); }

		return this;
	}

	public FloatComparer BePositive(string because = null)
	{
		if (Subject <= 0) { CompareException.New(because ?? $"{Subject} should be positive"); }

		return this;
	}

	public FloatComparer BeZero(string because = null)
	{
		if (Subject != 0) { CompareException.New(because ?? $"{Subject} should be zero"); }

		return this;
	}

	public FloatComparer Match(Func<float, bool> predicate, string because = null)
	{
		if (!predicate(Subject)) { CompareException.New(because ?? $"{Subject} did not match the predicate"); }

		return this;
	}
}