namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeHaveMinuteTests : BaseTest
{
	[Fact]
	public void BadHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().HaveMinute(31),
							"2024-06-15 10:30:45 should have minute 31"
						);
	}

	[Fact]
	public void BadHaveMinuteNullValue() { RunCompareFailTest(() => ((DateTime?)null).Should().HaveMinute(31), "null should have minute 31"); }

	[Fact]
	public void BadHaveMinuteWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => ((DateTime?)date).Should().HaveMinute(31, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().Not.HaveMinute(30),
							"2024-06-15 10:30:45 should not have minute 30"
						);
	}

	[Fact]
	public void BadNotHaveMinuteWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => ((DateTime?)date).Should().Not.HaveMinute(30, "custom because"),
							"custom because"
						);
	}

	[Fact]
	public void GoodHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().HaveMinute(30);
	}

	[Fact]
	public void GoodNotHaveMinute()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().Not.HaveMinute(31);
	}

	[Fact]
	public void GoodNotHaveMinuteWhenNull() { ((DateTime?)null).Should().Not.HaveMinute(30); }
}