using FatCat.Testing;

namespace Consumer.Sample;

public class PlainNegationSample
{
	public void SingleLineNegations(string name, object value, List<int> numbers)
	{
		value.Should().NotBeNull();
		name.Should().NotBeNullOrEmpty();
		name.Should().NotBeNullOrWhiteSpace();
		numbers.Should().NotContain(7);
		numbers.Should().NotBeEmpty();
		value.Should().NotBeSameAs(name);
	}
}
