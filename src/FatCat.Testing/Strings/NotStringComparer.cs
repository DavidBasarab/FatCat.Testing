using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Strings;

public class NotStringComparer(string subject) : NotComparerBase<string, NotStringComparer>(subject)
{
	public NotStringComparer Be(string expected, Options options = Options.CaseSensitive, string because = null)
	{
		if (StringEqualityHelper.AreEqual(Subject, expected, options))
		{
			CompareException.New(because ?? $"{Subject} should not be {expected}");
		}

		return this;
	}

	public NotStringComparer BeEmpty(string because = null)
	{
		if (Subject == string.Empty)
		{
			CompareException.New(because ?? "subject should not be empty");
		}

		return this;
	}

	public NotStringComparer BeEquivalentTo(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (StringEqualityHelper.AreEqual(Subject, expected, options))
		{
			CompareException.New(because ?? $"{Subject} should not be equivalent to {expected}");
		}

		return this;
	}

	public NotStringComparer BeNull(string because = null)
	{
		if (Subject == null)
		{
			CompareException.New(because ?? "subject should not be null");
		}

		return this;
	}

	public NotStringComparer BeNullOrEmpty(string because = null)
	{
		if (string.IsNullOrEmpty(Subject))
		{
			var display = Subject == null ? "null" : "subject";

			CompareException.New(because ?? $"{display} should not be null or empty");
		}

		return this;
	}

	public NotStringComparer BeNullOrWhiteSpace(string because = null)
	{
		if (string.IsNullOrWhiteSpace(Subject))
		{
			var display =
				Subject == null ? "null"
				: Subject.Length == 0 ? "subject"
				: Subject;

			CompareException.New(because ?? $"{display} should not be null or whitespace");
		}

		return this;
	}

	public NotStringComparer EndWith(string expected, string because = null)
	{
		if (Subject != null && Subject.EndsWith(expected, StringComparison.Ordinal))
		{
			CompareException.New(because ?? $"{Subject} should not end with {expected}");
		}

		return this;
	}

	public NotStringComparer EndWithEquivalentOf(string expected, string because = null)
	{
		if (Subject != null && Subject.EndsWith(expected, StringComparison.OrdinalIgnoreCase))
		{
			CompareException.New(because ?? $"{Subject} should not end with equivalent of {expected}");
		}

		return this;
	}

	public NotStringComparer HaveLength(int expected, string because = null)
	{
		if (Subject != null && Subject.Length == expected)
		{
			CompareException.New(because ?? $"{Subject} should not have length {expected}");
		}

		return this;
	}

	public NotStringComparer HaveValue(string because = null)
	{
		if (Subject != null)
		{
			CompareException.New(because ?? $"{Subject} should not have a value");
		}

		return this;
	}

	public NotStringComparer StartWith(string expected, string because = null)
	{
		if (Subject != null && Subject.StartsWith(expected, StringComparison.Ordinal))
		{
			CompareException.New(because ?? $"{Subject} should not start with {expected}");
		}

		return this;
	}

	public NotStringComparer StartWithEquivalentOf(string expected, string because = null)
	{
		if (Subject != null && Subject.StartsWith(expected, StringComparison.OrdinalIgnoreCase))
		{
			CompareException.New(because ?? $"{Subject} should not start with equivalent of {expected}");
		}

		return this;
	}
}
