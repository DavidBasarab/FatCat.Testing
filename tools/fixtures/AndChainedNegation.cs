using FatCat.Testing;

namespace Consumer.Sample;

public class AndChainedNegationSample
{
	public void ChainedThroughAnd(List<int> numbers)
	{
		numbers.Should().Contain(1).And.NotContain(7);
	}
}
