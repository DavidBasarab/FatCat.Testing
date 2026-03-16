namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeBeUtcTests : BaseTest
{
	[Fact]
	public void BadBeUtc()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => ((DateTime?)localDate).Should().BeUtc(), "2024-06-15 10:30:45 should be UTC");
	}

	[Fact]
	public void BadBeUtcNullValue() { RunCompareFailTest(() => ((DateTime?)null).Should().BeUtc(), "null should be UTC"); }

	[Fact]
	public void BadBeUtcWithBecause()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => ((DateTime?)localDate).Should().BeUtc("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeUtc()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(
							() => ((DateTime?)utcDate).Should().Not.BeUtc(),
							"2024-06-15 10:30:45 should not be UTC"
						);
	}

	[Fact]
	public void BadNotBeUtcWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => ((DateTime?)utcDate).Should().Not.BeUtc("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeUtc()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		((DateTime?)utcDate).Should().BeUtc();
	}

	[Fact]
	public void GoodNotBeUtc()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		((DateTime?)localDate).Should().Not.BeUtc();
	}

	[Fact]
	public void GoodNotBeUtcWhenNull() { ((DateTime?)null).Should().Not.BeUtc(); }
}