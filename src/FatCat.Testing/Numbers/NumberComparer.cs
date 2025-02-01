using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NumberComparer(int subject)
{
	public NotNumberComparer Not { get; } = new(subject);

	public NumberComparer Be(int expected)
	{
		if (subject != expected)
		{
			CompareException.Mismatch(subject, expected);
		}

		return this;
	}

	public NumberComparer BeGreaterThan(int expected)
	{
		if (subject < expected)
		{
			CompareException.New($"{subject} should be greater than {expected}");
		}

		return this;
	}

	public NumberComparer BeLessThan(int expected)
	{
		if (subject > expected)
		{
			CompareException.New($"{subject} should be less than {expected}");
		}

		return this;
	}

	public NumberComparer BeInRange(int lower, int upper)
	{
		var upperValue = upper;
		var lowerValue = lower;

		if (subject < lowerValue || subject > upperValue)
		{
			CompareException.New($"{subject} should be between {lowerValue} and {upperValue}");
		}

		return this;
	}
}
