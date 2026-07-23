using FatCat.Testing.Exceptions;

namespace FatCat.Testing.DateTimes;

public class DateTimeDifferenceChain(DateTimeComparer comparer, TimeSpan tolerance, DifferenceKind kind)
{
	private DateTime Subject
	{
		get { return comparer.Subject; }
	}

	private string SubjectFormatted
	{
		get { return Subject.ToString("yyyy-MM-dd HH:mm:ss"); }
	}

	private string KindPhrase
	{
		get
		{
			return kind switch
			{
				DifferenceKind.LessThan => "less than",
				DifferenceKind.MoreThan => "more than",
				DifferenceKind.AtLeast => "at least",
				DifferenceKind.Within => "within",
				DifferenceKind.Exactly => "exactly",
				_ => throw new ArgumentOutOfRangeException(nameof(kind)),
			};
		}
	}

	public DateTimeComparer Before(DateTime other, string because = null)
	{
		if (Subject >= other)
		{
			CompareException.New(
				because
					?? $"{SubjectFormatted} should be {KindPhrase} {tolerance} before {other:yyyy-MM-dd HH:mm:ss} but is not before it"
			);
		}

		var difference = other - Subject;

		if (!DifferenceIsSatisfied(difference))
		{
			CompareException.New(
				because
					?? $"{SubjectFormatted} should be {KindPhrase} {tolerance} before {other:yyyy-MM-dd HH:mm:ss} but the difference is {difference}"
			);
		}

		return comparer;
	}

	public DateTimeComparer After(DateTime other, string because = null)
	{
		if (Subject <= other)
		{
			CompareException.New(
				because
					?? $"{SubjectFormatted} should be {KindPhrase} {tolerance} after {other:yyyy-MM-dd HH:mm:ss} but is not after it"
			);
		}

		var difference = Subject - other;

		if (!DifferenceIsSatisfied(difference))
		{
			CompareException.New(
				because
					?? $"{SubjectFormatted} should be {KindPhrase} {tolerance} after {other:yyyy-MM-dd HH:mm:ss} but the difference is {difference}"
			);
		}

		return comparer;
	}

	private bool DifferenceIsSatisfied(TimeSpan difference)
	{
		return kind switch
		{
			DifferenceKind.LessThan => difference < tolerance,
			DifferenceKind.MoreThan => difference > tolerance,
			DifferenceKind.AtLeast => difference >= tolerance,
			DifferenceKind.Within => difference <= tolerance,
			DifferenceKind.Exactly => difference == tolerance,
			_ => throw new ArgumentOutOfRangeException(nameof(kind)),
		};
	}
}
