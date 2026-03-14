using FatCat.Testing.Numbers;

namespace FatCat.Testing;

public static class ShouldExtensions
{
	public static NumericComparer<byte> Should(this byte subject)
	{
		return new NumericComparer<byte>(subject);
	}

	public static NumericComparer<decimal> Should(this decimal subject)
	{
		return new NumericComparer<decimal>(subject);
	}

	public static NumericComparer<double> Should(this double subject)
	{
		return new NumericComparer<double>(subject);
	}

	public static NumericComparer<float> Should(this float subject)
	{
		return new NumericComparer<float>(subject);
	}

	public static NumericComparer<int> Should(this int subject)
	{
		return new NumericComparer<int>(subject);
	}

	public static NullableIntComparer Should(this int? subject)
	{
		return new NullableIntComparer(subject);
	}

	public static NumericComparer<long> Should(this long subject)
	{
		return new NumericComparer<long>(subject);
	}

	public static NumericComparer<nint> Should(this nint subject)
	{
		return new NumericComparer<nint>(subject);
	}

	public static NumericComparer<nuint> Should(this nuint subject)
	{
		return new NumericComparer<nuint>(subject);
	}

	public static NumericComparer<sbyte> Should(this sbyte subject)
	{
		return new NumericComparer<sbyte>(subject);
	}

	public static NumericComparer<short> Should(this short subject)
	{
		return new NumericComparer<short>(subject);
	}

	public static NumericComparer<uint> Should(this uint subject)
	{
		return new NumericComparer<uint>(subject);
	}

	public static NumericComparer<ulong> Should(this ulong subject)
	{
		return new NumericComparer<ulong>(subject);
	}

	public static NumericComparer<ushort> Should(this ushort subject)
	{
		return new NumericComparer<ushort>(subject);
	}
}
