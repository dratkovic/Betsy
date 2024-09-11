namespace Betsy.Contracts.Ticket;

public record TicketResponse(
    Guid Id,
    decimal TicketAmount,
    decimal Stake,
    decimal Vig,
    decimal PotentialPayout,
    decimal TotalQuota,
    List<BetTypeResponse> BetTypes)
{
}