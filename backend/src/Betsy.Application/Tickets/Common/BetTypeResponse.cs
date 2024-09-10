using Betsy.Domain;

namespace Betsy.Application.Tickets.Common;

public record BetTypeResponse(
    MatchResponse Match,
    string Title,
    decimal Quota)
{
    internal BetTypeResponse(Match match, BetType betType) :
        this(new MatchResponse(match), betType.Title, betType.Quota)
    {
    }
}
