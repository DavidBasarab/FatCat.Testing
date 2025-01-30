namespace FatCat.Testing;

public interface IShouldComparer<out T>
	where T : IShouldNotComparer
{
	T Not { get; }

	IShouldComparer<T> Be(object expected);
}
