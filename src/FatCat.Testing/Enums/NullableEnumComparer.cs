using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Enums;

public class NullableEnumComparer<T>(T? subject) : ComparerBase<T?, NullableEnumComparer<T>>(subject)
	where T : struct, Enum
{
	public NotNullableEnumComparer<T> Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject.HasValue ? $"{Subject.Value}" : "null";
	}

	public NullableEnumComparer<T> Be(T expected, string because = null)
	{
		if (!Subject.HasValue || !Subject.Value.Equals(expected))
			CompareException.New(because ?? $"{SubjectDisplay} should be {expected}");

		return this;
	}

	public NullableEnumComparer<T> BeDefined(string because = null)
	{
		if (!Subject.HasValue || !Enum.IsDefined(typeof(T), Subject.Value))
			CompareException.New(because ?? $"{SubjectDisplay} should be defined");

		return this;
	}

	public NullableEnumComparer<T> BeNull(string because = null)
	{
		if (Subject.HasValue)
			CompareException.New(because ?? $"{Subject.Value} should be null");

		return this;
	}

	public NullableEnumComparer<T> HaveFlag(T expected, string because = null)
	{
		if (!Subject.HasValue || !Subject.Value.HasFlag(expected))
			CompareException.New(because ?? $"{SubjectDisplay} should have flag {expected}");

		return this;
	}

	public NullableEnumComparer<T> HaveValue(string because = null)
	{
		if (!Subject.HasValue)
			CompareException.New(because ?? "value should not be null");

		return this;
	}
}
