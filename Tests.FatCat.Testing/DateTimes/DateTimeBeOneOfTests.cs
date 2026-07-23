namespace Tests.FatCat.Testing.DateTimes;

public class DateTimeBeOneOfTests : BaseTest
{
	[Fact]
	public void BadBeOneOf()
	{
		var subject = new DateTime(2026, 1, 3, 10, 0, 0);
		var first = new DateTime(2026, 1, 1, 10, 0, 0);
		var second = new DateTime(2026, 1, 2, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().BeOneOf(first, second));
	}

	[Fact]
	public void BadNotBeOneOf()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var first = new DateTime(2026, 1, 1, 10, 0, 0);
		var second = new DateTime(2026, 1, 2, 10, 0, 0);

		RunCompareFailTest(() => subject.Should().Not.BeOneOf(first, second));
	}

	[Fact]
	public void GoodBeOneOf()
	{
		var subject = new DateTime(2026, 1, 1, 10, 0, 0);
		var first = new DateTime(2026, 1, 1, 10, 0, 0);
		var second = new DateTime(2026, 1, 2, 10, 0, 0);

		subject.Should().BeOneOf(first, second);
	}

	[Fact]
	public void GoodNotBeOneOf()
	{
		var subject = new DateTime(2026, 1, 3, 10, 0, 0);
		var first = new DateTime(2026, 1, 1, 10, 0, 0);
		var second = new DateTime(2026, 1, 2, 10, 0, 0);

		subject.Should().Not.BeOneOf(first, second);
	}
}
