using FatCat.Testing.Comparers;
using FatCat.Testing.Equivalency;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Objects;

public class NotObjectComparer<T>(T subject) : NotComparerBase<T, NotObjectComparer<T>>(subject)
	where T : class
{
	public NotObjectComparer<T> Be(T expected, string because = null)
	{
		if (Equals(Subject, expected))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should not be {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}

	public NotObjectComparer<T> BeEquivalentTo(T expected, string because = null)
	{
		var result = EquivalencyComparer.Compare(Subject, expected);

		if (result.AreEquivalent)
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should not be equivalent to {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}

	public NotObjectComparer<T> BeNull(string because = null)
	{
		if (Subject == null)
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should not be null");
		}

		return this;
	}

	public NotObjectComparer<T> BeSameAs(T expected, string because = null)
	{
		if (ReferenceEquals(Subject, expected))
		{
			CompareException.New(
				because
					?? $"{ValueFormatter.Format(Subject)} should not be the same instance as {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}
}
