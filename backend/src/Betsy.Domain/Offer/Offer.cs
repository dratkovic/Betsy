using Betsy.Domain.Common;
using Throw;

namespace Betsy.Domain;

public sealed class Offer : EntityBase
{
    private readonly List<BetType> _betTypes = [];
    public Match Match { get; set; } = null!;
    public Guid MatchId { get; private set; }
    public IList<BetType> BetTypes => _betTypes.ToList();
    public bool IsSpecialOffer { get; private set; }

    private Offer() { }

    public Offer(
        Guid matchId,
        bool isSpecialOffer = false,
        Dictionary<string, decimal>? betTypes = null,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        MatchId = matchId;
        IsSpecialOffer = isSpecialOffer;

        if (betTypes is null) return;

        foreach (var (title, quota) in betTypes)
        {
            AddBetType(title, quota);
        }
    }

    public BetType AddBetType(string title, decimal quota)
    {
        var betType = new BetType(title, quota, Id, MatchId, IsSpecialOffer);

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