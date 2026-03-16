using FatCat.Testing.Comparers;
using FatCat.Testing.Exceptions;

namespace FatCat.Testing.TimeSpans;

public class NullableTimeSpanComparer(TimeSpan? subject)
	: ComparerBase<TimeSpan?, NullableTimeSpanComparer>(subject)
{
	public NotNullableTimeSpanComparer Not { get; } = new(subject);

	private string SubjectDisplay
	{
		get => Subject.HasValue ? $"{Subject.Value}" : "null";
	}

	public NullableTimeSpanComparer Be(TimeSpan expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value != expected) { CompareException.New(because ?? $"{SubjectDisplay} should be {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer BeCloseTo(TimeSpan expected, TimeSpan precision, string because = null)
	{
		if (!Subject.HasValue || Math.Abs((Subject.Value - expected).Ticks) > precision.Ticks) { CompareException.New(because ?? $"{SubjectDisplay} should be within {precision} of {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer BeGreaterThan(TimeSpan expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value <= expected) { CompareException.New(because ?? $"{SubjectDisplay} should be greater than {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer BeGreaterThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value < expected) { CompareException.New(because ?? $"{SubjectDisplay} should be greater than or equal to {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer BeLessThan(TimeSpan expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value >= expected) { CompareException.New(because ?? $"{SubjectDisplay} should be less than {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer BeLessThanOrEqualTo(TimeSpan expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value > expected) { CompareException.New(because ?? $"{SubjectDisplay} should be less than or equal to {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer BeNegative(string because = null)
	{
		if (!Subject.HasValue || Subject.Value >= TimeSpan.Zero) { CompareException.New(because ?? $"{SubjectDisplay} should be negative"); }

		return this;
	}

	public NullableTimeSpanComparer BeNull(string because = null)
	{
		if (Subject.HasValue) { CompareException.New(because ?? $"{Subject.Value} should be null"); }

		return this;
	}

	public NullableTimeSpanComparer BePositive(string because = null)
	{
		if (!Subject.HasValue || Subject.Value <= TimeSpan.Zero) { CompareException.New(because ?? $"{SubjectDisplay} should be positive"); }

		return this;
	}

	public NullableTimeSpanComparer HaveDays(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Days != expected) { CompareException.New(because ?? $"{SubjectDisplay} should have days {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer HaveHours(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Hours != expected) { CompareException.New(because ?? $"{SubjectDisplay} should have hours {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer HaveMilliseconds(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Milliseconds != expected) { CompareException.New(because ?? $"{SubjectDisplay} should have milliseconds {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer HaveMinutes(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Minutes != expected) { CompareException.New(because ?? $"{SubjectDisplay} should have minutes {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer HaveSeconds(int expected, string because = null)
	{
		if (!Subject.HasValue || Subject.Value.Seconds != expected) { CompareException.New(because ?? $"{SubjectDisplay} should have seconds {expected}"); }

		return this;
	}

	public NullableTimeSpanComparer HaveValue(string because = null)
	{
		if (!Subject.HasValue) { CompareException.New(because ?? "value should not be null"); }

		return this;
	}
}