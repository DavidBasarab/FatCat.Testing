namespace FatCat.Testing.Strings;

public static class AtLeast
{
	public static OccurrenceConstraint Once()
	{
		return new OccurrenceConstraint(1, OccurrenceMode.AtLeast);
	}

	public static OccurrenceConstraint Thrice()
	{
		return new OccurrenceConstraint(3, OccurrenceMode.AtLeast);
	}

	public static OccurrenceConstraint Times(int count)
	{
		return new OccurrenceConstraint(count, OccurrenceMode.AtLeast);
	}

	public static OccurrenceConstraint Twice()
	{
		return new OccurrenceConstraint(2, OccurrenceMode.AtLeast);
	}
}
