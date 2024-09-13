using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Application.Tickets.Common;
using Betsy.Domain;
using MediatR;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Betsy.Application.Tickets.Commands.Create;
public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, ErrorOr<TicketResponse>>
{
    private readonly IBetsyDbContext _dbContext;
    private readonly IUserSession _userSession;
    private readonly ILogger<CreateTicketCommandHandler> _logger;

    public CreateTicketCommandHandler(IBetsyDbContext dbContext, IUserSession userSession, ILogger<CreateTicketCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userSession = userSession;
        _logger = logger;
    }

    public async Task<ErrorOr<TicketResponse>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var userId = _userSession.GetCurrentUser().Id;
        
        var ticket = Ticket.Create(userId, request.TicketAmount);

        var betTypes = await _dbContext.Set<BetType>()
            .Include(x => x.Match)
            .Where(x => request.SelectedBetTypesIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (betTypes.Count != request.SelectedBetTypesIds.Count)
        {
            return Error.Validation(description: "Invalid bet types");
        }

        var ticketCreator = Ticket.Create(userId, request.TicketAmount,betTypes);

        if(ticketCreator.IsError)
        {
            return Error.Validation(description: ticketCreator.Errors.First().Description);
        }

        _dbContext.Set<Ticket>().Add(ticketCreator.Value);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TicketResponse(ticketCreator.Value, betTypes);
    }
}
