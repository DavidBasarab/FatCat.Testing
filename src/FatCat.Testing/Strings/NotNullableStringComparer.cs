#nullable enable

using System.Text.RegularExpressions;
using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Strings;

public class NotNullableStringComparer(string? subject)
	: NotComparerBase<string?, NotNullableStringComparer>(subject)
{
	public NotNullableStringComparer Be(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (StringEqualityHelper.AreEqual(Subject, expected, options)) { CompareException.New(because ?? $"{Subject} should not be {expected}"); }

		return this;
	}

	public NotNullableStringComparer BeEmpty(string? because = null)
	{
		if (Subject == string.Empty) { CompareException.New(because ?? "subject should not be empty"); }

		return this;
	}

	public NotNullableStringComparer BeEquivalentTo(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (StringEqualityHelper.AreEqual(Subject, expected, options)) { CompareException.New(because ?? $"{Subject} should not be equivalent to {expected}"); }

		return this;
	}

	public NotNullableStringComparer BeLowerCased(string? because = null)
	{
		if (StringEqualityHelper.IsLowerCased(Subject)) { CompareException.New(because ?? $"{Subject} should not be lower cased"); }

		return this;
	}

	public NotNullableStringComparer BeNull(string? because = null)
	{
		if (Subject == null) { CompareException.New(because ?? "subject should not be null"); }

		return this;
	}

	public NotNullableStringComparer BeNullOrEmpty(string? because = null)
	{
		if (string.IsNullOrEmpty(Subject))
		{
			var display = Subject == null ? "null" : "subject";

			CompareException.New(because ?? $"{display} should not be null or empty");
		}

		return this;
	}

	public NotNullableStringComparer BeNullOrWhiteSpace(string? because = null)
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

	public NotNullableStringComparer BeUpperCased(string? because = null)
	{
		if (StringEqualityHelper.IsUpperCased(Subject)) { CompareException.New(because ?? $"{Subject} should not be upper cased"); }

		return this;
	}

	public NotNullableStringComparer Contain(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject != null && StringEqualityHelper.CountOccurrences(Subject, expected, options) > 0) { CompareException.New(because ?? $"{Subject} should not contain {expected}"); }

		return this;
	}

	public NotNullableStringComparer ContainAll(
		string[] expected,
		Options options = Options.CaseSensitive,
		string? because = null
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

	public NotNullableStringComparer ContainAny(
		string[] expected,
		Options options = Options.CaseSensitive,
		string? because = null
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

	public NotNullableStringComparer EndWith(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject != null && Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{Subject} should not end with {expected}"); }

		return this;
	}

	public NotNullableStringComparer EndWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject != null && Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{Subject} should not end with equivalent of {expected}"); }

		return this;
	}

	public NotNullableStringComparer HaveLength(int expected, string? because = null)
	{
		if (Subject != null && Subject.Length == expected) { CompareException.New(because ?? $"{Subject} should not have length {expected}"); }

		return this;
	}

	public NotNullableStringComparer HaveValue(string? because = null)
	{
		if (Subject != null) { CompareException.New(because ?? $"{Subject} should not have a value"); }

		return this;
	}

	public NotNullableStringComparer Match(
		string pattern,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject != null && StringEqualityHelper.MatchesWildcard(Subject, pattern, options)) { CompareException.New(because ?? $"{Subject} should not match {pattern}"); }

		return this;
	}

	public NotNullableStringComparer MatchRegex(string pattern, string? because = null)
	{
		if (Subject != null && Regex.IsMatch(Subject, pattern)) { CompareException.New(because ?? $"{Subject} should not match regex {pattern}"); }

		return this;
	}

	public NotNullableStringComparer MatchRegex(Regex regex, string? because = null)
	{
		if (Subject != null && regex.IsMatch(Subject)) { CompareException.New(because ?? $"{Subject} should not match regex {regex}"); }

		return this;
	}

	public NotNullableStringComparer StartWith(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject != null && Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{Subject} should not start with {expected}"); }

		return this;
	}

	public NotNullableStringComparer StartWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject != null && Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{Subject} should not start with equivalent of {expected}"); }

		return this;
	}
}