using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Strings;

public class NotStringComparer(string subject) : NotComparerBase<string, NotStringComparer>(subject)
{
	public NotStringComparer Be(string expected, Options options = Options.CaseSensitive, string because = null)
	{
		if (StringEqualityHelper.AreEqual(Subject, expected, options))
			CompareException.New(because ?? $"{Subject} should not be {expected}");

		return this;
	}

	public NotStringComparer BeEquivalentTo(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (StringEqualityHelper.AreEqual(Subject, expected, options))
			CompareException.New(because ?? $"{Subject} should not be equivalent to {expected}");

		return this;
	}

	public NotStringComparer BeNull(string because = null)
	{
		if (Subject == null)
			CompareException.New(because ?? "subject should not be null");

		return this;
	}

	public NotStringComparer HaveValue(string because = null)
	{
		if (Subject != null)
			CompareException.New(because ?? $"{Subject} should not have a value");

		return this;
	}
}
