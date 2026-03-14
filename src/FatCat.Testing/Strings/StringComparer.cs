using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Strings;

public class StringComparer(string subject) : ComparerBase<string, StringComparer>(subject)
{
	public NotStringComparer Not { get; } = new(subject);

	public StringComparer Be(string expected, Options options = Options.CaseSensitive, string because = null)
	{
		if (!StringEqualityHelper.AreEqual(Subject, expected, options))
			CompareException.New(because ?? $"{Subject} should be {expected}");

		return this;
	}

	public StringComparer BeEquivalentTo(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (!StringEqualityHelper.AreEqual(Subject, expected, options))
			CompareException.New(because ?? $"{Subject} should be equivalent to {expected}");

		return this;
	}

	public StringComparer BeNull(string because = null)
	{
		if (Subject != null)
			CompareException.New(because ?? $"{Subject} should be null");

		return this;
	}

	public StringComparer HaveValue(string because = null)
	{
		if (Subject == null)
			CompareException.New(because ?? "subject should have a value");

		return this;
	}
}
