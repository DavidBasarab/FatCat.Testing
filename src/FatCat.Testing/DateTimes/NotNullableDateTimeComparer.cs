using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.DateTimes;

public class NotNullableDateTimeComparer(DateTime? subject)
	: NotComparerBase<DateTime?, NotNullableDateTimeComparer>(subject)
{
	private TimeSpan SubjectOffset
	{
		get =>
			Subject.Value.Kind == DateTimeKind.Utc
				? TimeSpan.Zero
				: TimeZoneInfo.Local.GetUtcOffset(Subject.Value);
	}

	public NotNullableDateTimeComparer Be(DateTime expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer BeAfter(DateTime expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value > expected)
		{
			CompareException.New(
				because
					?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be after {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer BeBefore(DateTime expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value < expected)
		{
			CompareException.New(
				because
					?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be before {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer BeCloseTo(DateTime expected, TimeSpan precision, string because = null)
	{
		if (Subject.HasValue && Math.Abs((Subject.Value - expected).Ticks) <= precision.Ticks)
		{
			CompareException.New(
				because
					?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be within {precision} of {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer BeLocal(string because = null)
	{
		if (Subject.HasValue && Subject.Value.Kind == DateTimeKind.Local)
		{
			CompareException.New(because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be local");
		}

		return this;
	}

	public NotNullableDateTimeComparer BeOnOrAfter(DateTime expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value >= expected)
		{
			CompareException.New(
				because
					?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be on or after {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer BeOnOrBefore(DateTime expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value <= expected)
		{
			CompareException.New(
				because
					?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be on or before {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer BeUtc(string because = null)
	{
		if (Subject.HasValue && Subject.Value.Kind == DateTimeKind.Utc)
		{
			CompareException.New(because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not be UTC");
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveDay(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Day == expected)
		{
			CompareException.New(because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have day {expected}");
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveHour(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Hour == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have hour {expected}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveKind(DateTimeKind expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Kind == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have kind {expected}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveMillisecond(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Millisecond == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have millisecond {expected}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveMinute(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Minute == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have minute {expected}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveMonth(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Month == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have month {expected}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveOffset(TimeSpan expected, string because = null)
	{
		if (Subject.HasValue && SubjectOffset == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have offset {expected}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveSecond(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Second == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have second {expected}"
			);
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveValue(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have a value");
		}

		return this;
	}

	public NotNullableDateTimeComparer HaveYear(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Year == expected)
		{
			CompareException.New(
				because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should not have year {expected}"
			);
		}

		return this;
	}
}
