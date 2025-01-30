using FatCat.Testing.Exceptions;

namespace FatCat.Testing;

internal class NumberComparer
{
	public void Compare(int expected, int actual)
	{
		if (actual != expected)
		{
			CompareException.Mismatch(expected, actual);
		}
	}

	public void NotCompare(int expected, int actual)
	{
		if (actual == expected)
		{
			CompareException.Match(expected, actual);
		}
	}
}
