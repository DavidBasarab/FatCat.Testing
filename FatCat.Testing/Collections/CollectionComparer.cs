using FatCat.Testing.Comparers;
using FatCat.Testing.Equivalency;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;
using FatCat.Testing.Strings;

namespace FatCat.Testing.Collections;

public class CollectionComparer<T>(IEnumerable<T> subject) : ComparerBase<IEnumerable<T>, CollectionComparer<T>>(subject)
{
	private readonly List<T> items = subject is null ? null : subject.ToList();

	public NotCollectionComparer<T> Not
	{
		get { return field ??= new NotCollectionComparer<T>(items); }
	}

	public CollectionComparer<T> Contain(T expected, string because = null)
	{
		if (items is null || !items.Contains(expected)) { CompareException.New(because ?? $"{FormatItems()} should contain {ValueFormatter.Format(expected)}"); }

		return this;
	}

	public CollectionComparer<T> BeEmpty(string because = null)
	{
		if (items is null || items.Count > 0) { CompareException.New(because ?? $"{FormatItems()} should be empty"); }

		return this;
	}

	public CollectionComparer<T> HaveCount(int expected, string because = null)
	{
		if (items is null || items.Count != expected) { CompareException.New(because ?? $"{FormatItems()} should have count {expected} but has {items?.Count ?? 0}"); }

		return this;
	}

	public CollectionComparer<T> ContainSingle(string because = null)
	{
		if (items is null || items.Count != 1) { CompareException.New(because ?? $"{FormatItems()} should contain a single element but has {items?.Count ?? 0}"); }

		return this;
	}

	public CollectionComparer<T> ContainSingle(Func<T, bool> predicate, string because = null)
	{
		var matchedCount = items?.Count(predicate) ?? 0;

		if (items is null || matchedCount != 1) { CompareException.New(because ?? $"{FormatItems()} should contain a single element matching the predicate but {matchedCount} matched"); }

		return this;
	}

	public CollectionComparer<T> Equal(IEnumerable<T> expected, string because = null)
	{
		var expectedItems = expected?.ToList();

		if (items is null || expectedItems is null || !items.SequenceEqual(expectedItems)) { CompareException.New(because ?? $"{FormatItems()} should equal {ValueFormatter.Format(expectedItems)}"); }

		return this;
	}

	public CollectionComparer<T> BeEquivalentTo(IEnumerable<T> expected, string because = null)
	{
		var expectedItems = expected?.ToList();
		var failure = FindEquivalencyFailure(expectedItems);

		if (failure is not null) { CompareException.New(because ?? failure); }

		return this;
	}

	public CollectionComparer<T> ContainEquivalentOf(T expected, string because = null)
	{
		if (items is null || !ContainsEquivalentOf(expected)) { CompareException.New(because ?? $"{FormatItems()} should contain an element equivalent to {ValueFormatter.Format(expected)}"); }

		return this;
	}

	public CollectionComparer<T> OnlyContain(Func<T, bool> predicate, string because = null)
	{
		if (items is null || !items.All(predicate)) { CompareException.New(because ?? $"{FormatItems()} should only contain elements matching the predicate"); }

		return this;
	}

	public CollectionComparer<T> OnlyHaveUniqueItems(string because = null)
	{
		if (items is null || items.Distinct().Count() != items.Count) { CompareException.New(because ?? $"{FormatItems()} should only have unique items"); }

		return this;
	}

	public CollectionComparer<T> ContainInOrder(IEnumerable<T> expected, string because = null)
	{
		var expectedItems = expected?.ToList();

		if (items is null || expectedItems is null || !ContainsInOrder(expectedItems)) { CompareException.New(because ?? $"{FormatItems()} should contain {ValueFormatter.Format(expectedItems)} in order"); }

		return this;
	}

	public CollectionComparer<T> ContainInOrder(params T[] expected) { return ContainInOrder((IEnumerable<T>)expected); }

	public CollectionComparer<T> BeInDescendingOrder(string because = null)
	{
		if (items is null || !IsInDescendingOrder()) { CompareException.New(because ?? $"{FormatItems()} should be in descending order"); }

		return this;
	}

	public CollectionComparer<T> BeInAscendingOrder(string because = null)
	{
		if (items is null || !IsInAscendingOrder()) { CompareException.New(because ?? $"{FormatItems()} should be in ascending order"); }

		return this;
	}

	public CollectionComparer<T> BeSubsetOf(IEnumerable<T> superset, string because = null)
	{
		var supersetItems = superset?.ToList();

		if (items is null || supersetItems is null || items.Any(element => !supersetItems.Contains(element))) { CompareException.New(because ?? $"{FormatItems()} should be a subset of {ValueFormatter.Format(supersetItems)}"); }

		return this;
	}

	public CollectionComparer<T> IntersectWith(IEnumerable<T> other, string because = null)
	{
		var otherItems = other?.ToList();

		if (items is null || otherItems is null || !items.Any(otherItems.Contains)) { CompareException.New(because ?? $"{FormatItems()} should intersect with {ValueFormatter.Format(otherItems)}"); }

		return this;
	}

	public CollectionComparer<T> AllSatisfy(Action<T> inspector, string because = null)
	{
		if (items is null) { CompareException.New(because ?? $"{FormatItems()} should have every element satisfy the inspector"); }

		var failingIndex = FindFailingInspectorIndex(inspector);

		if (failingIndex >= 0) { CompareException.New(because ?? $"{FormatItems()} should have every element satisfy the inspector but element at index {failingIndex} did not"); }

		return this;
	}

	public CollectionComparer<T> SatisfyRespectively(params Action<T>[] inspectors)
	{
		if (items is null || items.Count != inspectors.Length) { CompareException.New($"{FormatItems()} should satisfy {inspectors.Length} inspectors respectively but has {items?.Count ?? 0} elements"); }

		var failingIndex = FindFailingRespectiveInspectorIndex(inspectors);

		if (failingIndex >= 0) { CompareException.New($"{FormatItems()} should satisfy {inspectors.Length} inspectors respectively but element at index {failingIndex} did not"); }

		return this;
	}

	public CollectionComparer<T> AllBeOfType<TExpected>(string because = null) { return AllBeOfType(typeof(TExpected), because); }

	public CollectionComparer<T> AllBeOfType(Type expectedType, string because = null)
	{
		if (items is null || items.Any(element => element is null || element.GetType() != expectedType)) { CompareException.New(because ?? $"{FormatItems()} should have all elements of type {expectedType}"); }

		return this;
	}

	public CollectionComparer<T> AllBeEquivalentTo(T expected, string because = null)
	{
		if (items is null || !items.All(element => EquivalencyComparer.Compare(element, expected).AreEquivalent)) { CompareException.New(because ?? $"{FormatItems()} should have all elements equivalent to {ValueFormatter.Format(expected)}"); }

		return this;
	}

	public CollectionComparer<T> HaveCountGreaterThan(int expected, string because = null)
	{
		if (items is null || items.Count <= expected) { CompareException.New(because ?? $"{FormatItems()} should have count greater than {expected} but has {items?.Count ?? 0}"); }

		return this;
	}

	public CollectionComparer<T> HaveCountLessThan(int expected, string because = null)
	{
		if (items is null || items.Count >= expected) { CompareException.New(because ?? $"{FormatItems()} should have count less than {expected} but has {items?.Count ?? 0}"); }

		return this;
	}

	public CollectionComparer<T> HaveSameCount(IEnumerable<T> other, string because = null)
	{
		var otherItems = other?.ToList();

		if (items is null || otherItems is null || items.Count != otherItems.Count) { CompareException.New(because ?? $"{FormatItems()} should have the same count as {ValueFormatter.Format(otherItems)} ({otherItems?.Count ?? 0}) but has {items?.Count ?? 0}"); }

		return this;
	}

	public CollectionComparer<T> HaveElementAt(int index, T expected, string because = null)
	{
		if (items is null || index < 0 || index >= items.Count || !Equals(items[index], expected)) { CompareException.New(because ?? $"{FormatItems()} should have {ValueFormatter.Format(expected)} at index {index}"); }

		return this;
	}

	public CollectionComparer<T> ContainNulls(string because = null)
	{
		if (items is null || !items.Any(element => element is null)) { CompareException.New(because ?? $"{FormatItems()} should contain nulls"); }

		return this;
	}

	public CollectionComparer<T> ContainMatch(string wildcard, string because = null)
	{
		if (items is null || !items.Any(element => element is string text && StringEqualityHelper.MatchesWildcard(text, wildcard, Options.CaseSensitive))) { CompareException.New(because ?? $"{FormatItems()} should contain a match for {wildcard}"); }

		return this;
	}

	private int FindFailingInspectorIndex(Action<T> inspector)
	{
		for (var index = 0; index < items.Count; index++)
		{
			try
			{
				inspector(items[index]);
			}
			catch (Exception)
			{
				// The inspector signals a failing element by throwing — usually a CompareException from a nested assertion. Catching it turns the throw into the index of the element that did not satisfy the inspector.
				return index;
			}
		}

		return -1;
	}

	private int FindFailingRespectiveInspectorIndex(Action<T>[] inspectors)
	{
		for (var index = 0; index < inspectors.Length; index++)
		{
			try
			{
				inspectors[index](items[index]);
			}
			catch (Exception)
			{
				// The inspector signals a failing element by throwing — usually a CompareException from a nested assertion. Catching it turns the throw into the index of the element that did not satisfy its inspector.
				return index;
			}
		}

		return -1;
	}

	private bool IsInAscendingOrder()
	{
		var comparer = Comparer<T>.Default;

		for (var index = 0; index < items.Count - 1; index++)
		{
			if (comparer.Compare(items[index], items[index + 1]) > 0) { return false; }
		}

		return true;
	}

	private string FindEquivalencyFailure(List<T> expectedItems)
	{
		var summary = $"{FormatItems()} should be equivalent to {ValueFormatter.Format(expectedItems)}";

		if (items is null || expectedItems is null) { return summary; }

		if (items.Count != expectedItems.Count) { return $"{summary} but they have different counts ({items.Count} and {expectedItems.Count})"; }

		var unmatchedExpected = expectedItems.ToList();

		foreach (var actual in items)
		{
			var matchIndex = unmatchedExpected.FindIndex(candidate => EquivalencyComparer.Compare(actual, candidate).AreEquivalent);

			if (matchIndex < 0) { return $"{summary} but could not find a match for {ValueFormatter.Format(actual)}"; }

			unmatchedExpected.RemoveAt(matchIndex);
		}

		return null;
	}

	private bool ContainsEquivalentOf(T expected)
	{
		return items.Any(element => EquivalencyComparer.Compare(element, expected).AreEquivalent);
	}

	private bool ContainsInOrder(IReadOnlyList<T> expected)
	{
		var matchedCount = 0;

		foreach (var item in items)
		{
			if (matchedCount < expected.Count && Equals(item, expected[matchedCount])) { matchedCount++; }
		}

		return matchedCount == expected.Count;
	}

	private bool IsInDescendingOrder()
	{
		var comparer = Comparer<T>.Default;

		for (var index = 0; index < items.Count - 1; index++)
		{
			if (comparer.Compare(items[index], items[index + 1]) < 0) { return false; }
		}

		return true;
	}

	private string FormatItems() { return ValueFormatter.Format(items); }
}