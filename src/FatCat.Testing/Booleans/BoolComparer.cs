using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Booleans;

public class BoolComparer(bool subject) : ComparerBase<bool, BoolComparer>(subject)
{
	public NotBoolComparer Not { get; } = new(subject);

	public BoolComparer Be(bool expected, string because = null)
	{
		if (Subject != expected) { CompareException.New(because ?? $"{Subject} should be {expected}"); }

		return this;
	}

	public BoolComparer BeFalse(string because = null)
	{
		if (Subject) { CompareException.New(because ?? $"{Subject} should be False"); }

		return this;
	}

	public BoolComparer BeTrue(string because = null)
	{
		if (!Subject) { CompareException.New(because ?? $"{Subject} should be True"); }

		return this;
	}
}