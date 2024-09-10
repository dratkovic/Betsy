using Betsy.Domain;

namespace Betsy.Application.Tickets.Common;

public record TicketResponse(
    decimal TicketAmount,
    decimal Stake,
    decimal Vig,
    decimal PotentialPayout,
    decimal TotalQuota,
    List<BetTypeResponse> BetTypes)
{
    internal TicketResponse(
        Ticket ticket,
        List<BetType> betTypes) :
        this(ticket.TicketAmount,
            ticket.Stake,
            ticket.Vig,
            ticket.PotentialPayout,
            ticket.TotalQuota,
            betTypes.Select(x => new BetTypeResponse(x.Match, x)).ToList())
    {
    }
}