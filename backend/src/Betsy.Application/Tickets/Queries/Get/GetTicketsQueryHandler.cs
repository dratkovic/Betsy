using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Pagination;
using Betsy.Application.Tickets.Common;
using MediatR;
using ErrorOr;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Application.Tickets.Queries.Get;
using Betsy.Domain;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Application.Tickets.Queries.GetAll;
internal class GetTicketQueryHandler : IRequestHandler<GetTicketQuery, ErrorOr<TicketResponse>>
{
    private readonly IBetsyDbContext _dbContext;
    private readonly IPaginatedRepository<Ticket> _ticketRepository;
    private readonly IUserSession _userSession;

    public GetTicketQueryHandler(IBetsyDbContext dbContext, IPaginatedRepository<Ticket> ticketRepository, IUserSession userSession)
    {
        _dbContext = dbContext;
        _ticketRepository = ticketRepository;
        _userSession = userSession;
    }

    public async Task<ErrorOr<TicketResponse>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        var user = _userSession.GetCurrentUser();

        var query = _dbContext.Set<Ticket>()
            .Include(x => x.OfferSelections)
            .ThenInclude(x => x.BetType)
            .ThenInclude(x => x.Match)
            .Where(x => x.UserId == user.Id && !x.IsDeleted && x.Id == request.Id)
            .OrderByDescending(x => x.CreatedAt);

        var result = await query.FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            return Error.NotFound();
        }

        return new TicketResponse(result, result.OfferSelections.Select(x => x.BetType).ToList());
    }
}
