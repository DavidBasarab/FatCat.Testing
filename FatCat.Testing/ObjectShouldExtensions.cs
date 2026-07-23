using FatCat.Testing.Objects;

namespace FatCat.Testing;

public static class ObjectShouldExtensions
{
	public static ObjectComparer<T> Should<T>(this T subject)
		where T : class
	{
		return new ObjectComparer<T>(subject);
	}
}
