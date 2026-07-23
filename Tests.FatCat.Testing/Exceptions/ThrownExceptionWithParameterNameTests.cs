namespace Tests.FatCat.Testing.Exceptions;

public class ThrownExceptionWithParameterNameTests : BaseTest
{
	[Fact]
	public void BadWithParameterName()
	{
		Action action = () => throw new ArgumentNullException("param");

		RunCompareFailTest(() => action.Should().Throw<ArgumentNullException>().WithParameterName("other"));
	}

	[Fact]
	public void BadWithParameterNameShowsCorrectMessage()
	{
		Action action = () => throw new ArgumentNullException("param");

		RunCompareFailTest(() => action.Should().Throw<ArgumentNullException>().WithParameterName("other"), "thrown exception parameter name param should be other");
	}

	[Fact]
	public void BadWithParameterNameWhenNotArgumentExceptionShowsCorrectMessage()
	{
		Action action = () => throw new InvalidOperationException("boom");

		RunCompareFailTest(() => action.Should().Throw<InvalidOperationException>().WithParameterName("param"), "thrown InvalidOperationException should be an ArgumentException to read the parameter name but was not");
	}

	[Fact]
	public void BadWithParameterNameWithBecause()
	{
		Action action = () => throw new ArgumentNullException("param");

		RunCompareFailTest(() => action.Should().Throw<ArgumentNullException>().WithParameterName("other", "custom because"), "custom because");
	}

	[Fact]
	public void GoodWithParameterName()
	{
		Action action = () => throw new ArgumentNullException("param");

		action.Should().Throw<ArgumentNullException>().WithParameterName("param");
	}

	[Fact]
	public void GoodWithParameterNameChainsWithMessage()
	{
		Action action = () => throw new ArgumentException("bad arg", "param");

		action.Should().Throw<ArgumentException>().WithParameterName("param").WithMessage("bad arg (Parameter 'param')");
	}
}
