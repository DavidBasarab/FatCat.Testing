using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Collections;

public class CollectionComparer<T>(IEnumerable<T> subject) : ComparerBase<IEnumerable<T>, CollectionComparer<T>>(subject)
{
	public NotCollectionComparer<T> Not { get; } = new(subject);

	public CollectionComparer<T> Contain(T item, string because = null)
	{
		if (!Subject.Contains(item))
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should contain {ValueFormatter.Format(item)}");
		}

		return this;
	}

	public CollectionComparer<T> BeEmpty(string because = null)
	{
		if (Subject.Any())
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be empty");
		}

		return this;
	}

	public CollectionComparer<T> HaveCount(int expectedCount, string because = null)
	{
		if (Subject.Count() != expectedCount)
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should have count {expectedCount}");
		}

		return this;
	}

	public CollectionComparer<T> ContainSingle(string because = null)
	{
		if (Subject.Count() != 1)
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should contain a single element");
		}

		return this;
	}

	public CollectionComparer<T> ContainEquivalentOf(T item, string because = null)
	{
		if (!CollectionEquivalency.ContainsEquivalentOf(Subject, item))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should contain an equivalent of {ValueFormatter.Format(item)}"
			);
		}

		return this;
	}

	public CollectionComparer<T> OnlyContain(Func<T, bool> predicate, string because = null)
	{
		if (!Subject.All(predicate))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should only contain elements matching the predicate"
			);
		}

		return this;
	}

	public CollectionComparer<T> Equal(IEnumerable<T> expected, string because = null)
	{
		if (!Subject.SequenceEqual(expected))
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should equal {ValueFormatter.Format(expected)}");
		}

		return this;
	}

	public CollectionComparer<T> BeEquivalentTo(IEnumerable<T> expected, string because = null)
	{
		if (!CollectionEquivalency.AreEquivalent(Subject, expected))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should be equivalent to {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}

	public CollectionComparer<T> OnlyHaveUniqueItems(string because = null)
	{
		var items = Subject.ToList();

		if (items.Count != items.Distinct().Count())
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should only have unique items");
		}

		return this;
	}

	public CollectionComparer<T> BeInDescendingOrder(string because = null)
	{
		if (!IsDescending(Subject.ToList()))
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be in descending order");
		}

		return this;
	}

	private static bool IsDescending(List<T> items)
	{
		var comparer = Comparer<T>.Default;

		for (var index = 1; index < items.Count; index++)
		{
			if (comparer.Compare(items[index - 1], items[index]) < 0)
			{
				return false;
			}
		}

		return true;
	}
}
