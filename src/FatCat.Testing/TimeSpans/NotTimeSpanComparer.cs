using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.TimeSpans;

public class NotTimeSpanComparer(TimeSpan subject) : NotComparerBase<TimeSpan, NotTimeSpanComparer>(subject)
{
	public NotTimeSpanComparer Be(TimeSpan expected, string because = null)
	{
		if (Subject == expected) { CompareException.New(because ?? $"{Subject} should not be {expected}"); }

		return this;
	}

	public NotTimeSpanComparer BeCloseTo(TimeSpan expected, TimeSpan precision, string because = null)
	{
		if (Math.Abs((Subject - expected).Ticks) <= precision.Ticks) { CompareException.New(because ?? $"{Subject} should not be within {precision} of {expected}"); }

		return this;
	}

	public NotTimeSpanComparer BeGreaterThan(TimeSpan expected, string because = null)
	{
		if (Subject > expected) { CompareException.New(because ?? $"{Subject} should not be greater than {expected}"); }

		return this;
	}

	public NotTimeSpanComparer BeGreaterThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (Subject >= expected) { CompareException.New(because ?? $"{Subject} should not be greater than or equal to {expected}"); }

		return this;
	}

	public NotTimeSpanComparer BeLessThan(TimeSpan expected, string because = null)
	{
		if (Subject < expected) { CompareException.New(because ?? $"{Subject} should not be less than {expected}"); }

		return this;
	}

	public NotTimeSpanComparer BeLessThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (Subject <= expected) { CompareException.New(because ?? $"{Subject} should not be less than or equal to {expected}"); }

		return this;
	}

	public NotTimeSpanComparer BeNegative(string because = null)
	{
		if (Subject < TimeSpan.Zero) { CompareException.New(because ?? $"{Subject} should not be negative"); }

		return this;
	}

	public NotTimeSpanComparer BePositive(string because = null)
	{
		if (Subject > TimeSpan.Zero) { CompareException.New(because ?? $"{Subject} should not be positive"); }

		return this;
	}

	public NotTimeSpanComparer HaveDays(int expected, string because = null)
	{
		if (Subject.Days == expected) { CompareException.New(because ?? $"{Subject} should not have days {expected}"); }

		return this;
	}

	public NotTimeSpanComparer HaveHours(int expected, string because = null)
	{
		if (Subject.Hours == expected) { CompareException.New(because ?? $"{Subject} should not have hours {expected}"); }

		return this;
	}

	public NotTimeSpanComparer HaveMilliseconds(int expected, string because = null)
	{
		if (Subject.Milliseconds == expected) { CompareException.New(because ?? $"{Subject} should not have milliseconds {expected}"); }

		return this;
	}

	public NotTimeSpanComparer HaveMinutes(int expected, string because = null)
	{
		if (Subject.Minutes == expected) { CompareException.New(because ?? $"{Subject} should not have minutes {expected}"); }

		return this;
	}

	public NotTimeSpanComparer HaveSeconds(int expected, string because = null)
	{
		if (Subject.Seconds == expected) { CompareException.New(because ?? $"{Subject} should not have seconds {expected}"); }

		return this;
	}
}