using Betsy.Application.Common.Authorization;
using Betsy.Application.Tickets.Common;
using MediatR;
using ErrorOr;

namespace Betsy.Application.Tickets.Commands.Create;

[Authorize]
public record CreateTicketCommand(
    decimal TicketAmount,
    List<Guid> SelectedBetTypesIds) : IRequest<ErrorOr<TicketResponse>>
{
}
