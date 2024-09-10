using Betsy.Application.Common.Authorization;
using Betsy.Application.Common.Pagination;
using Betsy.Application.Tickets.Common;
using MediatR;
using ErrorOr;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain;

namespace Betsy.Application.Tickets.Queries;

[Authorize]
public class GetTicketsQuery : PaginationQuery, IRequest<ErrorOr<PaginationResult<TicketResponse>>>
{
}
