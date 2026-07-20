namespace FatCat.Testing.Strings;

public static class LessThan
{
	public static OccurrenceConstraint Once() { return new OccurrenceConstraint(1, OccurrenceMode.LessThan); }

	public static OccurrenceConstraint Thrice() { return new OccurrenceConstraint(3, OccurrenceMode.LessThan); }

	public static OccurrenceConstraint Times(int count) { return new OccurrenceConstraint(count, OccurrenceMode.LessThan); }

	public static OccurrenceConstraint Twice() { return new OccurrenceConstraint(2, OccurrenceMode.LessThan); }
}