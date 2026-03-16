using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.DateTimes;

public class NullableDateTimeComparer(DateTime? subject)
	: ComparerBase<DateTime?, NullableDateTimeComparer>(subject)
{
	public NotNullableDateTimeComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject.HasValue ? Subject.Value.ToString("yyyy-MM-dd HH:mm:ss") : "null";
	}

	private TimeSpan SubjectOffset
	{
		get =>
			Subject.Value.Kind == DateTimeKind.Utc
				? TimeSpan.Zero
				: TimeZoneInfo.Local.GetUtcOffset(Subject.Value);
	}

	public NullableDateTimeComparer Be(DateTime expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be {expected:yyyy-MM-dd HH:mm:ss}");
		}

		return this;
	}

	public NullableDateTimeComparer BeAfter(DateTime expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value <= expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be after {expected:yyyy-MM-dd HH:mm:ss}");
		}

		return this;
	}

	public NullableDateTimeComparer BeBefore(DateTime expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value >= expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be before {expected:yyyy-MM-dd HH:mm:ss}");
		}

		return this;
	}

	public NullableDateTimeComparer BeCloseTo(DateTime expected, TimeSpan precision, string because = null)
	{
		if (!Subject.HasValue || Math.Abs((Subject.Value - expected).Ticks) > precision.Ticks)
		{
			CompareException.New(
				because ?? $"{SubjectDisplay} should be within {precision} of {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NullableDateTimeComparer BeLocal(string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Kind != DateTimeKind.Local)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be local");
		}

		return this;
	}

	public NullableDateTimeComparer BeNull(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value:yyyy-MM-dd HH:mm:ss} should be null");
		}

		return this;
	}

	public NullableDateTimeComparer BeOnOrAfter(DateTime expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value < expected)
		{
			CompareException.New(
				because ?? $"{SubjectDisplay} should be on or after {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NullableDateTimeComparer BeOnOrBefore(DateTime expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value > expected)
		{
			CompareException.New(
				because ?? $"{SubjectDisplay} should be on or before {expected:yyyy-MM-dd HH:mm:ss}"
			);
		}

		return this;
	}

	public NullableDateTimeComparer BeUtc(string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Kind != DateTimeKind.Utc)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should be UTC");
		}

		return this;
	}

	public NullableDateTimeComparer HaveDay(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Day != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have day {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveHour(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Hour != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have hour {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveKind(DateTimeKind expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Kind != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have kind {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveMillisecond(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Millisecond != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have millisecond {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveMinute(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Minute != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have minute {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveMonth(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Month != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have month {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveOffset(TimeSpan expected, string because = null)
	{
		if (!Subject.HasValue || SubjectOffset != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have offset {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveSecond(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Second != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have second {expected}");
		}

		return this;
	}

	public NullableDateTimeComparer HaveValue(string because = null)
	{
		if (!Subject.HasValue)
		{
			CompareException.New(because ?? "value should not be null");
		}

		return this;
	}

	public NullableDateTimeComparer HaveYear(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Year != expected)
		{
			CompareException.New(because ?? $"{SubjectDisplay} should have year {expected}");
		}

		return this;
	}
}
