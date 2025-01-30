using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NumberShouldComparer(int subject) : IShouldComparer
{
	public IShouldNotComparer Not { get; } = new NotNumberShouldComparer(subject);

	public IShouldComparer Be(object expected)
	{
		if (subject != (int)expected)
		{
			CompareException.Mismatch(subject, expected);
		}

		return this;
	}
}
