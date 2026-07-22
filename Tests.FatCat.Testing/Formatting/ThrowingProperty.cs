namespace Tests.FatCat.Testing.Formatting;

internal class ThrowingProperty
{
	public string Value
	{
		get { throw new InvalidOperationException("boom"); }
	}
}