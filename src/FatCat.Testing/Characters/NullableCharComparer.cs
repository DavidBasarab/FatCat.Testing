using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Characters;

public class NullableCharComparer(char? subject) : ComparerBase<char?, NullableCharComparer>(subject)
{
	public NotNullableCharComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject.HasValue ? $"{Subject.Value}" : "null";
	}

	public NullableCharComparer Be(char expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be {expected}");
		}

		return this;
	}

	public NullableCharComparer BeControl(string because = null)
	{
		if (!Subject.HasValue || !char.IsControl(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be a control character");
		}

		return this;
	}

	public NullableCharComparer BeDigit(string because = null)
	{
		if (!Subject.HasValue || !char.IsDigit(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be a digit");
		}

		return this;
	}

	public NullableCharComparer BeLetter(string because = null)
	{
		if (!Subject.HasValue || !char.IsLetter(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be a letter");
		}

		return this;
	}

	public NullableCharComparer BeLetterOrDigit(string because = null)
	{
		if (!Subject.HasValue || !char.IsLetterOrDigit(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be a letter or digit");
		}

		return this;
	}

	public NullableCharComparer BeLowerCased(string because = null)
	{
		if (!Subject.HasValue || !char.IsLower(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be lower cased");
		}

		return this;
	}

	public NullableCharComparer BeNull(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value} should be null");
		}

		return this;
	}

	public NullableCharComparer BeUpperCased(string because = null)
	{
		if (!Subject.HasValue || !char.IsUpper(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be upper cased");
		}

		return this;
	}

	public NullableCharComparer BeWhiteSpace(string because = null)
	{
		if (!Subject.HasValue || !char.IsWhiteSpace(Subject.Value))
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be white space");
		}

		return this;
	}

	public NullableCharComparer HaveValue(string because = null)
	{
		if (!Subject.HasValue)
		{
			CompareException.New(because ?? "value should not be null");
		}

		return this;
	}
}
