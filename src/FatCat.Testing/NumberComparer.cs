using FatCat.Testing.Exceptions;

namespace FatCat.Testing;

public class NumberComparer
{
	public void Compare(int expected, int actual)
	{
		if (actual != expected)
		{
			CompareException.Mismatch(expected, actual);
		}
	}
}
