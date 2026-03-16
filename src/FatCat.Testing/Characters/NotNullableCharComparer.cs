using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Characters;

public class NotNullableCharComparer(char? subject) : NotComparerBase<char?, NotNullableCharComparer>(subject)
{
	public NotNullableCharComparer Be(char expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value == expected) { CompareException.New(because ?? $"{expected} should not be {Subject.Value}"); }

		return this;
	}

	public NotNullableCharComparer BeControl(string because = null)
	{
		if (Subject.HasValue && char.IsControl(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be a control character"); }

		return this;
	}

	public NotNullableCharComparer BeDigit(string because = null)
	{
		if (Subject.HasValue && char.IsDigit(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be a digit"); }

		return this;
	}

	public NotNullableCharComparer BeLetter(string because = null)
	{
		if (Subject.HasValue && char.IsLetter(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be a letter"); }

		return this;
	}

	public NotNullableCharComparer BeLetterOrDigit(string because = null)
	{
		if (Subject.HasValue && char.IsLetterOrDigit(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be a letter or digit"); }

		return this;
	}

	public NotNullableCharComparer BeLowerCased(string because = null)
	{
		if (Subject.HasValue && char.IsLower(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be lower cased"); }

		return this;
	}

	public NotNullableCharComparer BeUpperCased(string because = null)
	{
		if (Subject.HasValue && char.IsUpper(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be upper cased"); }

		return this;
	}

	public NotNullableCharComparer BeWhiteSpace(string because = null)
	{
		if (Subject.HasValue && char.IsWhiteSpace(Subject.Value)) { CompareException.New(because ?? $"{Subject.Value} should not be white space"); }

		return this;
	}

	public NotNullableCharComparer HaveValue(string because = null)
	{
		if (Subject.HasValue) { CompareException.New(because ?? $"{Subject.Value} should not have a value"); }

		return this;
	}
}