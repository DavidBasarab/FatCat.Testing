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

	public NotStringComparer Contain(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject != null && StringEqualityHelper.CountOccurrences(Subject, expected, options) > 0)
		{
			CompareException.New(because ?? $"{Subject} should not contain {expected}");
		}

		return this;
	}

	public NotStringComparer ContainAll(
		string[] expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		var allContained = expected.All(e => StringEqualityHelper.CountOccurrences(Subject, e, options) > 0);

		if (allContained)
		{
			CompareException.New(
				because ?? $"{Subject ?? "null"} should not contain all of [{string.Join(", ", expected)}]"
			);
		}

		return this;
	}

	public NotStringComparer ContainAny(
		string[] expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		var found = expected.Where(e => StringEqualityHelper.CountOccurrences(Subject, e, options) > 0).ToList();

		if (found.Count > 0)
		{
			CompareException.New(
				because ?? $"{Subject ?? "null"} should not contain any of [{string.Join(", ", expected)}]"
			);
		}

		return this;
	}

	public NotStringComparer EndWith(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject != null && Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options)))
		{
			CompareException.New(because ?? $"{Subject} should not end with {expected}");
		}

		return this;
	}

	public NotStringComparer EndWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject != null && Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options)))
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

	public NotStringComparer StartWith(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject != null && Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options)))
		{
			CompareException.New(because ?? $"{Subject} should not start with {expected}");
		}

		return this;
	}

	public NotStringComparer StartWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject != null && Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options)))
		{
			CompareException.New(because ?? $"{Subject} should not start with equivalent of {expected}");
		}

		return this;
	}
}
