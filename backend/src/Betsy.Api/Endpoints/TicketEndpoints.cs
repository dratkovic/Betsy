using Betsy.Api.Common;
using Betsy.Api.Endpoints.Internal;
using Betsy.Application.Offers.Queries.GetAll;
using Betsy.Application.Tickets.Commands;
using Betsy.Application.Tickets.Queries;
using Betsy.Contracts.Common;
using Betsy.Contracts.Ticket;
using FluentValidation.Results;
using MediatR;
using ErrorOr;

namespace Betsy.Api.Endpoints;

public class TicketEndpoints : IEndpoints
{
    private const string ContentType = "application/json";
    private const string Tag = "Tickets";
    private const string BaseRoute = "tickets";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseRoute}", GetTicketsAsync)
            .WithName("GetTickets")
            .Produces<PaginationResult<TicketResponse>>(200)
            .WithTags(Tag)
            .RequireAuthorization();

        app.MapPost($"{BaseRoute}", CreateTicketAsync)
            .WithName("CreateTicket")
            .Accepts<CreateTicketRequest>(ContentType)
            .Produces<TicketResponse>(201)
            .Produces<IEnumerable<ValidationFailure>>(400)
            .Produces(401)
            .WithTags(Tag)
            .RequireAuthorization();
    }

    internal static async Task<IResult> GetTicketsAsync(
        [AsParameters] PaginationQuery query,
        ISender sender)
    {
        var getAllQuery = new GetTicketsQuery()
        {
            Page = query.GetPage(),
            PageSize = query.GetPageSize(),
            Filter = query.Filter
        };

        var response = await sender.Send(getAllQuery);

        return response.Match(Results.Ok,
            Results.BadRequest);
    }

    internal static async Task<IResult> CreateTicketAsync(CreateTicketRequest request, ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateTicketCommand(request.TicketAmount, request.SelectedBetTypesIds);

        var response = await sender.Send(command, cancellationToken);

        return response.Match(r => Results.Created($"/{BaseRoute}/{r.Id}", r),
            e => e.HandleErrors());
    }
}
