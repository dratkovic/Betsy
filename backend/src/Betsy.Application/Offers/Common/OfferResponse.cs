using Betsy.Domain;

namespace Betsy.Application.Offers.Common;
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

public static class OfferResponseExtensions
{
    public static OfferResponse ToOfferResponse(this Offer offer)
    {
        return new OfferResponse(offer.Id, offer.MatchId,
            offer.IsSpecialOffer,
            offer.Match.NameOne,
            offer.Match.Sport,
            offer.Match.NameTwo,
            offer.Match.Description,
            offer.Match.StartsAtUtc,
            offer.BetTypes.Select(x => new OfferBetTypeResponse(x.Title, x.Quota)).ToList());
    }
}

