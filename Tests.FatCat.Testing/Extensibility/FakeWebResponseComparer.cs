using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace Tests.FatCat.Testing.Extensibility;

public class FakeWebResponseComparer(FakeWebResponse subject) : ComparerBase<FakeWebResponse, FakeWebResponseComparer>(subject)
{
	public NotFakeWebResponseComparer Not { get; } = new(subject);

	public FakeWebResponseComparer BeOk(string because = null)
	{
		if (Subject.StatusCode != 200)
		{
			CompareException.New(because ?? $"status code {Subject.StatusCode} should be 200 (OK)");
		}

		return this;
	}

	public FakeWebResponseComparer BeNotFound(string because = null)
	{
		if (Subject.StatusCode != 404)
		{
			CompareException.New(because ?? $"status code {Subject.StatusCode} should be 404 (Not Found)");
		}

		return this;
	}
}
