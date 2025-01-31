using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NumberComparer(int subject)
{
	public NotNumberComparer Not { get; } = new(subject);

	public NumberComparer BeInRange(object lower, object upper)
	{
		var upperValue = (int)upper;
		var lowerValue = (int)lower;

		if (subject < lowerValue || subject > upperValue)
		{
			CompareException.New($"{subject} should be between {lowerValue} and {upperValue}");
		}

		return this;
	}

	public NumberComparer Be(object expected)
	{
		if (subject != (int)expected)
		{
			CompareException.Mismatch(subject, expected);
		}

		return this;
	}
}
