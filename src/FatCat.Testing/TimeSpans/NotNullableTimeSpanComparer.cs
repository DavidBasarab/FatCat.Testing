using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.TimeSpans;

public class NotNullableTimeSpanComparer(TimeSpan? subject)
	: NotComparerBase<TimeSpan?, NotNullableTimeSpanComparer>(subject)
{
	public NotNullableTimeSpanComparer Be(TimeSpan expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value == expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer BeCloseTo(TimeSpan expected, TimeSpan precision, string because = null)
	{
		if (Subject.HasValue && Math.Abs((Subject.Value - expected).Ticks) <= precision.Ticks)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be within {precision} of {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer BeGreaterThan(TimeSpan expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value > expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be greater than {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer BeGreaterThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value >= expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be greater than or equal to {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer BeLessThan(TimeSpan expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value < expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be less than {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer BeLessThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value <= expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be less than or equal to {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer BeNegative(string because = null)
	{
		if (Subject.HasValue && Subject.Value < TimeSpan.Zero)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be negative");
		}

		return this;
	}

	public NotNullableTimeSpanComparer BePositive(string because = null)
	{
		if (Subject.HasValue && Subject.Value > TimeSpan.Zero)
		{
			CompareException.New(because ?? $"{Subject.Value} should not be positive");
		}

		return this;
	}

	public NotNullableTimeSpanComparer HaveDays(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Days == expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have days {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer HaveHours(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Hours == expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have hours {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer HaveMilliseconds(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Milliseconds == expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have milliseconds {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer HaveMinutes(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Minutes == expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have minutes {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer HaveSeconds(int expected, string because = null)
	{
		if (Subject.HasValue && Subject.Value.Seconds == expected)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have seconds {expected}");
		}

		return this;
	}

	public NotNullableTimeSpanComparer HaveValue(string because = null)
	{
		if (Subject.HasValue)
		{
			CompareException.New(because ?? $"{Subject.Value} should not have a value");
		}

		return this;
	}
}
