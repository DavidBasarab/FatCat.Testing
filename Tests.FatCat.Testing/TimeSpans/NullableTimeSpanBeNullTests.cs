namespace Tests.FatCat.Testing.TimeSpans;

public class NullableTimeSpanBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().BeNull(), "01:00:00 should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		var span = TimeSpan.FromHours(1);

		RunCompareFailTest(() => ((TimeSpan?)span).Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadHaveValue() { RunCompareFailTest(() => ((TimeSpan?)null).Should().HaveValue(), "value should not be null"); }

	[Fact]
	public void BadHaveValueWithBecause() { RunCompareFailTest(() => ((TimeSpan?)null).Should().HaveValue("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNull() { ((TimeSpan?)null).Should().BeNull(); }

	[Fact]
	public void GoodHaveValue()
	{
		var span = TimeSpan.FromHours(1);

		((TimeSpan?)span).Should().HaveValue();
	}
}