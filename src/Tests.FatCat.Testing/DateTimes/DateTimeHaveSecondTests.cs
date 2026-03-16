namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeHaveSecondTests : BaseTest
{
	[Fact]
	public void BadHaveSecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveSecond(46));
	}

	[Fact]
	public void BadHaveSecondShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveSecond(46), "2024-06-15 10:30:45 should have second 46");
	}

	[Fact]
	public void BadHaveSecondWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().HaveSecond(46, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotHaveSecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveSecond(45));
	}

	[Fact]
	public void BadNotHaveSecondShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => date.Should().Not.HaveSecond(45),
							"2024-06-15 10:30:45 should not have second 45"
						);
	}

	[Fact]
	public void BadNotHaveSecondWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.HaveSecond(45, "custom because"), "custom because");
	}

	[Fact]
	public void GoodHaveSecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().HaveSecond(45);
	}

	[Fact]
	public void GoodNotHaveSecond()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().Not.HaveSecond(46);
	}
}