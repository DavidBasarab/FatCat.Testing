namespace Tests.FatCat.Testing.Extensibility;

public static class FakeWebResponseShouldExtensions
{
	public static FakeWebResponseComparer Should(this FakeWebResponse subject) { return new FakeWebResponseComparer(subject); }
}
