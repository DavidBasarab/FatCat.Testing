namespace Tests.FatCat.Testing;

public class Wallet(int balance)
{
	public int Balance { get; private set; } = balance;

	public int Withdraw(int amount)
	{
		if (amount > Balance)
		{
			throw new InvalidOperationException("insufficient funds");
		}

		Balance -= amount;

		return Balance;
	}

	public async Task<int> WithdrawAsync(int amount)
	{
		await Task.Yield();

		return Withdraw(amount);
	}
}
