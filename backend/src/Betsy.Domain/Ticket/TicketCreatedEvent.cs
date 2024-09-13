using Betsy.Domain.Common;

namespace Betsy.Domain;
public record  TicketCreatedEvent(Ticket Ticket) : IDomainEvent
{
}
