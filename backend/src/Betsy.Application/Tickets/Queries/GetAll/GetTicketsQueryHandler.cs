using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Pagination;
using Betsy.Application.Tickets.Common;
using MediatR;
using ErrorOr;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Application.Tickets.Queries.GetAll;
internal class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, ErrorOr<PaginationResult<TicketResponse>>>
{
    private readonly IBetsyDbContext _dbContext;
    private readonly IPaginatedRepository<Ticket> _ticketRepository;
    private readonly IUserSession _userSession;

    public GetTicketsQueryHandler(IBetsyDbContext dbContext, IPaginatedRepository<Ticket> ticketRepository, IUserSession userSession)
    {
        _dbContext = dbContext;
        _ticketRepository = ticketRepository;
        _userSession = userSession;
    }

    public async Task<ErrorOr<PaginationResult<TicketResponse>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var user = _userSession.GetCurrentUser();

        var query = _dbContext.Set<Ticket>()
            .Include(x => x.OfferSelections)
            .ThenInclude(x => x.BetType)
            .ThenInclude(x => x.Match)
            .AsNoTracking()
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.CreatedAt);

        var result = await _ticketRepository.GetPaginatedAsync(request,
            query,
            cancellationToken);

        var data = result.Data.Select(x => new TicketResponse(
            x,
            x.OfferSelections.Select(y => y.BetType).ToList()
            )).ToList();

        return new PaginationResult<TicketResponse>(data, result.TotalRecords, result.Page, result.PageSize);
    }
}
