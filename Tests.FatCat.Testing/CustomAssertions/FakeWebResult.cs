namespace Tests.FatCat.Testing.CustomAssertions;

public class FakeWebResult(int statusCode)
{
	public int StatusCode { get; } = statusCode;
}
