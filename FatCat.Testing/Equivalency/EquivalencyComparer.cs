using System.Collections;
using System.Reflection;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Equivalency;

public static class EquivalencyComparer
{
	private const int MaxCollectionElements = 32;

	private const int MaxDepth = 10;

	public static EquivalencyResult Compare(object subject, object expected)
	{
		return Compare(subject, expected, depth: 0, new HashSet<object>(ReferenceEqualityComparer.Instance));
	}

	private static EquivalencyResult Compare(object subject, object expected, int depth, HashSet<object> visited)
	{
		if (subject == null && expected == null) { return Equivalent(); }

		if (subject == null) { return NotEquivalent(path: "", $"expected {FormatValue(expected)} but found null"); }

		if (expected == null) { return NotEquivalent(path: "", $"expected null but found {FormatValue(subject)}"); }

		if (depth > MaxDepth) { return Equivalent(); }

		var customRule = ApplyCustomRule(subject, expected);

		if (customRule != null) { return customRule; }

		if (OverridesEquals(subject.GetType())) { return CompareByEquals(subject, expected); }

		if (visited.Contains(subject)) { return Equivalent(); }

		visited.Add(subject);

		try
		{
			if (subject is IEnumerable subjectItems && expected is IEnumerable expectedItems)
			{
				return CompareEnumerables(subjectItems, expectedItems, depth, visited);
			}

			return CompareProperties(subject, expected, depth, visited);
		}
		finally
		{
			visited.Remove(subject);
		}
	}

	private static EquivalencyResult CompareByEquals(object subject, object expected)
	{
		if (Equals(subject, expected)) { return Equivalent(); }

		return NotEquivalent(path: "", $"expected {FormatValue(expected)} but found {FormatValue(subject)}");
	}

	private static EquivalencyResult CompareProperties(object subject, object expected, int depth, HashSet<object> visited)
	{
		foreach (var property in ReadableProperties(subject.GetType()))
		{
			object subjectValue;
			object expectedValue;

			try
			{
				subjectValue = property.GetValue(subject);
				expectedValue = property.GetValue(expected);
			}
			catch (Exception exception)
			{
				// A property getter can throw — often it is the very value under assertion. Report the throw as the difference instead of failing the comparison for the wrong reason.
				return NotEquivalent(property.Name, $"<threw {UnwrapExceptionTypeName(exception)}>");
			}

			var result = Compare(subjectValue, expectedValue, depth + 1, visited);

			if (!result.AreEquivalent) { return NotEquivalent(PrependMember(property.Name, result.Path), result.Difference); }
		}

		return Equivalent();
	}

	private static EquivalencyResult CompareEnumerables(IEnumerable subject, IEnumerable expected, int depth, HashSet<object> visited)
	{
		var subjectItems = TakeCapped(subject);
		var expectedItems = TakeCapped(expected);

		if (subjectItems.Count != expectedItems.Count)
		{
			return NotEquivalent(path: "", $"expected {expectedItems.Count} items but found {subjectItems.Count}");
		}

		var unmatched = subjectItems.ToList();

		foreach (var expectedItem in expectedItems)
		{
			var matchIndex = unmatched.FindIndex(candidate => Compare(candidate, expectedItem, depth + 1, visited).AreEquivalent);

			if (matchIndex < 0) { return NotEquivalent(path: "", $"could not find match for {FormatValue(expectedItem)}"); }

			unmatched.RemoveAt(matchIndex);
		}

		return Equivalent();
	}

	private static List<object> TakeCapped(IEnumerable enumerable)
	{
		List<object> items = [];

		foreach (var item in enumerable)
		{
			if (items.Count >= MaxCollectionElements) { break; }

			items.Add(item);
		}

		return items;
	}

	private static EquivalencyResult ApplyCustomRule(object subject, object expected)
	{
		// Phase 09 seam: the Using<T>() / WhenTypeIs<T>() override registry is consulted here, before the Equals base case and the member walk. It ships empty, so today there is never a custom rule.
		return null;
	}

	private static IEnumerable<PropertyInfo> ReadableProperties(Type type)
	{
		// Mirrors ValueFormatter's member selection exactly: public readable instance properties, in declaration order. One notion of "a type's members" across the library.
		return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(property => property.CanRead && property.GetIndexParameters().Length == 0)
			.OrderBy(property => property.MetadataToken);
	}

	private static bool OverridesEquals(Type type)
	{
		var equalsMethod = type.GetMethod("Equals", [typeof(object)]);

		return equalsMethod != null && equalsMethod.DeclaringType != typeof(object);
	}

	private static string FormatValue(object value)
	{
		if (value is string text) { return $"\"{text}\""; }

		if (value is char character) { return $"'{character}'"; }

		return ValueFormatter.Format(value);
	}

	private static string PrependMember(string member, string childPath)
	{
		if (string.IsNullOrEmpty(childPath)) { return member; }

		return $"{member}.{childPath}";
	}

	private static string UnwrapExceptionTypeName(Exception exception)
	{
		if (exception is TargetInvocationException && exception.InnerException != null) { return exception.InnerException.GetType().Name; }

		return exception.GetType().Name;
	}

	private static EquivalencyResult Equivalent() { return new EquivalencyResult(areEquivalent: true, path: "", difference: null); }

	private static EquivalencyResult NotEquivalent(string path, string difference) { return new EquivalencyResult(areEquivalent: false, path, difference); }
}
