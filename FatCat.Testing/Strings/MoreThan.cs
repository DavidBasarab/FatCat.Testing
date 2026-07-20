namespace FatCat.Testing.Strings;

public static class MoreThan
{
	public static OccurrenceConstraint Once() { return new OccurrenceConstraint(1, OccurrenceMode.MoreThan); }

	public static OccurrenceConstraint Thrice() { return new OccurrenceConstraint(3, OccurrenceMode.MoreThan); }

	public static OccurrenceConstraint Times(int count) { return new OccurrenceConstraint(count, OccurrenceMode.MoreThan); }

	public static OccurrenceConstraint Twice() { return new OccurrenceConstraint(2, OccurrenceMode.MoreThan); }
}