namespace Betsy.Domain.Users;

public sealed class Wallet
{
    public decimal Balance { get; private set; }
    public string Currency { get; private set; }

    public Wallet(decimal balance, string currency = Currencies.Eur)
    {
        Balance = balance;
        Currency = currency;
    }

    internal bool CanWithdraw(decimal amount)
    {
        return Balance >= amount;
    }
}
