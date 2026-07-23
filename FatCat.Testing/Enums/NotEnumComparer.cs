using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Enums;

public class NotEnumComparer<T>(T subject) : NotComparerBase<T, NotEnumComparer<T>>(subject)
	where T : struct, Enum
{
	public NotEnumComparer<T> Be(T expected, string because = null)
	{
		if (Subject.Equals(expected))
		{
			CompareException.New(because ?? $"{Subject} should not be {expected}");
		}

		return this;
	}

	public NotEnumComparer<T> BeDefined(string because = null)
	{
		if (Enum.IsDefined(typeof(T), Subject))
		{
			CompareException.New(because ?? $"{Subject} should not be defined");
		}

		return this;
	}

	public NotEnumComparer<T> HaveFlag(T expected, string because = null)
	{
		if (Subject.HasFlag(expected))
		{
			CompareException.New(because ?? $"{Subject} should not have flag {expected}");
		}

		return this;
	}

	public NotEnumComparer<T> HaveSameNameAs<TOther>(TOther other, string because = null)
		where TOther : struct, Enum
	{
		if (Enum.GetName(Subject) == Enum.GetName(other))
		{
			CompareException.New(because ?? $"{Subject} should not have the same name as {other}");
		}

		return this;
	}

	public NotEnumComparer<T> HaveSameValueAs<TOther>(TOther other, string because = null)
		where TOther : struct, Enum
	{
		if (Convert.ToInt64(Subject) == Convert.ToInt64(other))
		{
			CompareException.New(because ?? $"{Subject} should not have the same value as {other}");
		}

		return this;
	}
}
