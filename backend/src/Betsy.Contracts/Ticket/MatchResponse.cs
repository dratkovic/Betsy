namespace Betsy.Contracts.Ticket;

public record MatchResponse(
    string NameOne,
    string? NameTwo,
    string Description,
    DateTime StartsAtUtc,
    string? CorrelationId

    )
{
}
