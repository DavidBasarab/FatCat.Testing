namespace Tests.FatCat.Testing.CustomAssertions;

public static class ResultShouldExtensions
{
	public static WebResultComparer Should(this FakeWebResult subject)
	{
		return new WebResultComparer(subject);
	}
}
