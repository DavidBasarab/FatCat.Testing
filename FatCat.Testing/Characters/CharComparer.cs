using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Characters;

public class CharComparer(char subject) : ComparerBase<char, CharComparer>(subject)
{
	public NotCharComparer Not { get; } = new(subject);

	public CharComparer Be(char expected, string because = null)
	{
		if (Subject != expected) { CompareException.New(because ?? $"{Subject} should be {expected}"); }

		return this;
	}

	public CharComparer BeControl(string because = null)
	{
		if (!char.IsControl(Subject)) { CompareException.New(because ?? $"{Subject} should be a control character"); }

		return this;
	}

	public CharComparer BeDigit(string because = null)
	{
		if (!char.IsDigit(Subject)) { CompareException.New(because ?? $"{Subject} should be a digit"); }

		return this;
	}

	public CharComparer BeLetter(string because = null)
	{
		if (!char.IsLetter(Subject)) { CompareException.New(because ?? $"{Subject} should be a letter"); }

		return this;
	}

	public CharComparer BeLetterOrDigit(string because = null)
	{
		if (!char.IsLetterOrDigit(Subject)) { CompareException.New(because ?? $"{Subject} should be a letter or digit"); }

		return this;
	}

	public CharComparer BeLowerCased(string because = null)
	{
		if (!char.IsLower(Subject)) { CompareException.New(because ?? $"{Subject} should be lower cased"); }

		return this;
	}

	public CharComparer BeUpperCased(string because = null)
	{
		if (!char.IsUpper(Subject)) { CompareException.New(because ?? $"{Subject} should be upper cased"); }

		return this;
	}

	public CharComparer BeWhiteSpace(string because = null)
	{
		if (!char.IsWhiteSpace(Subject)) { CompareException.New(because ?? $"{Subject} should be white space"); }

		return this;
	}
}