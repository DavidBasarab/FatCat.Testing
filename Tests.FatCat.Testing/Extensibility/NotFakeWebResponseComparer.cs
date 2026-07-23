using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing.Extensibility;

public class NotFakeWebResponseComparer(FakeWebResponse subject)
	: NotComparerBase<FakeWebResponse, NotFakeWebResponseComparer>(subject)
{
	public NotFakeWebResponseComparer BeOk(string because = null)
	{
		if (Subject.StatusCode == 200)
		{
			CompareException.New(because ?? $"status code {Subject.StatusCode} should not be 200 (OK)");
		}

		return this;
	}
}
