using FatCat.Testing.Comparers;
using FatCat.Testing.Equivalency;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Objects;

public class ObjectComparer<T>(T subject) : ComparerBase<T, ObjectComparer<T>>(subject)
	where T : class
{
	public NotObjectComparer<T> Not { get; } = new(subject);

	public ObjectComparer<T> Be(T expected, string because = null)
	{
		if (!Equals(Subject, expected))
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be {ValueFormatter.Format(expected)}");
		}

		return this;
	}

	public ObjectComparer<T> BeEquivalentTo(T expected, string because = null)
	{
		var result = EquivalencyComparer.Compare(Subject, expected);

		if (!result.AreEquivalent)
		{
			CompareException.New(because ?? BuildEquivalencyMessage(expected, result));
		}

		return this;
	}

	private string BuildEquivalencyMessage(T expected, EquivalencyResult result)
	{
		var summary = $"{ValueFormatter.Format(Subject)} should be equivalent to {ValueFormatter.Format(expected)}";

		if (string.IsNullOrEmpty(result.Path))
		{
			return summary;
		}

		return $"{summary} but {result.Path} differs: {result.Difference}";
	}

	public ObjectComparer<T> BeNull(string because = null)
	{
		if (Subject != null)
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be null");
		}

		return this;
	}

	public ObjectComparer<T> BeSameAs(T expected, string because = null)
	{
		if (!ReferenceEquals(Subject, expected))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should be the same instance as {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}
}
