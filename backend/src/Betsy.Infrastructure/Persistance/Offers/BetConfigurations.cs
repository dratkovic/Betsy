using Betsy.Domain;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betsy.Infrastructure.Persistance.Offers;
public class BetConfigurations : AbstractEntityConfiguration<BetType>
{
    protected override void ConfigureEntity(EntityTypeBuilder<BetType> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);
    }
}