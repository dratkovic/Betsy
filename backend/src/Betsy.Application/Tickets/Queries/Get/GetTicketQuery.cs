using Betsy.Application.Common.Authorization;
using Betsy.Application.Tickets.Common;
using ErrorOr;
using MediatR;

namespace Betsy.Application.Tickets.Queries.Get;

[Authorize]
public record GetTicketQuery(Guid Id) : IRequest<ErrorOr<TicketResponse>>
{}

