using FatCat.Testing.Comparers;

namespace Tests.FatCat.Testing.CustomAssertions;

public class WebResultComparer(FakeWebResult subject) : ComparerBase<FakeWebResult, WebResultComparer>(subject)
{
	public WebResultComparer BeOk(string because = null)
	{
		Subject.StatusCode.Should().Be(200, because ?? $"Expected OK but web result was {Subject.StatusCode}");

		return this;
	}

	public WebResultComparer BeNotFound(string because = null)
	{
		Subject.StatusCode.Should().Be(404, because ?? $"Expected Not Found but web result was {Subject.StatusCode}");

		return this;
	}
}
