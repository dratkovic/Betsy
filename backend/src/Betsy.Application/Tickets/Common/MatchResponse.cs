using Betsy.Domain;

namespace Betsy.Application.Tickets.Common;

public record MatchResponse(
    string NameOne,
    string? NameTwo,
    string Description,
    DateTime StartsAtUtc,
    string? CorrelationId

    )
{
    internal MatchResponse(Match match) : this(match.NameOne,
        match.NameTwo,
        match.Description,
        match.StartsAtUtc,
        match.CorrelationId)
    {
    }
}
