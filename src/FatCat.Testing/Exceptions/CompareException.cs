namespace FatCat.Testing.Exceptions;

public class CompareException(object expect, object actual) : Exception($"Expected {expect}, but got {actual}")
{
	public static void Mismatch(object expect, object actual)
	{
		throw new CompareException(expect, actual);
	}
}
