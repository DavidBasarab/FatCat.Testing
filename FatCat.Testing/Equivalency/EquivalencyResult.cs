namespace FatCat.Testing.Equivalency;

internal class EquivalencyResult(bool isEquivalent, string difference)
{
	public bool IsEquivalent { get; } = isEquivalent;

	public string Difference { get; } = difference;

	public static EquivalencyResult Equivalent()
	{
		return new EquivalencyResult(true, null);
	}

	public static EquivalencyResult NotEquivalent(string difference)
	{
		return new EquivalencyResult(false, difference);
	}
}
