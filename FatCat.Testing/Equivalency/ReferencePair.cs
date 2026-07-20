using System.Runtime.CompilerServices;

namespace FatCat.Testing.Equivalency;

internal class ReferencePair(object subject, object expected)
{
	public object Subject { get; } = subject;

	public object Expected { get; } = expected;

	public override bool Equals(object other)
	{
		return other is ReferencePair pair
			&& ReferenceEquals(Subject, pair.Subject)
			&& ReferenceEquals(Expected, pair.Expected);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(RuntimeHelpers.GetHashCode(Subject), RuntimeHelpers.GetHashCode(Expected));
	}
}
