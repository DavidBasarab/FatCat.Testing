namespace Tests.FatCat.Testing.Exceptions;

public class ThrownExceptionWhereTests : BaseTest
{
	[Fact]
	public void BadWhere()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().Where(exception => exception.Message == "bang"));
	}

	[Fact]
	public void BadWhereShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().Where(exception => exception.Message == "bang"), "thrown exception should match the predicate but did not");
	}

	[Fact]
	public void BadWhereWithBecause()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().Where(exception => exception.Message == "bang", "custom because"), "custom because");
	}

	[Fact]
	public void GoodWhere()
	{
		Action action = () => throw new InvalidOperationException("boom");

		action.Should().Throw<InvalidOperationException>().Where(exception => exception.Message == "boom");
	}

	[Fact]
	public void GoodWhereChains()
	{
		Action action = () => throw new InvalidOperationException("boom");

		action.Should().Throw<InvalidOperationException>().Where(exception => exception.Message == "boom").WithMessage("boom");
	}
}
