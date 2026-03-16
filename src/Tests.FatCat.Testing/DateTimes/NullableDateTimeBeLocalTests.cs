namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeBeLocalTests : BaseTest
{
	[Fact]
	public void BadBeLocal()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => ((DateTime?)utcDate).Should().BeLocal(), "2024-06-15 10:30:45 should be local");
	}

	[Fact]
	public void BadBeLocalNullValue() { RunCompareFailTest(() => ((DateTime?)null).Should().BeLocal(), "null should be local"); }

	[Fact]
	public void BadBeLocalWithBecause()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		RunCompareFailTest(() => ((DateTime?)utcDate).Should().BeLocal("custom because"), "custom because");
	}

	[Fact]
	public void BadNotBeLocal()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(
							() => ((DateTime?)localDate).Should().Not.BeLocal(),
							"2024-06-15 10:30:45 should not be local"
						);
	}

	[Fact]
	public void BadNotBeLocalWithBecause()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		RunCompareFailTest(() => ((DateTime?)localDate).Should().Not.BeLocal("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeLocal()
	{
		var localDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Local);

		((DateTime?)localDate).Should().BeLocal();
	}

	[Fact]
	public void GoodNotBeLocal()
	{
		var utcDate = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

		((DateTime?)utcDate).Should().Not.BeLocal();
	}

	[Fact]
	public void GoodNotBeLocalWhenNull() { ((DateTime?)null).Should().Not.BeLocal(); }
}