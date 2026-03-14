using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Strings;

public class StringComparer(string subject) : ComparerBase<string, StringComparer>(subject)
{
	public NotStringComparer Not { get; } = new(subject);

	public StringComparer Be(string expected, Options options = Options.CaseSensitive, string because = null)
	{
		if (!StringEqualityHelper.AreEqual(Subject, expected, options))
		{
			CompareException.New(because ?? $"{Subject} should be {expected}");
		}

		return this;
	}

	public StringComparer BeEmpty(string because = null)
	{
		if (Subject != string.Empty)
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should be empty");
		}

		return this;
	}

	public StringComparer BeEquivalentTo(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (!StringEqualityHelper.AreEqual(Subject, expected, options))
		{
			CompareException.New(because ?? $"{Subject} should be equivalent to {expected}");
		}

		return this;
	}

	public StringComparer BeNull(string because = null)
	{
		if (Subject != null)
		{
			CompareException.New(because ?? $"{Subject} should be null");
		}

		return this;
	}

	public StringComparer BeNullOrEmpty(string because = null)
	{
		if (!string.IsNullOrEmpty(Subject))
		{
			CompareException.New(because ?? $"{Subject} should be null or empty");
		}

		return this;
	}

	public StringComparer BeNullOrWhiteSpace(string because = null)
	{
		if (!string.IsNullOrWhiteSpace(Subject))
		{
			CompareException.New(because ?? $"{Subject} should be null or whitespace");
		}

		return this;
	}

	public StringComparer EndWith(string expected, string because = null)
	{
		if (Subject == null || !Subject.EndsWith(expected, StringComparison.Ordinal))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should end with {expected}");
		}

		return this;
	}

	public StringComparer EndWithEquivalentOf(string expected, string because = null)
	{
		if (Subject == null || !Subject.EndsWith(expected, StringComparison.OrdinalIgnoreCase))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should end with equivalent of {expected}");
		}

		return this;
	}

	public StringComparer HaveLength(int expected, string because = null)
	{
		if (Subject == null || Subject.Length != expected)
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should have length {expected}");
		}

		return this;
	}

	public StringComparer HaveValue(string because = null)
	{
		if (Subject == null)
		{
			CompareException.New(because ?? "subject should have a value");
		}

		return this;
	}

	public StringComparer StartWith(string expected, string because = null)
	{
		if (Subject == null || !Subject.StartsWith(expected, StringComparison.Ordinal))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should start with {expected}");
		}

		return this;
	}

	public StringComparer StartWithEquivalentOf(string expected, string because = null)
	{
		if (Subject == null || !Subject.StartsWith(expected, StringComparison.OrdinalIgnoreCase))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should start with equivalent of {expected}");
		}

		return this;
	}
}
