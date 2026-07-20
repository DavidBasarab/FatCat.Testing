#nullable enable

using System.Text.RegularExpressions;
using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Strings;

public class NullableStringComparer(string? subject) : ComparerBase<string?, NullableStringComparer>(subject)
{
	public NotNullableStringComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject ?? "null";
	}

	public NullableStringComparer Be(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (!StringEqualityHelper.AreEqual(Subject, expected, options)) { CompareException.New(because ?? $"{SubjectDisplay} should be {expected}"); }

		return this;
	}

	public NullableStringComparer BeEmpty(string? because = null)
	{
		if (Subject != string.Empty) { CompareException.New(because ?? $"{SubjectDisplay} should be empty"); }

		return this;
	}

	public NullableStringComparer BeEquivalentTo(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (!StringEqualityHelper.AreEqual(Subject, expected, options)) { CompareException.New(because ?? $"{SubjectDisplay} should be equivalent to {expected}"); }

		return this;
	}

	public NullableStringComparer BeLowerCased(string? because = null)
	{
		if (!StringEqualityHelper.IsLowerCased(Subject)) { CompareException.New(because ?? $"{SubjectDisplay} should be lower cased"); }

		return this;
	}

	public NullableStringComparer BeNull(string? because = null)
	{
		if (Subject != null) { CompareException.New(because ?? $"{Subject} should be null"); }

		return this;
	}

	public NullableStringComparer BeNullOrEmpty(string? because = null)
	{
		if (!string.IsNullOrEmpty(Subject)) { CompareException.New(because ?? $"{Subject} should be null or empty"); }

		return this;
	}

	public NullableStringComparer BeNullOrWhiteSpace(string? because = null)
	{
		if (!string.IsNullOrWhiteSpace(Subject)) { CompareException.New(because ?? $"{Subject} should be null or whitespace"); }

		return this;
	}

	public NullableStringComparer BeUpperCased(string? because = null)
	{
		if (!StringEqualityHelper.IsUpperCased(Subject)) { CompareException.New(because ?? $"{SubjectDisplay} should be upper cased"); }

		return this;
	}

	public NullableStringComparer Contain(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (StringEqualityHelper.CountOccurrences(Subject, expected, options) == 0) { CompareException.New(because ?? $"{SubjectDisplay} should contain {expected}"); }

		return this;
	}

	public NullableStringComparer Contain(
		string expected,
		OccurrenceConstraint occurrence,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		var count = StringEqualityHelper.CountOccurrences(Subject, expected, options);

		if (!occurrence.IsSatisfiedBy(count))
		{
			CompareException.New(
								because
								?? $"{SubjectDisplay} should contain {expected} {occurrence.Description()} but found {count}"
								);
		}

		return this;
	}

	public NullableStringComparer ContainAll(
		string[] expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		var missing = expected
					.Where(e => StringEqualityHelper.CountOccurrences(Subject, e, options) == 0)
					.ToList();

		if (missing.Count > 0)
		{
			CompareException.New(
								because ?? $"{SubjectDisplay} should contain all of [{string.Join(", ", missing)}]"
								);
		}

		return this;
	}

	public NullableStringComparer ContainAny(
		string[] expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		var hasAny = expected.Any(e => StringEqualityHelper.CountOccurrences(Subject, e, options) > 0);

		if (!hasAny)
		{
			CompareException.New(
								because ?? $"{SubjectDisplay} should contain at least one of [{string.Join(", ", expected)}]"
								);
		}

		return this;
	}

	public NullableStringComparer EndWith(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject == null || !Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{SubjectDisplay} should end with {expected}"); }

		return this;
	}

	public NullableStringComparer EndWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject == null || !Subject.EndsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{SubjectDisplay} should end with equivalent of {expected}"); }

		return this;
	}

	public NullableStringComparer HaveLength(int expected, string? because = null)
	{
		if (Subject == null || Subject.Length != expected) { CompareException.New(because ?? $"{SubjectDisplay} should have length {expected}"); }

		return this;
	}

	public NullableStringComparer HaveValue(string? because = null)
	{
		if (Subject == null) { CompareException.New(because ?? "subject should have a value"); }

		return this;
	}

	public NullableStringComparer Match(
		string pattern,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject == null || !StringEqualityHelper.MatchesWildcard(Subject, pattern, options)) { CompareException.New(because ?? $"{SubjectDisplay} should match {pattern}"); }

		return this;
	}

	public NullableStringComparer MatchRegex(string pattern, string? because = null)
	{
		if (Subject == null || !Regex.IsMatch(Subject, pattern)) { CompareException.New(because ?? $"{SubjectDisplay} should match regex {pattern}"); }

		return this;
	}

	public NullableStringComparer MatchRegex(Regex regex, string? because = null)
	{
		if (Subject == null || !regex.IsMatch(Subject)) { CompareException.New(because ?? $"{SubjectDisplay} should match regex {regex}"); }

		return this;
	}

	public NullableStringComparer StartWith(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject == null || !Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{SubjectDisplay} should start with {expected}"); }

		return this;
	}

	public NullableStringComparer StartWithEquivalentOf(
		string expected,
		Options options = Options.CaseSensitive,
		string? because = null
	)
	{
		if (Subject == null || !Subject.StartsWith(expected, StringEqualityHelper.ToComparison(options))) { CompareException.New(because ?? $"{SubjectDisplay} should start with equivalent of {expected}"); }

		return this;
	}
}