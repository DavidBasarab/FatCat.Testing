using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing.CustomAssertions;

public class ProvingComparer(int subject) : ComparerBase<int, ProvingComparer>(subject)
{
	public ProvingComparer BeTheAnswer(string because = null)
	{
		if (Subject != 42)
		{
			CompareException.New(because ?? $"{Subject} should be the answer");
		}

		return this;
	}
}
