namespace Betsy.Contracts.Ticket;

public record CreateTicketRequest(
    decimal TicketAmount,
    List<Guid> SelectedBetTypesIds)
{
}
