using System.Reflection;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Equivalency;

internal class StructuralEquivalency
{
	public EquivalencyResult Compare(object subject, object expected)
	{
		return Compare(subject, expected, "", []);
	}

	private EquivalencyResult Compare(object subject, object expected, string path, HashSet<ReferencePair> visited)
	{
		if (ReferenceEquals(subject, expected))
		{
			return EquivalencyResult.Equivalent();
		}

		if (subject is null || expected is null)
		{
			return Difference(path, subject, expected);
		}

		var type = subject.GetType();

		if (EquivalencyOptions.TryGetRule(type, out var rule))
		{
			return BuildResult(rule(subject, expected), path, subject, expected);
		}

		if (IsScalar(type))
		{
			return BuildResult(Equals(subject, expected), path, subject, expected);
		}

		if (!visited.Add(new ReferencePair(subject, expected)))
		{
			return EquivalencyResult.Equivalent();
		}

		return CompareMembers(subject, expected, type, path, visited);
	}

	private EquivalencyResult CompareMembers(
		object subject,
		object expected,
		Type type,
		string path,
		HashSet<ReferencePair> visited
	)
	{
		foreach (var property in ReadableProperties(type))
		{
			var memberPath = BuildPath(path, property.Name);
			var subjectValue = ReadValue(property, subject);
			var expectedValue = ReadValue(property, expected);
			var result = Compare(subjectValue, expectedValue, memberPath, visited);

			if (!result.IsEquivalent)
			{
				return result;
			}
		}

		return EquivalencyResult.Equivalent();
	}

	private static EquivalencyResult BuildResult(bool areEquivalent, string path, object subject, object expected)
	{
		if (areEquivalent)
		{
			return EquivalencyResult.Equivalent();
		}

		return Difference(path, subject, expected);
	}

	private static EquivalencyResult Difference(string path, object subject, object expected)
	{
		var expectedText = ValueFormatter.Format(expected);
		var subjectText = ValueFormatter.Format(subject);

		if (path.Length == 0)
		{
			return EquivalencyResult.NotEquivalent($"Expected {expectedText} but found {subjectText}");
		}

		return EquivalencyResult.NotEquivalent($"Expected {path} to be {expectedText} but found {subjectText}");
	}

	private static string BuildPath(string path, string name)
	{
		if (path.Length == 0)
		{
			return name;
		}

		return $"{path}.{name}";
	}

	private static object ReadValue(PropertyInfo property, object target)
	{
		try
		{
			return property.GetValue(target);
		}
		catch
		{
			// ignored — a property getter that throws must not break comparison
			return null;
		}
	}

	private static IEnumerable<PropertyInfo> ReadableProperties(Type type)
	{
		return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(property => property.CanRead && property.GetIndexParameters().Length == 0);
	}

	private static bool IsScalar(Type type)
	{
		return type == typeof(string)
			|| type.IsPrimitive
			|| type.IsEnum
			|| type == typeof(decimal)
			|| type == typeof(Guid)
			|| type == typeof(DateTime)
			|| type == typeof(DateTimeOffset)
			|| type == typeof(TimeSpan);
	}
}
