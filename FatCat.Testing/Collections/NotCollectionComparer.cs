using FatCat.Testing.Comparers;
using FatCat.Testing.Equivalency;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Collections;

public class NotCollectionComparer<T>(IEnumerable<T> subject) : NotComparerBase<IEnumerable<T>, NotCollectionComparer<T>>(subject)
{
	private readonly List<T> items = subject is null ? null : subject.ToList();

	public NotCollectionComparer<T> Contain(T expected, string because = null)
	{
		if (items is null || items.Contains(expected)) { CompareException.New(because ?? $"{FormatItems()} should not contain {ValueFormatter.Format(expected)}"); }

		return this;
	}

	public NotCollectionComparer<T> BeEmpty(string because = null)
	{
		if (items is null || items.Count == 0) { CompareException.New(because ?? $"{FormatItems()} should not be empty"); }

		return this;
	}

	public NotCollectionComparer<T> HaveCount(int expected, string because = null)
	{
		if (items is null || items.Count == expected) { CompareException.New(because ?? $"{FormatItems()} should not have count {expected}"); }

		return this;
	}

	public NotCollectionComparer<T> BeEquivalentTo(IEnumerable<T> expected, string because = null)
	{
		var expectedItems = expected?.ToList();

		if (items is null || IsEquivalentTo(expectedItems)) { CompareException.New(because ?? $"{FormatItems()} should not be equivalent to {ValueFormatter.Format(expectedItems)}"); }

		return this;
	}

	public NotCollectionComparer<T> ContainEquivalentOf(T expected, string because = null)
	{
		if (items is null || ContainsEquivalentOf(expected)) { CompareException.New(because ?? $"{FormatItems()} should not contain an element equivalent to {ValueFormatter.Format(expected)}"); }

		return this;
	}

	private bool IsEquivalentTo(List<T> expectedItems)
	{
		if (expectedItems is null || items.Count != expectedItems.Count) { return false; }

		var unmatchedExpected = expectedItems.ToList();

		foreach (var actual in items)
		{
			var matchIndex = unmatchedExpected.FindIndex(candidate => EquivalencyComparer.Compare(actual, candidate).AreEquivalent);

			if (matchIndex < 0) { return false; }

			unmatchedExpected.RemoveAt(matchIndex);
		}

		return true;
	}

	private bool ContainsEquivalentOf(T expected)
	{
		return items.Any(element => EquivalencyComparer.Compare(element, expected).AreEquivalent);
	}

	private string FormatItems() { return ValueFormatter.Format(items); }
}