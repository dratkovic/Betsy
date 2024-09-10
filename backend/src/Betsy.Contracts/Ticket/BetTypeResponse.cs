namespace Betsy.Contracts.Ticket;

public record BetTypeResponse(
    MatchResponse Match,
    string Title,
    decimal Quota)
{
}
