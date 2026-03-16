using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Enums;

public class EnumComparer<T>(T subject) : ComparerBase<T, EnumComparer<T>>(subject)
	where T : struct, Enum
{
	public NotEnumComparer<T> Not { get; } = new(subject);

	public EnumComparer<T> Be(T expected, string because = null)
	{
		if (!Subject.Equals(expected))
			CompareException.New(because ?? $"{Subject} should be {expected}");

		return this;
	}

	public EnumComparer<T> BeDefined(string because = null)
	{
		if (!Enum.IsDefined(typeof(T), Subject))
			CompareException.New(because ?? $"{Subject} should be defined");

		return this;
	}

	public EnumComparer<T> HaveFlag(T expected, string because = null)
	{
		if (!Subject.HasFlag(expected))
			CompareException.New(because ?? $"{Subject} should have flag {expected}");

		return this;
	}
}
