namespace Tests.FatCat.Testing.Extensibility;

public static class FakeWebResponseComposingHelper
{
	public static int ReadStatusCode(FakeWebResponseComparer comparer) { return comparer.Subject.StatusCode; }
}
