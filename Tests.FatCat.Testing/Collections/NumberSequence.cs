using System.Collections;

namespace Tests.FatCat.Testing.Collections;

public class NumberSequence(IEnumerable<int> numbers) : IEnumerable<int>
{
	public IEnumerator<int> GetEnumerator()
	{
		return numbers.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
