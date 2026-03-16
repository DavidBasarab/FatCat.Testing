using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.DateTimes;

public class NotDateTimeComparer(DateTime subject) : NotComparerBase<DateTime, NotDateTimeComparer>(subject)
{
	private string SubjectFormatted
	{
		get => Subject.ToString("yyyy-MM-dd HH:mm:ss");
	}

	private TimeSpan SubjectOffset
	{
		get => Subject.Kind == DateTimeKind.Utc ? TimeSpan.Zero : TimeZoneInfo.Local.GetUtcOffset(Subject);
	}

	public NotDateTimeComparer Be(DateTime expected, string because = null)
	{
		if (Subject == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not be {expected:yyyy-MM-dd HH:mm:ss}");
		}

		return this;
	}

	public NotDateTimeComparer BeAfter(DateTime expected, string because = null)
	{
		if (Subject > expected)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should not be after {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotDateTimeComparer BeBefore(DateTime expected, string because = null)
	{
		if (Subject < expected)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should not be before {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotDateTimeComparer BeCloseTo(DateTime expected, TimeSpan precision, string because = null)
	{
		if (Math.Abs((Subject - expected).Ticks) <= precision.Ticks)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should not be within {precision} of {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotDateTimeComparer BeLocal(string because = null)
	{
		if (Subject.Kind == DateTimeKind.Local)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not be local");
		}

		return this;
	}

	public NotDateTimeComparer BeOnOrAfter(DateTime expected, string because = null)
	{
		if (Subject >= expected)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should not be on or after {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotDateTimeComparer BeOnOrBefore(DateTime expected, string because = null)
	{
		if (Subject <= expected)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should not be on or before {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotDateTimeComparer BeUtc(string because = null)
	{
		if (Subject.Kind == DateTimeKind.Utc)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not be UTC");
		}

		return this;
	}

	public NotDateTimeComparer HaveDay(int expected, string because = null)
	{
		if (Subject.Day == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have day {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveHour(int expected, string because = null)
	{
		if (Subject.Hour == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have hour {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveKind(DateTimeKind expected, string because = null)
	{
		if (Subject.Kind == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have kind {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveMillisecond(int expected, string because = null)
	{
		if (Subject.Millisecond == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have millisecond {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveMinute(int expected, string because = null)
	{
		if (Subject.Minute == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have minute {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveMonth(int expected, string because = null)
	{
		if (Subject.Month == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have month {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveOffset(TimeSpan expected, string because = null)
	{
		if (SubjectOffset == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have offset {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveSecond(int expected, string because = null)
	{
		if (Subject.Second == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have second {expected}");
		}

		return this;
	}

	public NotDateTimeComparer HaveYear(int expected, string because = null)
	{
		if (Subject.Year == expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should not have year {expected}");
		}

		return this;
	}
}
