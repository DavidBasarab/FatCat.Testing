namespace Tests.FatCat.Testing.Equivalency;

public class ThrowingMember
{
	public string Value
	{
		get { throw new InvalidOperationException("boom"); }
	}
}
