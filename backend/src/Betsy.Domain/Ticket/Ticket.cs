using Betsy.Domain.Common;
using ErrorOr;
using Throw;
using static System.Decimal;

namespace Betsy.Domain;
public sealed class Ticket : EntityBase
{
    private const decimal VigRatio = 0.05m;
    private List<TicketOfferSelection> _offerSelections = new();

    public Guid UserId { get; private set; }
    private User User { get; set; } = null!;

    public decimal TicketAmount { get; private set; } 
    public decimal Stake { get; private set; }
    public decimal Vig { get; private set; } 
    public decimal PotentialPayout { get; private set; } 
    public decimal TotalQuota { get; private set; } = 1;
    public bool ContainsSpecialOffer { get; private set; } = false;

    public IReadOnlyList<TicketOfferSelection> OfferSelections => _offerSelections;

    private Ticket() { }

    public Ticket(
        Guid userId, 
        decimal ticketAmount,
        List<BetType>? selectedBetTypes = null,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        UserId = userId;
        TicketAmount = ticketAmount;
        Vig = Round(VigRatio * TicketAmount, 2, MidpointRounding.AwayFromZero);
        Stake = ticketAmount - Vig;

        if (selectedBetTypes != null)
        {
            foreach (var selectedBetType in selectedBetTypes)
            {
                var result = AddOfferSelection(selectedBetType);

                if (result.IsError)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }

    public ErrorOr<TicketOfferSelection> AddOfferSelection(BetType selectedBetType)
    {
        if (selectedBetType.Quota < 1)
        {
            return Error.Validation(description: "Invalid bet type");
        }

        if (_offerSelections.Any(x => x.BetType.OfferId == selectedBetType.OfferId))
        {
            return Error.Validation(description: "Ticket already contains selection from offer");
        }

        if (_offerSelections.Any(x => x.BetType.IsSpecialOffer && x.BetType.MatchId == selectedBetType.MatchId))
        {
            return Error.Validation(description: "Cannot mix normal and special offer for the same match");
        }

        if (selectedBetType.IsSpecialOffer && _offerSelections.Any(x => x.BetType.MatchId == selectedBetType.MatchId))
        {
            return Error.Validation(description: "Cannot mix normal and special offer for the same match");
        }

        if (selectedBetType.IsSpecialOffer && ContainsSpecialOffer)
        {
            return Error.Validation(description: "Only one special offer is allowed in the ticket");
        }

        var offerSelection = new TicketOfferSelection(Id, selectedBetType);

        _offerSelections.Add(offerSelection);

        ContainsSpecialOffer = selectedBetType.IsSpecialOffer || ContainsSpecialOffer;

        TotalQuota = Round(TotalQuota * selectedBetType.Quota, 2);
        PotentialPayout = Round(Stake * TotalQuota,2);

        return offerSelection;
    }

    public ErrorOr<Success> IsValid()
    {
        if(!_offerSelections.Any())
            return Error.Validation(description: "Ticket must contain at least one offer selection");

        if (ContainsSpecialOffer)
        {
            var countTypesWithQuota =
                _offerSelections.Count(x => x.BetType is { IsSpecialOffer: false, Quota: >= 1.1m });

            if (countTypesWithQuota < 5)
            {
                return Error.Validation(description: "Ticket must contain at least 5 regular bet types with quota greater than 1.1");
            }
        }

        return new Success();
    }
}