using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Booleans;

public class NotBoolComparer(bool subject) : NotComparerBase<bool, NotBoolComparer>(subject)
{
	public NotBoolComparer Be(bool expected, string because = null)
	{
		if (Subject == expected)
		{
			CompareException.New(because ?? $"{expected} should not be {Subject}");
		}

		return this;
	}

	public NotBoolComparer BeFalse(string because = null)
	{
		if (!Subject)
		{
			CompareException.New(because ?? $"{Subject} should not be False");
		}

		return this;
	}

	public NotBoolComparer BeTrue(string because = null)
	{
		if (Subject)
		{
			CompareException.New(because ?? $"{Subject} should not be True");
		}

		return this;
	}
}
