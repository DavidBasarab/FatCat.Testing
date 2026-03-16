using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Booleans;

public class NotNullableBoolComparer(bool? subject) : NotComparerBase<bool?, NotNullableBoolComparer>(subject)
{
	public NotNullableBoolComparer Be(bool expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value == expected)
		{
			CompareException.New(because ?? $"{expected} should not be {Subject.Value}");
		}

		return this;
	}

	public NotNullableBoolComparer BeFalse(string because = null)
	{
		if (Subject.HasValue && !Subject.Value)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be False");
		}

		return this;
	}

	public NotNullableBoolComparer BeTrue(string because = null)
	{
		if (Subject.HasValue && Subject.Value)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be True");
		}

		return this;
	}

	public NotNullableBoolComparer HaveValue(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have a value");
		}

		return this;
	}
}
