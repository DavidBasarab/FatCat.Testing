namespace FatCat.Testing.Strings;

public class OccurrenceConstraint
{
	internal int Count { get; }

	internal OccurrenceMode Mode { get; }

	internal OccurrenceConstraint(int count, OccurrenceMode mode)
	{
		Count = count;
		Mode = mode;
	}

	internal string Description()
	{
		var unit = Count == 1 ? "time" : "times";

		if (Mode == OccurrenceMode.Exactly) { return $"exactly {Count} {unit}"; }

		if (Mode == OccurrenceMode.AtLeast) { return $"at least {Count} {unit}"; }

		if (Mode == OccurrenceMode.AtMost) { return $"at most {Count} {unit}"; }

		if (Mode == OccurrenceMode.MoreThan) { return $"more than {Count} {unit}"; }

		return $"less than {Count} {unit}";
	}

	internal bool IsSatisfiedBy(int actual)
	{
		if (Mode == OccurrenceMode.Exactly) { return actual == Count; }

		if (Mode == OccurrenceMode.AtLeast) { return actual >= Count; }

		if (Mode == OccurrenceMode.AtMost) { return actual <= Count; }

		if (Mode == OccurrenceMode.MoreThan) { return actual > Count; }

		return actual < Count;
	}
}

internal enum OccurrenceMode
{
	Exactly,
	AtLeast,
	AtMost,
	MoreThan,
	LessThan,
}