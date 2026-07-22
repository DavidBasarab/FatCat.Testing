using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

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

	private string FormatItems() { return ValueFormatter.Format(items); }
}