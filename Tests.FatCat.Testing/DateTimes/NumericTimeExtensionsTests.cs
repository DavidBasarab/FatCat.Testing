using FatCat.Testing.DateTimes;

namespace Tests.FatCat.Testing.DateTimes;

public class NumericTimeExtensionsTests : BaseTest
{
	[Fact]
	public void DaysMatchesTimeSpanFromDays()
	{
		3.Days().Should().Be(TimeSpan.FromDays(3));
	}

	[Fact]
	public void HoursMatchesTimeSpanFromHours()
	{
		2.Hours().Should().Be(TimeSpan.FromHours(2));
	}

	[Fact]
	public void MillisecondsMatchesTimeSpanFromMilliseconds()
	{
		250.Milliseconds().Should().Be(TimeSpan.FromMilliseconds(250));
	}

	[Fact]
	public void MinutesMatchesTimeSpanFromMinutes()
	{
		30.Minutes().Should().Be(TimeSpan.FromMinutes(30));
	}

	[Fact]
	public void SecondsMatchesTimeSpanFromSeconds()
	{
		45.Seconds().Should().Be(TimeSpan.FromSeconds(45));
	}
}
