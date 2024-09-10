namespace Betsy.Contracts.Offer;
public sealed record OfferResponse(
    Guid OfferId,
    Guid MatchId,
    bool IsSpecialOffer,
    string NameOne,
    string Sport,
    string? NameTwo,
    string Description,
    DateTime StartsAtUtc,
    List<OfferBetTypeResponse> BettingTypes)
{
}

public sealed record OfferBetTypeResponse(
    string Title,
    decimal Quota
    )
{
}

