namespace Tests.FatCat.Testing.Extensibility;

public class CustomComparerTests : BaseTest
{
	[Fact]
	public void GoodBeOk() { new FakeWebResponse { StatusCode = 200 }.Should().BeOk(); }

	[Fact]
	public void BadBeOk() { RunCompareFailTest(() => new FakeWebResponse { StatusCode = 500 }.Should().BeOk()); }

	[Fact]
	public void BadBeOkShowsCorrectMessage()
	{
		RunCompareFailTest(() => new FakeWebResponse { StatusCode = 500 }.Should().BeOk(), "status code 500 should be 200 (OK)");
	}

	[Fact]
	public void BadBeOkWithBecause()
	{
		RunCompareFailTest(() => new FakeWebResponse { StatusCode = 500 }.Should().BeOk("custom because"), "custom because");
	}

	[Fact]
	public void GoodBeOkChains() { new FakeWebResponse { StatusCode = 200 }.Should().BeOk().BeOk(); }

	[Fact]
	public void GoodBeNotFound() { new FakeWebResponse { StatusCode = 404 }.Should().BeNotFound(); }

	[Fact]
	public void BadBeNotFound() { RunCompareFailTest(() => new FakeWebResponse { StatusCode = 200 }.Should().BeNotFound()); }

	[Fact]
	public void BadBeNotFoundShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new FakeWebResponse { StatusCode = 200 }.Should().BeNotFound(),
							"status code 200 should be 404 (Not Found)"
						);
	}

	[Fact]
	public void GoodNotBeOk() { new FakeWebResponse { StatusCode = 500 }.Should().Not.BeOk(); }

	[Fact]
	public void BadNotBeOk() { RunCompareFailTest(() => new FakeWebResponse { StatusCode = 200 }.Should().Not.BeOk()); }

	[Fact]
	public void BadNotBeOkShowsCorrectMessage()
	{
		RunCompareFailTest(
							() => new FakeWebResponse { StatusCode = 200 }.Should().Not.BeOk(),
							"status code 200 should not be 200 (OK)"
						);
	}

	[Fact]
	public void GoodCustomComparerUsesInheritedBeOfType() { new FakeWebResponse { StatusCode = 200 }.Should().BeOfType<FakeWebResponse>(); }

	[Fact]
	public void GoodComposingHelperReadsSubjectFromOutside()
	{
		var comparer = new FakeWebResponse { StatusCode = 200 }.Should();

		FakeWebResponseComposingHelper.ReadStatusCode(comparer).Should().Be(200);
	}
}
