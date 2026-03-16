namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeHaveHourTests : BaseTest
{
	[Fact]
	public void BadHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().HaveHour(11),
							"2024-06-15 10:30:45 should have hour 11"
						);
	}

	[Fact]
	public void BadHaveHourNullValue() { RunCompareFailTest(() => ((DateTime?)null).Should().HaveHour(11), "null should have hour 11"); }

	[Fact]
	public void BadHaveHourWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => ((DateTime?)date).Should().HaveHour(11, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().Not.HaveHour(10),
							"2024-06-15 10:30:45 should not have hour 10"
						);
	}

	[Fact]
	public void BadNotHaveHourWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => ((DateTime?)date).Should().Not.HaveHour(10, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().HaveHour(10);
	}

	[Fact]
	public void GoodNotHaveHour()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().Not.HaveHour(11);
	}

	[Fact]
	public void GoodNotHaveHourWhenNull() { ((DateTime?)null).Should().Not.HaveHour(10); }
}