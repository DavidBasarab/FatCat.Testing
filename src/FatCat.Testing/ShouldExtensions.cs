using FatCat.Testing.Numbers;

namespace FatCat.Testing;

public static class ShouldExtensions
{
	public static IntComparer Should(this int subject)
	{
		return new IntComparer(subject);
	}
}
