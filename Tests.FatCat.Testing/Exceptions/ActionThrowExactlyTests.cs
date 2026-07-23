namespace Tests.FatCat.Testing.Exceptions;

public class ActionThrowExactlyTests : BaseTest
{
	[Fact]
	public void BadNotThrowExactly()
	{
		Action action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().Not.ThrowExactly<ArgumentException>());
	}

	[Fact]
	public void BadNotThrowExactlyShowsCorrectMessage()
	{
		Action action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().Not.ThrowExactly<ArgumentException>(), "should not throw exactly ArgumentException but did");
	}

	[Fact]
	public void BadNotThrowExactlyWithBecause()
	{
		Action action = () => throw new ArgumentException("boom");

		RunCompareFailTest(() => action.Should().Not.ThrowExactly<ArgumentException>("custom because"), "custom because");
	}

	[Fact]
	public void BadThrowExactly()
	{
		Action action = () => { };

		RunCompareFailTest(() => action.Should().ThrowExactly<ArgumentException>());
	}

	[Fact]
	public void BadThrowExactlyShowsCorrectMessage()
	{
		Action action = () => { };

		RunCompareFailTest(() => action.Should().ThrowExactly<ArgumentException>(), "should throw exactly ArgumentException but no exception was thrown");
	}

	[Fact]
	public void BadThrowExactlyWhenDerivedType()
	{
		Action action = () => throw new ArgumentNullException("param");

		RunCompareFailTest(() => action.Should().ThrowExactly<ArgumentException>());
	}

	[Fact]
	public void BadThrowExactlyWhenDerivedTypeShowsCorrectMessage()
	{
		Action action = () => throw new ArgumentNullException("param");

		RunCompareFailTest(() => action.Should().ThrowExactly<ArgumentException>(), "should throw exactly ArgumentException but threw ArgumentNullException");
	}

	[Fact]
	public void BadThrowExactlyWithBecause()
	{
		Action action = () => { };

		RunCompareFailTest(() => action.Should().ThrowExactly<ArgumentException>("custom because"), "custom because");
	}

	[Fact]
	public void GoodNotThrowExactly()
	{
		Action action = () => throw new ArgumentNullException("param");

		action.Should().Not.ThrowExactly<ArgumentException>();
	}

	[Fact]
	public void GoodThrowExactly()
	{
		Action action = () => throw new ArgumentException("boom");

		action.Should().ThrowExactly<ArgumentException>();
	}

	[Fact]
	public void GoodThrowExactlyChainsWithMessage()
	{
		Action action = () => throw new ArgumentException("boom");

		action.Should().ThrowExactly<ArgumentException>().WithMessage("boom");
	}
}
