using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Booleans;

public class NullableBoolComparer(bool? subject) : ComparerBase<bool?, NullableBoolComparer>(subject)
{
	public NotNullableBoolComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject.HasValue ? $"{Subject.Value}" : "null";
	}

	public NullableBoolComparer Be(bool expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value != expected) { CompareException.New(because ?? $"{SubjectDisplay} should be {expected}"); }

		return this;
	}

	public NullableBoolComparer BeFalse(string because = null)
	{
		if (!Subject.HasValue || Subject.Value) { CompareException.New(because ?? $"{SubjectDisplay} should be False"); }

		return this;
	}

	public NullableBoolComparer BeNull(string because = null)
	{
		if (Subject.HasValue) { CompareException.New(because ?? $"{Subject.Value} should be null"); }

		return this;
	}

	public NullableBoolComparer BeTrue(string because = null)
	{
		if (!Subject.HasValue || !Subject.Value) { CompareException.New(because ?? $"{SubjectDisplay} should be True"); }

		return this;
	}

	public NullableBoolComparer HaveValue(string because = null)
	{
		if (!Subject.HasValue) { CompareException.New(because ?? "value should not be null"); }

		return this;
	}
}