using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Characters;

public class NotCharComparer(char subject) : NotComparerBase<char, NotCharComparer>(subject)
{
	public NotCharComparer Be(char expected, string because = null)
	{
		if (Subject == expected) { CompareException.New(because ?? $"{expected} should not be {Subject}"); }

		return this;
	}

	public NotCharComparer BeControl(string because = null)
	{
		if (char.IsControl(Subject)) { CompareException.New(because ?? $"{Subject} should not be a control character"); }

		return this;
	}

	public NotCharComparer BeDigit(string because = null)
	{
		if (char.IsDigit(Subject)) { CompareException.New(because ?? $"{Subject} should not be a digit"); }

		return this;
	}

	public NotCharComparer BeLetter(string because = null)
	{
		if (char.IsLetter(Subject)) { CompareException.New(because ?? $"{Subject} should not be a letter"); }

		return this;
	}

	public NotCharComparer BeLetterOrDigit(string because = null)
	{
		if (char.IsLetterOrDigit(Subject)) { CompareException.New(because ?? $"{Subject} should not be a letter or digit"); }

		return this;
	}

	public NotCharComparer BeLowerCased(string because = null)
	{
		if (char.IsLower(Subject)) { CompareException.New(because ?? $"{Subject} should not be lower cased"); }

		return this;
	}

	public NotCharComparer BeUpperCased(string because = null)
	{
		if (char.IsUpper(Subject)) { CompareException.New(because ?? $"{Subject} should not be upper cased"); }

		return this;
	}

	public NotCharComparer BeWhiteSpace(string because = null)
	{
		if (char.IsWhiteSpace(Subject)) { CompareException.New(because ?? $"{Subject} should not be white space"); }

		return this;
	}
}