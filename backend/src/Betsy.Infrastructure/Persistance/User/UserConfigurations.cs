﻿using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betsy.Infrastructure.Persistance.User;
public class UserConfigurations : AbstractEntityConfiguration<Domain.User>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Domain.User> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property("_passwordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.OwnsOne(x => x.Wallet, navigationBuilder =>
        {
            navigationBuilder.Property(x => x.Currency).HasMaxLength(3);
        });
    }
}