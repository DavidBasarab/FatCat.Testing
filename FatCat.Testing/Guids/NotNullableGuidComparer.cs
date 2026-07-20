using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Guids;

public class NotNullableGuidComparer(Guid? subject) : NotComparerBase<Guid?, NotNullableGuidComparer>(subject)
{
	public NotNullableGuidComparer Be(Guid expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value == expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be {expected}");
		}

		return this;
	}

	public NotNullableGuidComparer BeEmpty(string because = null)
	{
		if (Subject.HasValue && Subject.Value == Guid.Empty)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be empty");
		}

		return this;
	}

	public NotNullableGuidComparer HaveValue(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have a value");
		}

		return this;
	}
}
