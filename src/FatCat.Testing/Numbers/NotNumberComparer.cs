using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NotNumberComparer(int subject)
{
	public NotNumberComparer Be(int expected)
	{
		if (subject == expected)
		{
			CompareException.Match(expected, subject);
		}

		return this;
	}

	public NotNumberComparer BeInRange(int lower, int upper)
	{
		var upperValue = upper;
		var lowerValue = lower;

		if (subject >= lowerValue && subject <= upperValue)
		{
			CompareException.New($"{subject} should not be between {lowerValue} and {upperValue}");
		}

		return this;
	}
}
