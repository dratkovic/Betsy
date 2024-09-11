using Betsy.Api.Endpoints.Internal;
using Betsy.Application.Common.Pagination;
using Betsy.Application.Offers.Queries.GetAll;
using Betsy.Contracts.Offer;
using MediatR;
using PaginationQuery = Betsy.Contracts.Common.PaginationQuery;

namespace Betsy.Api.Endpoints;

public class OffersEndpoints : IEndpoints
{
    private const string Tag = "Offers";
    private const string BaseRoute = "offers";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseRoute}/special-offers", GetSpecialOffersAsync)
            .WithName("GetSpecialOffers")
            .Produces<PaginationResult<OfferResponse>>(200)
            .WithTags(Tag)
            .CacheOutput();

        app.MapGet($"{BaseRoute}/{{sport}}", GetOffersAsync)
            .WithName("GetOffersBySport")
            .Produces<PaginationResult<OfferResponse>>(200)
            .WithTags(Tag)
            .CacheOutput();

        app.MapGet($"{BaseRoute}", GetOffersAsync)
            .WithName("GetOffers")
            .Produces<PaginationResult<OfferResponse>>(200)
            .WithTags(Tag)
            .CacheOutput();
    }

    internal static async Task<IResult> GetOffersAsync(
        [AsParameters] PaginationQuery query,
        string? sport,
        ISender sender)
    {
        var getAllQuery = new GetOffersQuery
        {
            Page = query.GetPage(),
            PageSize = query.GetPageSize(),
            Filter = query.Filter,
            Sport = sport
        };

        var response = await sender.Send(getAllQuery);

        return response.Match(Results.Ok,
            Results.BadRequest);
    }

    internal static async Task<IResult> GetSpecialOffersAsync(
        [AsParameters] PaginationQuery query,
        ISender sender)
    {
        var getAllQuery = new GetOffersQuery
        {
            Page = query.GetPage(),
            PageSize = query.GetPageSize(),
            Filter = query.Filter,
            SpecialOffers = true
        };

        var response = await sender.Send(getAllQuery);

        return response.Match(Results.Ok,
            Results.BadRequest);
    }
}
