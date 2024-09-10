using Betsy.Domain;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betsy.Infrastructure.Persistance.Offer;
public class OfferConfigurations : AbstractEntityConfiguration<Domain.Offer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Domain.Offer> builder)
    {
        builder.HasMany<BetType>(x => x.BetTypes)
            .WithOne()
            .HasForeignKey(x=>x.OfferId);
    }
}