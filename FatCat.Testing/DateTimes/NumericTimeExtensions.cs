namespace FatCat.Testing.DateTimes;

public static class NumericTimeExtensions
{
	public static TimeSpan Days(this int value)
	{
		return TimeSpan.FromDays(value);
	}

	public static TimeSpan Hours(this int value)
	{
		return TimeSpan.FromHours(value);
	}

	public static TimeSpan Milliseconds(this int value)
	{
		return TimeSpan.FromMilliseconds(value);
	}

	public static TimeSpan Minutes(this int value)
	{
		return TimeSpan.FromMinutes(value);
	}

	public static TimeSpan Seconds(this int value)
	{
		return TimeSpan.FromSeconds(value);
	}
}
