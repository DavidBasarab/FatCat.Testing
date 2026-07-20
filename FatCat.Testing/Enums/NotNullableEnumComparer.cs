using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Enums;

public class NotNullableEnumComparer<T>(T? subject) : NotComparerBase<T?, NotNullableEnumComparer<T>>(subject)
	where T : struct, Enum
{
	public NotNullableEnumComparer<T> Be(T expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Equals(expected)) { CompareException.New(because ?? $"{Subject.Value} should not be {expected}"); }

		return this;
	}

	public NotNullableEnumComparer<T> BeDefined(string because = null)
	{
		if (Subject.HasValue && Enum.IsDefined(typeof(T), Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be defined"); }

		return this;
	}

	public NotNullableEnumComparer<T> HaveFlag(T expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.HasFlag(expected)) { CompareException.New(because ?? $"{Subject.Value} should not have flag {expected}"); }

		return this;
	}

	public NotNullableEnumComparer<T> HaveValue(string because = null)
	{
		if (Subject.HasValue) { CompareException.New(because ?? $"{Subject.Value} should not have a value"); }

		return this;
	}
}