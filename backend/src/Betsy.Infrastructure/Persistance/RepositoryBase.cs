using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain.Common;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Betsy.Infrastructure.Persistance;

public abstract class RepositoryBase<T>(BetsyDbContext dbContext) : IBaseRepository<T>
    where T : EntityBase
{
    protected readonly BetsyDbContext _dbContext = dbContext;

    public virtual async Task AddAsync(T entity, CancellationToken token = default)
    {
        await _dbContext.AddAsync(entity, token);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var query = _dbContext.GetDbSet<T>().Where(x => !x.IsDeleted && x.Id == id);

        query = ApplyIncludes(query);

        return await query.FirstOrDefaultAsync(token);
    }

    public virtual Task UpdateAsync(T entity, CancellationToken token = default)
    {
        _dbContext.Update(entity);
        return Task.CompletedTask;
    }

    protected IQueryable<T> ApplyIncludes(IQueryable<T> query)
    {
        foreach (var include in GetDefaultIncludePredicates())
        {
            query = query.Include(include);
        }
        return query;
    }

    protected virtual IEnumerable<Expression<Func<T, object>>> GetDefaultIncludePredicates()
    {
        return Enumerable.Empty<Expression<Func<T, object>>>();
    }
}
