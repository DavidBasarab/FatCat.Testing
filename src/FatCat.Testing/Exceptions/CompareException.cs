namespace FatCat.Testing.Exceptions;

public class CompareException(string message) : Exception(message)
{
	public static void Match(object subject, object expected)
	{
		throw new CompareException($"{subject} should not be {expected}");
	}

	public static void Mismatch(object subject, object expected)
	{
		throw new CompareException($"{subject} should be {expected}");
	}

	public static void New(string message)
	{
		throw new CompareException(message);
	}
}
