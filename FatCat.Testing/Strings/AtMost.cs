namespace FatCat.Testing.Strings;

public static class AtMost
{
	public static OccurrenceConstraint Once() { return new OccurrenceConstraint(1, OccurrenceMode.AtMost); }

	public static OccurrenceConstraint Thrice() { return new OccurrenceConstraint(3, OccurrenceMode.AtMost); }

	public static OccurrenceConstraint Times(int count) { return new OccurrenceConstraint(count, OccurrenceMode.AtMost); }

	public static OccurrenceConstraint Twice() { return new OccurrenceConstraint(2, OccurrenceMode.AtMost); }
}