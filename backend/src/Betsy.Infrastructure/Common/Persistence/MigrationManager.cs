using Betsy.Domain;
using Betsy.Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Betsy.Infrastructure.Common.Persistence;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var environment = services.GetRequiredService<IHostEnvironment>();
        var logger = services.GetRequiredService<ILogger<BetsyDbContext>>();
        if (!environment.IsDevelopment()) return host;

        try
        {
            var context = services.GetService<BetsyDbContext>();
            context!.Database.EnsureCreated();

            SeedData(context, services.GetRequiredService<IPasswordHasher>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
        }

        return host;
    }

    private static void SeedData(BetsyDbContext context, IPasswordHasher passwordHasher)
    {
        var user = context.Users.FirstOrDefault(x=>x.Email == "jerry@betsy.hr");

        if(user is not null) return;

        // For the simplicity of the example, we are seeding one user with a password hash
        // this is not done in configurator because of issue in ef with navigation props
        // https://github.com/dotnet/efcore/issues/31373
        var hashedPassword = passwordHasher.HashPassword("P@ssword12").Value;
        var seedUser = new User("Jerry", "Cantrell", "jerry@betsy.hr", hashedPassword, "EUR");
        seedUser.CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001");
        seedUser.ModifiedBy = Guid.Parse("00000000-0000-0000-0000-000000000001");
        seedUser.CreatedAt = DateTime.UtcNow;
        seedUser.ModifiedAt = DateTime.UtcNow;

        context.Add(seedUser);
        context.SaveChanges();
    }
}