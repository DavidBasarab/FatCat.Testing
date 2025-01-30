namespace FatCat.Testing;

public interface IRangeComparer : IShouldComparer<INotRangeComparer>
{
	IRangeComparer BeInRange(object lower, object upper);
}

public interface INotRangeComparer : IShouldNotComparer
{
	INotRangeComparer BeInRange(object lower, object upper);
}
