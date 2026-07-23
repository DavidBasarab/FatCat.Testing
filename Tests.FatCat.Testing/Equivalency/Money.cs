namespace Tests.FatCat.Testing.Equivalency;

public class Money(decimal amount, string currency)
{
	public decimal Amount { get; } = amount;

	public string Currency { get; } = currency;

	public override bool Equals(object obj)
	{
		if (obj is not Money other)
		{
			return false;
		}

		return Amount == other.Amount;
	}

	public override int GetHashCode()
	{
		return Amount.GetHashCode();
	}
}
