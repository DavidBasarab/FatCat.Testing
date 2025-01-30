using FatCat.Testing.Numbers;

namespace FatCat.Testing;

public static class ShouldExtensions
{
	public static IShouldComparer Should(this int subject)
	{
		return new NumberShouldComparer(subject);
	}
}
