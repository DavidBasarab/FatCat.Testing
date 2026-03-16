namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeHaveYearTests : BaseTest
{
	[Fact]
	public void BadHaveYear()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveYear(2025));
	}

	[Fact]
	public void BadHaveYearShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveYear(2025), "2024-06-15 10:30:45 should have year 2025");
	}

	[Fact]
	public void BadHaveYearWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveYear(2025, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveYear()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveYear(2024));
	}

	[Fact]
	public void BadNotHaveYearShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => date.Should().Not.HaveYear(2024),
							"2024-06-15 10:30:45 should not have year 2024"
						);
	}

	[Fact]
	public void BadNotHaveYearWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveYear(2024, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveYear()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().HaveYear(2024);
	}

	[Fact]
	public void GoodNotHaveYear()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().Not.HaveYear(2025);
	}
}