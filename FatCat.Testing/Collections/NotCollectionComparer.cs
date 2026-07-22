using FatCat.Testing.Comparers;
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

	private string FormatItems() { return ValueFormatter.Format(items); }
}