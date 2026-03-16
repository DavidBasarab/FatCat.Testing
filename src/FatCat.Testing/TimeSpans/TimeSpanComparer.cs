using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.TimeSpans;

public class TimeSpanComparer(TimeSpan subject) : ComparerBase<TimeSpan, TimeSpanComparer>(subject)
{
	public NotTimeSpanComparer Not { get; } = new(subject);

	public TimeSpanComparer Be(TimeSpan expected, string because = null)
	{
		if (Subject != expected)
		{
			CompareException.New(because ?? $"{Subject} should be {expected}");
		}

		return this;
	}

	public TimeSpanComparer BeCloseTo(TimeSpan expected, TimeSpan precision, string because = null)
	{
		if (Math.Abs((Subject - expected).Ticks) > precision.Ticks)
		{
			CompareException.New(because ?? $"{Subject} should be within {precision} of {expected}");
		}

		return this;
	}

	public TimeSpanComparer BeGreaterThan(TimeSpan expected, string because = null)
	{
		if (Subject <= expected)
		{
			CompareException.New(because ?? $"{Subject} should be greater than {expected}");
		}

		return this;
	}

	public TimeSpanComparer BeGreaterThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (Subject < expected)
		{
			CompareException.New(because ?? $"{Subject} should be greater than or equal to {expected}");
		}

		return this;
	}

	public TimeSpanComparer BeLessThan(TimeSpan expected, string because = null)
	{
		if (Subject >= expected)
		{
			CompareException.New(because ?? $"{Subject} should be less than {expected}");
		}

		return this;
	}

	public TimeSpanComparer BeLessThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (Subject > expected)
		{
			CompareException.New(because ?? $"{Subject} should be less than or equal to {expected}");
		}

		return this;
	}

	public TimeSpanComparer BeNegative(string because = null)
	{
		if (Subject >= TimeSpan.Zero)
		{
			CompareException.New(because ?? $"{Subject} should be negative");
		}

		return this;
	}

	public TimeSpanComparer BePositive(string because = null)
	{
		if (Subject <= TimeSpan.Zero)
		{
			CompareException.New(because ?? $"{Subject} should be positive");
		}

		return this;
	}

	public TimeSpanComparer HaveDays(int expected, string because = null)
	{
		if (Subject.Days != expected)
		{
			CompareException.New(because ?? $"{Subject} should have days {expected}");
		}

		return this;
	}

	public TimeSpanComparer HaveHours(int expected, string because = null)
	{
		if (Subject.Hours != expected)
		{
			CompareException.New(because ?? $"{Subject} should have hours {expected}");
		}

		return this;
	}

	public TimeSpanComparer HaveMilliseconds(int expected, string because = null)
	{
		if (Subject.Milliseconds != expected)
		{
			CompareException.New(because ?? $"{Subject} should have milliseconds {expected}");
		}

		return this;
	}

	public TimeSpanComparer HaveMinutes(int expected, string because = null)
	{
		if (Subject.Minutes != expected)
		{
			CompareException.New(because ?? $"{Subject} should have minutes {expected}");
		}

		return this;
	}

	public TimeSpanComparer HaveSeconds(int expected, string because = null)
	{
		if (Subject.Seconds != expected)
		{
			CompareException.New(because ?? $"{Subject} should have seconds {expected}");
		}

		return this;
	}
}
