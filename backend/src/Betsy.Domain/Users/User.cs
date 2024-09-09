using Betsy.Domain.Common;
using Betsy.Domain.Users;

namespace Betsy.Domain;

public class User : EntityBase
{
    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public string Email { get; } = null!;
    public Wallet Wallet { get; private set; } = null!;

    private readonly string _passwordHash = null!;

    private User() { }

    public User(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        string currency = Currencies.Eur,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Wallet = new Wallet(0, currency);
        _passwordHash = passwordHash;
    }
    
    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, _passwordHash);
    }
}
