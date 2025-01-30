using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NumberComparer(int subject) : IShouldComparer<INotRangeComparer>, IRangeComparer
{
	public INotRangeComparer Not { get; } = new NotNumberComparer(subject);

	public IRangeComparer BeInRange(object lower, object upper)
	{
		var upperValue = (int)upper;
		var lowerValue = (int)lower;

		if (subject < lowerValue || subject > upperValue)
		{
			CompareException.New($"{subject} should be between {lowerValue} and {upperValue}");
		}

		return this;
	}

	IShouldComparer<INotRangeComparer> IShouldComparer<INotRangeComparer>.Be(object expected)
	{
		if (subject != (int)expected)
		{
			CompareException.Mismatch(subject, expected);
		}

		return this;
	}
}
