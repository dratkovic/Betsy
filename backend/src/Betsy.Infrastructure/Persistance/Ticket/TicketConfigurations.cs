using Betsy.Domain;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betsy.Infrastructure.Persistance.Ticket;
public class TicketConfigurations : AbstractEntityConfiguration<Domain.Ticket>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Domain.Ticket> builder)
    {
        builder.HasMany(x => x.OfferSelections)
            .WithOne()
            .HasForeignKey(x => x.TicketId);

        builder.HasIndex(x => x.CreatedAt).IsDescending();
    }
}