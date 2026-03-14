namespace FatCat.Testing.Strings;

internal static class StringEqualityHelper
{
	internal static bool AreEqual(string left, string right, Options options)
	{
		var comparison =
			options == Options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

		return string.Equals(left, right, comparison);
	}

	internal static int CountOccurrences(string source, string value, Options options)
	{
		if (source == null || value == null || value.Length == 0)
		{
			return 0;
		}

		var comparison = ToComparison(options);
		var count = 0;
		var index = 0;

		while ((index = source.IndexOf(value, index, comparison)) >= 0)
		{
			count++;
			index += value.Length;
		}

		return count;
	}

	internal static StringComparison ToComparison(Options options)
	{
		return options == Options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
	}
}
