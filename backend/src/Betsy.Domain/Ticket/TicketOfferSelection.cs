using Betsy.Domain.Common;

namespace Betsy.Domain;

public class TicketOfferSelection : EntityBase
{
    public Guid TicketId { get; private set; }
    public Guid BetTypeId { get; private set; }
    public BetType BetType { get; private set; } = null!;

    private TicketOfferSelection() { }

    public TicketOfferSelection(Guid ticketId, BetType betType)
    {
        TicketId = ticketId;
        BetType = betType;
        BetTypeId = betType.Id;
    }
}