using Betsy.Domain;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betsy.Infrastructure.Persistance.Offer;
public class MatchConfigurations : AbstractEntityConfiguration<Match>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Match> builder)
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

        builder.Property(x => x.CorrelationId)
            .HasMaxLength(50);

        builder.HasIndex(x => x.StartsAtUtc).IsDescending();
        builder.HasIndex(x => x.NameOne);
        builder.HasIndex(x => x.NameTwo);
    }
}