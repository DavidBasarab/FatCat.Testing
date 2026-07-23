using Registrar = FatCat.Testing.Equivalency.Equivalency;

namespace Tests.FatCat.Testing.Equivalency;

// The test namespace mirrors the source folder (Equivalency), which shadows the FatCat.Testing.Equivalency.Equivalency type. The Registrar alias reaches the real registration surface consumers call as Equivalency.Using<T>.
public class EquivalencyUsingTests : BaseTest, IDisposable
{
	public void Dispose() { Registrar.Reset(); }

	[Fact]
	public void GoodUsingDateTimeCloseness()
	{
		Registrar.Using<DateTime>((subject, expected) => (subject - expected).Duration() <= TimeSpan.FromSeconds(1));

		var timestamp = new DateTime(2026, 7, 22, 10, 0, 0);
		var actual = new Reservation { Name = "Ada", Timestamp = timestamp };
		var expected = new Reservation { Name = "Ada", Timestamp = timestamp.AddMilliseconds(500) };

		actual.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void BadUsingDateTimeCloseness()
	{
		Registrar.Using<DateTime>((subject, expected) => (subject - expected).Duration() <= TimeSpan.FromSeconds(1));

		var timestamp = new DateTime(2026, 7, 22, 10, 0, 0);
		var actual = new Reservation { Name = "Ada", Timestamp = timestamp };
		var expected = new Reservation { Name = "Ada", Timestamp = timestamp.AddSeconds(2) };

		RunCompareFailTest(() => actual.Should().BeEquivalentTo(expected));
	}

	[Fact]
	public void GoodUsingShortCircuitsRecursion()
	{
		Registrar.Using<ThrowingMember>((subject, expected) => true);

		new ThrowingMember().Should().BeEquivalentTo(new ThrowingMember());
	}

	[Fact]
	public void GoodResetClearsRules()
	{
		Registrar.Using<DateTime>((subject, expected) => (subject - expected).Duration() <= TimeSpan.FromSeconds(1));

		Registrar.Reset();

		var timestamp = new DateTime(2026, 7, 22, 10, 0, 0);
		var actual = new Reservation { Name = "Ada", Timestamp = timestamp };
		var expected = new Reservation { Name = "Ada", Timestamp = timestamp.AddSeconds(2) };

		RunCompareFailTest(() => actual.Should().BeEquivalentTo(expected));
	}

	[Fact]
	public void GoodLastRegistrationWins()
	{
		Registrar.Using<DateTime>((subject, expected) => false);
		Registrar.Using<DateTime>((subject, expected) => true);

		var timestamp = new DateTime(2026, 7, 22, 10, 0, 0);
		var actual = new Reservation { Name = "Ada", Timestamp = timestamp };
		var expected = new Reservation { Name = "Ada", Timestamp = timestamp.AddSeconds(30) };

		actual.Should().BeEquivalentTo(expected);
	}
}
