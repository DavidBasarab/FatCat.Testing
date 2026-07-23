using FatCat.Testing;

namespace Consumer.Sample;

public class LineBrokenChainSample
{
	public void NegationSplitAcrossLines(object value)
	{
		value.Should().NotBeNull();
	}
}
