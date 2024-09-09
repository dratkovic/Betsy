using System.Linq.Expressions;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Application.Common.Pagination;
using Betsy.Domain.Common;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Infrastructure.Persistance;

public abstract class PaginatedRepositoryBase<T> : RepositoryBase<T>, IPaginatedRepository<T> where T : EntityBase
{
    public PaginatedRepositoryBase(BetsyDbContext dbContext) : base(dbContext)
    {
    }

    protected abstract Expression<Func<T, bool>> GetFilterPredicate(string filter);
    protected abstract Expression<Func<T, object>>? GetIncludePredicate();

    public async Task<PaginationResult<T>> GetPaginatedAsync(PaginationQuery pagination, Expression<Func<T, bool>>? whereExpression, CancellationToken token)
    {
        var skip = (pagination.Page - 1) * pagination.PageSize;

        var query = _dbContext.GetDbSet<T>().Where(x => !x.IsDeleted);

        if (GetIncludePredicate() != null)
        {
            query = query.Include(GetIncludePredicate()!);
        }

        if (pagination.Filter != null)
        {
            query = query.Where(GetFilterPredicate(pagination.Filter));
        }

        if (whereExpression != null)
        {
            query = query.Where(whereExpression);
        }

        query = query.Skip(skip).Take(pagination.PageSize);
        var data = await query.ToListAsync(token);

        var totalRecordsQuery = _dbContext.GetDbSet<T>().Where(x => !x.IsDeleted);
        if (pagination.Filter != null)
        {
            totalRecordsQuery = totalRecordsQuery.Where(GetFilterPredicate(pagination.Filter));
        }

        var totalRecords = await totalRecordsQuery.CountAsync(token);

        return new PaginationResult<T>(data, totalRecords, pagination.Page, pagination.PageSize);
    }
}
