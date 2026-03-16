using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.DateTimes;

public class DateTimeComparer(DateTime subject) : ComparerBase<DateTime, DateTimeComparer>(subject)
{
	public NotDateTimeComparer Not { get; } = new(subject);

	private string SubjectFormatted
	{
		get => Subject.ToString("yyyy-MM-dd HH:mm:ss");
	}

	private TimeSpan SubjectOffset
	{
		get => Subject.Kind == DateTimeKind.Utc ? TimeSpan.Zero : TimeZoneInfo.Local.GetUtcOffset(Subject);
	}

	public DateTimeComparer Be(DateTime expected, string because = null)
	{
		if (Subject != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should be {expected:yyyy-MM-dd HH:mm:ss}");
		}

		return this;
	}

	public DateTimeComparer BeAfter(DateTime expected, string because = null)
	{
		if (Subject <= expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should be after {expected:yyyy-MM-dd HH:mm:ss}");
		}

		return this;
	}

	public DateTimeComparer BeBefore(DateTime expected, string because = null)
	{
		if (Subject >= expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should be before {expected:yyyy-MM-dd HH:mm:ss}");
		}

		return this;
	}

	public DateTimeComparer BeCloseTo(DateTime expected, TimeSpan precision, string because = null)
	{
		if (Math.Abs((Subject - expected).Ticks) > precision.Ticks)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should be within {precision} of {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public DateTimeComparer BeLocal(string because = null)
	{
		if (Subject.Kind != DateTimeKind.Local)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should be local");
		}

		return this;
	}

	public DateTimeComparer BeOnOrAfter(DateTime expected, string because = null)
	{
		if (Subject < expected)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should be on or after {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public DateTimeComparer BeOnOrBefore(DateTime expected, string because = null)
	{
		if (Subject > expected)
		{
			CompareException.New(
				because ?? $"{SubjectFormatted} should be on or before {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public DateTimeComparer BeUtc(string because = null)
	{
		if (Subject.Kind != DateTimeKind.Utc)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should be UTC");
		}

		return this;
	}

	public DateTimeComparer HaveDay(int expected, string because = null)
	{
		if (Subject.Day != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have day {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveHour(int expected, string because = null)
	{
		if (Subject.Hour != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have hour {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveKind(DateTimeKind expected, string because = null)
	{
		if (Subject.Kind != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have kind {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveMillisecond(int expected, string because = null)
	{
		if (Subject.Millisecond != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have millisecond {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveMinute(int expected, string because = null)
	{
		if (Subject.Minute != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have minute {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveMonth(int expected, string because = null)
	{
		if (Subject.Month != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have month {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveOffset(TimeSpan expected, string because = null)
	{
		if (SubjectOffset != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have offset {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveSecond(int expected, string because = null)
	{
		if (Subject.Second != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have second {expected}");
		}

		return this;
	}

	public DateTimeComparer HaveYear(int expected, string because = null)
	{
		if (Subject.Year != expected)
		{
			CompareException.New(because ?? $"{SubjectFormatted} should have year {expected}");
		}

		return this;
	}
}
