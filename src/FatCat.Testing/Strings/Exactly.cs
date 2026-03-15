namespace FatCat.Testing.Strings;

public static class Exactly
{
	public static OccurrenceConstraint Once()
	{
		return new OccurrenceConstraint(1, OccurrenceMode.Exactly);
	}

	public static OccurrenceConstraint Thrice()
	{
		return new OccurrenceConstraint(3, OccurrenceMode.Exactly);
	}

	public static OccurrenceConstraint Times(int count)
	{
		return new OccurrenceConstraint(count, OccurrenceMode.Exactly);
	}

	public static OccurrenceConstraint Twice()
	{
		return new OccurrenceConstraint(2, OccurrenceMode.Exactly);
	}
}
