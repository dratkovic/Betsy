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

        Ticket ticket;
        try
        {
            ticket = new Ticket(userId, request.TicketAmount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating ticket");
            return Error.Validation(description: e.Message);
        }

        var betTypes = await _dbContext.Set<BetType>()
            .Include(x => x.Match)
            .Where(x => request.SelectedBetTypesIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (betTypes.Count != request.SelectedBetTypesIds.Count)
        {
            return Error.Validation(description: "Invalid bet types");
        }

        foreach (var betType in betTypes)
        {
            var result = ticket.AddOfferSelection(betType);

            if (result.IsError)
            {
                return Error.Validation(description: result.Errors.First().Description);
            }
        }

        var validTicket = ticket.IsValid();

        if(validTicket.IsError)
        {
            return Error.Validation(description: validTicket.Errors.First().Description);
        }

        _dbContext.Set<Ticket>().Add(ticket);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TicketResponse(ticket, betTypes);
    }
}
