namespace FatCat.Testing.Numbers;

public interface INumberComparer : IShouldComparer<INotRangeComparer>, IRangeComparer { }

public interface INotNumberComparer : INotRangeComparer { }
