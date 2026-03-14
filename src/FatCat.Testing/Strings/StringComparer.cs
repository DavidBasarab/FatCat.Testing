using System.Text.RegularExpressions;
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

	public StringComparer BeLowerCased(string because = null)
	{
		if (!StringEqualityHelper.IsLowerCased(Subject))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should be lower cased");
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

	public StringComparer BeUpperCased(string because = null)
	{
		if (!StringEqualityHelper.IsUpperCased(Subject))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should be upper cased");
		}

		return this;
	}

	public StringComparer Contain(string expected, Options options = Options.CaseSensitive, string because = null)
	{
		if (StringEqualityHelper.CountOccurrences(Subject, expected, options) == 0)
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should contain {expected}");
		}

		return this;
	}

	public StringComparer Contain(
		string expected,
		OccurrenceConstraint occurrence,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		var count = StringEqualityHelper.CountOccurrences(Subject, expected, options);

		if (!occurrence.IsSatisfiedBy(count))
		{
			CompareException.New(
				because
					?? $"{Subject ?? "null"} should contain {expected} {occurrence.Description()} but found {count}"
			);
		}

		return this;
	}

	public StringComparer ContainAll(
		string[] expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		var missing = expected
			.Where(e => StringEqualityHelper.CountOccurrences(Subject, e, options) == 0)
			.ToList();

		if (missing.Count > 0)
		{
			CompareException.New(
				because ?? $"{Subject ?? "null"} should contain all of [{string.Join(", ", missing)}]"
			);
		}

		return this;
	}

	public StringComparer ContainAny(
		string[] expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		var hasAny = expected.Any(e => StringEqualityHelper.CountOccurrences(Subject, e, options) > 0);

		if (!hasAny)
		{
			CompareException.New(
				because ?? $"{Subject ?? "null"} should contain at least one of [{string.Join(", ", expected)}]"
			);
		}

		return this;
	}

	public StringComparer EndWith(string expected, Options options = Options.CaseSensitive, string because = null)
	{
		if (Subject == null || !Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options)))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should end with {expected}");
		}

		return this;
	}

	public StringComparer EndWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject == null || !Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options)))
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

	public StringComparer Match(string pattern, Options options = Options.CaseSensitive, string because = null)
	{
		if (Subject == null || !StringEqualityHelper.MatchesWildcard(Subject, pattern, options))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should match {pattern}");
		}

		return this;
	}

	public StringComparer MatchRegex(string pattern, string because = null)
	{
		if (Subject == null || !Regex.IsMatch(Subject, pattern))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should match regex {pattern}");
		}

		return this;
	}

	public StringComparer MatchRegex(Regex regex, string because = null)
	{
		if (Subject == null || !regex.IsMatch(Subject))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should match regex {regex}");
		}

		return this;
	}

	public StringComparer StartWith(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject == null || !Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options)))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should start with {expected}");
		}

		return this;
	}

	public StringComparer StartWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string because = null
	)
	{
		if (Subject == null || !Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options)))
		{
			CompareException.New(because ?? $"{Subject ?? "null"} should start with equivalent of {expected}");
		}

		return this;
	}
}
