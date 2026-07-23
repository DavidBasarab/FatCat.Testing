using System.Collections;
using System.Reflection;

namespace FatCat.Testing.Formatting;

public static class ValueFormatter
{
	private const int MaxCollectionElements = 32;

	private const int MaxDepth = 5;

	public static string Format(object value)
	{
		return Format(value, isNested: false, depth: 0, new HashSet<object>(ReferenceEqualityComparer.Instance));
	}

	private static string Format(object value, bool isNested, int depth, HashSet<object> visited)
	{
		if (value == null)
		{
			return "null";
		}

		if (value is string text)
		{
			return isNested ? $"\"{text}\"" : text;
		}

		if (value is char character)
		{
			return isNested ? $"'{character}'" : $"{character}";
		}

		if (OverridesToString(value))
		{
			return value.ToString();
		}

		if (depth > MaxDepth)
		{
			return "{ … }";
		}

		if (value is IEnumerable enumerable)
		{
			return FormatEnumerable(enumerable, depth, visited);
		}

		return FormatObject(value, depth, visited);
	}

	private static string FormatEnumerable(IEnumerable enumerable, int depth, HashSet<object> visited)
	{
		if (visited.Contains(enumerable))
		{
			return $"{{ cyclic reference to {enumerable.GetType().Name} }}";
		}

		visited.Add(enumerable);

		try
		{
			return BuildEnumerableText(enumerable, depth, visited);
		}
		finally
		{
			visited.Remove(enumerable);
		}
	}

	private static string BuildEnumerableText(IEnumerable enumerable, int depth, HashSet<object> visited)
	{
		List<string> elements = [];
		var remainder = 0;

		foreach (var item in enumerable)
		{
			if (elements.Count < MaxCollectionElements)
			{
				elements.Add(Format(item, isNested: true, depth + 1, visited));
			}
			else
			{
				remainder++;
			}
		}

		if (remainder > 0)
		{
			return $"[{string.Join(", ", elements)}, …and {remainder} more]";
		}

		return $"[{string.Join(", ", elements)}]";
	}

	private static string FormatObject(object value, int depth, HashSet<object> visited)
	{
		if (visited.Contains(value))
		{
			return $"{{ cyclic reference to {value.GetType().Name} }}";
		}

		visited.Add(value);

		try
		{
			return BuildObjectText(value, depth, visited);
		}
		finally
		{
			visited.Remove(value);
		}
	}

	private static string BuildObjectText(object value, int depth, HashSet<object> visited)
	{
		var typeName = value.GetType().Name;
		var members = ReadableProperties(value.GetType())
			.Select(property => $"{property.Name} = {FormatProperty(value, property, depth, visited)}")
			.ToList();

		if (members.Count == 0)
		{
			return $"{typeName} {{ }}";
		}

		return $"{typeName} {{ {string.Join(", ", members)} }}";
	}

	private static IEnumerable<PropertyInfo> ReadableProperties(Type type)
	{
		return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(property => property.CanRead && property.GetIndexParameters().Length == 0)
			.OrderBy(property => property.MetadataToken);
	}

	private static string FormatProperty(object owner, PropertyInfo property, int depth, HashSet<object> visited)
	{
		try
		{
			return Format(property.GetValue(owner), isNested: true, depth + 1, visited);
		}
		catch (Exception exception)
		{
			// A property getter can throw — often it is the very value under assertion. Render the failure so the formatter never fails the test for the wrong reason.
			return $"<threw {UnwrapExceptionTypeName(exception)}>";
		}
	}

	private static string UnwrapExceptionTypeName(Exception exception)
	{
		if (exception is TargetInvocationException && exception.InnerException != null)
		{
			return exception.InnerException.GetType().Name;
		}

		return exception.GetType().Name;
	}

	private static bool OverridesToString(object value)
	{
		var toStringMethod = value.GetType().GetMethod("ToString", Type.EmptyTypes);

		return toStringMethod != null && toStringMethod.DeclaringType != typeof(object);
	}
}
