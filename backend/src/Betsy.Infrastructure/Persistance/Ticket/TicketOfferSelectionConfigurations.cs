using Betsy.Domain;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betsy.Infrastructure.Persistance.Ticket;
public class TicketOfferSelectionConfigurations : AbstractEntityConfiguration<TicketOfferSelection>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TicketOfferSelection> builder)
    {
    }
}