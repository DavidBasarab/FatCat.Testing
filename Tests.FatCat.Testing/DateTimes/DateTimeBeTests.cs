namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeTests : BaseTest
{
	[Fact]
	public void BadBe()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var otherDate = new DateTime(2024, 6, 15, 10, 30, 46);

		RunCompareFailTest(() => date.Should().Be(otherDate));
	}

	[Fact]
	public void BadBeShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var otherDate = new DateTime(2024, 6, 15, 10, 30, 46);

		RunCompareFailTest(() => date.Should().Be(otherDate), "2024-06-15 10:30:45 should be 2024-06-15 10:30:46");
	}

	[Fact]
	public void BadBeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var otherDate = new DateTime(2024, 6, 15, 10, 30, 46);

		RunCompareFailTest(() => date.Should().Be(otherDate, "custom because"), "custom because");
	}

	[Fact]
	public void BadNotBe()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.Be(date));
	}

	[Fact]
	public void BadNotBeShowsCorrectMessage()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(
							() => date.Should().Not.Be(date),
							"2024-06-15 10:30:45 should not be 2024-06-15 10:30:45"
						);
	}

	[Fact]
	public void BadNotBeWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => date.Should().Not.Be(date, "custom because"), "custom because");
	}

	[Fact]
	public void GoodBe()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		date.Should().Be(date);
	}

	[Fact]
	public void GoodNotBe()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);
		var otherDate = new DateTime(2024, 6, 15, 10, 30, 46);

		date.Should().Not.Be(otherDate);
	}
}