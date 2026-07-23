using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Guids;

public class GuidComparer(Guid subject) : ComparerBase<Guid, GuidComparer>(subject)
{
	public NotGuidComparer Not { get; } = new(subject);

	public GuidComparer Be(Guid expected, string because = null)
	{
		if (Subject != expected) { CompareException.New(because ?? $"{Subject} should be {expected}"); }

		return this;
	}

	public GuidComparer Be(string expected, string because = null)
	{
		if (!Guid.TryParse(expected, out var parsed)) { throw new ArgumentException($"'{expected}' is not a valid Guid"); }

		return Be(parsed, because);
	}

	public GuidComparer BeEmpty(string because = null)
	{
		if (Subject != Guid.Empty) { CompareException.New(because ?? $"{Subject} should be empty"); }

		return this;
	}
}