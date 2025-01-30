namespace FatCat.Testing;

public interface IShouldComparer
{
	IShouldNotComparer Not { get; }

	IShouldComparer Be(object expected);
}
