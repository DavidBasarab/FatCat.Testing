using System.Collections.Concurrent;

namespace FatCat.Testing.Equivalency;

public static class EquivalencyOptions
{
	private static readonly ConcurrentDictionary<Type, Func<object, object, bool>> rules = new();

	public static void Using<T>(Func<T, T, bool> rule)
	{
		rules[typeof(T)] = (subject, expected) => rule((T)subject, (T)expected);
	}

	public static void Reset()
	{
		rules.Clear();
	}

	public static bool TryGetRule(Type type, out Func<object, object, bool> rule)
	{
		return rules.TryGetValue(type, out rule);
	}
}
