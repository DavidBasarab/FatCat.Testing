using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Objects;

public class ObjectComparer(object subject) : ComparerBase<object, ObjectComparer>(subject)
{
	public NotObjectComparer Not { get; } = new(subject);

	public ObjectComparer Be(object expected, string because = null)
	{
		if (!Equals(Subject, expected))
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be {ValueFormatter.Format(expected)}");
		}

		return this;
	}

	public ObjectComparer BeNull(string because = null)
	{
		if (Subject is not null)
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should be null");
		}

		return this;
	}

	public ObjectComparer BeSameAs(object expected, string because = null)
	{
		if (!ReferenceEquals(Subject, expected))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should be the same instance as {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}
}
