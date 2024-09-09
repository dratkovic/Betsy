using Betsy.Domain.Common;
using Throw;

namespace Betsy.Domain;

public sealed class Offer : EntityBase
{
    private readonly List<BetType> _betTypes = [];

    public string NameOne {get; private set; } = string.Empty;
    public string? NameTwo { get; private set; } 
    public string Description { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }
    public IList<BetType> BetTypes => _betTypes.ToList();
    public bool IsSpecialOffer {get; private set; }

    // if we will create an offer from other service that handle matches
    // results and start/finish etc. we can use this property to correlate
    public string? CorellationId { get; private set; }

    public string Sport { get; private set; } = string.Empty;

    private Offer() { }

    public Offer(
        string nameOne,
        string description,
        DateTime startsAtUtc, 
        Sport sport,
        string? nameTwo = null,
        bool isSpecialOffer = false,
        string? corellationId = null,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        nameOne.Throw("Invalid name").IfEmpty();
        description.Throw("Invalid description").IfEmpty();
        startsAtUtc.Throw("Invalid start date").IfLessThan(DateTime.UtcNow.AddMinutes(10));

        NameOne = nameOne;
        NameTwo = nameTwo;
        Description = description;
        StartsAtUtc = startsAtUtc;
        Sport = sport.ToString();
        IsSpecialOffer = isSpecialOffer;
        CorellationId = corellationId;
    }

    public BetType AddBetType(string title, decimal quota)
    {
        var betType = new BetType(title, quota, Id);

        _betTypes.Throw("Betting type already exists").IfContains(betType);
        
        _betTypes.Add(betType);

        return betType;
    }

    public void RemoveBetType(string title)
    {
        var betType = _betTypes.FirstOrDefault(x => x.Title == title);

        betType.ThrowIfNull("Betting type not found");

        _betTypes.Remove(betType);
    }
}