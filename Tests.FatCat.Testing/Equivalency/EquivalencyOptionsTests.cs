using FatCat.Testing.Equivalency;

namespace Tests.FatCat.Testing.Equivalency;

public class EquivalencyOptionsTests : BaseTest
{
	[Fact]
	public void CloseDateTimesAreEquivalentWithinRegisteredTolerance()
	{
		try
		{
			EquivalencyOptions.Using<DateTime>((subject, expected) => Math.Abs((subject - expected).TotalSeconds) <= 10);

			object subject = new DateTime(2026, 7, 20, 12, 0, 0);
			object expected = new DateTime(2026, 7, 20, 12, 0, 3);

			subject.Should().BeEquivalentTo(expected);
		}
		finally
		{
			EquivalencyOptions.Reset();
		}
	}

	[Fact]
	public void FarDateTimesAreNotEquivalentWithinRegisteredTolerance()
	{
		try
		{
			EquivalencyOptions.Using<DateTime>((subject, expected) => Math.Abs((subject - expected).TotalSeconds) <= 1);

			object subject = new DateTime(2026, 7, 20, 12, 0, 0);

			RunCompareFailTest(() => subject.Should().BeEquivalentTo(new DateTime(2026, 7, 20, 12, 0, 3)));
		}
		finally
		{
			EquivalencyOptions.Reset();
		}
	}

	[Fact]
	public void RegisteredRuleAppliesToNestedDateTimeMember()
	{
		try
		{
			EquivalencyOptions.Using<DateTime>((subject, expected) => Math.Abs((subject - expected).TotalSeconds) <= 10);

			var subject = new CalendarEvent { Title = "Standup", OccursAt = new DateTime(2026, 7, 20, 9, 0, 0) };
			var expected = new CalendarEvent { Title = "Standup", OccursAt = new DateTime(2026, 7, 20, 9, 0, 3) };

			subject.Should().BeEquivalentTo(expected);
		}
		finally
		{
			EquivalencyOptions.Reset();
		}
	}

	[Fact]
	public void ResetRemovesRegisteredRule()
	{
		try
		{
			EquivalencyOptions.Using<DateTime>((subject, expected) => Math.Abs((subject - expected).TotalSeconds) <= 10);
			EquivalencyOptions.Reset();

			object subject = new DateTime(2026, 7, 20, 12, 0, 0);

			RunCompareFailTest(() => subject.Should().BeEquivalentTo(new DateTime(2026, 7, 20, 12, 0, 3)));
		}
		finally
		{
			EquivalencyOptions.Reset();
		}
	}

	[Fact]
	public void TryGetRuleReturnsFalseWhenNoRuleRegistered()
	{
		EquivalencyOptions.Reset();

		var found = EquivalencyOptions.TryGetRule(typeof(DateTime), out var rule);

		found.Should().BeFalse();
		rule.Should().BeNull();
	}
}
