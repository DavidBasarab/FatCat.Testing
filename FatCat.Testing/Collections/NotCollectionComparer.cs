using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Collections;

public class NotCollectionComparer<T>(IEnumerable<T> subject)
	: NotComparerBase<IEnumerable<T>, NotCollectionComparer<T>>(subject)
{
	public NotCollectionComparer<T> Contain(T item, string because = null)
	{
		if (Subject.Contains(item))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should not contain {ValueFormatter.Format(item)}"
			);
		}

		return this;
	}

	public NotCollectionComparer<T> BeEmpty(string because = null)
	{
		if (!Subject.Any())
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should not be empty");
		}

		return this;
	}

	public NotCollectionComparer<T> ContainEquivalentOf(T item, string because = null)
	{
		if (CollectionEquivalency.ContainsEquivalentOf(Subject, item))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should not contain an equivalent of {ValueFormatter.Format(item)}"
			);
		}

		return this;
	}

	public NotCollectionComparer<T> BeEquivalentTo(IEnumerable<T> expected, string because = null)
	{
		if (CollectionEquivalency.AreEquivalent(Subject, expected))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should not be equivalent to {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}
}
