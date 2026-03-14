namespace FatCat.Testing.Strings;

internal static class StringEqualityHelper
{
	internal static bool AreEqual(string left, string right, Options options)
	{
		var comparison =
			options == Options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

		return string.Equals(left, right, comparison);
	}
}
