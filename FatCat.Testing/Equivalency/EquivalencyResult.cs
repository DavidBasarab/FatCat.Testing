namespace FatCat.Testing.Equivalency;

public class EquivalencyResult(bool areEquivalent, string path, string difference)
{
	public bool AreEquivalent { get; } = areEquivalent;

	public string Path { get; } = path;

	public string Difference { get; } = difference;
}
