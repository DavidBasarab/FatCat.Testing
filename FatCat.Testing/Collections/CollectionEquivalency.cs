using FatCat.Testing.Equivalency;

namespace FatCat.Testing.Collections;

internal static class CollectionEquivalency
{
	public static bool ContainsEquivalentOf<T>(IEnumerable<T> subject, T item)
	{
		var engine = new StructuralEquivalency();

		return subject.Any(element => engine.Compare(element, item).IsEquivalent);
	}

	public static bool AreEquivalent<T>(IEnumerable<T> subject, IEnumerable<T> expected)
	{
		var engine = new StructuralEquivalency();
		var subjectList = subject.ToList();
		var remaining = expected.ToList();

		if (subjectList.Count != remaining.Count)
		{
			return false;
		}

		foreach (var element in subjectList)
		{
			var matchIndex = remaining.FindIndex(candidate => engine.Compare(element, candidate).IsEquivalent);

			if (matchIndex < 0)
			{
				return false;
			}

			remaining.RemoveAt(matchIndex);
		}

		return true;
	}
}
