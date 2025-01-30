using FatCat.Testing.Numbers;

namespace FatCat.Testing;

public static class ShouldExtensions
{
	public static IRangeComparer Should(this int subject)
	{
		return new NumberComparer(subject);
	}
}
