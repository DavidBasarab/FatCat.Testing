using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Numbers;

public class NotNumberShouldComparer(int subject) : IShouldNotComparer
{
	public IShouldNotComparer Be(object expected)
	{
		if (subject == (int)expected)
		{
			CompareException.Match(expected, subject);
		}

		return this;
	}
}
