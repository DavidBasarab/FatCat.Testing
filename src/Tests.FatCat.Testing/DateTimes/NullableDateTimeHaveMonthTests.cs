namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeHaveMonthTests : BaseTest
{
	[Fact]
	public void BadHaveMonth()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().HaveMonth(7),
			"2024-06-15 10:30:45 should have month 7"
		);
	}

	[Fact]
	public void BadHaveMonthNullValue()
	{
		RunCompareFailTest(() => ((DateTime?)null).Should().HaveMonth(7), "null should have month 7");
	}

	[Fact]
	public void BadHaveMonthWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => ((DateTime?)date).Should().HaveMonth(7, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveMonth()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
			() => ((DateTime?)date).Should().Not.HaveMonth(6),
			"2024-06-15 10:30:45 should not have month 6"
		);
	}

	[Fact]
	public void BadNotHaveMonthWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => ((DateTime?)date).Should().Not.HaveMonth(6, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveMonth()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().HaveMonth(6);
	}

	[Fact]
	public void GoodNotHaveMonth()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().Not.HaveMonth(7);
	}

	[Fact]
	public void GoodNotHaveMonthWhenNull()
	{
		((DateTime?)null).Should().Not.HaveMonth(6);
	}
}
