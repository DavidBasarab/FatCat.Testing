namespace FatCat.Testing.Exceptions;

public class CompareException(string message) : Exception(message)
{
	public static void Mismatch(object expect, object actual)
	{
		throw new CompareException($"{expect} should be {actual}");
	}

	public static void Match(object expect, object actual)
	{
		throw new CompareException($"{expect} should not be {actual}");
	}
}
