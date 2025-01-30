using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NotNumberComparer(int subject) : IShouldNotComparer, INotRangeComparer
{
	public IShouldNotComparer Be(object expected)
	{
		if (subject == (int)expected)
		{
			CompareException.Match(expected, subject);
		}

		return this;
	}

	public INotRangeComparer BeInRange(object lower, object upper)
	{
		var upperValue = (int)upper;
		var lowerValue = (int)lower;

		if (subject >= lowerValue && subject <= upperValue)
		{
			CompareException.New($"{subject} should not be between {lowerValue} and {upperValue}");
		}

		return this;
	}
}
