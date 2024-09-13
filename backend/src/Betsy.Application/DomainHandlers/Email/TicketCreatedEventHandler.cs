using Betsy.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Betsy.Application.DomainHandlers.Email;
public class TicketCreatedEventHandler(ILogger<TicketCreatedEventHandler> _logger) : INotificationHandler<TicketCreatedEvent>
{
    public Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("TODO: We need a handler to send confirmation email etc..");

        return Task.CompletedTask;
    }
}
