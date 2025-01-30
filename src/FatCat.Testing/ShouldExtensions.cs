namespace FatCat.Testing;

public static class ShouldExtensions
{
	public static ShouldComparer Should(this int subject)
	{
		return new ShouldComparer(subject);
	}
}

public class ShouldComparer(object subject)
{
	public NotShouldComparer Not
	{
		get => new(subject);
	}

	public ShouldComparer Be(object expected)
	{
		// new NumberComparer().Compare(subject, expected);

		if (expected is int intExpected)
		{
			new NumberComparer().Compare((int)subject, intExpected);
		}

		return this;
	}
}

public class NotShouldComparer(object subject)
{
	public NotShouldComparer Be(object expected)
	{
		if (expected is int intExpected)
		{
			new NumberComparer().NotCompare((int)subject, intExpected);
		}

		return this;
	}
}
