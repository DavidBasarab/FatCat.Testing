namespace Tests.FatCat.Testing.Exceptions;

public class ThrownExceptionWithInnerExceptionTests : BaseTest
{
	[Fact]
	public void BadWithInnerException()
	{
		Action action = () => throw new InvalidOperationException("outer");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithInnerException<ArgumentException>());
	}

	[Fact]
	public void BadWithInnerExceptionWhenNoInnerShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("outer");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithInnerException<ArgumentException>(), "thrown InvalidOperationException should have inner exception ArgumentException but had none");
	}

	[Fact]
	public void BadWithInnerExceptionWhenWrongInnerShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("outer", new FormatException("inner"));

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithInnerException<ArgumentException>(), "thrown InvalidOperationException should have inner exception ArgumentException but had FormatException");
	}

	[Fact]
	public void BadWithInnerExceptionWithBecause()
	{
		Action action = () => throw new InvalidOperationException("outer");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithInnerException<ArgumentException>("custom because"), "custom because");
	}

	[Fact]
	public void GoodWithInnerException()
	{
		Action action = () => throw new InvalidOperationException("outer", new ArgumentException("inner"));

		action.Should().Throw<InvalidOperationException>().WithInnerException<ArgumentException>();
	}

	[Fact]
	public void GoodWithInnerExceptionChainsWithMessage()
	{
		Action action = () => throw new InvalidOperationException("outer", new ArgumentException("inner"));

		action.Should().Throw<InvalidOperationException>().WithInnerException<ArgumentException>().WithMessage("inner");
	}

	[Fact]
	public void GoodWithInnerExceptionMatchesDerivedType()
	{
		Action action = () => throw new InvalidOperationException("outer", new ArgumentNullException("param"));

		action.Should().Throw<InvalidOperationException>().WithInnerException<ArgumentException>();
	}
}
