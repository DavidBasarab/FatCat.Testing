using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Objects;

public class ObjectComparer<T>(T subject) : ComparerBase<T, ObjectComparer<T>>(subject)
	where T : class
{
	public NotObjectComparer<T> Not { get; } = new(subject);

	public ObjectComparer<T> Be(T expected, string because = null)
	{
		if (!Equals(Subject, expected)) { CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be {ValueFormatter.Format(expected)}"); }

		return this;
	}

	public ObjectComparer<T> BeNull(string because = null)
	{
		if (Subject != null) { CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be null"); }

		return this;
	}

	public ObjectComparer<T> BeSameAs(T expected, string because = null)
	{
		if (!ReferenceEquals(Subject, expected)) { CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be the same instance as {ValueFormatter.Format(expected)}"); }

		return this;
	}
}
