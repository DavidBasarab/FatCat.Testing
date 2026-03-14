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

	public StringComparer HaveValue(string because = null)
	{
		if (Subject == null)
		{
			CompareException.New(because ?? "subject should have a value");
		}

		return this;
	}
}
