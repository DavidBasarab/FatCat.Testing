namespace Tests.FatCat.Testing.DateTimes;

public class NullableDateTimeBeNullTests : BaseTest
{
	[Fact]
	public void BadBeNull()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => ((DateTime?)date).Should().BeNull(), "2024-06-15 10:30:45 should be null");
	}

	[Fact]
	public void BadBeNullWithBecause()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		RunCompareFailTest(() => ((DateTime?)date).Should().BeNull("custom because"), "custom because");
	}

	[Fact]
	public void BadHaveValue() { RunCompareFailTest(() => ((DateTime?)null).Should().HaveValue(), "value should not be null"); }

	[Fact]
	public void BadHaveValueWithBecause() { RunCompareFailTest(() => ((DateTime?)null).Should().HaveValue("custom because"), "custom because"); }

	[Fact]
	public void GoodBeNull() { ((DateTime?)null).Should().BeNull(); }

	[Fact]
	public void GoodHaveValue()
	{
		var date = new DateTime(2024, 6, 15, 10, 30, 45);

		((DateTime?)date).Should().HaveValue();
	}
}