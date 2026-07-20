using System.Collections;
using System.Reflection;

namespace FatCat.Testing.Formatting;

public static class ValueFormatter
{
	private const int MaxDepth = 3;

	private const int MaxEnumerableItems = 10;

	public static string Format(object value)
	{
		return Format(value, 0, new HashSet<object>(ReferenceEqualityComparer.Instance));
	}

	private static string Format(object value, int depth, HashSet<object> visited)
	{
		if (value == null)
		{
			return "null";
		}

		if (value is string text)
		{
			return $"\"{text}\"";
		}

		if (value is char character)
		{
			return $"'{character}'";
		}

		if (IsScalar(value.GetType()))
		{
			return $"{value}";
		}

		if (depth >= MaxDepth)
		{
			return "…";
		}

		if (value is IEnumerable enumerable)
		{
			return FormatEnumerable(enumerable, depth, visited);
		}

		return FormatObject(value, depth, visited);
	}

	private static string FormatEnumerable(IEnumerable enumerable, int depth, HashSet<object> visited)
	{
		if (!visited.Add(enumerable))
		{
			return "…";
		}

		List<string> items = [];
		var count = 0;

		foreach (var item in enumerable)
		{
			if (count == MaxEnumerableItems)
			{
				items.Add("…");

				break;
			}

			items.Add(Format(item, depth + 1, visited));
			count++;
		}

		visited.Remove(enumerable);

		return items.Count == 0 ? "{ }" : $"{{ {string.Join(", ", items)} }}";
	}

	private static string FormatObject(object value, int depth, HashSet<object> visited)
	{
		if (!visited.Add(value))
		{
			return "…";
		}

		var type = value.GetType();
		var members = ReadableProperties(type)
			.Select(property => $"{property.Name} = {FormatMember(property, value, depth, visited)}")
			.ToList();

		visited.Remove(value);

		return members.Count == 0 ? type.Name : $"{type.Name} {{ {string.Join(", ", members)} }}";
	}

	private static IEnumerable<PropertyInfo> ReadableProperties(Type type)
	{
		return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(property => property.CanRead && property.GetIndexParameters().Length == 0);
	}

	private static string FormatMember(PropertyInfo property, object target, int depth, HashSet<object> visited)
	{
		try
		{
			return Format(property.GetValue(target), depth + 1, visited);
		}
		catch
		{
			// ignored — a property getter that throws must not break message rendering
			return "…";
		}
	}

	private static bool IsScalar(Type type)
	{
		return type.IsPrimitive
			|| type.IsEnum
			|| type == typeof(decimal)
			|| type == typeof(Guid)
			|| type == typeof(DateTime)
			|| type == typeof(DateTimeOffset)
			|| type == typeof(TimeSpan);
	}
}
