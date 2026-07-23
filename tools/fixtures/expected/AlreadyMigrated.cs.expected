using FatCat.Testing;

namespace Consumer.Sample;

public class AlreadyMigratedSample
{
	public void AlreadyUsingTheNotProperty(string name, object value, List<int> numbers)
	{
		value.Should().Not.BeNull();
		name.Should().Not.BeNullOrEmpty();
		numbers.Should().Not.Contain(7);
		numbers.Should().Not.BeEmpty();
	}
}
