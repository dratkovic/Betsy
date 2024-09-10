using Betsy.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Application.Common.Interfaces.Repositories;

public interface IBetsyDbContext
{
    DbSet<T> Set<T>() where T : EntityBase;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    int SaveChanges();
}
