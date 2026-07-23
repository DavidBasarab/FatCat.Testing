using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Guids;

public class NullableGuidComparer(Guid? subject) : ComparerBase<Guid?, NullableGuidComparer>(subject)
{
	public NotNullableGuidComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject.HasValue ? $"{Subject.Value}" : "null";
	}

	public NullableGuidComparer Be(Guid expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be {expected}");
		}

		return this;
	}

	public NullableGuidComparer Be(string expected, string because = null)
	{
		if (!Guid.TryParse(expected, out var parsed))
		{
			throw new ArgumentException($"'{expected}' is not a valid Guid");
		}

		return Be(parsed, because);
	}

	public NullableGuidComparer BeEmpty(string because = null)
	{
		if (!Subject.HasValue || Subject.Value != Guid.Empty)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be empty");
		}

		return this;
	}

	public NullableGuidComparer BeNull(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value} should be null");
		}

		return this;
	}

	public NullableGuidComparer HaveValue(string because = null)
	{
		if (!Subject.HasValue)
		{
			CompareException.New(because ?? "value should not be null");
		}

		return this;
	}
}
