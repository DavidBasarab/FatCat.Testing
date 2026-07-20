using FatCat.Testing.Comparers;
using FatCat.Testing.Equivalency;
using FatCat.Testing.Exceptions;
using FatCat.Testing.Formatting;

namespace FatCat.Testing.Objects;

public class NotObjectComparer(object subject) : NotComparerBase<object, NotObjectComparer>(subject)
{
	public NotObjectComparer Be(object expected, string because = null)
	{
		if (Equals(Subject, expected))
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should not be {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}

	public NotObjectComparer BeEquivalentTo(object expected, string because = null)
	{
		var result = new StructuralEquivalency().Compare(Subject, expected);

		if (result.IsEquivalent)
		{
			CompareException.New(
				because ?? $"{ValueFormatter.Format(Subject)} should not be equivalent to {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}

	public NotObjectComparer BeNull(string because = null)
	{
		if (Subject is null)
		{
			CompareException.New(because ?? $"{ValueFormatter.Format(Subject)} should not be null");
		}

		return this;
	}

	public NotObjectComparer BeSameAs(object expected, string because = null)
	{
		if (ReferenceEquals(Subject, expected))
		{
			CompareException.New(
				because
					?? $"{ValueFormatter.Format(Subject)} should not be the same instance as {ValueFormatter.Format(expected)}"
			);
		}

		return this;
	}
}
