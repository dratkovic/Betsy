using Betsy.Domain;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betsy.Infrastructure.Persistance.Offers;
public class OfferConfigurations : AbstractEntityConfiguration<Offer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Offer> builder)
    {
        builder.Property(x => x.NameOne)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.NameTwo)
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.StartsAtUtc)
            .IsRequired();

        builder.Property(x => x.Sport)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CorellationId)
            .HasMaxLength(50);

        builder.HasMany<BetType>(x => x.BetTypes)
            .WithOne()
            .HasForeignKey(x=>x.OfferId);
    }
}