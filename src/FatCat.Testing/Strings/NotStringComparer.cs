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

	public NotStringComparer HaveValue(string because = null)
	{
		if (Subject != null)
		{
			CompareException.New(because ?? $"{Subject} should not have a value");
		}

		return this;
	}
}
