using Betsy.Api.Endpoints.Internal;
using Betsy.Application.Common.Pagination;
using Betsy.Application.Offers.Queries.GetAll;
using Betsy.Contracts.Offer;
using MediatR;
using PaginationQuery = Betsy.Contracts.Common.PaginationQuery;

namespace Betsy.Api.Endpoints;

public class TicketEndpoints : IEndpoints
{
    private const string Tag = "Tickets";
    private const string BaseRoute = "tickets";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {

    }
}
