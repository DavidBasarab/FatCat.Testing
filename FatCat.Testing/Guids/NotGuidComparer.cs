using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Guids;

public class NotGuidComparer(Guid subject) : NotComparerBase<Guid, NotGuidComparer>(subject)
{
	public NotGuidComparer Be(Guid expected, string because = null)
	{
		if (Subject == expected)
		{
			CompareException.New(because ?? $"{Subject} should not be {expected}");
		}

		return this;
	}

	public NotGuidComparer BeEmpty(string because = null)
	{
		if (Subject == Guid.Empty)
		{
			CompareException.New(because ?? $"{Subject} should not be empty");
		}

		return this;
	}
}
