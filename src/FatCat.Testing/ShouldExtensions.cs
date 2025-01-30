using FatCat.Testing.Exceptions;

namespace FatCat.Testing;

public static class ShouldExtensions
{
	public static NumberShouldComparer Should(this int subject)
	{
		return new NumberShouldComparer(subject);
	}
}

public class NumberShouldComparer(int subject)
	: ShouldComparer<NumberShouldComparer, NotNumberShouldComparer>(subject)
{
	public NumberShouldComparer BeInRange(int lower, int upper)
	{
		throw new NotImplementedException();
	}

	public override NumberShouldComparer Be(object expected)
	{
		if (subject != (int)expected)
		{
			CompareException.Mismatch(expected, subject);
		}

		return this;
	}
}

public class NotNumberShouldComparer(int subject) : NotShouldComparer<NotNumberShouldComparer>(subject)
{
	public override NotNumberShouldComparer Be(object expected)
	{
		throw new NotImplementedException();
	}
}

public abstract class ShouldComparer<TComparer, TNotComparer>(object subject)
	where TComparer : ShouldComparer<TComparer, TNotComparer>
	where TNotComparer : new()
{
	public TNotComparer Not
	{
		get => new(subject);
	}

	public abstract TComparer Be(object expected);
}

public abstract class NotShouldComparer<TNotComparer>(object subject)
	where TNotComparer : NotShouldComparer<TNotComparer>
{
	public abstract TNotComparer Be(object expected);
}
